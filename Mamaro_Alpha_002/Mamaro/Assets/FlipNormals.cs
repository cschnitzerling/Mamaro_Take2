using UnityEngine;
using System.Collections;

public class FlipNormals : MonoBehaviour {



	// Use this for initialization
	void Start () 
	{
		SkinnedMeshRenderer mesh = gameObject.GetComponent<SkinnedMeshRenderer>();

		print (mesh.sharedMesh.normals[0]);
		print (mesh.sharedMesh.normals.Length);

		Vector3[] temparray = mesh.sharedMesh.normals;

		for (int i = 0; i < mesh.sharedMesh.normals.Length; i ++)
		{
			//print (temparray[i]);
			temparray[i] *= -1;
			//print (temparray[i]);
		}

		mesh.sharedMesh.normals.Equals(temparray);

		print (mesh.sharedMesh.normals[0]);
		print (mesh.sharedMesh.normals.Length);
//		Vector3[] tempVect = mesh.sharedMesh.vertices;
//		Vector2[]tempUV = mesh.sharedMesh.uv;
//		int[] tempTri = mesh.sharedMesh.triangles;
//
//		mesh.sharedMesh.Clear();
//
//		print ("1" +tempTri.Length);
//
//
//		int[] newTri = new int[tempTri.Length];
//
//		for (int i = 0; i < tempTri.Length; i ++)
//		{
//			newTri[i] = tempTri[tempTri.Length - i]; 
//		}
//
//		//////Rest
//		mesh.sharedMesh.vertices = tempVect;
//		mesh.sharedMesh.uv = tempUV;
//		mesh.sharedMesh.triangles = tempTri;
//
//		//mesh.sharedMesh.triangles = newTri;
//		mesh.sharedMesh.RecalculateNormals();
//
//		print ("2" + mesh.sharedMesh.triangles.Length);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
