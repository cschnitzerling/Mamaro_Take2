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

	public float runMaxTime;
	public float runCooldownRate;
	public float timerRun;

	public float dodgeForceVert;
	public float dodgeForceHorz;

	public float pitchSpeed;
	public float maxUpAngle;
	public float maxDownAngle;
	public bool fullRotUp = false;
	public bool fullRotDown = false;
	public Quaternion startRot;

	float dodgeTimer;

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

		startRot = Camera.main.transform.localRotation;
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
			dodgeTimer += Time.deltaTime;

			if (dodgeTimer > 1)
			{
				isDodge = false;
				dodgeTimer = 0;
			}

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


			//print (Quaternion.Angle(Camera.main.transform.localRotation, startRot));
			
			// user input (needs converting to xbox)
			float input = cameraEuler.x * Time.deltaTime * pitchSpeed;

			// the applied variable to rotation
			float appliedRot = 0.0f;

			startRot.y = Camera.main.transform.rotation.y;
			startRot.z = Camera.main.transform.rotation.z;



			// attempting to rot up && check at limit
			if(input > 0 && !fullRotUp)
			{


				// prevent over rotation
				if(Quaternion.Angle(Camera.main.transform.localRotation, startRot) + input > maxUpAngle)
				{
					appliedRot += -2.0f;//maxUpAngle - Quaternion.Angle(Camera.main.transform.localRotation, startRot) - 0.8f;

					fullRotUp = true;
				}
				else
				{
					// unlimit opposite rot
					fullRotDown = false;

					appliedRot = input;
				}
			}
			
			
			// attempting to rot up && check if within limit
			else if(input < 0 && !fullRotDown)
			{


				// prevent over rotation
				if(Quaternion.Angle(Camera.main.transform.localRotation, startRot) - input > maxDownAngle)
				{
					//appliedRot = maxDownAngle - Quaternion.Angle(Camera.main.transform.localRotation, startRot) - 0.8f;
					appliedRot += 2.0f;
					fullRotDown = true;
				}
				else
				{
					// unlimit opposite rot
					fullRotUp = false;
					
					appliedRot = input;
				}
			}

			//print (appliedRot);

			// apply rotation
			Camera.main.transform.Rotate(new Vector3(input, 0.0f, 0.0f));
			Mamaro_Attack.inst.bulletSpawn.transform.Rotate(new Vector3(input, 0.0f, 0.0f));


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
		dodgeTimer = 0;
	}
}


























