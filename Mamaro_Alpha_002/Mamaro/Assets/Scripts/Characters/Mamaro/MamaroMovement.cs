using UnityEngine;
using System.Collections;

public class MamaroMovement : MonoBehaviour {

	Rigidbody rb;

	public Vector3 moveDir;
	public Vector3 rotateEuler;
	public Vector3 cameraEuler;
	public float walkSpeed;
	public float runSpeed;
	public float turnSpeed;
	public float pitchSpeed;

	public float runMaxTime;
	public float runCooldownRate;
	public float timerRun;

	public float dodgeForceVert;
	public float dodgeForceHorz;

	public float cameraMaxPitch;
	public float cameraMinPitch;

	public bool isRun;
	public bool isDodge;

	public AbilitySocket socketMove;

	public static MamaroMovement inst;
	public AnimationState walkAnim;

	//Animation Variables
	Animator anim;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		if (inst == null)
		{
			inst = this;
		}
		anim = GetComponentInChildren<Animator>();
		isRun = false;
		isDodge = false;

	}

	// Use this for initialization
	void Start () 
	{
		socketMove = Ability_Manager.inst.GetSocket(Sockets.Speed);


	}
	
	// Update is called once per frame
	void Update () {

		//Set Speeds of all
		if (isRun)
		{
			//Incriment timer run.
			if (timerRun < runMaxTime)
			{
				timerRun += Time.deltaTime;
			}
			moveDir *= runSpeed * (1 + ((float)socketMove.GetCoreCount() / 4));
			rotateEuler *= turnSpeed / 2;
		}
		else
		{
			//Decrease timer Run
			if (timerRun > 0)
			{
				timerRun -= runCooldownRate * Time.deltaTime;
			}
			moveDir *= walkSpeed * (1 + ((float)socketMove.GetCoreCount() / 4));
			rotateEuler *= turnSpeed;
		}

		//Set All Data to Rigid Body.
		if (isDodge)
		{


		}
		else
		{
			//Remove any Roll
			rotateEuler.z = 0;
			//Set Rotation
			transform.Rotate (rotateEuler);

			//Set y velocity to previous so that gravity takes effect
			moveDir.y = rb.velocity.y;
			//Set Velocity
			rb.velocity = moveDir;


			//if (Camera.main.transform.rotation.x < cameraMaxPitch)
			{
				Camera.main.transform.Rotate(cameraEuler);
				Mamaro_Attack.inst.bulletSpawn.transform.Rotate (cameraEuler);
			}


//			if ((Camera.main.transform.rotation.x > cameraMinPitch && cameraEuler.x > 0) || (Camera.main.transform.rotation.x < cameraMaxPitch && cameraEuler.x < 0))
//			{
//				Camera.main.transform.Rotate(cameraEuler);
//			}
		}

		//Add in booster to gravity
		rb.AddForce(Vector3.down * 5000);

		//Animation speed
		anim.SetFloat("Speed",moveDir.magnitude / 15);
	

	}

	//for 4 direction dodging
	public void Dodge(Direction dir)
	{
		if (!isDodge)
		{
			switch (dir)
			{
			case Direction.Forward:
				rb.AddForce(Vector3.forward * dodgeForceHorz  * (1 + ((float)socketMove.GetCoreCount() / 4)),ForceMode.Impulse);
				break;
			case Direction.Back:
				rb.AddForce(Vector3.forward * -dodgeForceHorz * (1 + ((float)socketMove.GetCoreCount() / 4)),ForceMode.Impulse);
				break;
			case Direction.Left:
				rb.AddForce(Vector3.right * -dodgeForceHorz * (1 + ((float)socketMove.GetCoreCount() / 4)),ForceMode.Impulse);
				break;
			case Direction.Right:
				rb.AddForce(Vector3.right * dodgeForceHorz * (1 + ((float)socketMove.GetCoreCount() / 4)),ForceMode.Impulse);
				break;
			}
			rb.AddForce(Vector3.up * dodgeForceVert * (1 + ((float)socketMove.GetCoreCount() / 4)),ForceMode.Impulse);

			isDodge = true;
		}
	}

	public void Dodge(Vector3 dir)
	{
		if (!isDodge)
		{
			rb.velocity = Vector3.zero;
			rb.AddForce(dir * dodgeForceHorz  * (1 + ((float)socketMove.GetCoreCount() / 4)),ForceMode.Impulse);
			rb.AddForce(Vector3.up * dodgeForceVert,ForceMode.Impulse);
			isDodge = true;
		}
	}

	void OnCollisionEnter(Collision col)
	{
		isDodge = false;



	}
}


























