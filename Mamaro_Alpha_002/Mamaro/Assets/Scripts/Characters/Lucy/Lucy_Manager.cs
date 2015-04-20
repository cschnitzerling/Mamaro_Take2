using UnityEngine;
using System.Collections;

public class Lucy_Manager : MonoBehaviour {

	public enum LucyState{Idle, Repair, Scared, Petrified, Tapping};
	public static Lucy_Manager inst;

	[SerializeField]
	private LucyState state;
	Mamaro_Manager mamaro;
	GameObject lucyTapping;

	[System.NonSerialized]
	public int fear;
	public int fearMax;
	public int fearScaredLevel;
	public int fearFrightenedLevel;

	public float fearDecreaseTime;
	private float timerFearDecrease;

	public float repairDelay;//The time after takeing fear damage untill recharge begins.
	private float timerRepairDelay;//timer for the delay on recharge

	public float repairInterval;//The timer in between when lucy is scared and repairing.
	private float timerRepair;

	public int repairAmountRepair;//The Amount that lucy repairs mamaro
	private int repairAmountScared;//The Amount that lucy repairs mamar While in scared mode

	private bool isRepairing;

	private Vector3 meterVec = Vector3.one;
	public RectTransform meter;
	public Material color;

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
		fear = 50;
		mamaro = Mamaro_Manager.inst;
		Audio_Manager.inst.PlayRecursive(AA.Chr_Lucy_Cry_1, transform.position, "LucyCry");
		Audio_Manager.inst.StopRecursive("LucyCry");
		Audio_Manager.inst.SetVolume("LucyCry", 0.5f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		meterVec.y = ((float)fear / (float)fearMax);
		meter.localScale = meterVec;
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
			if(!Audio_Manager.inst.IsPlaying("LucyCry"))
			{
				Audio_Manager.inst.PlayRecursive("LucyCry");
			}
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
		}                                      //
		////////////////////////////////////////





	}

	//Allows the Fear Meter to be increased or decreased.
	public void OnChangeFear(FearType fearType)
	{
		if (fear <= fearMax && fear >= 0)
		{
			fear += (int)fearType;
		}
		if (fear > fearMax)
		{
			fear = fearMax;
		}
		else if (fear < 0)
		{
			fear = 0;
		}



		if (fear > fearFrightenedLevel)
		{

			ChangeState(LucyState.Petrified);
		}
		else if(fear > fearScaredLevel)
		{
			ChangeState(LucyState.Scared);
		}
		else 
		{
			Audio_Manager.inst.StopRecursive("LucyCry");
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
		if (!mamaro.isMalfunctioning)
		{
			ChangeState(LucyState.Idle);
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
			}

			if (mamaro.health > mamaro.maxHealth)
			{
				mamaro.health = mamaro.maxHealth;
			}
		}
	}
}


































