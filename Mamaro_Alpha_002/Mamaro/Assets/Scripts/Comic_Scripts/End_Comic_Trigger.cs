using UnityEngine;
using System.Collections;

public class End_Comic_Trigger : MonoBehaviour {
	public bool isIn;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider col){
		if (col.gameObject.tag == "MainCamera") {
			isIn = true;
		}
}

	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "MainCamera") {
			isIn = false;
		}
	}

}
