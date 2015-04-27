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

	private bool aRangeOnce = false;
	private bool aPunchOnce = false;

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
			isChargePunch = true;

			// play swing back once
			if(!aPunchOnce)
			{
				aPunchOnce = true;
				Audio_Manager.inst.PlayOnce(AA.Chr_Mamaro_Attack_ChargePunch_1);
			}
		}
	}

	public void OnSendFear(int socketCount)
	{
		switch(socketCount)
		{
		case 0:
			break;
		case 1:
			Lucy_Manager.inst.OnChangeFear(FearType.AttackLv1);
			break;
		case 2:
			Lucy_Manager.inst.OnChangeFear(FearType.AttackLv2);
			break;
		case 3:
			Lucy_Manager.inst.OnChangeFear(FearType.AttackLv3);
			break;
		case 4:
			Lucy_Manager.inst.OnChangeFear(FearType.AttackLv4);
			break;
		}
	}

	public void ButtonUpPunch()
	{
		if (isChargePunch)
		{
			//Addfear
			OnSendFear(Ability_Manager.inst.GetSocket(Sockets.Melee).GetCoreCount());
			
			isChargePunch = false;
			isAttackPunch = true;
			timerShotDelay = shotDelay;
		}

		aPunchOnce = false;
		Audio_Manager.inst.PlayOnce(AA.Chr_Mamaro_Movement_Dodge_2);
	}

	// applies ranged attack sequence
	public void ButtonDownRange()
	{
		if (!isChargePunch && punchCharge == 0 && timerShotDelay == 0)
		{
			isChargeRange = true;

			// play charge audio once
			if(!aRangeOnce)
			{
				aRangeOnce = true;
				Audio_Manager.inst.PlayOnce(AA.Chr_Robot_Attack_CannonCharge_3);
				Audio_Manager.inst.PlayRecursive(AA.Chr_Robot_Attack_HoldCharge_1, transform.position, "RangeKey");
			}
		}
	}

	public void ButtonUpRange()
	{
		if (isChargeRange)
		{
			//Addfear
			OnSendFear(Ability_Manager.inst.GetSocket(Sockets.Ranged).GetCoreCount());

			isChargeRange = false;
			isAttackRange = true;
			Audio_Manager.inst.PlayOnce(AA.Chr_Robot_Attack_CannonFire_1);
			timerShotDelay = shotDelay;
		}

		aRangeOnce = false;
		Audio_Manager.inst.DestroyRecursive("RangeKey");
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

			Instantiate (bullet,bulletSpawn.transform.position,bulletSpawn.transform.rotation);

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
