using UnityEngine;
using System.Collections;

public class Enemy_Projectile : MonoBehaviour {

	public float speed;
	public float destroyTime;


	private float timerDestroy;
	private Rigidbody rb;

	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Acceleration);

		timerDestroy += Time.deltaTime;
		if(timerDestroy > destroyTime)
			Destroy(gameObject);
	}

	void OnCollisionEnter(Collision otherObj)
	{
		if(otherObj.collider.tag == "Player")
		{
			Mamaro_Manager.inst.OnTakeDamage(15);
			Destroy(gameObject);
		}

		Destroy(gameObject);
	}
}
