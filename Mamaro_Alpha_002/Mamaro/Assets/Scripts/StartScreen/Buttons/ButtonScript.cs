using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {
	public bool mouseOver;
	public Mamaro_StartAnim starAnim;
	// Use this for initialization
	void Awake () {
		starAnim = GameObject.FindGameObjectWithTag ("Player").GetComponent<Mamaro_StartAnim> ();
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
					starAnim.ArmActive();
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

