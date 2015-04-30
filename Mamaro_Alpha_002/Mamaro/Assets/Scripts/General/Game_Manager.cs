using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour 
{
	// static instance
	public static Game_Manager inst;

	// static access
	private Mamaro_Attack mAttack;
	private MamaroMovement mMove;
	private Lucy_Manager Lucy;
	private Ability_Manager abMan;
	private Cam_Manager cam;

	public bool coreDestroyed = false;

	public bool isPaused = false;

	// inspector assigned vars
	public float malfunctionSecs;

	// private vars
	public bool isMalfunction = false;
	public float Timer_mal = 0.0f;

	// class var : assign in start()
	public MonoBehaviour[] scripts;

	// public inpector assigned var
	public string[] leftEnabled;

	public GameObject malfunction;
	public GameObject destroyCoreScreen;

	public SpriteRenderer malFuncSprite;

	//Malfunction Vars
	public float buttonAdd;
	public float malPercent;
	public float malAmount;

	public GameObject arm;
	public GameObject startPos;
	public GameObject endPos;


	public Vector3 resetPos;
	public Quaternion resetRot;
	//public Transform resetPos;



	void Awake()
	{
		// assign static instance
		if(inst == null)
			inst = this;
	}

	void Start()
	{

		//Set Time Scale
		Time.timeScale = 1;
		// link static vars
		mAttack = Mamaro_Attack.inst;
		mMove = MamaroMovement.inst;
		Lucy = Lucy_Manager.inst;
		abMan = Ability_Manager.inst;
		cam = Cam_Manager.inst;

		resetPos = Mamaro_Manager.inst.gameObject.transform.position;
		resetRot = Mamaro_Manager.inst.gameObject.transform.rotation;
		scripts = FindObjectsOfType<MonoBehaviour>();
	}

	public void IncreaseSanity()
	{
		float amount = buttonAdd - ((float)Mamaro_Manager.inst.GetTotalCores() / 2);

		// limit negative value
		if (malPercent - amount < 0.0f)
			malPercent = 0.0f;
		else
			malPercent -= amount;
	}

	void Update()
	{

		
		// to quit the game
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.LoadLevel("StartScreen");
		}

		// check for malMode
		if(isMalfunction)
		{

			// start counting
			Timer_mal += Time.deltaTime;

			if(!coreDestroyed)
			{
				cam.ShakeCam(Shake.Small);

				malPercent += malAmount * Time.deltaTime;
				
				// Move arm towards Lucy
				Vector3 pos = Vector3.Lerp(startPos.transform.position, endPos.transform.position, (malPercent / 100));
				Quaternion rot =  Quaternion.Lerp(startPos.transform.rotation, endPos.transform.rotation, (malPercent / 100));
				arm.transform.position = pos;
				arm.transform.rotation = rot;
				
				float percent = (float)malPercent / 100f;
				
				Color tempColour = malFuncSprite.color;
				
				tempColour.a = ((malPercent * 0.01f));
				Vector3 tempScale = new Vector3 (1 - (percent /2),1 - (percent /2),1 - (percent /2));
				malFuncSprite.gameObject.transform.localScale = tempScale;
				malFuncSprite.color = tempColour;

				// player resisted the malfunction
				if(Timer_mal >= malfunctionSecs)
				{
					Timer_mal = 0.0f;
					MalfunctionMode(false);
				}

				// game over
				if(malPercent >= 100.0f)
				{
					Application.LoadLevel("EndScene");
				}
			}
			else
			{
				if(Timer_mal >= 2.5f)
				{
					Timer_mal = 0.0f;
					MalfunctionMode(false);
				}
			}
		}
	}

	/// Puts player back at the start pos
	public void ResetPlayer()
	{
		print("hit");
		Mamaro_Manager.inst.gameObject.transform.position = resetPos;
		Mamaro_Manager.inst.gameObject.transform.rotation = resetRot;
	}

	/// Switches scripts on or off
	public void MalfunctionMode(bool on)
	{
		if(on)
		{
			isMalfunction = true;
			// set up malMode
			Timer_mal = 0.0f;
			//isMalfunction = true;

			// put cam into position
			cam.LerpTo(CamPos.Malfunction);
		}
		else
		{
			malPercent = 0.0f;
			ToggleMulfulction(false);
			isMalfunction = false;
			cam.LerpTo(CamPos.Original);
		}
	}
	
	/// switches off all relavant scripts in respects to malfunction state
	private List<MonoBehaviour> disabledScripts = new List<MonoBehaviour>();
	private void EnableScripts(bool turnOn)
	{
		// disable all but listed
		if(turnOn)
		{
			// enable all the disabled scripts
			foreach(MonoBehaviour script in disabledScripts)
			{
				script.enabled = true;
			}
			
			// empty disabled list
			disabledScripts.Clear();
		}
		else
		{	// switch all enabled off
			foreach(MonoBehaviour script in scripts)
			{
				// only switch off active scripts
				if(script != null && script.enabled == true)
				{
					//print (script.name);
					if (script.name != "GAME_MANAGER")
					{
						script.enabled = false;
						disabledScripts.Add(script);
					}
				}
			}
		}
	}
	
	public void ToggleMulfulction (bool toEnable)
	{
		if(!coreDestroyed)
		{
			arm.transform.position = startPos.transform.position;
			arm.transform.rotation = startPos.transform.rotation;
			malPercent = 0.0f;
			malfunction.SetActive(toEnable);
		}
		else
		{
			destroyCoreScreen.SetActive(toEnable);
		}

		isMalfunction = true;
		EnableScripts(!toEnable);
	}

	public enum button {A, X, Y, B, Start};
	public GameObject pauseScreen;
	public GameObject buttonLayout;
	public GameObject areYouSure;
	public Vector3 pausePos;

	// handles user input during the pause state
	public void PauseInput(button input)
	{
		switch (input) 
		{
		case button.A:

			// check in first screen
			if(!buttonLayout.activeSelf && !areYouSure.activeSelf)
				isPaused = false;

			// allow quit in are you sure screen
			if(areYouSure.activeSelf)
				Application.LoadLevel("SplashScreen");

			break;

		case button.B:

			// turn off are you sure image
			if(areYouSure.activeSelf)
				areYouSure.SetActive(false);				

			// show are you sure image
			else if(!buttonLayout.activeSelf && !areYouSure.activeSelf)
				areYouSure.SetActive(true);

			// turn off button layou image
			else if(buttonLayout.activeSelf)
				buttonLayout.SetActive(false);

			break;

		case button.X:

			// check in first screen
			if(!areYouSure.activeSelf && !buttonLayout.activeSelf)
				Application.LoadLevel("Level");

			break;

		case button.Y:

			// show button layout image
			if(!areYouSure.activeSelf && !buttonLayout.activeSelf)
				buttonLayout.SetActive(true);

			break;

		case button.Start:

			if (!isPaused)
			{
				//Puase the Game
				Time.timeScale = 0.0f;
				pauseScreen.SetActive(true);
				isPaused = true;
//				pausePos = Mamaro_Manager.inst.gameObject.transform.position;
				//MamaroMovement.inst.gameObject.GetComponent<Rigidbody>().isKinematic = true;
				//MamaroMovement.inst.moveDir = Vector3.zero;
			}
			else 
			{
				//UnPause
				// turn off all images before exiting state
				Time.timeScale = 1.0f;
				buttonLayout.SetActive(false);
				areYouSure.SetActive(false);
				pauseScreen.SetActive(false);
				isPaused = false;
//				MamaroMovement.inst.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
//				Mamaro_Manager.inst.gameObject.transform.position = pausePos;
//				MamaroMovement.inst.gameObject.GetComponent<Rigidbody>().isKinematic = false;
				//MamaroMovement.inst.moveDir = Vector3.zero;
			}


			break;

		default:
			Debug.Log("Switch fell through. Check you shiz!");
			break;
		}
	}

















}
