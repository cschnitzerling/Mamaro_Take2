using UnityEngine;
using System.Collections;

public class Enemy_Projectile : MonoBehaviour {

	public float speed;
	public float destroyTime;
	public int damageAmount;
	public GameObject explosion;

	[Range(0.0f, 1.0f)]
	public float pHitVolume = 1.0f;
	[Range(0.0f, 1.0f)]
	public float explodeVolume = 1.0f;

	private Audio_Manager am;
	private float timerDestroy;
	private Rigidbody rb;

	// Use this for initialization
	void Start () 
	{
		am = Audio_Manager.inst;
		rb = GetComponent<Rigidbody>();
		transform.rotation = Quaternion.LookRotation(Mamaro_Manager.inst.transform.position - transform.position);
	}
	
	// Update is called once per frame
	void Update () 
	{
		rb.AddRelativeForce(Vector3.forward * (speed*100) * Time.deltaTime, ForceMode.Acceleration);

		//LookTowards(Mamaro_Manager.inst.transform.position);

		// destroy after x seconds
		timerDestroy += Time.deltaTime;
		if(timerDestroy > destroyTime)
			OnDestroy();
	}

	/// slerp towards facing target
	public void LookTowards(Vector3 target)
	{
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target - transform.position), 0.25f);
	}

	void OnDestroy()
	{
		Instantiate(explosion, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

	void OnCollisionEnter(Collision otherObj)
	{
		if(otherObj.transform.tag == "Player")
		{
			// play audio at mamaro pos
			am.PlayOneShot(AA.Chr_Robot_Attack_CannonHit_1, pHitVolume);

			// send damage 
			Mamaro_Manager.inst.OnTakeDamage(damageAmount);
		}
		else
		{
			// play audio at collision pos
			am.PlayOneShot(AA.Env_General_ElectricalExplosion, transform.position, explodeVolume);
		}

		OnDestroy();
	}
}
