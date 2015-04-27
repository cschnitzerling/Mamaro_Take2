using UnityEngine;
using System.Collections;

public class WinScene_Target : MonoBehaviour {

	Vector3 pos;
	Quaternion rot;

	public float pauseTime;
	public float moveTime;

	// Use this for initialization
	void Start () 
	{
		pos = gameObject.transform.position;
		rot = gameObject.transform.rotation;
	}

	public Vector3 GetPosition()
	{
		return pos;
	}

	public Quaternion GetRotation()
	{
		return rot;
	}

}
