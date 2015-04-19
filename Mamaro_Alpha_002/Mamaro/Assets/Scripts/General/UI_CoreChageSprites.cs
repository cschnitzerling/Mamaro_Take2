using UnityEngine;
using System.Collections;

public class UI_CoreChageSprites : MonoBehaviour {

	float timer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (timer > 0)
		{
			timer -= Time.deltaTime;
		}
		else 
		{
			gameObject.SetActive(false);
		}


	}

	public void TurnOn (float aliveTime)
	{
		gameObject.SetActive(true);
		timer = aliveTime;
	}
}
