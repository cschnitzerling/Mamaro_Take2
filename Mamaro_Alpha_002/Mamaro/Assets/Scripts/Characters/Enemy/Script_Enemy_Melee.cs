using UnityEngine;
using System.Collections;

public class Script_Enemy_Melee : MonoBehaviour 
{
	//static access vars
	MamaroMovement playerMove;	// access to player position, etc.
	
	// inspector assigned vars
	public float moveSpeed;
	public float sprintSpeed;
	public float engagmentRadius;
	public int lowHealthThreshold;
	
	// private vars
	public EnemyState state = EnemyState.Standby;
	
	
	// Use this for initialization
	void Start () 
	{
		playerMove = MamaroMovement.inst;

		//To RemoveWArning ############################################################
		int i = (int)playerMove.moveDir.x;										  //###
		i ++;																	  //###
		// ############################################################################
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (state) 
		{
			// player has not yet been within engagment range
		case EnemyState.Standby:
			
			break;
			
			// player is within engagment range. Enemy health is sufficient
		case EnemyState.Offensive:
			
			break;
			
			// player is within engagement range. Enemy is low on health
		case EnemyState.Defensive:
			
			break;
			
		case EnemyState.Stalking:
			
			break;

		case EnemyState.Dead:

			break;

		// error catch
		default:
			Debug.LogError("Switch statement fell through. Please revise.");
			break;
		}
	}
	
	// draw inspector gizoms
	void OnDrawGizmos() 
	{
		// Display the alert radius
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, engagmentRadius);
	}
}
