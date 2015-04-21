using UnityEngine;
using System.Collections;

public class Particle_Destroy : MonoBehaviour 
{
	public float destroyTime = 1.0f;
	private float timer = 0.0f;

	// Update is called once per frame
	void Update () 
	{
		// destroy obj once time expires
		timer += Time.deltaTime;
		if(timer >= destroyTime)
			Destroy(gameObject);
	}
}
