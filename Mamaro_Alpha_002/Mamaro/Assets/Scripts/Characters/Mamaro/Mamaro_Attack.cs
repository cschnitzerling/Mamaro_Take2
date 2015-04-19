using UnityEngine;
using System.Collections;

public class Mamaro_Attack : MonoBehaviour 
{
	public static Mamaro_Attack inst;

	//inspector assigned vars
	public int maxAttack, maxRangedCoolDown;
	[Range(15.0f, 50.0f)]
	public float chargeRate = 10.0f;
	public Collider fistCollider;

	// private vars
	public bool isAttacking = false;
	public float punchCharge = 0.0f, rangedCharge = 0.0f;

	public float shotDelay = 2.0f;
	public float timerShotDelay = 0.0f;

	public bool isChargePunch = false;
	public bool isChargeRange = false;

	public bool isAttackPunch = false;
	public bool isAttackRange = false;

	public GameObject bullet;
	public GameObject bulletSpawn;

	float timerxx;


	//Animation Variables
	Animator anim;


	// Use this for initialization
	void Awake() 
	{
		if (inst == null)	
		{
			inst = this;
		}
		anim = GetComponentInChildren<Animator>();
		fistCollider.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//TODO apply gate for if pause here
		ChargePunch();
		ChargeRanged();

		if (timerShotDelay > 0)
		{
			timerShotDelay -= Time.deltaTime;

			if (timerShotDelay < 0)
			{
				timerShotDelay = 0;
			}
		}

		//check to turn off fist collider
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("CHR_Mamaro_Anim_Idle"))
		{
			fistCollider.enabled = false;
			isAttacking = false;
		}

		timerxx += Time.deltaTime;

	}


	// applies punch attack sequence
	public void ButtonDownPunch()
	{
		if (!isChargeRange && punchCharge == 0 && timerShotDelay == 0)
		{
			Mamaro_Attack.inst.isChargePunch = true;
		}
	}
	public void ButtonUpPunch()
	{
		if (isChargePunch)
		{
			Mamaro_Attack.inst.isChargePunch = false;
			Mamaro_Attack.inst.isAttackPunch = true;
			timerShotDelay = shotDelay;
		}
	}

	// applies ranged attack sequence
	public void ButtonDownRange()
	{
		if (!isChargePunch && punchCharge == 0 && timerShotDelay == 0)
		{
			Mamaro_Attack.inst.isChargeRange = true;
		}
	}
	public void ButtonUpRange()
	{
		if (isChargeRange)
		{
			Mamaro_Attack.inst.isChargeRange = false;
			Mamaro_Attack.inst.isAttackRange = true;
			timerShotDelay = shotDelay;
		}
	}

	// adds punch charge from 0 to 100 in respects to time held
	private void ChargePunch()
	{

		if (isAttackPunch)
			{

			// reduce at half charge rate until empty
			//Set Animation to start
			anim.SetTrigger("Trig_MeeleAttack");
			anim.SetBool("Bool_MeeleCharge", false);

			isAttackPunch = false;
		}
		// receive held input
		else if(isChargePunch)
		{
			anim.SetBool("Bool_MeeleCharge", true);

			if(punchCharge < 100.0f)
			{
				punchCharge += Time.deltaTime * chargeRate;

				// limit to max of 100
				if(punchCharge > 100.0f)
					punchCharge = 100.0f;
			}
		}
		else
		{
			punchCharge -= Time.deltaTime * (chargeRate * 2f);
			
			if(punchCharge < 0.0f)
			{
				isAttacking = false;
				punchCharge = 0.0f;
			}
		}


	}

	// adds ranged charge from 0 to 100 in respects to time held
	private void ChargeRanged()
	{
		if (isAttackRange)
		{
			anim.SetTrigger("Trig_RangedAttack");
			anim.SetBool("Bool_RangedCharge", false);

			Instantiate (bullet,bulletSpawn.transform.position,transform.rotation);
			Lucy_Manager.inst.OnChangeFear(FearType.AttackLv3);

			isAttackRange = false;

		}
		// receive held input
		if(isChargeRange)
		{
			anim.SetBool("Bool_RangedCharge", true);

			if(rangedCharge < 100.0f)
			{
				rangedCharge += Time.deltaTime * chargeRate;
				
				// limit to max of 100
				if(rangedCharge > 100.0f)
					rangedCharge = 100.0f;
			}
		}
		else
		{
			rangedCharge -= Time.deltaTime * (chargeRate * 2f);
			
			if(rangedCharge < 0.0f)
			{
				isAttacking = false;
				rangedCharge = 0.0f;
			}
		}

	}
}
