using UnityEngine;
using System.Collections;

public class PressStart_Flicker : MonoBehaviour 
{
	[Range(1, 100)]
	public int OnSpeed = 1;
	[Range(1, 100)]
	public int OffSpeed = 1;
	public GameObject text;

	public float timer;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;

		if(text.activeSelf)
		{
			if(timer > 1.0f - (float)OnSpeed / 100.0f)
			{
				timer = 0.0f;
				
				text.SetActive(!text.activeSelf);
			}
		}
		else
		{
			if(timer > 1.0f - (float)OffSpeed / 100.0f)
			{
				timer = 0.0f;
				
				text.SetActive(!text.activeSelf);	
			}
		}


	}
}
