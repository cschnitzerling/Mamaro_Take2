using UnityEngine;
using System.Collections;

public class LaserFire : MonoBehaviour {

	public float speed;
	public int damage;

	public GameObject explosion;
	float timer;

	Rigidbody rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();

		damage *= Ability_Manager.inst.sockets[1].GetCoreCount() + 1;
	}

	// Use this for initialization
	void Start () 
	{
		rb.velocity = transform.forward * speed;
	}
	
	void OnTriggerEnter(Collider col)
	{
		Instantiate(explosion,transform.position,transform.rotation);

		if (col.tag == "Enemy")
		{

			col.SendMessage("OnTakeDamage",20);
		}

		Destroy (gameObject);
	}
}
