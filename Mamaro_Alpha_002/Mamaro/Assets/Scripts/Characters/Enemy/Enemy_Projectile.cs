using UnityEngine;
using System.Collections;

public class Enemy_Projectile : MonoBehaviour {

	public float speed;
	public float destroyTime;
	public int damageAmount;
	public GameObject explosion;
	
	private float timerDestroy;
	private Rigidbody rb;

	// Use this for initialization
	void Start () 
	{
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
		Audio_Manager.inst.PlayOnce(AA.Env_General_PhysicalExpolsion_2, transform.position, 0.5f);
		Destroy(gameObject);
	}

	void OnCollisionEnter(Collision otherObj)
	{
		if(otherObj.transform.tag == "Player")
		{
			Mamaro_Manager.inst.OnTakeDamage(damageAmount);
		}

		OnDestroy();
	}
}
