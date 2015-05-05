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

		// check for pause
		if(!Game_Manager.inst.isPaused)
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
		else
		{
			MamaroMovement.inst.moveDir = Vector3.zero;
			if(InputDevice == ControllerType.Contoller)
			PauseInput();
		}



		//###################################
		//Set Previous COntroller State
		prevState = state;
		//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
	}

	// allows for user input during the in game pause state
	private void PauseInput()
	{
		// start button input
		if(prevState.Buttons.Start != ButtonState.Pressed && state.Buttons.Start == ButtonState.Pressed)
			Game_Manager.inst.PauseInput(Game_Manager.button.Start);

		// A button input
		if(state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released)
			Game_Manager.inst.PauseInput(Game_Manager.button.A);

		// X button input
		if(state.Buttons.X == ButtonState.Pressed && prevState.Buttons.X == ButtonState.Released)
			Game_Manager.inst.PauseInput(Game_Manager.button.X);

		// Y button input
		if(state.Buttons.Y == ButtonState.Pressed && prevState.Buttons.Y == ButtonState.Released)
			Game_Manager.inst.PauseInput(Game_Manager.button.Y);

		// B button input
		if(state.Buttons.B == ButtonState.Pressed && prevState.Buttons.B == ButtonState.Released)
			Game_Manager.inst.PauseInput(Game_Manager.button.B);
	}

	void XboxController()
	{

		// pause game on press start
		if(prevState.Buttons.Start != ButtonState.Pressed && state.Buttons.Start == ButtonState.Pressed)
		{
			Game_Manager.inst.PauseInput(Game_Manager.button.Start);
		}
		
		//LeftStickMovement
		move.moveDir = Mamaro_Manager.inst.transform.right * state.ThumbSticks.Left.X / 2;
		move.moveDir += Mamaro_Manager.inst.transform.forward * state.ThumbSticks.Left.Y;
		
		//RightStickMovement
		move.rotateEuler.y = state.ThumbSticks.Right.X;


		move.cameraEuler.x = -state.ThumbSticks.Right.Y;


	
		// Quick Time controls
		if(Game_Manager.inst.isMalfunction)
		{
			if(state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released)
				Game_Manager.inst.IncreaseSanity();
		}
		else
		{
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
				if (state.ThumbSticks.Left.X != 0)
				{
					Vector3 tempDir = Camera.main.transform.right * state.ThumbSticks.Left.X;
					tempDir = tempDir.normalized;
					move.Dodge(tempDir);
				}
			}

			//Interact With Cores
			if (fusionCores.Count > 0)
			{
				//PickupCores
				if (state.Buttons.X == ButtonState.Pressed)
				{
					fusionCores[0].DestroyCore();

				}
				else if (state.Buttons.B == ButtonState.Pressed)
				{
					fusionCores[0].CollectCore();
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

		}





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
				Game_Manager.inst.IncreaseSanity();
		}

		////////////////////////////////////////////////
		//Punch Attack
		if (Input.GetKey(KeyCode.E))
		{
			Mamaro_Attack.inst.ButtonDownPunch();
		}
		if (Input.GetKeyUp(KeyCode.E))
		{
			Mamaro_Attack.inst.ButtonUpPunch();
		}

		//Range Attack
		if (Input.GetKey(KeyCode.Q))
		{
			Mamaro_Attack.inst.ButtonDownRange();
		}
		if (Input.GetKeyUp(KeyCode.Q))
		{
			Mamaro_Attack.inst.ButtonUpRange();
		}
		
		//Block
		if (Input.GetKeyDown(KeyCode.Z))
		{
			mamaro.SetBlocking(true);
		}
		if (Input.GetKeyUp(KeyCode.Z))
		{
			mamaro.SetBlocking(false);
		}//////////////////////////////////////////////////////////////////////


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
	}
}
