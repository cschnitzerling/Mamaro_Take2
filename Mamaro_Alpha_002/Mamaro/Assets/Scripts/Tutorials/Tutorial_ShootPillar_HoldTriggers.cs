using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Tutorial_ShootPillar_HoldTriggers : MonoBehaviour {
	public GameObject shootPillar;
	public Tutorial_ShootPillar_AnimatorController shootPillarAnim;
	public Script_Destruction_Manager[] childBlowUps = new Script_Destruction_Manager[0];
	// Use this for initialization
	void Start () {
		shootPillar = GameObject.FindGameObjectWithTag ("Tutorial_ShootPillar");
		shootPillarAnim = shootPillar.GetComponent<Tutorial_ShootPillar_AnimatorController> ();
		childBlowUps = GetComponentsInChildren<Script_Destruction_Manager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("1"))
		{
			OnTakeDamage();
	}
	}

	////////////////////////////////////////////
	/// NEED COLLISION CHECK
	///////////////////////////////////////////
	public void OnTakeDamage()
	{
		if (Ability_Manager.inst.GetSocket (Sockets.Ranged).GetCoreCount() > 1)
		{
			gameObject.GetComponent<BoxCollider>().enabled = false;
			for (int i = 0; i < childBlowUps.Length; i ++) {
				childBlowUps [i].BlowUp ();
			}

			shootPillarAnim.Active();
		}
	}
}
