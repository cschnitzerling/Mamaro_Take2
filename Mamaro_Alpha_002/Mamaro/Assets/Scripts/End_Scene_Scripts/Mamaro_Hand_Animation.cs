using UnityEngine;
using System.Collections;

public class Mamaro_Hand_Animation : MonoBehaviour {
	public Animator anim;
	public bool handAnim;
	public bool handActive;
	public bool handEnd;
	public float time;
	// Use this for initialization
	void Awake () {
		anim = GetComponent<Animator> ();
		anim.SetBool ("Active", false);
	}
	
	// Update is called once per frame
	void Update () {
		time = Time.time;
		if (Time.time >= 3.5f) {
			anim.SetBool ("Active", true);
		}
		if (Input.GetKeyDown ("space")) {
				anim.SetBool ("Active", true);
		}
	}
	
}
