using UnityEngine;
using System.Collections;

public class Tutorial_ShootPillar_AnimatorController : MonoBehaviour {
	public Animator anim;
	// Use this for initialization
	void Awake () {
		anim = GetComponent<Animator> ();
		anim.SetBool("Falling", false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Active(){
		anim.SetBool("Falling", true);
	}
}
