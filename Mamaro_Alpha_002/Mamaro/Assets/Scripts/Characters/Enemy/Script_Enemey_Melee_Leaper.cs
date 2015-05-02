using UnityEngine;
using System.Collections;

public class Script_Enemey_Melee_Leaper : MonoBehaviour {
		
	public enum EnemyState
	{
		Wander,
		Chase,
		Attack,
		Rotate,
		Dead
	}
	
	Mamaro_Manager player;
	
	//Enemy Stats
	public int health;
	public int damage;
	
	//Target Range
	RaycastHit hit = new RaycastHit();
	Ray look = new Ray();
	public float lineSight;
	public float lineSightAttack;
	
	//Enemy Movement
	public float speedWalk;
	public float speedRun;
	float enemyRotate;
	public float enemyRotateSpeed;
	Vector3 enemyMove;
	
	//Enemy States
	EnemyState state = EnemyState.Wander;
	
	//Enemy Times
	float timer = 0;
	public float wanderTime = 5f;
	public float rotateTime = 2f;
	public float chaseTime = 3f;
	float idleTime;
	
	//Attack Stats
	public float attackSpeed;
	public float attackRadius;
	//public float attackForce;
	public float attackTime;
	public float attackUp;

	public bool isAttacking;

	Animator anim;

	public MeleeHitBox hitCollider;
	
	// Use this for initialization
	void Start () 
	{
		anim = gameObject.GetComponentInChildren<Animator>();
		state = EnemyState.Rotate;
		player = Mamaro_Manager.inst;

		hitCollider.SetDamage(damage);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		GetComponent<Rigidbody>().AddForce(Vector3.down * 100);
		//Line of Sight Check
		look = new Ray ((transform.position + (Vector3.up * 1)),((player.transform.position) - transform.position));
		if (state == EnemyState.Dead)
		{
		}
		else if (state != EnemyState.Attack)
		{
			if (Physics.Raycast (look,out hit, lineSightAttack))
			{
				if (hit.collider.tag == "Player")
				{
					OnChangeState(EnemyState.Attack);
				}

			}
			else if (Physics.Raycast (look,out hit, (lineSight)))
			{
				if (state != EnemyState.Chase)
				{
					if (hit.collider.tag == "Player")
					{
						OnChangeState(EnemyState.Chase);
					}
				}
			}
		}


		switch(state)
		{
		case EnemyState.Wander:
			Wander ();
			break;
		case EnemyState.Rotate:
			Rotate ();
			break;
		case EnemyState.Chase:
			Chase ();
			break;
		case EnemyState.Attack:
			Attack ();
			break;
		case EnemyState.Dead:
			Dead ();
			break;
		}
	}
	
	void Chase()
	{
		timer += Time.deltaTime;
		attackTime = 1f;
		Vector3 playerVec = player.transform.position;
		Vector3 enemyVec = transform.position;
		playerVec.y = 0;
		enemyVec.y = 0;
		
		transform.rotation = Quaternion.FromToRotation (Vector3.forward, playerVec - enemyVec);
		
		GetComponent<Rigidbody>().AddRelativeForce (Vector3.forward * speedRun);
		
		if (timer > chaseTime)
		{
			OnChangeState(EnemyState.Wander);
		}
	}
	
	void Wander()
	{
		
		timer += Time.deltaTime;
		
		GetComponent<Rigidbody>().AddRelativeForce (Vector3.forward * speedWalk);
		
		if (timer > wanderTime)
		{
			OnChangeState(EnemyState.Rotate);
		}
	}
	
	void Rotate()
	{
		timer += Time.deltaTime;
		GetComponent<Rigidbody>().AddRelativeTorque(Vector3.up * enemyRotate * enemyRotateSpeed);
		if (timer > rotateTime)
		{
			OnChangeState(EnemyState.Wander);
		}
	}
	
	void Attack()
	{
		timer += Time.deltaTime;
		
		if (timer > attackTime && !isAttacking)
		{
			Vector3 playerVec = player.transform.position;
			Vector3 enemyVec = transform.position;
			playerVec.y = 0;
			enemyVec.y = 0;

			GetComponent<Rigidbody>().AddRelativeForce((Vector3.forward * attackSpeed) + (Vector3.up * attackUp),ForceMode.Impulse);

			timer = 0;
			isAttacking = true;

			anim.SetTrigger("Trig_Attack");
		}
		if (timer > 2f)
		{
			isAttacking = false;
			OnChangeState(EnemyState.Wander);
			//anim.SetTrigger("Trig_Idle");
		}
	}
	
	void Dead()
	{
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		Vector3 rot = (new Vector3 (Random.Range (3f, 10f), Random.Range (3f, 10f), Random.Range (3f, 10f)) * 0.1f);
		GetComponent<Rigidbody>().AddRelativeTorque (rot,ForceMode.Impulse);
		GetComponent<Rigidbody>().AddForce((transform.position - player.transform.position).normalized * 2 ,ForceMode.Impulse);
		Destroy (gameObject,3);
	}

	void OnChangeState(EnemyState es)
	{
		state = es;

		switch (es)
		{
		case EnemyState.Attack:
			Vector3 playerVec = player.transform.position;
			Vector3 enemyVec = transform.position;
			playerVec.y = 0;
			enemyVec.y = 0;
			
			transform.rotation = Quaternion.FromToRotation (Vector3.forward, playerVec - enemyVec);
			timer = 0;

			anim.SetTrigger("Trig_Charge");
			break;
		case EnemyState.Chase:

			timer = 0;
			break;
		case EnemyState.Dead:
			timer = 0;
			break;
		case EnemyState.Rotate:
			timer = 0;
			enemyRotate = Random.Range (-1.0f, 1.0f);
			break;
		case EnemyState.Wander:

			timer = 0;
			break;
		}


	}
	
	public void OnTakeDamage(int dam)
	{
		health -= dam;
		
		if (state == EnemyState.Wander)
		{
			OnChangeState(EnemyState.Chase);
		}
		
		if (health <= 0 && state != EnemyState.Dead)
		{
			OnChangeState(EnemyState.Dead);
		}
	}
	
	void OnCollisionEnter(Collision col)
	{
		if (col.collider.tag == "Player")
		{
			//col.gameObject.SendMessage ("OnTakeDamage", damage,SendMessageOptions.DontRequireReceiver);
			//player.knockBackForce = attackForce;
			//col.gameObject.SendMessage ("KnockBack", transform.position);
		}
	}
}