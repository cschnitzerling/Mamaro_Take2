using UnityEngine;
using System.Collections;

public class Mamaro_WinScene : MonoBehaviour {

	public Animator anim;
	public bool handAnim;
	public bool armActive;
	public bool endScene = false, walkOff = false;
	public float time;
	public FadeInOut fadeOut;
	// Use this for initialization
	void Awake () {
		//fadeOut = GameObject.FindGameObjectWithTag ("FadeScene").GetComponent<FadeInOut> ();
		anim = GetComponent<Animator> ();
		anim.SetBool ("Active", false);
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetBool ("ArmActive", armActive);
		transform.Translate(0,0,.06f);
		if (walkOff) {
			//transform.Translate(0,0,.06f);
		}
		if (endScene) {
			//fadeOut.nextSceneTwo = true;
		}
		}

	public void ArmActive(){
		anim.SetBool ("ArmActive", true);
	}
}