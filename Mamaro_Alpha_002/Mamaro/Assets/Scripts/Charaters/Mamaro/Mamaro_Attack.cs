using UnityEngine;
using System.Collections;

public class Mamaro_Attack : MonoBehaviour 
{
	public static Mamaro_Attack inst;

	//inspector assigned vars
	public int maxAttack, maxRangedCoolDown;
	[Range(15.0f, 30.0f)]
	public float chargeRate = 10.0f;
	public Collider fistCollider;

	// private vars
	public bool isAttacking = false;
	public float punchCharge = 0.0f, rangedCharge = 0.0f;

	public bool isChargePunch = false;
	public bool isChargeRange = false;

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
		PunchAttack ();
		RangedAttack();
		ChargePunch();
		ChargeRanged();

		//check to turn off fist collider
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("CHR_Mamaro_Anim_Idle"))
		{
			fistCollider.enabled = false;
			isAttacking = false;
		}

		timerxx += Time.deltaTime;

	}


	// applies punch attack sequence
	private void PunchAttack()
	{

	}

	// applies ranged attack sequence
	private void RangedAttack()
	{
	
	}

	// adds punch charge from 0 to 100 in respects to time held
	private void ChargePunch()
	{
		// receive held input
		if(isChargePunch)
		{
			if(!isAttacking)
			{
				if (punchCharge == 0)
				{
					//Set Animation to start
					anim.SetBool("Bool_MeeleCharge", true);

					punchCharge += 20;
				}

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
		else
		{
			// reduce at half charge rate until empty
			if(punchCharge > 0.0f)
			{
				if (!isAttacking)
				{
					isAttacking = true;
					
					//Set Animation to start
						anim.SetTrigger("Trig_MeeleAttack");
						anim.SetBool("Bool_MeeleCharge", false);

					if (timerxx > 2)
					{
						timerxx = 0;
						Lucy_Manager.inst.OnChangeFear(FearType.AttackLv3);
					}
				}

				punchCharge -= Time.deltaTime * (chargeRate * 2f); 

				// don't drop below 0.0f
				if(punchCharge < 0.0f)
				{
					isAttacking = false;
					punchCharge = 0.0f;
				}
			}
		}
	}

	// adds ranged charge from 0 to 100 in respects to time held
	private void ChargeRanged()
	{
		// receive held input
		if(isChargeRange)
		{
			if (!isAttacking)
			{
				if (rangedCharge == 0)
				{
					//Set Animation to start
					anim.SetBool("Bool_RangedCharge", true);
					rangedCharge += 30;
				}

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
		else 
		{
			// reduce at half charge rate until empty
			if(rangedCharge > 0.0f)
			{
				if (!isAttacking)
				{
					isAttacking = true;
					//Set Animation to start

						anim.SetTrigger("Trig_RangedAttack");
						anim.SetBool("Bool_RangedCharge", false);

					if (timerxx > 2)
					{
						Instantiate (bullet,bulletSpawn.transform.position,transform.rotation);
						timerxx = 0;
						Lucy_Manager.inst.OnChangeFear(FearType.AttackLv3);
					}

				}

				rangedCharge -= Time.deltaTime * (chargeRate * 2f);

				if(rangedCharge < 0.0f)
				{
					isAttacking = false;
					rangedCharge = 0.0f;
				}
			}
		}
	}
}
