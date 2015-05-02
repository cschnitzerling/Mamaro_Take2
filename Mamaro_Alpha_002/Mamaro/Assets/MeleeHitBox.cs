using UnityEngine;
using System.Collections;

public class MeleeHitBox : MonoBehaviour 
{
	int damage;


	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
		{
			col.gameObject.SendMessage("OnTakeDamage", damage);
		}

	}

	public void SetDamage(int dam)
	{
		damage = dam;
	}
}
