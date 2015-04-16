using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial_Movement : MonoBehaviour {

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
			//Left Stiick Movement.

			if (MamaroController.inst.state.ThumbSticks.Left.X != 0 || MamaroController.inst.state.ThumbSticks.Left.Y != 0 )
			{
				runTimer = true;
			}
			break;
		case 2:
			//Left Stiick Movement.
			if (MamaroController.inst.state.ThumbSticks.Right.X != 0)
			{
				runTimer = true;
			}
			break;
		case 3:
			if (MamaroController.inst.state.Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed)
			{
			runTimer = true;
			}
			break;
		case 4:
			runTimer = true;
			break;
		case 5:
			Destroy (gameObject);
			break;
		}
	}

}
