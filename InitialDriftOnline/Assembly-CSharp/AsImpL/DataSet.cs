using System.Collections.Generic;
using UnityEngine;

namespace AsImpL;

public class DataSet
{
	public struct FaceIndices
	{
		public int vertIdx;

		public int uvIdx;

		public int normIdx;
	}

	public class ObjectData
	{
		public string name;

		public List<FaceGroupData> faceGroups = new List<FaceGroupData>();

		public List<FaceIndices> allFaces = new List<FaceIndices>();

		public bool hasNormals;

		public bool hasColors;
	}

	public class FaceGroupData
	{
		public string name;

		public string materialName;

		public List<FaceIndices> faces;

		public bool IsEmpty => faces.Count == 0;

		public FaceGroupData()
		{
			faces = new List<FaceIndices>();
		}
	}

	public List<ObjectData> objectList = new List<ObjectData>();

	public List<Vector3> vertList = new List<Vector3>();

	public List<Vector2> uvList = new List<Vector2>();

	public List<Vector3> normalList = new List<Vector3>();

	public List<Color> colorList = new List<Color>();

	private int unnamedGroupIndex = 1;

	private ObjectData currObjData;

	private FaceGroupData currGroup;

	private bool noFaceDefined = true;

	public string CurrGroupName
	{
		get
		{
			if (currGroup == null)
			{
				return "";
			}
			return currGroup.name;
		}
	}

	public bool IsEmpty => vertList.Count == 0;

	public static string GetFaceIndicesKey(FaceIndices fi)
	{
		return fi.vertIdx + "/" + fi.uvIdx + "/" + fi.normIdx;
	}

	public static string FixMaterialName(string mtlName)
	{
		return mtlName.Replace(':', '_').Replace('\\', '_').Replace('/', '_')
			.Replace('*', '_')
			.Replace('?', '_')
			.Replace('<', '_')
			.Replace('>', '_')
			.Replace('|', '_');
	}

	public DataSet()
	{
		ObjectData objectData = new ObjectData
		{
			name = "default"
		};
		objectList.Add(objectData);
		currObjData = objectData;
		FaceGroupData item = new FaceGroupData
		{
			name = "default"
		};
		objectData.faceGroups.Add(item);
		currGroup = item;
	}

	public void AddObject(string objectName)
	{
		string materialName = currObjData.faceGroups[currObjData.faceGroups.Count - 1].materialName;
		if (noFaceDefined)
		{
			objectList.Remove(currObjData);
		}
		ObjectData objectData = new ObjectData();
		objectData.name = objectName;
		objectList.Add(objectData);
		FaceGroupData faceGroupData = new FaceGroupData();
		faceGroupData.materialName = materialName;
		faceGroupData.name = "default";
		objectData.faceGroups.Add(faceGroupData);
		currGroup = faceGroupData;
		currObjData = objectData;
	}

	public void AddGroup(string groupName)
	{
		string materialName = currObjData.faceGroups[currObjData.faceGroups.Count - 1].materialName;
		if (currGroup.IsEmpty)
		{
			currObjData.faceGroups.Remove(currGroup);
		}
		FaceGroupData faceGroupData = new FaceGroupData();
		faceGroupData.materialName = materialName;
		if (groupName == null)
		{
			groupName = "Unnamed-" + unnamedGroupIndex;
			unnamedGroupIndex++;
		}
		faceGroupData.name = groupName;
		currObjData.faceGroups.Add(faceGroupData);
		currGroup = faceGroupData;
	}

	public void AddMaterialName(string matName)
	{
		if (!currGroup.IsEmpty)
		{
			AddGroup(matName);
		}
		if (currGroup.name == "default")
		{
			currGroup.name = matName;
		}
		currGroup.materialName = matName;
	}

	public void AddVertex(Vector3 vertex)
	{
		vertList.Add(vertex);
	}

	public void AddUV(Vector2 uv)
	{
		uvList.Add(uv);
	}

	public void AddNormal(Vector3 normal)
	{
		normalList.Add(normal);
	}

	public void AddColor(Color color)
	{
		colorList.Add(color);
		currObjData.hasColors = true;
	}

	public void AddFaceIndices(FaceIndices faceIdx)
	{
		noFaceDefined = false;
		currGroup.faces.Add(faceIdx);
		currObjData.allFaces.Add(faceIdx);
		if (faceIdx.normIdx >= 0)
		{
			currObjData.hasNormals = true;
		}
	}

	public void PrintSummary()
	{
		string text = "This data set has " + objectList.Count + " object(s)\n  " + vertList.Count + " vertices\n  " + uvList.Count + " uvs\n  " + normalList.Count + " normals";
		foreach (ObjectData @object in objectList)
		{
			text = text + "\n  " + @object.name + " has " + @object.faceGroups.Count + " group(s)";
			foreach (FaceGroupData faceGroup in @object.faceGroups)
			{
				text = text + "\n    " + faceGroup.name + " has " + faceGroup.faces.Count + " faces(s)";
			}
		}
		Debug.Log(text);
	}
}
