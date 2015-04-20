using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#
using System.Collections.Generic;

public class MamaroController : MonoBehaviour {

	public enum ControllerType{Keyboard, Contoller};

	public ControllerType InputDevice;

	public static MamaroController inst;
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

	public List<FusionCore> fusionCores;

	void Awake()
	{
		if (inst == null)
		{
			inst = this;
		}
	}

	void Start()
	{
		move = MamaroMovement.inst;
		mamaro = Mamaro_Manager.inst;
		QT = Script_QuickTime.inst;
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
		
		
		//LeftStickMovement
		move.moveDir = Mamaro_Manager.inst.transform.right * state.ThumbSticks.Left.X / 2;
		move.moveDir += Mamaro_Manager.inst.transform.forward * state.ThumbSticks.Left.Y;
		
		//RightStickMovement
		move.rotateEuler.y = state.ThumbSticks.Right.X;

	

		//LeftStick Clicked
		//if moveing and not run for to long
		if (move.moveDir.magnitude > 0 && move.timerRun < move.runMaxTime)
		{
			//LeftStick pressed and was not previously pressed
			if (state.Buttons.LeftStick == ButtonState.Pressed && prevState.Buttons.LeftStick == ButtonState.Released)
			{
				move.isRun = !move.isRun;
			}
		}
		else
		{
			move.isRun = false;
		}
		
		//Dodge Controls
		if (state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released && move.moveDir.magnitude > 0)
		{
			Vector3 tempDir;
			tempDir = move.moveDir;
			tempDir = tempDir.normalized;
			move.Dodge(tempDir);
		}

		// Quick Time controls
		if(Game_Manager.inst.isMalfunction)
		{
			if(state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released)
				Game_Manager.inst.IncreaseSanity();
		}


		//Interact With Cores
		if (fusionCores.Count > 0)
		{
			//PickupCores
			if (state.Buttons.X == ButtonState.Pressed)
			{
				fusionCores[0].DestroyCore();

			}
			else if (state.Buttons.Y == ButtonState.Pressed)
			{
				fusionCores[0].CollectCore();
				print ("Y");
			}
		}

		//Punch Attack
		if (state.Triggers.Right > 0)
		{
			Mamaro_Attack.inst.ButtonDownPunch();
		}
		if (state.Triggers.Right == 0 && prevState.Triggers.Right > 0)
		{
			Mamaro_Attack.inst.ButtonUpPunch();
		}

		//Range Attack
		if (state.Triggers.Left > 0)
		{
			Mamaro_Attack.inst.ButtonDownRange();
		}
		if (state.Triggers.Left == 0 && prevState.Triggers.Left > 0)
		{
			Mamaro_Attack.inst.ButtonUpRange();
		}

		//Block
		if (state.Buttons.LeftShoulder == ButtonState.Pressed && prevState.Buttons.LeftShoulder == ButtonState.Released)
		{
			mamaro.SetBlocking(true);
		}
		if (state.Buttons.LeftShoulder == ButtonState.Released && prevState.Buttons.LeftShoulder == ButtonState.Pressed)
		{
			mamaro.SetBlocking(false);
		}






		//Adust Ability Cores
		if (state.DPad.Left == ButtonState.Pressed && prevState.DPad.Left == ButtonState.Released)
		{
			Ability_Manager.inst.SelectSocketLeft();
		}
		else if (state.DPad.Right == ButtonState.Pressed && prevState.DPad.Right == ButtonState.Released)
		{
			Ability_Manager.inst.SelectSocketRight();
		}

		if (state.DPad.Up == ButtonState.Pressed && prevState.DPad.Up == ButtonState.Released)
		{
			Ability_Manager.inst.SocketAdd();
		}
		else if (state.DPad.Down == ButtonState.Pressed && prevState.DPad.Down == ButtonState.Released)
		{
			Ability_Manager.inst.SocketRemove();
		}







		//###################################
		//Set Previous COntroller State
		prevState = state;
		//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
	}

	void KeyboardController()
	{
		//LeftStickMovement
		move.moveDir = transform.right * Input.GetAxis("Horizontal") / 2;
		move.moveDir += transform.forward * Input.GetAxis("Vertical");
		
		//RightStickMovement
		move.rotateEuler.y = Input.GetAxis ("Mouse X");
		
		//LeftStick Clicked
		//if moveing and not run for to long
		if (move.moveDir.magnitude > 0 && move.timerRun < move.runMaxTime)
		{
			//LeftStick pressed and was not previously pressed
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				move.isRun = !move.isRun;
			}
		}
		else
		{
			move.isRun = false;
		}
		
		//Dodge Controls
		if (Input.GetKeyDown(KeyCode.Space) && move.moveDir.magnitude > 0)
		{
			Vector3 tempDir;
			tempDir = move.moveDir;
			tempDir = tempDir.normalized;
			move.Dodge(tempDir);
		}

		// Quick Time controls
		if(mamaro.isMalfunctioning)
		{
			if(Input.GetKeyDown(KeyCode.Space))
				QT.Resist();
		}

		//Punch Attack
		if (Input.GetKey(KeyCode.E) && !Mamaro_Attack.inst.isChargeRange)
		{
			Mamaro_Attack.inst.isChargePunch = true;
		}
		else
		{
			Mamaro_Attack.inst.isChargePunch = false;
		}
		
		//Range Attack
		if (Input.GetKey(KeyCode.Q) && !Mamaro_Attack.inst.isChargePunch)
		{
			Mamaro_Attack.inst.isChargeRange = true;
		}
		else
		{
			Mamaro_Attack.inst.isChargeRange = false;
		}

		//Interact With Cores
		if (fusionCores.Count > 0)
		{
			//PickupCores
			if (Input.GetKey(KeyCode.X))
			{
				fusionCores[0].DestroyCore();
				
			}
			else if (Input.GetKey(KeyCode.Y))
			{
				fusionCores[0].CollectCore();
			}
		}


		//Adust Ability Cores
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			Ability_Manager.inst.SelectSocketLeft();
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			Ability_Manager.inst.SelectSocketRight();
		}
		
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			Ability_Manager.inst.SocketAdd();
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			Ability_Manager.inst.SocketRemove();
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
