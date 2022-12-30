using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.PlayerServices;

public class SteamworksRemoteStorageManager : MonoBehaviour
{
	[Serializable]
	public struct FileAddress : IEquatable<FileAddress>
	{
		public int fileIndex;

		public int fileSize;

		public string fileName;

		public DateTime UtcTimestamp;

		public DateTime LocalTimestamp
		{
			get
			{
				return UtcTimestamp.ToLocalTime();
			}
			set
			{
				UtcTimestamp = value.ToUniversalTime();
			}
		}

		public static bool operator ==(FileAddress obj1, FileAddress obj2)
		{
			return obj1.Equals(obj2);
		}

		public static bool operator !=(FileAddress obj1, FileAddress obj2)
		{
			return !obj1.Equals(obj2);
		}

		public bool Equals(FileAddress other)
		{
			if (fileIndex == other.fileIndex && fileName == other.fileName)
			{
				return fileSize == other.fileSize;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj.GetType() == GetType())
			{
				return Equals((FileAddress)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (((fileIndex.GetHashCode() * 397) ^ fileSize.GetHashCode()) * 397) ^ fileName.GetHashCode();
		}
	}

	private static SteamworksRemoteStorageManager s_instance;

	public static List<FileAddress> files = new List<FileAddress>();

	private static CallResult<RemoteStorageFileReadAsyncComplete_t> fileReadAsyncComplete;

	private static Callback<RemoteStorageFileShareResult_t> fileShareResult;

	private static CallResult<RemoteStorageFileWriteAsyncComplete_t> fileWriteAsyncComplete;

	[Header("Remote Storage")]
	public List<SteamDataLibrary> GameDataModel = new List<SteamDataLibrary>();

	[Header("Events")]
	public UnityEvent FileReadAsyncComplete;

	public UnityEvent FileWriteAsyncComplete;

	[Obsolete("Avoid using singleton models, most funcitonality has been moved to be a static funciton where appropreate, remaining funcitonality is availabel via direct API call such as SteamRemoteStorage interface.", false)]
	public static SteamworksRemoteStorageManager Instance
	{
		get
		{
			if (s_instance == null)
			{
				return new GameObject("HeathenSteamCloud").AddComponent<SteamworksRemoteStorageManager>();
			}
			return s_instance;
		}
	}

	[Obsolete("Use SteamRemoteStorage.IsCloudEnabledForAccount", false)]
	public bool IsCloudEnabledForAccount => SteamRemoteStorage.IsCloudEnabledForAccount();

	[Obsolete("Use SteamRemoteStorage.IsCloudEnabledForApp", false)]
	public bool IsCloudEnabledForApp => SteamRemoteStorage.IsCloudEnabledForApp();

	public static bool Initalized { get; private set; }

	private void Start()
	{
		s_instance = this;
		if (!Initalized)
		{
			Initalized = true;
			fileReadAsyncComplete = CallResult<RemoteStorageFileReadAsyncComplete_t>.Create();
			fileShareResult = Callback<RemoteStorageFileShareResult_t>.Create(HandleFileShareResult);
			fileWriteAsyncComplete = CallResult<RemoteStorageFileWriteAsyncComplete_t>.Create();
		}
	}

	private static void HandleFileWriteAsyncComplete(RemoteStorageFileWriteAsyncComplete_t param, bool bIOFailure)
	{
		if (s_instance != null)
		{
			s_instance.FileWriteAsyncComplete.Invoke();
		}
	}

	private static void HandleFileShareResult(RemoteStorageFileShareResult_t param)
	{
	}

	private static void HandleFileReadAsyncComplete(RemoteStorageFileReadAsyncComplete_t param, bool bIOFailure)
	{
		if (s_instance != null)
		{
			s_instance.FileReadAsyncComplete.Invoke();
		}
	}

	public static void RefreshFileList()
	{
		files.Clear();
		if (s_instance != null)
		{
			s_instance.ClearAvailableLibraries();
		}
		int fileCount = SteamRemoteStorage.GetFileCount();
		for (int i = 0; i < fileCount; i++)
		{
			int pnFileSizeInBytes;
			string fileNameAndSize = SteamRemoteStorage.GetFileNameAndSize(i, out pnFileSizeInBytes);
			long fileTimestamp = SteamRemoteStorage.GetFileTimestamp(fileNameAndSize);
			DateTime utcTimestamp = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(fileTimestamp);
			FileAddress fileAddress = default(FileAddress);
			fileAddress.fileIndex = i;
			fileAddress.fileName = fileNameAndSize;
			fileAddress.fileSize = pnFileSizeInBytes;
			fileAddress.UtcTimestamp = utcTimestamp;
			FileAddress item = fileAddress;
			files.Add(item);
			if (s_instance != null)
			{
				SteamDataLibrary dataModelLibrary = s_instance.GetDataModelLibrary(fileNameAndSize);
				if (dataModelLibrary != null)
				{
					dataModelLibrary.availableFiles.Add(item);
				}
			}
		}
	}

	public SteamDataLibrary GetDataModelLibrary(string fileName)
	{
		if (!string.IsNullOrEmpty(fileName))
		{
			if (GameDataModel.Exists((SteamDataLibrary p) => fileName.StartsWith(p.filePrefix)))
			{
				return GameDataModel.First((SteamDataLibrary p) => fileName.StartsWith(p.filePrefix));
			}
			return null;
		}
		return null;
	}

	public SteamDataLibrary GetDataModelLibrary(FileAddress address)
	{
		return GetDataModelLibrary(address.fileName);
	}

	public SteamDataLibrary GetDataModelLibrary(SteamDataFile file)
	{
		if (file != null)
		{
			return GetDataModelLibrary(file.address.fileName);
		}
		return null;
	}

	private void ClearAvailableLibraries()
	{
		foreach (SteamDataLibrary item in GameDataModel)
		{
			item.availableFiles.Clear();
		}
	}

	public void SetCloudEnabledForApp(bool enable)
	{
		SteamRemoteStorage.SetCloudEnabledForApp(enable);
	}

	public bool SetSyncPlatforms(SteamDataFile file, ERemoteStoragePlatform platform)
	{
		return SteamRemoteStorage.SetSyncPlatforms(file.address.fileName, platform);
	}

	public bool SetSyncPlatforms(FileAddress address, ERemoteStoragePlatform platform)
	{
		return SteamRemoteStorage.SetSyncPlatforms(address.fileName, platform);
	}

	public bool SetSyncPlatforms(string fileName, ERemoteStoragePlatform platform)
	{
		return SteamRemoteStorage.SetSyncPlatforms(fileName, platform);
	}

	public static DateTime GetFileTimestamp(string fileName)
	{
		DateTime result = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		if (SteamRemoteStorage.FileExists(fileName))
		{
			long fileTimestamp = SteamRemoteStorage.GetFileTimestamp(fileName);
			result.AddSeconds(fileTimestamp);
		}
		return result;
	}

	public static DateTime GetFileTimestamp(FileAddress address)
	{
		DateTime result = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		if (SteamRemoteStorage.FileExists(address.fileName))
		{
			long fileTimestamp = SteamRemoteStorage.GetFileTimestamp(address.fileName);
			result = (address.UtcTimestamp = result.AddSeconds(fileTimestamp));
		}
		return result;
	}

	public static bool FileDelete(string fileName)
	{
		if (files.Exists((FileAddress p) => p.fileName == fileName))
		{
			FileAddress item = files.First((FileAddress p) => p.fileName == fileName);
			files.Remove(item);
			if (s_instance != null)
			{
				SteamDataLibrary dataModelLibrary = s_instance.GetDataModelLibrary(item.fileName);
				if (dataModelLibrary != null)
				{
					dataModelLibrary.availableFiles.Remove(item);
				}
			}
		}
		return SteamRemoteStorage.FileDelete(fileName);
	}

	public static bool FileDelete(FileAddress address)
	{
		files.Remove(address);
		if (s_instance != null)
		{
			SteamDataLibrary dataModelLibrary = s_instance.GetDataModelLibrary(address.fileName);
			if (dataModelLibrary != null)
			{
				dataModelLibrary.availableFiles.Remove(address);
			}
		}
		return SteamRemoteStorage.FileDelete(address.fileName);
	}

	public bool FileExists(string fileName)
	{
		return SteamRemoteStorage.FileExists(fileName);
	}

	public bool FileExists(FileAddress address)
	{
		return SteamRemoteStorage.FileExists(address.fileName);
	}

	public static SteamDataFile FileReadSteamDataFile(string fileName)
	{
		FileAddress fileAddress = default(FileAddress);
		fileAddress.fileIndex = -1;
		fileAddress.fileName = fileName;
		FileAddress address = fileAddress;
		if (files.Exists((FileAddress p) => p.fileName == fileName))
		{
			address = files.First((FileAddress p) => p.fileName == fileName);
		}
		else
		{
			GetFileTimestamp(address);
		}
		byte[] array = new byte[address.fileSize];
		SteamRemoteStorage.FileRead(address.fileName, array, array.Length);
		return new SteamDataFile
		{
			address = address,
			binaryData = array,
			apiCall = null,
			result = EResult.k_EResultOK
		};
	}

	public static string FileReadString(string fileName, Encoding encoding)
	{
		FileAddress fileAddress = default(FileAddress);
		fileAddress.fileIndex = -1;
		fileAddress.fileName = fileName;
		FileAddress address = fileAddress;
		if (files.Exists((FileAddress p) => p.fileName == fileName))
		{
			address = files.First((FileAddress p) => p.fileName == fileName);
		}
		else
		{
			GetFileTimestamp(address);
		}
		byte[] array = new byte[address.fileSize];
		SteamRemoteStorage.FileRead(address.fileName, array, array.Length);
		return encoding.GetString(array);
	}

	public static T FileReadJson<T>(string fileName, Encoding encoding)
	{
		FileAddress fileAddress = default(FileAddress);
		fileAddress.fileIndex = -1;
		fileAddress.fileName = fileName;
		FileAddress address = fileAddress;
		if (files.Exists((FileAddress p) => p.fileName == fileName))
		{
			address = files.First((FileAddress p) => p.fileName == fileName);
		}
		else
		{
			GetFileTimestamp(address);
		}
		byte[] array = new byte[address.fileSize];
		SteamRemoteStorage.FileRead(address.fileName, array, array.Length);
		return JsonUtility.FromJson<T>(encoding.GetString(array));
	}

	public static byte[] FileReadData(string fileName)
	{
		int fileSize = SteamRemoteStorage.GetFileSize(fileName);
		byte[] array = new byte[fileSize];
		SteamRemoteStorage.FileRead(fileName, array, fileSize);
		return array;
	}

	public static SteamDataFile FileReadSteamDataFile(FileAddress address)
	{
		byte[] array = new byte[address.fileSize];
		SteamRemoteStorage.FileRead(address.fileName, array, array.Length);
		return new SteamDataFile
		{
			address = address,
			binaryData = array,
			apiCall = null,
			result = EResult.k_EResultOK
		};
	}

	public static string FileReadString(FileAddress address, Encoding encoding)
	{
		byte[] array = new byte[address.fileSize];
		SteamRemoteStorage.FileRead(address.fileName, array, array.Length);
		return encoding.GetString(array);
	}

	public static T FileReadJson<T>(FileAddress address, Encoding encoding)
	{
		byte[] array = new byte[address.fileSize];
		SteamRemoteStorage.FileRead(address.fileName, array, array.Length);
		return JsonUtility.FromJson<T>(encoding.GetString(array));
	}

	public static byte[] FileReadData(FileAddress address)
	{
		byte[] array = new byte[address.fileSize];
		SteamRemoteStorage.FileRead(address.fileName, array, array.Length);
		return array;
	}

	public static SteamDataFile FileReadAsync(string fileName)
	{
		FileAddress fileAddress = default(FileAddress);
		fileAddress.fileIndex = -1;
		fileAddress.fileName = fileName;
		FileAddress address = fileAddress;
		if (files.Exists((FileAddress p) => p.fileName == fileName))
		{
			address = files.First((FileAddress p) => p.fileName == fileName);
		}
		else
		{
			GetFileTimestamp(address);
		}
		SteamDataFile data = new SteamDataFile
		{
			address = address
		};
		data.apiCall = SteamRemoteStorage.FileReadAsync(address.fileName, 0u, (uint)address.fileSize);
		fileReadAsyncComplete.Set(data.apiCall.Value, delegate(RemoteStorageFileReadAsyncComplete_t p, bool f)
		{
			data.HandleFileReadAsyncComplete(p, f);
			HandleFileReadAsyncComplete(p, f);
		});
		return data;
	}

	public static SteamDataFile FileReadAsync(FileAddress address)
	{
		SteamDataFile data = new SteamDataFile
		{
			address = address
		};
		data.apiCall = SteamRemoteStorage.FileReadAsync(address.fileName, 0u, (uint)address.fileSize);
		fileReadAsyncComplete.Set(data.apiCall.Value, delegate(RemoteStorageFileReadAsyncComplete_t p, bool f)
		{
			data.HandleFileReadAsyncComplete(p, f);
			HandleFileReadAsyncComplete(p, f);
		});
		return data;
	}

	public bool FileForget(string fileName)
	{
		return SteamRemoteStorage.FileForget(fileName);
	}

	public bool FileForget(FileAddress address)
	{
		return SteamRemoteStorage.FileForget(address.fileName);
	}

	public static bool FileWrite(SteamDataFile file)
	{
		if (file != null && file.binaryData.Length != 0 && !string.IsNullOrEmpty(file.address.fileName))
		{
			if (file.linkedLibrary != null)
			{
				file.ReadFromLibrary(file.linkedLibrary);
			}
			if (SteamRemoteStorage.FileWrite(file.address.fileName, file.binaryData, file.binaryData.Length))
			{
				file.address.UtcTimestamp = GetFileTimestamp(file.address);
				return true;
			}
			return false;
		}
		return false;
	}

	public static bool FileWrite(string fileName, byte[] data)
	{
		if (data.Length != 0 && !string.IsNullOrEmpty(fileName))
		{
			if (!files.Exists((FileAddress p) => p.fileName == fileName))
			{
				FileAddress fileAddress = default(FileAddress);
				fileAddress.fileIndex = -1;
				fileAddress.fileName = fileName;
				fileAddress.fileSize = data.Length;
				fileAddress.UtcTimestamp = DateTime.UtcNow;
				FileAddress item = fileAddress;
				files.Add(item);
			}
			bool num = SteamRemoteStorage.FileWrite(fileName, data, data.Length);
			if (!num)
			{
				SteamRemoteStorage.GetQuota(out var _, out var puAvailableBytes);
				if (puAvailableBytes < Convert.ToUInt64(data.Length))
				{
					Debug.LogWarning("Insufficent storage space available on the Steam Remote Storage target.");
					return num;
				}
				Debug.LogWarning("Failed to save the file to the Steam Remote Storage ... Please consult your Steamworks documentaiton regarding Steam Remote Storage.");
				return num;
			}
			Debug.Log("File " + fileName + " saved to Steam Remote Storage.");
			return num;
		}
		Debug.LogWarning("Failed to save the file to the Steam Remote Storage ... " + ((data.Length < 0) ? "You did not pass any data to be saved! " : "") + (string.IsNullOrEmpty(fileName) ? "You did not provide a valid file name! " : ""));
		return false;
	}

	public static bool FileWrite(string fileName, string body, Encoding encoding)
	{
		byte[] bytes = encoding.GetBytes(body);
		if (bytes.Length != 0 && !string.IsNullOrEmpty(fileName))
		{
			if (!files.Exists((FileAddress p) => p.fileName == fileName))
			{
				FileAddress fileAddress = default(FileAddress);
				fileAddress.fileIndex = -1;
				fileAddress.fileName = fileName;
				fileAddress.fileSize = bytes.Length;
				fileAddress.UtcTimestamp = DateTime.UtcNow;
				FileAddress item = fileAddress;
				files.Add(item);
			}
			bool num = SteamRemoteStorage.FileWrite(fileName, bytes, bytes.Length);
			if (!num)
			{
				SteamRemoteStorage.GetQuota(out var _, out var puAvailableBytes);
				if (puAvailableBytes < Convert.ToUInt64(bytes.Length))
				{
					Debug.LogWarning("Insufficent storage space available on the Steam Remote Storage target.");
					return num;
				}
				Debug.LogWarning("Failed to save the file to the Steam Remote Storage ... Please consult your Steamworks documentaiton regarding Steam Remote Storage.");
				return num;
			}
			Debug.Log("File " + fileName + " saved to Steam Remote Storage.");
			return num;
		}
		Debug.LogWarning("Failed to save the file to the Steam Remote Storage ... " + ((bytes.Length < 0) ? "You did not pass any data to be saved! " : "") + (string.IsNullOrEmpty(fileName) ? "You did not provide a valid file name! " : ""));
		return false;
	}

	public static bool FileWrite(string fileName, object JsonObject, Encoding encoding)
	{
		return FileWrite(fileName, JsonUtility.ToJson(JsonObject), encoding);
	}

	public static bool FileWrite(string fileName, SteamDataLibrary lib)
	{
		if (lib != null && !string.IsNullOrEmpty(fileName))
		{
			if (s_instance != null && s_instance.GameDataModel.Exists((SteamDataLibrary p) => p == lib))
			{
				FileAddress fileAddress = default(FileAddress);
				if (files.Exists((FileAddress p) => p.fileName == fileName))
				{
					fileAddress = files.First((FileAddress p) => p.fileName == fileName);
				}
				else
				{
					fileAddress.fileIndex = -1;
					fileAddress.fileName = fileName;
					fileAddress.UtcTimestamp = DateTime.UtcNow;
					files.Add(fileAddress);
					lib.availableFiles.Add(fileAddress);
				}
				SteamDataFile steamDataFile = new SteamDataFile
				{
					address = fileAddress,
					linkedLibrary = lib,
					result = EResult.k_EResultOK
				};
				steamDataFile.ReadFromLibrary(steamDataFile.linkedLibrary);
				lib.activeFile = steamDataFile;
				lib.SyncToBuffer(out steamDataFile.binaryData);
				bool num = SteamRemoteStorage.FileWrite(fileName, steamDataFile.binaryData, steamDataFile.binaryData.Length);
				if (!num)
				{
					SteamRemoteStorage.GetQuota(out var _, out var puAvailableBytes);
					if (puAvailableBytes < Convert.ToUInt64(steamDataFile.binaryData.Length))
					{
						Debug.LogWarning("Insufficent storage space available on the Steam Remote Storage target.");
						return num;
					}
					Debug.LogWarning("Failed to save the file to the Steam Remote Storage ... Please consult your Steamworks documentaiton regarding Steam Remote Storage.");
					return num;
				}
				Debug.Log("File " + fileName + " saved to Steam Remote Storage.");
				return num;
			}
			lib.SyncToBuffer(out var buffer);
			bool num2 = SteamRemoteStorage.FileWrite(fileName, buffer, buffer.Length);
			if (!num2)
			{
				SteamRemoteStorage.GetQuota(out var _, out var puAvailableBytes2);
				if (puAvailableBytes2 < Convert.ToUInt64(buffer.Length))
				{
					Debug.LogWarning("Insufficent storage space available on the Steam Remote Storage target.");
					return num2;
				}
				Debug.LogWarning("Failed to save the file to the Steam Remote Storage ... Please consult your Steamworks documentaiton regarding Steam Remote Storage.");
				return num2;
			}
			Debug.Log("File " + fileName + " saved to Steam Remote Storage.");
			return num2;
		}
		Debug.LogWarning("Failed to save the file to the Steam Remote Storage ... " + ((lib == null) ? "You did not pass any data to be saved! " : "") + (string.IsNullOrEmpty(fileName) ? "You did not provide a valid file name! " : ""));
		return false;
	}

	public static SteamDataFile FileWriteAsync(SteamDataFile file)
	{
		if (file != null && file.binaryData.Length != 0 && !string.IsNullOrEmpty(file.address.fileName))
		{
			SteamDataFile nDataFile = new SteamDataFile
			{
				address = file.address,
				binaryData = new List<byte>(file.binaryData).ToArray(),
				linkedLibrary = file.linkedLibrary
			};
			if (nDataFile.linkedLibrary != null)
			{
				nDataFile.ReadFromLibrary(nDataFile.linkedLibrary);
			}
			nDataFile.apiCall = SteamRemoteStorage.FileWriteAsync(nDataFile.address.fileName, nDataFile.binaryData, (uint)nDataFile.binaryData.Length);
			fileWriteAsyncComplete.Set(nDataFile.apiCall.Value, delegate(RemoteStorageFileWriteAsyncComplete_t p, bool f)
			{
				nDataFile.HandleFileWriteAsyncComplete(p, f);
				HandleFileWriteAsyncComplete(p, f);
			});
			return nDataFile;
		}
		Debug.LogWarning("Failed to save the file to the Steam Remote Storage ... " + ((file.binaryData.Length < 0) ? "You did not pass any data to be saved! " : "") + (string.IsNullOrEmpty(file.address.fileName) ? "You did not provide a valid file name! " : ""));
		return new SteamDataFile
		{
			result = EResult.k_EResultFail
		};
	}

	public static SteamDataFile FileWriteAsync(string fileName, object jsonObject, Encoding encoding)
	{
		SteamDataFile steamDataFile = new SteamDataFile();
		steamDataFile.address = new FileAddress
		{
			fileName = fileName
		};
		steamDataFile.SetDataFromObject(jsonObject, encoding);
		return FileWriteAsync(steamDataFile);
	}

	public static SteamDataFile FileWriteAsync(string fileName, byte[] data)
	{
		return FileWriteAsync(new SteamDataFile
		{
			address = new FileAddress
			{
				fileName = fileName
			},
			binaryData = data
		});
	}

	public static SteamDataFile FileWriteAsync(string fileName, SteamDataLibrary lib)
	{
		if (lib != null && !string.IsNullOrEmpty(fileName))
		{
			FileAddress fileAddress = default(FileAddress);
			if (files.Exists((FileAddress p) => p.fileName == fileName))
			{
				fileAddress = files.First((FileAddress p) => p.fileName == fileName);
			}
			else
			{
				fileAddress.fileIndex = -1;
				fileAddress.fileName = fileName;
				fileAddress.UtcTimestamp = DateTime.UtcNow;
				files.Add(fileAddress);
				lib.availableFiles.Add(fileAddress);
			}
			SteamDataFile file = new SteamDataFile
			{
				address = fileAddress,
				linkedLibrary = lib,
				result = EResult.k_EResultOK
			};
			lib.activeFile = file;
			lib.SyncToBuffer(out file.binaryData);
			file.apiCall = SteamRemoteStorage.FileWriteAsync(fileName, file.binaryData, (uint)file.binaryData.Length);
			fileWriteAsyncComplete.Set(file.apiCall.Value, delegate(RemoteStorageFileWriteAsyncComplete_t p, bool f)
			{
				file.HandleFileWriteAsyncComplete(p, f);
				HandleFileWriteAsyncComplete(p, f);
			});
			return file;
		}
		Debug.LogWarning("Failed to save the file to the Steam Remote Storage ... " + ((lib == null) ? "You did not pass any data to be saved! " : "") + (string.IsNullOrEmpty(fileName) ? "You did not provide a valid file name! " : ""));
		return new SteamDataFile
		{
			result = EResult.k_EResultFail
		};
	}
}
