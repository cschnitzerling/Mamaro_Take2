﻿using UnityEngine;
using System.Collections;

public class Lucy_Manager : MonoBehaviour {

	public enum LucyState{Idle, Repair, Scared, Petrified, Tapping};
	public static Lucy_Manager inst;

	[SerializeField]
	private LucyState state;
	Mamaro_Manager mamaro;
	GameObject lucyTapping;

	public int fear;
	public int fearMax;
	private float fearScaredLevel;
	private float fearFrightenedLevel;

	public float fearDecreaseTime;
	private float timerFearDecrease;
	
	// fear bar vars
	public GameObject upgradeParticle;
	private int barDivide = 4;
	public RectTransform fearBar;
	public float maxBarY;
	public RectTransform meter;
	public float maxMeterY;
	public Vector3 meterVec = Vector3.one;
	public Material color;
	public ParticleSystem particle;
	private float partStartSpeed;

	// audio vars
	private Audio_Manager am;
	[Range(0.0f, 1.0f)]
	public float cryVolume = 1.0f;
	private string keyCry = "Cry";


	public float repairDelay;//The time after taking fear damage untill recharge begins.
	private float timerRepairDelay;//timer for the delay on recharge

	public float repairInterval;//The timer in between when lucy is scared and repairing.
	private float timerRepair;

	public int repairAmountRepair;//The Amount that lucy repairs mamaro
	private int repairAmountScared;//The Amount that lucy repairs mamar While in scared mode

	private bool isRepairing;
	
	public GameObject lucyIncFear;
	public GameObject lucy;
	public GameObject lucyFixEffect;
	Animator anim;
	
	void Awake()
	{
		if (inst == null)
		{
			inst = this;
		}
	}

	// Use this for initialization
	void Start () 
	{
		// set the fear/meter bar largest Y scale
		maxBarY = fearBar.rect.height;
		maxMeterY = meter.rect.height;
		fearBar.sizeDelta = new Vector2(fearBar.rect.width, maxBarY / barDivide);

		partStartSpeed = particle.startSpeed;

		fear = 0; 

		anim = lucy.GetComponent<Animator>();
		mamaro = Mamaro_Manager.inst;

		am = Audio_Manager.inst;

	}
	
	// Update is called once per frame
	void Update () 
	{
		// keep audio source with camera
		//Audio_Manager.inst.SetRecursivePos("LucyCry", Camera.main.transform.position);

		// scale fear bar to current divide
		fearMax = (int)maxMeterY / barDivide;

		// scale fear thresholds
		fearScaredLevel = (fearMax / barDivide) * 0.4f;		// hard coded //////////////////////
		fearFrightenedLevel = (fearMax / barDivide) * 0.7f; // hard coded //////////////////////

		// scale bar size in respects to divide value
		fearBar.sizeDelta = new Vector2(fearBar.rect.width, Mathf.Lerp(fearBar.rect.height, maxBarY / barDivide, 0.075f));

		// scale meter in respects to fear value
		meterVec.y = ((float)fear / (float)fearMax);
		meter.localScale = meterVec;


		// reduce fear faster when blocking
		if(mamaro.isBlocking)
		{
			OnChangeFear(FearType.Decrease);
			//TODO apply some pretty particles indicating faster reduction.
		}


		if (fear > 0)
		{
			timerFearDecrease += Time.deltaTime;
			if (timerFearDecrease > fearDecreaseTime)
			{
				OnChangeFear(FearType.Decrease);
				timerFearDecrease = 0;
			}
			if (fear < 0)
			{
				fear = 0;
			}
		}

		//Check to see if mamaro is in malfuntioning quicktime event.
		if (mamaro.isMalfunctioning)
		{
			ChangeState(LucyState.Tapping);
		}

		//Switch for each of Lucy's States.
		switch (state)
		{
		case LucyState.Idle:
			Idle ();
			break;
		case LucyState.Repair:
			Repair();
			break;
		case LucyState.Scared:
			Scared();
			break;
		case LucyState.Petrified:
			Petrified ();
			break;
		case LucyState.Tapping:
			Tapping ();
			break;
		}

		///////////////////////////////////////////////////
		/// Input Hacks                                 //
		///                                            //
		if (Input.GetKeyDown(KeyCode.P))              //
		{                                            //
			OnChangeFear(FearType.Damage);          //
		}                                          //
		if (Input.GetKeyDown(KeyCode.O))          //
		{                                        //
			mamaro.health -= 20;                //
		}
		if(Input.GetKeyDown(KeyCode.F12))
			UpgradeFear();
		////////////////////////////////////////





	}

	//Allows the Fear Meter to be increased or decreased.
	public void OnChangeFear(FearType fearType)
	{
		if ((int)fearType > 0)
		{
			//lucyIncFear.SetActive(true);
		}

		if (fear <= fearMax / barDivide && fear >= 0)
		{
			// bar reduces relative to the bar size
			if(fearType == FearType.Decrease)
				fear += (int)fearType * (5 - barDivide);
			else
				fear += (int)fearType;
		}

		if (fear > fearMax / barDivide)
		{
			fear = fearMax / barDivide;
		}
		else if (fear < 0)
		{
			fear = 0;
		}



		if (fear > fearFrightenedLevel)
		{
			anim.SetBool("Bool_Scared", true);
			anim.SetBool("Bool_Repair", false);

			//Audio_Manager.inst.PlayRecursive("LucyCry");

			ChangeState(LucyState.Petrified);
		}
		else if(fear > fearScaredLevel)
		{
			anim.SetBool("Bool_Scared", false);
			anim.SetBool("Bool_Repair", true);
			//Audio_Manager.inst.StopRecursive("LucyCry");
			ChangeState(LucyState.Scared);
		}
		else 
		{
			anim.SetBool("Bool_Scared", false);
			anim.SetBool("Bool_Repair", false);
			//Audio_Manager.inst.StopRecursive("LucyCry");
			ChangeState(LucyState.Idle);
		}
	}

	/// <summary>
	/// Lucy's Idle State
	/// </summary>
	// Allows Lucy to Run Idle Animation
	// Change to Repair if Mamaro looses health
	// Play Lucy Audio
	void Idle()
	{
		anim.SetBool("Bool_Scared", false);
		anim.SetBool("Bool_Repair", false);
		if (mamaro.health < mamaro.maxHealth)
		{
			ChangeState(LucyState.Repair);
		}
	}

	/// <summary>
	/// Lucy's Repair State
	/// </summary>
	/// Plays Repair Animation
	/// Slowly Repairs Mamaro
	void Repair()
	{
		anim.SetBool("Bool_Scared", false);
		anim.SetBool("Bool_Repair", true);
		if (mamaro.health == mamaro.maxHealth)
		{
			ChangeState(LucyState.Idle);
		}

		RepairMamaro(repairAmountRepair);
	}

	/// <summary>
	/// Scared this instance.
	/// </summary>
	/// Repairs Mamaro Intermitently
	/// Swaps between repai animaion and Scared Animation.
	void Scared()
	{
		RepairMamaro(repairAmountScared);
	}

	/// <summary>
	/// Lucy's Petrified State
	/// </summary>
	/// Plays Petrified Animation
	/// will not repair mamaro
	void Petrified()
	{

	}

	/// <summary>
	/// For Use when entering Maamaros Quick time event
	/// </summary>
	/// Activates Lucy's Tapping Gameobject on mamaros eye lens
	void Tapping()
	{
		//if(!Audio_Manager.inst.IsPlaying("LucyCry") && !Game_Manager.inst.coreDestroyed)
		{
			//Audio_Manager.inst.PlayRecursive("LucyCry");
		}
//		else
		{
			//TODO play laughing
		}

		if (!mamaro.isMalfunctioning)
		{
			ChangeState(LucyState.Idle);
			//Audio_Manager.inst.StopRecursive("LucyCry");
		}
	}

	/// <summary>
	/// Changes the state.
	/// </summary>
	/// <param name="lState">lState.</param>
	void ChangeState(LucyState lState)
	{
		switch (lState)
		{
		case LucyState.Idle:
			color.color = Color.blue;
			break;
		case LucyState.Repair:
			color.color = Color.blue;
			break;
		case LucyState.Scared:
			color.color = Color.yellow;
			break;
		case LucyState.Petrified:
			color.color = Color.red;
			break;
		case LucyState.Tapping:
			break;
		}
		state = lState;
	}


	/// <summary>
	/// Modifies mamaros health
	/// </summary>
	/// <param name="amount">Amount.</param>
	void RepairMamaro(int amount)
	{
		timerRepair += Time.deltaTime;
		
		//Reparing
		if (timerRepair > repairInterval)
		{
			if (mamaro.health < mamaro.maxHealth)
			{
				mamaro.health += amount;
				timerRepair -= repairInterval;

				// show effect each fix
				lucyFixEffect.SetActive(true);
			}

			if (mamaro.health > mamaro.maxHealth)
			{
				mamaro.health = mamaro.maxHealth;
			}
		}
	}
	
	/// sequence when I core was destroyed
	public void UpgradeFear()
	{
		// reset fear level
		fear = 0;
		
		// reduce barDivide (no lower than 1)
		if(barDivide > 1)
			barDivide--;

		// speed scaled in respects to divide
		particle.startSpeed = partStartSpeed / barDivide;

		// switch on particles
		upgradeParticle.SetActive(true);
	}
}




