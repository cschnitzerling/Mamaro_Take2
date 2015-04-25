using UnityEngine;
using System.Collections;

public class Particle_Destroy : MonoBehaviour 
{
	public bool destroy = true;
	public float playSeconds = 1.0f;
	
	private float timer = 0.0f;

	// Update is called once per frame
	void Update () 
	{
		// destroy or diable obj once time expires
		timer += Time.deltaTime;
		if(timer >= playSeconds)
		{
			if(destroy)
				Destroy(gameObject);
			else
				this.gameObject.SetActive(false);

			timer = 0.0f;
		}
	}
}
