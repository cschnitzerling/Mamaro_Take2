using UnityEngine;
using System.Collections;

public class JointBreakScript : MonoBehaviour {
	public JointDestroyScript parentDestroy;
	// Use this for initialization
	void Start () {
		parentDestroy = GetComponentInParent<JointDestroyScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnJointBreak(float breakForce) {
		Debug.Log("Joint Broke!, force: " + breakForce);
		parentDestroy.Destroy ();
	}
}
