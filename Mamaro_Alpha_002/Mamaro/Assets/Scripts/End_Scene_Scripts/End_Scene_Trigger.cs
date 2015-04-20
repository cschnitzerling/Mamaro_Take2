using UnityEngine;
using System.Collections;

public class End_Scene_Trigger : MonoBehaviour {
	public FadeInOut triggerEnd;
	// Use this for initialization
	void Awake () {
		triggerEnd = GameObject.FindGameObjectWithTag ("FadeScene").GetComponent<FadeInOut> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Player") {
			//Debug.Log("EEEK");
			triggerEnd.sceneEnding = true;
		}
	}
}
