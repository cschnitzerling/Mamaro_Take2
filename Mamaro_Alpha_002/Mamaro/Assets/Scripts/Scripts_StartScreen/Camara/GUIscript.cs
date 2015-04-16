using UnityEngine;
using System.Collections;

public class GUIscript : MonoBehaviour {
	public bool mouseOver;
	public HUDScript hudScriptPuased;
	public bool dontCheck, MenuisActive;
	
	// Use this for initialization
	void Awake(){
		
	}
	void Start () {
		hudScriptPuased = transform.GetComponentInParent<HUDScript> ();
	}
	public void OnMouseEnter(){
		mouseOver = true;

	}
	
	public void OnMouseOver(){
		mouseOver = true;
	}
	
	public void OnMouseExit(){
		mouseOver = false;
	}
	
	
	// Update is called once per frame
	void Update () {

		dontCheck = hudScriptPuased.isPause;

		if (hudScriptPuased.isPause) {
			//MenuisActive = true;
				} else {
			//MenuisActive = false;
				}

		if(Input.GetMouseButtonDown(0) && mouseOver == true && MenuisActive == false){
				if(transform.name == "StructionsButton"){
					Application.LoadLevel("Structions");
				}

	}
}
}