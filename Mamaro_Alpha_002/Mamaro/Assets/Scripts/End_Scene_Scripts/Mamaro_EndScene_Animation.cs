using UnityEngine;
using System.Collections;

public class Mamaro_EndScene_Animation : MonoBehaviour {
	public Animator anim;
	public Mamaro_Hand_Animation handAnim;
	public float roundTimeLeft, roundTimeSeconds, startTime;
	public bool walk;
	// Use this for initialization
	void Awake () {
		handAnim = GetComponentInChildren<Mamaro_Hand_Animation> ();
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		roundTimeLeft = Time.time - startTime;

		if (roundTimeLeft >= roundTimeSeconds)
		{
			anim.SetBool ("Active", true);
			startTime = Time.time;
			roundTimeLeft = 0;
		}
		if (anim.GetBool ("Walk") == true) {
			walk = true;
		}
		if (walk) {
			Walk();
		}


	}

	public void Walk()
	{
		anim.SetBool ("Walk", true);
		transform.Translate(0,0,.06f);
	}
}
