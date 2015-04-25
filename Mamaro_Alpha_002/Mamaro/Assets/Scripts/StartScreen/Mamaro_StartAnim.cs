﻿using UnityEngine;
using System.Collections;

public class Mamaro_StartAnim : MonoBehaviour {

	public Animator anim;
	public bool handAnim;
	public bool armActive;
	public bool endScene = false;
	public float time;
	public FadeInOut fadeOut;
	// Use this for initialization
	void Awake () {
		fadeOut = GameObject.FindGameObjectWithTag ("FadeScene").GetComponent<FadeInOut> ();
		anim = GetComponent<Animator> ();
		anim.SetBool ("Active", false);
	}
	
	// Update is called once per frame
	void Update () {
		if (endScene) {
			fadeOut.nextSceneTwo = true;
		}
		}

	public void ArmActive(){
		anim.SetBool ("ArmActive", true);
	}
}