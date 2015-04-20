using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#
using System.Collections.Generic;

public class Comic_Controller: MonoBehaviour {
	
	public enum ControllerType{Keyboard, Contoller};
	
	public ControllerType InputDevice;

	public Comic_Camera_Move CamMove;

	public static Comic_Controller inst;
	//###########################################
	//Required For X Input
	bool playerIndexSet = false;
	public PlayerIndex playerIndex;
	public GamePadState state;
	public GamePadState prevState;
	//############################################

	
	void Awake()
	{
		CamMove = Camera.main.GetComponent<Comic_Camera_Move> ();
		if (inst == null)
		{
			inst = this;
		}
	}
	
	void Start()
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (InputDevice)
		{
		case ControllerType.Contoller:
			XboxController();
			break;
		case ControllerType.Keyboard:
			KeyboardController();
			break;
		}
	}
	
	void XboxController()
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
		
		


		
		//Dodge Controls
		if (state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released)
		{
			CamMove.NewTarget();
		}


		
		
		
		//Punch Attack
		if (state.Buttons.RightShoulder == ButtonState.Pressed)
		{

		}
		else
		{

		}
		
		//Range Attack
		if (state.Buttons.LeftShoulder == ButtonState.Pressed)
		{

		}
		else
		{

		}
		
		
		
		
		
		
		
		//Adust Ability Cores
		if (state.DPad.Left == ButtonState.Pressed && prevState.DPad.Left == ButtonState.Released)
		{

		}
		else if (state.DPad.Right == ButtonState.Pressed && prevState.DPad.Right == ButtonState.Released)
		{

		}
		
		if (state.DPad.Up == ButtonState.Pressed && prevState.DPad.Up == ButtonState.Released)
		{

		}
		else if (state.DPad.Down == ButtonState.Pressed && prevState.DPad.Down == ButtonState.Released)
		{

		}
		
		
		
		
		
		
		
		//###################################
		//Set Previous COntroller State
		prevState = state;
		//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
	}
	
	void KeyboardController()
	{
		

		
		//Dodge Controls
		if (Input.GetKeyDown(KeyCode.Space))
		{
			CamMove.NewTarget();
		}
		
		// Quick Time controls
			if (Input.GetKeyDown (KeyCode.Space)) {
		}
		
		//Punch Attack
		if (Input.GetKey(KeyCode.E))
		{

		}
		else
		{

		}
		
		//Range Attack
		if (Input.GetKey(KeyCode.Q))
		{

		}
		else
		{

		}

		if(Input.GetKeyDown(KeyCode.Space))
			CamMove.NewTarget();
		
		
		//Adust Ability Cores
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{

		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{

		}
		
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{

		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{

		}
		
		
		//		if (Input.GetKeyDown(KeyCode.LeftArrow))
		//		{
		//			Ability_Manager_Chris_try.inst.SelectSocketLeft();
		//		}
		//		if (Input.GetKeyDown(KeyCode.RightArrow))
		//		{
		//			Ability_Manager_Chris_try.inst.SelectSocketRight();
		//		}
	}
}