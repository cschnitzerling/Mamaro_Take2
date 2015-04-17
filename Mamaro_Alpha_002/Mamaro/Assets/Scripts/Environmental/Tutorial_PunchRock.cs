using UnityEngine;
using System.Collections;

public class Tutorial_PunchRock : MonoBehaviour 
{
	// static access vars
	private Ability_Manager abMan;

	// inspector assigned vars
	public string mamaroFistTag;
	public int punchCoresRequired = 1;

	// private vars
	private Rigidbody rigidB;

	void Start()
	{
		abMan = Ability_Manager.inst;
		rigidB = GetComponent<Rigidbody>();
	}

	// check for punch collision
	void OnCollisionEnter(Collision otherObj)
	{
		// check soley for fist
		if(otherObj.collider.tag == mamaroFistTag)
		{
			// make sure x amoun of cores are applied to punch
			if(abMan.sockets[0].GetCoreCount() >= punchCoresRequired)
			{
				//TODO do something to the rock???
				//TODO play rock break sound

				////////////////////////////////
				/// Temporary Visual effect/////
				rigidB.isKinematic = false;
				rigidB.AddForce(new Vector3(0, 5, 10), ForceMode.Impulse);
				transform.Rotate(new Vector3(10, 0, -10));
				Cam_Manager.inst.ShakeCam(Shake.Small);
			}

			//TODO play rock hit (blunt) sound
		}
	}
}
