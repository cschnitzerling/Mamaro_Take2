using UnityEngine;
using System.Collections;

public class Comic_Trigger : MonoBehaviour {
	public bool isIn, speedUp, isPuased;
	public bool isEnd, IsSpeedUp, IsSpeedUpTwo;
	public Comic_Controller CamCon;
	public FadeInOut FadetoNext;
	// Use this for initialization
	void Awake () {
		CamCon = Camera.main.GetComponent<Comic_Controller> ();
		FadetoNext = GameObject.FindGameObjectWithTag ("FadeScene").GetComponent<FadeInOut> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider col){
			if (col.gameObject.tag == "MainCamera") {
				isIn = true;
				if (IsSpeedUp) {
				CamCon.speedUp = true;
				}

			if (IsSpeedUpTwo) {
				CamCon.speedUpTwo = true;
			}

			if (isEnd) {
				FadetoNext.nextScene = true;
				isPuased = true;
			}
			} 
	}

	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "MainCamera") {
			isIn = false;
			CamCon.speedUp = false;
			CamCon.speedUpTwo = false;
			isPuased = false;

		}
	}

}
