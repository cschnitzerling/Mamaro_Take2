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
	public bool malMode = false;
	public float Timer_mal = 0.0f;

	// class var : assign in start()
	private MonoBehaviour[] scripts;
	// public inpector assigned var
	public MonoBehaviour[] leftEnabled;

	public GameObject malfunction;

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

	void Update()
	{
		// check for malMode
		if(malMode)
		{


			//TODO add cam effect

			// start counting
			Timer_mal += Time.deltaTime;

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
			//camera lerp

			//turn on malfunction gameobject







			// turn off relevant scripts
			mAttack.enabled = false;
			mMove.enabled = false;
			Lucy.enabled = false;
			abMan.enabled = false;

			// set up malMode
			Timer_mal = 0.0f;
			malMode = true;

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

			malMode = false;
			cam.LerpTo(CamPos.Original);
		}
	}
	
	/// switches off all scripts but leaves on the ones passed in. if parameter if left empty
	/// all scripts will be switched back on.
	private List<MonoBehaviour> disabledScripts = new List<MonoBehaviour>();
	private void EnableScripts(MonoBehaviour[] enabled = null)
	{
		// switch all enabled off
		foreach(MonoBehaviour script in scripts)
		{
			// only switch off active scripts
			if(script.enabled == true)
			{
				script.enabled = false;
				disabledScripts.Add(script);
			}
		}
		
		// disable all but listed
		if(enabled != null)
		{
			foreach(MonoBehaviour script in enabled)
			{
				script.enabled = true;
			}
		}
		else
		{
			// enable all the disabled scripts
			foreach(MonoBehaviour script in disabledScripts)
			{
				script.enabled = true;
			}
			
			// empty disabled list
			disabledScripts.Clear();
		}
	}

	public void ToggleMulfulction (bool toEnable)
	{
		malfunction.SetActive(toEnable);
	}
	



}
