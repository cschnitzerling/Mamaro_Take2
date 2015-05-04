using UnityEngine;
using System.Collections;

public class LaserFire : MonoBehaviour {

	public float speed;
	public int damage;

	// audio vars
	private Audio_Manager am;
	[Range(0.0f, 1.0f)]
	private float explodeVolume = 1.0f;

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
		am = Audio_Manager.inst;
		rb.velocity = transform.forward * speed;
	}
	
	void OnTriggerEnter(Collider col)
	{
		Instantiate(explosion,transform.position,transform.rotation);

		if (col.tag == "Enemy")
		{
			col.SendMessage("OnTakeDamage",20);
		}

		am.PlayOneShot(AA.Env_General_physicalExplosion, transform.position, explodeVolume);

		Destroy (gameObject);
	}
}
