using UnityEngine;
using System.Collections;

public class MeleeHitBox : MonoBehaviour 
{
	int damage;
	public AudioClip hitSound;
	public bool playSound;
	void OnTriggerEnter(Collider col)
	{
		if(playSound){
			AudioSource.PlayClipAtPoint (hitSound, Camera.main.transform.position, 1f);
		}

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
