using UnityEngine;
using System.Collections;

public class Mamaro_Hand_Animation : MonoBehaviour {
	public Animator anim;
	public bool handAnim;
	public bool handActive;
	public bool handEnd;
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
