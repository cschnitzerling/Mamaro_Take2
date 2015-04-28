using UnityEngine;
using System.Collections;

public class Mamaro_Manager : MonoBehaviour 
{
	// static instance
	public static Mamaro_Manager inst;

	// static class access
	private Game_Manager GM;
	private Ability_Manager abMan;
	private Cam_Manager cam;
	private Lucy_Manager lucy;

	// inspector assigned vars
	public int maxHealth = 100;
	public int smallDamage, mediumDamage, largeDamage;	// in regards to how much cam shake to apply
	public Vector3 camMalfuncPos;
	public GameObject coreUpgradeParticles;

	// non-inspector assigned vars
	//[HideInInspector]
	public int health;
	public bool isMalfunctioning = false;
	public bool isBlocking = false;

	// private vars
	public int meleeCores, speedCores, rangedCores, shieldCores;

	Animator anim;

	void Awake()
	{
		if (inst == null)
			inst = this;
		anim = GetComponentInChildren<Animator>();
	}

	// Use this for initialization
	void Start () 
	{
		GM = Game_Manager.inst;
		abMan = Ability_Manager.inst;
		cam = Cam_Manager.inst;
		health = maxHealth;
		lucy = Lucy_Manager.inst;

		//To RemoveWArning ############################################################
		int i = (int)lucy.fear;													  //###
		i ++;																	  //###
		// ############################################################################
	}
	
	// Update is called once per frame
	void Update () 
	{
		// update local ability vars from abMan
		UpdateCores();

		////////////////////////////////////////////
		/// Testing damage function	///////////////
		//////////////////////////////////////////
		if(Input.GetKeyDown(KeyCode.F4))	/////
		{								   /////
			//int testDamage = largeDamage; /////
			OnTakeDamage(20);
			//OnCorePickUp();
		}							    /////
		////////////////////////////////////
	}

	// applies damage of x amount
	public void OnTakeDamage(int amount)
	{
		// check if any health
		if(health > 0)
		{
			// let Lucy know
			//lucy.OnChangeFear(FearType.Damage);

			// reduce when blocking
			if(isBlocking)
				amount = amount / Ability_Manager.inst.sockets[3].GetCoreCount() + 1;

			// check for dead
			if(health - amount <= 0)
			{
				health = 0;
				print("bla bla black sheeep");
				OnDeath();
			}
			else
			{
				health -= amount;

				// apply appropriate cam shake
				if(amount >= largeDamage)
				{
					cam.ShakeCam(Shake.Large);
					//TODO play large damage audio
				}
				else if(amount <= smallDamage)
				{
					cam.ShakeCam(Shake.Small);
					//TODO play small damage audio
				}
				else
				{
					cam.ShakeCam(Shake.Medium);
					//TODO play medium damage audio
				}
			}
		}

	}

	void OnDeath()
	{

		//OnCorePickUp();
		Ability_Manager.inst.spareCores = 10;
		OnCorePickUp ();

	}

	public void SetBlocking(bool input)
	{
		isBlocking = input;
		anim.SetBool("Bool_Block", input);
	}

	/// triggers malfunction sequence
	public void OnCorePickUp()
	{
		abMan.AddSpareCore();
		isMalfunctioning = true;
		GM.MalfunctionMode(true);

		// show effects at spare's pos
		coreUpgradeParticles.SetActive(true);
	}

	// adds x amount to health
	public void OnRepair(int amount)
	{
		// check if health is full
		if(health < maxHealth)
		{
			// don't overfill health
			if(health + amount > maxHealth)
				health = maxHealth;
			else
				health += amount;

			Audio_Manager.inst.PlayOnce(AA.Chr_Mamaro_Attack_ChargePunch_1);
		}
	}

	/// returns the total amount of cores aquired
	public int GetTotalCores()
	{
		return meleeCores + rangedCores + shieldCores + speedCores + abMan.GetSpareCount();
	}

	// links core values with Ability_Manager
	private void UpdateCores()
	{
		meleeCores = abMan.sockets[0].GetCoreCount();
		speedCores = abMan.sockets[1].GetCoreCount();
		rangedCores = abMan.sockets[2].GetCoreCount();
		shieldCores = abMan.sockets[3].GetCoreCount();
	}
}
