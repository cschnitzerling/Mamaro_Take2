using UnityEngine;
using System.Collections;

public class Mamaro_LaserShot : MonoBehaviour 
{
	
	public float speed = 5.0f;

	private float timer = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position += transform.forward * speed * Time.deltaTime;//(transform.forward * speed * Time.deltaTime);

		timer += Time.deltaTime;
		if(timer > 10.0f)
			Destroy(gameObject);
	}

	void OnCollisionEnter(Collision otherObj)
	{
		if(otherObj.collider.tag == "Enemy")
		{
			otherObj.gameObject.SendMessage("OnTakeDamage", SendMessageOptions.DontRequireReceiver);
		}

		Audio_Manager.inst.PlayOnce(AA.Env_General_ElectricalExplosion, otherObj.transform.position);
		Destroy(gameObject);
	}
}
