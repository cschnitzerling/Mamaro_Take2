using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {


	float timer;
	ParticleSystem[] parts;

	void Start()
	{
		parts = gameObject.GetComponentsInChildren<ParticleSystem>();

		for (int i = 0; i < parts.Length; i++) 
		{
			parts[i].startSpeed = parts[i].startSpeed * Ability_Manager.inst.sockets[1].GetCoreCount() + 1;
		}
	}

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (timer > 0.5f)
		{
			for (int i = 0; i < parts.Length; i ++)
			{
				parts[i].Stop();
			}
		}

		if (timer > 5)
		{
			Destroy(gameObject);
		}
	}
}
