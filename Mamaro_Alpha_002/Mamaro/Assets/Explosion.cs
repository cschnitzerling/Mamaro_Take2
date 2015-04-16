using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	float timer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (timer > 1)
		{
			ParticleSystem[] temp = gameObject.GetComponentsInChildren<ParticleSystem>();

			for (int i = 0; i < temp.Length; i ++)
			{
				temp[i].Stop ();
			}

		}

		if (timer > 5)
		{
			Destroy(gameObject);
		}
	}
}
