using UnityEngine;
using System.Collections;

public class Mamaro_Punch : MonoBehaviour 
{
	Mamaro_Manager mMan;
	Mamaro_Attack mAtt;
	Ability_Manager ability;


	// Use this for initialization
	void Start () 
	{
		mMan = Mamaro_Manager.inst;
		mAtt = Mamaro_Attack.inst;
		ability = Ability_Manager.inst;
	}


	void OnTriggerEnter(Collider col)
	{
		int dam = (int)((1 + ability.GetSocket(Sockets.Melee).GetCoreCount()) * mAtt.maxAttack * mAtt.punchCharge) / 100;
		print (dam);
		col.SendMessage("OnTakeDamage",dam,SendMessageOptions.DontRequireReceiver);
	}

}
