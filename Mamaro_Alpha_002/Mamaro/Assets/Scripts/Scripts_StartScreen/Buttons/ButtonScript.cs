using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {
	public bool mouseOver;
	// Use this for initialization
	void Start () {
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
		int i = Application.loadedLevel;
		if(Input.GetMouseButton(0) && mouseOver == true){
			if(mouseOver){
				if(transform.name == "StartButton"){
					Application.LoadLevel("Level");
				}
				else if(transform.name == "GoBackStartButton")
				{
					Application.LoadLevel("StartScreen");
				}
					else if(transform.name == "CreditsButton"){
							Application.LoadLevel("Credits");
						}
						else if(transform.name == "StructionsButton"){
								Application.LoadLevel("Structions");
							}
				
			}else{
			}
		}
	}
}

