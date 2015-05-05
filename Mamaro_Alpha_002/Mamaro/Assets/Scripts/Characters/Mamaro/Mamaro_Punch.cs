using UnityEngine;
using System.Collections;

public class Mamaro_Punch : MonoBehaviour 
{
	Mamaro_Manager mMan;
	Mamaro_Attack mAtt;
	Ability_Manager ability;
	public bool playSound;
	Audio_Collider_Foley lucyGasp;
	public GameObject colDust;
	// Use this for initialization
	void Start () 
	{
		GameObject colDustGrab = Resources.Load ("Envi_Destructables/Rock_Hit_Particles") as GameObject;
		colDust = colDustGrab;
		lucyGasp = GetComponent<Audio_Collider_Foley> ();
		mMan = Mamaro_Manager.inst;
		mAtt = Mamaro_Attack.inst;
		ability = Ability_Manager.inst;
	}


	void OnTriggerEnter(Collider col)
	{
			if(playSound){
			lucyGasp.lucyGasping = true;
			lucyGasp.HitFoley();
		}
		if (col.tag == "Chunk") {
			Script_Destruction_Chunks derig = col.GetComponent<Script_Destruction_Chunks>();
			derig.Collide();

		}
		int dam = (int)((1 + ability.GetSocket(Sockets.Melee).GetCoreCount()) * mAtt.maxAttack * mAtt.punchCharge) / 100;
		print (dam);
		col.SendMessage("OnTakeDamage",dam,SendMessageOptions.DontRequireReceiver);
	}
	


}
