using UnityEngine;
using System.Collections;

public class Lightanimation_Controller : MonoBehaviour {
	public Animator anim;
	public bool isActive;
	public bool isPlaying;
	public bool hasPressed;
	public bool isIn;
	public bool move;
	public bool hasMoved;
	public Comic_Camera_Move movetoNext;

	// Use this for initialization
	void Awake () {
		movetoNext = Camera.main.GetComponent<Comic_Camera_Move> ();
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!hasMoved) {
			if (move) {
				MoveCam ();
				hasMoved = true;
			}
		}
		if (isPlaying) {
			isActive = false;
			hasPressed = false;
		}
			anim.SetBool ("Active", isActive);
	}
	void OnTriggerStay(Collider col){
		if (col.gameObject.tag == "MainCamera") {
			isIn = true;
			Debug.Log ("EEEK");
			if (hasPressed) {
				isActive = true;
			}
		}
	}

	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "MainCamera") {
			isIn = false; 
			hasMoved = false;
		}
	}

	public void MoveCam(){
		movetoNext.NewTarget ();
	}
}
