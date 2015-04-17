using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	//Malfunction Vars
	public float buttonAdd;
	public float malPercent;
	public float malAmount;

	public GameObject arm;
	public GameObject startPos;
	public GameObject endPos;


	void Awake()
	{
		// assign static instance
		if(inst == null)
			inst = this;
	}

	void Start()
	{
		// link static vars
		mAttack = Mamaro_Attack.inst;
		mMove = MamaroMovement.inst;
		Lucy = Lucy_Manager.inst;
		abMan = Ability_Manager.inst;
		cam = Cam_Manager.inst;

		scripts = FindObjectsOfType<MonoBehaviour>();
	}

	public void IncreaseSanity()
	{
		print (buttonAdd - ((float)Mamaro_Manager.inst.GetTotalCores() / 2));
		malPercent -= buttonAdd - ((float)Mamaro_Manager.inst.GetTotalCores() / 2);
	}

	void Update()
	{
		// check for malMode
		if(isMalfunction)
		{


			//TODO add cam effect

			// start counting
			Timer_mal += Time.deltaTime;

			malPercent += malAmount * Time.deltaTime;

			Vector3 pos = Vector3.Lerp(startPos.transform.position, endPos.transform.position, (malPercent / 100));
			Quaternion rot =  Quaternion.Lerp(startPos.transform.rotation, endPos.transform.rotation, (malPercent / 100));

			arm.transform.position = pos;
			arm.transform.rotation = rot;

			// check for complete
			if(Timer_mal >= malfunctionSecs)
			{
				Timer_mal = 0.0f;
				MalfunctionMode(false);
			}
		}
	}

	/// Switches scripts on or off
	public void MalfunctionMode(bool on)
	{
		if(on)
		{
			// turn off relevant scripts
			mAttack.enabled = false;
			mMove.enabled = false;
			Lucy.enabled = false;
			abMan.enabled = false;

			// set up malMode
			Timer_mal = 0.0f;
			isMalfunction = true;

			// put cam into position
			cam.LerpTo(CamPos.Malfunction);

		}
		else
		{

			ToggleMulfulction(false);
			mAttack.enabled = true;
			mMove.enabled = true;
			Lucy.enabled = true;
			abMan.enabled = true; 

			isMalfunction = false;
			cam.LerpTo(CamPos.Original);
		}
	}
	
	/// switches off all scripts but leaves on the ones passed in. if parameter if left empty
	/// all scripts will be switched back on.
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
				if(script.enabled == true)
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

//	public bool CheckEnableList(string name)
//	{
//		for (int i = 0; i < leftEnabled.Length; i ++)
//		{
//			if (name == leftEnabled[i])
//			{
//				return true;
//			}
//		}
//
//		return false;
//
//	}

	public void ToggleMulfulction (bool toEnable)
	{
		malfunction.SetActive(toEnable);

		if (toEnable)
		{
			EnableScripts (false);
			malPercent = 0;
		}
		else
		{
			EnableScripts(true);
		}

	}
	



}
