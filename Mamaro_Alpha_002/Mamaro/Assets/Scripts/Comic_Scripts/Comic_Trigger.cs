using UnityEngine;
using System.Collections;

public class Comic_Trigger : MonoBehaviour {
	public bool isIn, speedUp;
	public bool isEnd, IsSpeedUp;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider col){
		if (IsSpeedUp) {
			if (col.gameObject.tag == "MainCamera") {
				isIn = true;
				if (IsSpeedUp) {
				speedUp = true;
				}
			} 
		}
	}

	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "MainCamera") {
			isIn = false;
			speedUp = false;
		}
	}

}
