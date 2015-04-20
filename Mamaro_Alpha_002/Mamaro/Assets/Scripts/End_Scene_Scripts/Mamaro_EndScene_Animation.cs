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
	}
	
	// Update is called once per frame
	void Update () {
		handMarmActive = handAnim.handActive;
		handMarmEnded = handAnim.handEnd;

		anim.SetBool ("HandActive", handMarmActive);

		if (anim.GetBool ("Active") == true) {
			transform.Translate(0,0,.06f);
		}

			if(handMarmEnded){
			anim.SetBool ("Active", true);
		}
	}

	public void HandActive()
	{
	}
}
