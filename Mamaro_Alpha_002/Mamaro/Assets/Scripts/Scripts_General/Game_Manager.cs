using UnityEngine;
using System.Collections;

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
				cam.LerpTo(CamPos.Original);
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
			malMode = true;

			// put cam into position
			cam.LerpTo(CamPos.Malfunction);

		}
		else
		{
			mAttack.enabled = true;
			mMove.enabled = true;
			Lucy.enabled = true;
			abMan.enabled = true; 

			malMode = false;
		}
	}


}
