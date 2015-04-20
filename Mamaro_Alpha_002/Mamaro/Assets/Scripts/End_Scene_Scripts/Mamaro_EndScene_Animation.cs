using UnityEngine;
using System.Collections;

public class Mamaro_EndScene_Animation : MonoBehaviour {
	public Animator anim;
	public bool handMarmActive, handMarmEnded;
	public Mamaro_Hand_Animation handAnim;
	// Use this for initialization
	void Awake () {
		handAnim = GetComponentInChildren<Mamaro_Hand_Animation> ();
		anim = GetComponent<Animator> ();
		anim.SetBool ("Active", false);
	}
	
	// Update is called once per frame
	void Update () {
		handMarmActive = handAnim.handActive;
		handMarmEnded = handAnim.handEnd;
		if (anim.GetBool ("Active") == true) {
			transform.Translate(0,0,.08f);
		}
		if (Input.GetKeyDown ("space")) {
			if(handMarmActive){
			anim.SetBool ("Active", true);
			}
		}
	}

	public void HandActive()
	{
	}
}
