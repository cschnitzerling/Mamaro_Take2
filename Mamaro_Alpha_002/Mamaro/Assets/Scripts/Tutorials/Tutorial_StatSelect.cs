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
				lessonTime = 6;
				runTimer = true;
			break;
		case 2:
			//Select Stat
			if (MamaroController.inst.state.DPad.Left == XInputDotNetPure.ButtonState.Pressed || MamaroController.inst.state.DPad.Right == XInputDotNetPure.ButtonState.Pressed)
			{
				lessonTime = 3;
				runTimer = true;
			}
			break;
		case 3:
			//Pressing up or down to open the socket.
			if (MamaroController.inst.state.DPad.Up == XInputDotNetPure.ButtonState.Pressed || MamaroController.inst.state.DPad.Down == XInputDotNetPure.ButtonState.Pressed)
			{
				lessonTime = 3;
				runTimer = true;
			}
			break;
		case 4:
			//Press up or down to add or remove.
			if (MamaroController.inst.state.DPad.Up == XInputDotNetPure.ButtonState.Pressed || MamaroController.inst.state.DPad.Down == XInputDotNetPure.ButtonState.Pressed)
			{
				lessonTime = 3;
				runTimer = true;
			}
			break;
		case 5:
			//try attacking the rock  with more power alocated to the melee
			lessonTime = 6;
			runTimer = true;
			break;
		case 6:
			//If no spare sockets are available it will take power from the opposite skill
			lessonTime = 6;
			runTimer = true;
			break;
		case 7:
			Destroy (gameObject);
			break;
		}
	}

}
