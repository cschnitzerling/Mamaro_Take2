using UnityEngine;
using System.Collections;

public class LucyMeterFlash : MonoBehaviour {

	public float timeOn;

	float timer;

	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;

		if (timer > timeOn)
		{
			timer = 0;
			gameObject.SetActive(false);
		}



	}
}
