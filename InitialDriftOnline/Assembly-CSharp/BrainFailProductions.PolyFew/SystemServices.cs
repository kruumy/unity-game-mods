using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace BrainFailProductions.PolyFew;

public static class SystemServices
{
	[Serializable]
	public struct RegexPatterns
	{
		public string netError;

		public string nullOrEmpty;

		public string generalError;

		public string apiMistmatch;

		public string parametersMismatch;

		public string nothing;
	}

	public struct MessagePatternPair
	{
		public string patternAppended { get; private set; }

		public string parsedMessage { get; private set; }

		public MessagePatternPair(string patternAppended, string parsedMessage)
		{
			this.patternAppended = patternAppended;
			this.parsedMessage = parsedMessage;
		}
	}

	public class HTTPMethod
	{
		public enum HTTPMethods
		{
			POST,
			GET
		}

		public readonly string methodName;

		public HTTPMethod(HTTPMethods method)
		{
			methodName = Enum.GetName(typeof(HTTPMethods), method);
		}
	}

	public enum ImageFormat
	{
		PNG,
		JPG,
		EXR
	}

	public static RegexPatterns regexPatterns;

	private static void SetPatterns()
	{
		regexPatterns.netError = "<neterror>";
		regexPatterns.nullOrEmpty = "<nullorempty>";
		regexPatterns.generalError = "<generalerror>";
		regexPatterns.apiMistmatch = "<apimismatch>";
		regexPatterns.parametersMismatch = "<parametersmismatch>";
		regexPatterns.nothing = "";
	}

	public static IEnumerator UnityAsyncGETRequest(string encodedUrl, Action<string, long> callback, int? timeout = null, Dictionary<string, string> headers = null)
	{
		SetPatterns();
		UnityWebRequest webRequest = new UnityWebRequest(encodedUrl);
		webRequest.timeout = ((!timeout.HasValue) ? webRequest.timeout : timeout.Value);
		webRequest.method = "GET";
		DownloadHandlerBuffer downloadHandlerBuffer = (DownloadHandlerBuffer)(webRequest.downloadHandler = new DownloadHandlerBuffer());
		if (headers != null)
		{
			foreach (KeyValuePair<string, string> header in headers)
			{
				webRequest.SetRequestHeader(header.Key, header.Value);
			}
		}
		yield return webRequest.SendWebRequest();
		long responseCode = webRequest.responseCode;
		if (webRequest.isHttpError || webRequest.isNetworkError)
		{
			callback("<neterror>" + webRequest.error, responseCode);
		}
		else if (string.IsNullOrEmpty(webRequest.downloadHandler.text))
		{
			callback("<nullorempty>Error! server returned an empty response.", responseCode);
		}
		else
		{
			callback(webRequest.downloadHandler.text, responseCode);
		}
	}

	public static void UnityBlockingGETRequest(string encodedUrl, Action<string, long> callback, int? timeout = null, Dictionary<string, string> headers = null)
	{
		SetPatterns();
		UnityWebRequest unityWebRequest = new UnityWebRequest(encodedUrl);
		unityWebRequest.timeout = ((!timeout.HasValue) ? unityWebRequest.timeout : timeout.Value);
		unityWebRequest.method = "GET";
		DownloadHandlerBuffer downloadHandlerBuffer = (DownloadHandlerBuffer)(unityWebRequest.downloadHandler = new DownloadHandlerBuffer());
		if (headers != null)
		{
			foreach (KeyValuePair<string, string> header in headers)
			{
				unityWebRequest.SetRequestHeader(header.Key, header.Value);
			}
		}
		unityWebRequest.SendWebRequest();
		while (!unityWebRequest.isDone)
		{
		}
		long responseCode = unityWebRequest.responseCode;
		if (unityWebRequest.isHttpError || unityWebRequest.isNetworkError)
		{
			callback("<neterror>" + unityWebRequest.error, responseCode);
		}
		else if (string.IsNullOrEmpty(unityWebRequest.downloadHandler.text))
		{
			callback("<nullorempty>Error! server returned an empty response.", responseCode);
		}
		else
		{
			callback(unityWebRequest.downloadHandler.text, responseCode);
		}
	}

	public static void UnityBlockingPOSTRequest(string baseUrl, Action<string, long> callback, byte[] data, int? timeout = null, Dictionary<string, string> headers = null)
	{
		SetPatterns();
		UnityWebRequest unityWebRequest = new UnityWebRequest(baseUrl);
		unityWebRequest.timeout = ((!timeout.HasValue) ? unityWebRequest.timeout : timeout.Value);
		unityWebRequest.method = "POST";
		UploadHandlerRaw uploadHandler = new UploadHandlerRaw(data);
		DownloadHandlerBuffer downloadHandler = new DownloadHandlerBuffer();
		unityWebRequest.uploadHandler = uploadHandler;
		unityWebRequest.downloadHandler = downloadHandler;
		unityWebRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
		if (headers != null)
		{
			foreach (KeyValuePair<string, string> header in headers)
			{
				unityWebRequest.SetRequestHeader(header.Key, header.Value);
			}
		}
		unityWebRequest.SendWebRequest();
		while (!unityWebRequest.isDone)
		{
		}
		long responseCode = unityWebRequest.responseCode;
		if (unityWebRequest.isHttpError || unityWebRequest.isNetworkError)
		{
			callback("<neterror>" + unityWebRequest.error, responseCode);
		}
		else if (string.IsNullOrEmpty(unityWebRequest.downloadHandler.text))
		{
			callback("<nullorempty>Error! server returned an empty response.", responseCode);
		}
		else
		{
			callback(unityWebRequest.downloadHandler.text, responseCode);
		}
	}

	public static IEnumerator UnityAsyncPOSTRequest(string baseUrl, Action<string, long> callback, byte[] data, int? timeout = null, Dictionary<string, string> headers = null)
	{
		SetPatterns();
		UnityWebRequest webRequest = new UnityWebRequest(baseUrl);
		webRequest.timeout = ((!timeout.HasValue) ? webRequest.timeout : timeout.Value);
		webRequest.method = "POST";
		UploadHandlerRaw uploadHandler = new UploadHandlerRaw(data);
		DownloadHandlerBuffer downloadHandler = new DownloadHandlerBuffer();
		webRequest.uploadHandler = uploadHandler;
		webRequest.downloadHandler = downloadHandler;
		webRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
		if (headers != null)
		{
			foreach (KeyValuePair<string, string> header in headers)
			{
				webRequest.SetRequestHeader(header.Key, header.Value);
			}
		}
		yield return webRequest.SendWebRequest();
		long responseCode = webRequest.responseCode;
		if (webRequest.isHttpError || webRequest.isNetworkError)
		{
			callback("<neterror>" + webRequest.error, responseCode);
		}
		else if (string.IsNullOrEmpty(webRequest.downloadHandler.text))
		{
			callback("<nullorempty>Error! server returned an empty response.", responseCode);
		}
		else
		{
			callback(webRequest.downloadHandler.text, responseCode);
		}
	}

	public static async Task SendHTTPRequestAsync(string baseUrl, HTTPMethod requestMethod, Action<string, HttpStatusCode?> callback, Dictionary<string, string> requestParameters, byte[] postData, string contentType, int? timeout = null, Dictionary<string, string> header = null)
	{
		SetPatterns();
		await Task.Delay(0);
		HttpWebRequest request;
		try
		{
			request = (HttpWebRequest)WebRequest.Create(baseUrl);
		}
		catch (Exception ex)
		{
			callback(regexPatterns.generalError + "+" + ex.ToString(), null);
			return;
		}
		HttpWebResponse httpResponse = null;
		try
		{
			request.Timeout = ((!timeout.HasValue) ? 100000 : timeout.Value);
			request.Method = requestMethod.methodName;
			request.Headers = new WebHeaderCollection();
			request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			if (header != null)
			{
				foreach (KeyValuePair<string, string> item in header)
				{
					request.Headers.Add(item.Key, item.Value);
				}
			}
			if (requestParameters != null)
			{
				string queryStringFromKeyValues = GetQueryStringFromKeyValues(requestParameters);
				if (requestMethod.methodName == "GET")
				{
					baseUrl += queryStringFromKeyValues;
				}
				else
				{
					byte[] paramsData = Encoding.UTF8.GetBytes(queryStringFromKeyValues);
					request.ContentLength = paramsData.Length;
					using Stream stream = await request.GetRequestStreamAsync();
					stream.Write(paramsData, 0, paramsData.Length);
				}
			}
			if (requestParameters == null && postData != null && requestMethod.methodName == "POST")
			{
				request.ContentLength = postData.Length;
				await Task.Run(delegate
				{
					using Stream stream2 = request.GetRequestStream();
					stream2.Write(postData, 0, postData.Length);
				});
			}
			await Task.Run(delegate
			{
				httpResponse = (HttpWebResponse)request.GetResponse();
			});
			if (httpResponse.StatusCode != HttpStatusCode.OK)
			{
				callback(regexPatterns.netError + "+" + httpResponse.StatusDescription, httpResponse.StatusCode);
			}
			else
			{
				callback(await new StreamReader(httpResponse.GetResponseStream()).ReadToEndAsync(), httpResponse.StatusCode);
			}
			httpResponse.Dispose();
		}
		catch (Exception ex2)
		{
			HttpStatusCode? arg = ((httpResponse == null) ? null : new HttpStatusCode?(httpResponse.StatusCode));
			if (ex2.InnerException is WebException || ex2.InnerException is SocketException)
			{
				WebException ex3 = ex2 as WebException;
				if (ex3.Status == WebExceptionStatus.Timeout)
				{
					callback(regexPatterns.generalError + "+" + ex3.ToString(), arg);
				}
				else
				{
					callback(regexPatterns.netError + "+" + ex3.ToString(), arg);
				}
			}
			else
			{
				callback(regexPatterns.generalError + "+" + ex2.ToString(), arg);
			}
		}
	}

	public static void SendHTTPRequestBlocking(string baseUrl, HTTPMethod requestMethod, Action<string, HttpStatusCode?> callback, Dictionary<string, string> requestParameters, byte[] postData, string contentType, int? timeout = null, Dictionary<string, string> header = null)
	{
		SetPatterns();
		HttpWebResponse httpWebResponse = null;
		try
		{
			if (requestParameters != null && requestMethod.methodName == "GET")
			{
				string queryStringFromKeyValues = GetQueryStringFromKeyValues(requestParameters);
				baseUrl = baseUrl + "?" + queryStringFromKeyValues;
			}
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(baseUrl);
			httpWebRequest.Timeout = ((!timeout.HasValue) ? 100000 : timeout.Value);
			httpWebRequest.Method = requestMethod.methodName;
			httpWebRequest.Headers = new WebHeaderCollection();
			httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			httpWebRequest.ContentType = contentType;
			if (header != null)
			{
				foreach (KeyValuePair<string, string> item in header)
				{
					httpWebRequest.Headers.Add(item.Key, item.Value);
				}
			}
			if (requestParameters != null && requestMethod.methodName == "POST")
			{
				string queryStringFromKeyValues2 = GetQueryStringFromKeyValues(requestParameters);
				byte[] bytes = Encoding.ASCII.GetBytes(queryStringFromKeyValues2);
				httpWebRequest.ContentLength = bytes.Length;
				using Stream stream = httpWebRequest.GetRequestStream();
				stream.Write(bytes, 0, bytes.Length);
			}
			else if (requestParameters == null && requestMethod.methodName != "GET")
			{
				httpWebRequest.ContentLength = 0L;
			}
			if (requestParameters == null && postData != null && requestMethod.methodName == "POST")
			{
				httpWebRequest.ContentLength = postData.Length;
				using Stream stream2 = httpWebRequest.GetRequestStream();
				stream2.Write(postData, 0, postData.Length);
			}
			httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			if (httpWebResponse.StatusCode != HttpStatusCode.OK)
			{
				callback(regexPatterns.netError + "+" + httpWebResponse.StatusDescription, httpWebResponse.StatusCode);
			}
			else
			{
				string arg = new StreamReader(httpWebResponse.GetResponseStream()).ReadToEnd();
				callback(arg, httpWebResponse.StatusCode);
			}
			httpWebResponse.Dispose();
		}
		catch (Exception ex)
		{
			HttpStatusCode? arg2 = httpWebResponse?.StatusCode;
			if (ex.InnerException is WebException || ex.InnerException is SocketException)
			{
				WebException ex2 = ex as WebException;
				if (ex2.Status == WebExceptionStatus.Timeout)
				{
					callback(regexPatterns.generalError + "+" + ex2.ToString(), arg2);
				}
				else
				{
					callback(regexPatterns.netError + "+" + ex2.ToString(), arg2);
				}
			}
			else
			{
				callback(regexPatterns.generalError + "+" + ex.ToString(), arg2);
			}
		}
	}

	public static async Task AsyncResourceDownload(string resourceUrl, Action<byte[], string, HttpStatusCode?> callback, int? timeout = null)
	{
		SetPatterns();
		await Task.Delay(0);
		HttpWebRequest request;
		try
		{
			request = (HttpWebRequest)WebRequest.Create(resourceUrl);
		}
		catch (Exception ex)
		{
			callback(null, ex.ToString(), null);
			return;
		}
		HttpWebResponse httpResponse = null;
		try
		{
			request.Timeout = ((!timeout.HasValue) ? 100000 : timeout.Value);
			await Task.Run(delegate
			{
				httpResponse = (HttpWebResponse)request.GetResponse();
			});
			if (httpResponse.StatusCode != HttpStatusCode.OK)
			{
				callback(null, httpResponse.StatusDescription, httpResponse.StatusCode);
			}
			else
			{
				Stream responseStream = httpResponse.GetResponseStream();
				byte[] arg = null;
				try
				{
					using BinaryReader binaryReader = new BinaryReader(responseStream);
					arg = binaryReader.ReadBytes((int)responseStream.Length);
				}
				catch (Exception ex2)
				{
					Debug.LogWarning(ex2);
					callback(arg, ex2.ToString(), httpResponse.StatusCode);
				}
				callback(arg, "", httpResponse.StatusCode);
			}
			httpResponse.Dispose();
		}
		catch (Exception ex3)
		{
			HttpStatusCode? arg2 = ((httpResponse == null) ? null : new HttpStatusCode?(httpResponse.StatusCode));
			if (ex3.InnerException is WebException || ex3.InnerException is SocketException)
			{
				WebException ex4 = ex3 as WebException;
				if (ex4.Status == WebExceptionStatus.Timeout)
				{
					callback(null, ex4.ToString(), arg2);
				}
				else
				{
					callback(null, ex4.ToString(), arg2);
				}
			}
			else
			{
				callback(null, ex3.ToString(), arg2);
			}
		}
	}

	public static async Task AsyncReachabilityCheck(string testUrl, Action<bool> callback)
	{
		HTTPMethod requestMethod = new HTTPMethod(HTTPMethod.HTTPMethods.GET);
		await SendHTTPRequestAsync(testUrl, requestMethod, delegate(string response, HttpStatusCode? statusCode)
		{
			if (statusCode.HasValue && statusCode == HttpStatusCode.OK)
			{
				callback(obj: true);
			}
			else
			{
				callback(obj: false);
			}
		}, null, null, "application/json");
	}

	public static void BlockingReachabilityCheck(string url, Action<bool> callback)
	{
		HTTPMethod requestMethod = new HTTPMethod(HTTPMethod.HTTPMethods.GET);
		SendHTTPRequestBlocking(url, requestMethod, delegate(string response, HttpStatusCode? statusCode)
		{
			if (statusCode.HasValue && statusCode == HttpStatusCode.OK)
			{
				callback(obj: true);
			}
			else
			{
				callback(obj: false);
			}
		}, null, null, "application/json");
	}

	public static MessagePatternPair ParseResponseMessage(string message)
	{
		string text = null;
		string nothing = regexPatterns.nothing;
		if (Regex.IsMatch(message, regexPatterns.netError, RegexOptions.Compiled))
		{
			text = message.Replace(regexPatterns.netError + "+", "");
			nothing = regexPatterns.netError;
		}
		else if (Regex.IsMatch(message, regexPatterns.apiMistmatch, RegexOptions.Compiled))
		{
			text = message.Replace(regexPatterns.apiMistmatch + "+", "");
			nothing = regexPatterns.apiMistmatch;
		}
		else if (Regex.IsMatch(message, regexPatterns.generalError, RegexOptions.Compiled))
		{
			text = message.Replace(regexPatterns.generalError + "+", "");
			nothing = regexPatterns.generalError;
		}
		else if (Regex.IsMatch(message, regexPatterns.parametersMismatch, RegexOptions.Compiled))
		{
			text = message.Replace(regexPatterns.parametersMismatch + "+", "");
			nothing = regexPatterns.parametersMismatch;
		}
		else if (Regex.IsMatch(message, regexPatterns.nullOrEmpty, RegexOptions.Compiled))
		{
			text = message.Replace(regexPatterns.nullOrEmpty + "+", "");
			nothing = regexPatterns.nullOrEmpty;
		}
		else
		{
			text = null;
			nothing = regexPatterns.nothing;
		}
		return new MessagePatternPair(nothing, text);
	}

	public static bool IsSuccessStatusCode(long statusCode)
	{
		if ((int)statusCode >= 200)
		{
			return (int)statusCode <= 299;
		}
		return false;
	}

	public static string GetQueryStringFromKeyValues(Dictionary<string, string> parameters)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, string> parameter in parameters)
		{
			list.Add(parameter.Key + "=" + Uri.EscapeDataString(parameter.Value));
		}
		return string.Join("&", list);
	}

	public static async Task RunDelayedCommand(int secs, Action command)
	{
		await Task.Delay(secs * 1000);
		command();
	}

	public static byte[] ReadAllBytes(Stream source)
	{
		long position = source.Position;
		source.Position = 0L;
		try
		{
			byte[] array = new byte[4096];
			int num = 0;
			int num2;
			while ((num2 = source.Read(array, num, array.Length - num)) > 0)
			{
				num += num2;
				if (num == array.Length)
				{
					int num3 = source.ReadByte();
					if (num3 != -1)
					{
						byte[] array2 = new byte[array.Length * 2];
						Buffer.BlockCopy(array, 0, array2, 0, array.Length);
						Buffer.SetByte(array2, num, (byte)num3);
						array = array2;
						num++;
					}
				}
			}
			byte[] array3 = array;
			if (array.Length != num)
			{
				array3 = new byte[num];
				Buffer.BlockCopy(array, 0, array3, 0, num);
			}
			return array3;
		}
		finally
		{
			source.Position = position;
		}
	}

	public static async Task WriteTextureAsync(Texture2D texture, ImageFormat format, string fileName, string path, Action<string> callback)
	{
		try
		{
			byte[] data = null;
			switch (format)
			{
			case ImageFormat.PNG:
				data = texture.EncodeToPNG();
				if (!fileName.ToLower().Contains(".png"))
				{
					fileName += ".png";
				}
				break;
			case ImageFormat.JPG:
				data = texture.EncodeToJPG();
				if (!fileName.ToLower().Contains(".jpg"))
				{
					fileName += ".jpg";
				}
				break;
			case ImageFormat.EXR:
				data = texture.EncodeToEXR();
				if (!fileName.ToLower().Contains(".exr"))
				{
					fileName += ".exr";
				}
				break;
			}
			if (data == null)
			{
				Debug.Log("Failed encoding");
			}
			path = ((!path.EndsWith("/") && !path.EndsWith("\\")) ? (path + "/" + fileName) : (path + fileName));
			using (FileStream fileStream = File.Create(path))
			{
				await fileStream.WriteAsync(data, 0, data.Length);
			}
			callback("");
			Debug.Log(data.Length / 1024 + "Kb was saved as: " + path);
		}
		catch (Exception ex)
		{
			callback(ex.ToString());
		}
	}

	public static async Task WriteBytesAsync(byte[] data, string fullPath, Action<string> callback)
	{
		try
		{
			using (FileStream fileStream = File.Create(fullPath))
			{
				await fileStream.WriteAsync(data, 0, data.Length);
			}
			callback("");
			Debug.Log(data.Length / 1024 + "Kb was saved as: " + fullPath);
		}
		catch (Exception ex)
		{
			callback(ex.ToString());
		}
	}
}
