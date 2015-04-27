using UnityEngine;
using System.Collections;

public class Reset_Scene_Trigger : MonoBehaviour {
	public FadeInOut triggerEnd;
	public bool isEnd;
	// Use this for initialization
	void Awake () {
		triggerEnd = GameObject.FindGameObjectWithTag ("FadeScene").GetComponent<FadeInOut> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Player") {
			if(gameObject.tag == "isEnd"){
				Application.LoadLevel("WinScene");
			}
			//Debug.Log("EEEK");
			triggerEnd.sceneReseting = true;
		}
	}
}
