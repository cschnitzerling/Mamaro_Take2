using UnityEngine;
using System.Collections;

public class Lightanimation_Controller : MonoBehaviour {
	public Animator anim;
	// Use this for initialization
	void Awake () {
		anim = GetComponent<Animator> ();
		anim.SetBool ("Active", false);
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("space")) {
			anim.SetBool ("Active", true);
		}
	}
}
