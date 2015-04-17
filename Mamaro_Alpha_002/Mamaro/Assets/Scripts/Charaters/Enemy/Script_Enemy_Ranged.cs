using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Script_Enemy_Ranged : MonoBehaviour 
{

	//static access vars
	Mamaro_Manager mamaroM;	// access to player position, etc.

	// inspector assigned vars
	public float moveSpeed;
	public float rotSpeed;
	public float catchUpSpeed;
	public float engagementRadius;
	public int lowHealthThreshold;
	public float explosionRadius = 15.0F;
	public float explosionPower = 20.0F;

	// private vars
	public int health = 100;
	public EnemyState state = EnemyState.Standby;
	public bool alert = false;
	public float pDist;
	public Vector3 destPos;
	private float keptDistance;
	public Rigidbody rb;

	// Use this for initialization
	void Start () 
	{
		mamaroM = Mamaro_Manager.inst;
		rb = GetComponent<Rigidbody>();
		destPos = this.transform.position;

		keptDistance = engagementRadius * 0.75f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		pDist = Vector3.Distance(mamaroM.transform.position, this.transform.position);

		///test////////
		if(Input.GetKeyDown(KeyCode.F5))
			OnTakeDamage(50);


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
			if(alert && !mamaroM.isMalfunctioning)
			{
				// check which state to revert to
				if(pDist < engagementRadius)
				{
					// check health level
					if(health <= lowHealthThreshold)
						state = EnemyState.Defensive;
					else
						state = EnemyState.Offensive;
				}
				else
					state = EnemyState.Stalking;
			}

			break;

		// player is within engagment range. Enemy health is sufficient
		case EnemyState.Offensive:

			// is player in engagement range
			if(pDist < engagementRadius)
			{
				// is the player too close
				if(pDist < keptDistance)
				{
					// switch to defence with new destPos
					destPos = GetNewPos();
					state = EnemyState.Defensive;
				}
				else
				{
					// attack the player at rate x
				}
			}
			else
			{
				// catch up to the player
				state = EnemyState.Stalking;
			}

			// apply movement

			break;

		// player is within engagement range. Enemy is low on health
		case EnemyState.Defensive:

			MoveTo(destPos, moveSpeed);


			break;

		case EnemyState.Stalking:

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
	
	/// moves the enemy towards the destPos
	private void MoveTo(Vector3 pos, float speed)
	{
		// not yet reached destPos
		if(Vector3.Distance(this.transform.position, destPos) > 1.0f)
		{
			// face destPos
			LookTowards(destPos);
			transform.Translate(-transform.forward * speed * Time.deltaTime);// = new Vector3(pPos.x, pPos.y, pPos.z - speed * Time.deltaTime);
		}
		else
		{
			// face players
			LookTowards(mamaroM.transform.position);
			state = EnemyState.Offensive;
		}
	}

	// slowly looks faces target pos
	public void LookTowards(Vector3 pos)
	{
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(pos - transform.position), rotSpeed * Time.deltaTime);
	}

	/// reduces health and checks for death
	public void OnTakeDamage(int amount)
	{
		//TODO play hit audio

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
			//TODO apply particles
			Explode();
			Destroy(this);
		}
	}

	/// Adds explosive force to surrounding rigid bodies
	public void Explode()
	{
		Vector3 explosionPos = this.transform.position + transform.forward * 2;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);

		foreach (Collider hit in colliders)
		{
			if (hit && hit.GetComponent<Rigidbody>())
			{
				hit.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, explosionPos, explosionPower, 1.0F, ForceMode.Impulse);
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
