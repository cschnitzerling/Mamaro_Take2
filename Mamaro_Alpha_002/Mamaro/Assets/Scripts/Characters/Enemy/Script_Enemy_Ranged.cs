using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Script_Enemy_Ranged : MonoBehaviour 
{

	//static access vars
	Mamaro_Manager mamaroM;	// access to player position, etc.

	// inspector assigned vars
	public float moveSpeed = 20;
	public float rotSpeed = 10;
	public float catchUpSpeed = 40;
	public float engagementRadius = 200.0f;
	public int lowHealthThreshold;
	public float explosionRadius = 15.0F;
	public float explosionPower = 20.0F;
	public GameObject projectile;
	public Transform shootPos;
	public float fireRate = 5.0f;
	public float destroyTime = 5.0f;
	public GameObject explosion;
	public GameObject fusionCore;

	// private vars
	public int health = 100;
	public EnemyState state = EnemyState.Standby;
	public bool alert = false;
	public bool isMoving = false;
	public float pDist;
	public Vector3 destPos;
	private float keptDistance;
	public Rigidbody rb;
	private NavMeshAgent nav;
	private float timerFire = 0.0f;
	private float timerDestroy;
	private string audioKey;

	// Use this for initialization
	void Start () 
	{
		audioKey = GetInstanceID().ToString();
		Audio_Manager.inst.PlayRecursive(AA.Chr_Robot_Attack_HoldCharge_1, transform.position, audioKey);
		nav = GetComponent<NavMeshAgent>();
		mamaroM = Mamaro_Manager.inst;
		rb = GetComponent<Rigidbody>();
		destPos = this.transform.position;

		// 3/4 the value of engagement radius
		keptDistance = engagementRadius * 0.75f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// track player dist
		pDist = Vector3.Distance(mamaroM.transform.position, this.transform.position);
		Audio_Manager.inst.SetRecursivePos(audioKey, transform.position);

		///test////////
		if(Input.GetKeyDown(KeyCode.F5))
			OnTakeDamage(50);

		// check for destroy
		if(timerDestroy > 0.0f)
		{
			timerDestroy -= Time.deltaTime;
			if(timerDestroy <= 0.5f)
				Destroy(gameObject);
		}


		switch (state) 
		{
		// player has not yet been within engagment range
		case EnemyState.Standby:


			// check if mamaro is within engagment range
			if(!alert && pDist < engagementRadius)
			{
				alert = true;
				state = EnemyState.Offensive;
			}

			// wait for mamaro to stop malfunctioning
			if(alert && !Game_Manager.inst.isMalfunction)
			{
				// player within range
				if(pDist < engagementRadius)
					state = EnemyState.Offensive;
				else
					state = EnemyState.Stalking;
			}

			break;

		// player is within engagment range. Enemy health is sufficient
		case EnemyState.Offensive:

			// always face Mamaro
			LookTowards(mamaroM.transform.position);
			nav.SetDestination(destPos);
			nav.speed = moveSpeed;

			// reached destination
			if(nav.remainingDistance < 1.0f)
				isMoving = false;

			// louder volume when moving
			if(isMoving)
				Audio_Manager.inst.SetVolume(audioKey, Mathf.Lerp(Audio_Manager.inst.GetVolume(audioKey), 1.0f, 0.1f));
			else
				Audio_Manager.inst.SetVolume(audioKey, Mathf.Lerp(Audio_Manager.inst.GetVolume(audioKey), 0.5f, 0.1f));

			if(!isMoving)
			{
				// is player in engagement range
				if(pDist < engagementRadius)
				{
					// is the player too close
					if(pDist < keptDistance)
					{
						// change pos
						state = EnemyState.Defensive;
					}
				}
				else
				{
					// catch up to the player
					state = EnemyState.Stalking;
				}
			}

			// fire every x seconds
			timerFire += Time.deltaTime;
			if(timerFire >= fireRate)
			{
				Audio_Manager.inst.PlayOnce(AA.Chr_Robot_Attack_Laser_1, transform.position);
				Instantiate(projectile, shootPos.position, shootPos.rotation);
				timerFire = 0.0f;
			}


			break;

		// player is within keptDistance
		case EnemyState.Defensive:

			isMoving = true;
			destPos = GetNewPos();
			state = EnemyState.Offensive;

			break;

		case EnemyState.Stalking:

			// increase speed to catch up to player
			destPos = mamaroM.transform.position;
			nav.SetDestination(destPos);
			nav.speed = catchUpSpeed;

			// check if back in range
			if(pDist < engagementRadius)
				state = EnemyState.Offensive;

			break;

		// error catch
		default:
			Debug.LogError("Switch statement fell through. Please revise.");
			break;
		}
	}
	
	/// slerp towards facing target
	public void LookTowards(Vector3 target)
	{
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target - transform.position), rotSpeed * Time.deltaTime);
	}

	/// reduces health and checks for death
	public void OnTakeDamage(int amount)
	{
		Audio_Manager.inst.PlayOnce(AA.Chr_Robot_Damage_MetalOnMetal_2, transform.position);

		// check if already dead
		if(health > 0)
		{
			health -= amount;
		}

		// check for death
		if(health <= 0)
		{
			// death sequence
			DetachChildren(this.transform);
			Explode();
			Audio_Manager.inst.PlayOnce(AA.Env_General_PhysicalExpolsion_2, transform.position);
			timerDestroy = destroyTime;
		}
	}

	/// Adds explosive force to surrounding rigid bodies
	public void Explode()
	{
		Destroy (nav);
		Audio_Manager.inst.PlayOnce(AA.Env_General_PhysicalExpolsion_2, transform.position);
		Vector3 centerPos = new Vector3 (transform.position.x, transform.position.y + 10.0f, transform.position.z);
		Instantiate (fusionCore, centerPos, Quaternion.identity);
		Instantiate(explosion, centerPos, Quaternion.identity);
		Vector3 explosionPos = this.transform.position + transform.forward * 2;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);

		foreach (Collider hit in colliders)
		{
			if (hit && hit.GetComponent<Rigidbody>())
			{
				hit.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, explosionPos, explosionPower, 0.35F, ForceMode.Impulse);
			}
		}
	}

	// deparents children up to x deep
	private void DetachChildren(Transform trans)
	{
		// check each child
		foreach(Transform child0 in trans)
		{
			// is parent
			if(child0.childCount > 0)
			{
				// check each child
				foreach(Transform child1 in child0)
				{
					// is parent
					if(child1.childCount > 0)
					{
						// check each child
						foreach(Transform child2 in child1)
						{
							// is parent
							if(child2.childCount > 0)
							{
								// check each child
								foreach(Transform child3 in child2)
								{
									// add rigid bodies and collider
									if(child3.gameObject.GetComponent<Rigidbody>() == null)
										child3.gameObject.AddComponent<Rigidbody>();
									if(child3.gameObject.GetComponent<BoxCollider>() == null)
										child3.gameObject.AddComponent<BoxCollider>();
								}
							}

							// add rigid bodies and collider
							if(child2.gameObject.GetComponent<Rigidbody>() == null)
								child2.gameObject.AddComponent<Rigidbody>();
							if(child2.gameObject.GetComponent<BoxCollider>() == null)
								child2.gameObject.AddComponent<BoxCollider>();
						}
					}

					// add rigid bodies and collider
					if(child1.gameObject.GetComponent<Rigidbody>() == null)
						child1.gameObject.AddComponent<Rigidbody>();
					if(child1.gameObject.GetComponent<BoxCollider>() == null)
						child1.gameObject.AddComponent<BoxCollider>();
				}
			}

			// add rigid bodies and collider
			if(child0.gameObject.GetComponent<Rigidbody>() == null)
				child0.gameObject.AddComponent<Rigidbody>();
			if(child0.gameObject.GetComponent<BoxCollider>() == null)
				child0.gameObject.AddComponent<BoxCollider>();
		}
	}

	/// returns a valid new position within the given range
	private Vector3 GetNewPos()
	{
		bool foundPath = false;
		Vector3 tempV;
		float breakTimer = 0.0f;

		// only return a clear path position
		while(!foundPath)
		{
			// break for constant loop defence
			breakTimer += Time.deltaTime;
			if(breakTimer > 2.0f)
			{
				Debug.LogError("While loop was stuck in constant loop. Check your logic!");
				foundPath = true;
			}

			// pick a pos within the specefied range
			tempV = transform.position + Random.insideUnitSphere * engagementRadius;
			Vector3 selected = new Vector3(tempV.x, transform.position.y, tempV.z);

			// within engagement range but out of keptDistance range
			float fromPlayer = Vector3.Distance(mamaroM.transform.position, selected);
			if(fromPlayer > keptDistance && fromPlayer < engagementRadius)
			{
				// check if point A and B have an obstacle in the way
				RaycastHit hit;
				if(!Physics.Linecast(this.transform.position, selected, out hit))
				{
					foundPath = true;
					return selected;
				}
			}
		}

		// return this poeition in case of path not found
		return this.transform.position;
	}

	/// draw inspector gizoms
	void OnDrawGizmos() 
	{
		// engagement radius
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, engagementRadius);
		
		// destPos
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(destPos, 2.0f);
	}
}
