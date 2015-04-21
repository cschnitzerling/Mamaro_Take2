using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial_StatSelect : MonoBehaviour {

	public int lesson;

	[SerializeField]
	public List<GameObject> textPrompts;

	public float timerLesson;
	public float lessonTime;

	public bool runTimer;


	// Use this for initialization
	void Start () 
	{
		lesson = 1;
		runTimer = false;
		textPrompts[0].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (runTimer)
		{
			timerLesson += Time.deltaTime;
			if (timerLesson > lessonTime)
			{
				runTimer = false;
				textPrompts[lesson - 1].SetActive(false);
				if (lesson < textPrompts.Count)
					textPrompts[lesson].SetActive(true);
				timerLesson = 0;
				lesson ++;
			}
		}

		switch (lesson)
		{
		case 1:
			//Intro
			runTimer = true;
			break;
		case 2:
			//Select Stat
			runTimer = true;
			break;
		case 3:
			//Pressing up or down to open the socket.
			runTimer = true;
			break;
		case 4:
			//Press up or down to add or remove.
			runTimer = true;
			break;
		case 7:
			Destroy (gameObject);
			break;
		}
	}

}
