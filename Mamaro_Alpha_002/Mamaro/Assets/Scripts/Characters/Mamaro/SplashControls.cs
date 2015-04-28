using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#
using System.Collections.Generic;

public class SplashControls : MonoBehaviour {

	//###########################################
	//Required For X Input
	bool playerIndexSet = false;
	public PlayerIndex playerIndex;
	public GamePadState state;
	public GamePadState prevState;
	//############################################

	MamaroMovement move;
	Mamaro_Manager mamaro;
	Script_QuickTime QT;
	
	void Start()
	{
		move = MamaroMovement.inst;
		mamaro = Mamaro_Manager.inst;
		QT = Script_QuickTime.inst;
	}

	// Update is called once per frame
	void Update () 
	{
		//#############################################################
		// Find a PlayerIndex, for a single player game
		// From XInput
		if (!playerIndexSet || !prevState.IsConnected)
		{
			for (int i = 0; i < 4; ++i)
			{
				PlayerIndex testPlayerIndex = (PlayerIndex)i;
				GamePadState testState = GamePad.GetState(testPlayerIndex);
				if (testState.IsConnected)
				{
					Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
					playerIndex = testPlayerIndex;
					playerIndexSet = true;
				}
			}
		}
		state = GamePad.GetState(playerIndex);
		//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^


		if (Application.loadedLevel == 0)
		{
			if(prevState.Buttons.Start != ButtonState.Pressed && state.Buttons.Start == ButtonState.Pressed)
				Application.LoadLevel("ComicScene");
		}
		if (Application.loadedLevel == 1)
		{
			if(prevState.Buttons.Start != ButtonState.Pressed && state.Buttons.Start == ButtonState.Pressed)
				Application.LoadLevel("Level");
		}
		if (Application.loadedLevel == 2)
		{
			if(prevState.Buttons.Start != ButtonState.Pressed && state.Buttons.Start == ButtonState.Pressed)
				Application.LoadLevel("StartScreen");
		}
		if (Application.loadedLevel == 5)
		{
			if(prevState.Buttons.Start != ButtonState.Pressed && state.Buttons.Start == ButtonState.Pressed)
				Application.LoadLevel("SplashScreen");
		}



		//###################################
		//Set Previous COntroller State
		prevState = state;
		//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
	}
}
