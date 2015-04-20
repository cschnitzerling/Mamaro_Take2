using UnityEngine;
using System.Collections;

public class Script_Destruction_Manager : MonoBehaviour {
	public GameObject destructionObject;
	public GameObject mamaro;
	public BoxCollider mamaroFistCol;
	public Script_Destruction_Chunks[] chunklets = new Script_Destruction_Chunks[0];
	public int punchCount;
	// Use this for initialization
	void Awake () {
		mamaro = GameObject.FindGameObjectWithTag ("Player");
		mamaroFistCol = mamaro.GetComponentInChildren<BoxCollider> ();
		if (gameObject.tag == "Wall") {
			GameObject WallDestroLoad = Resources.Load ("Envi_Destructables/Kelpi_Wall_Busted") as GameObject;
			destructionObject = WallDestroLoad;
		} else if (gameObject.tag == "Build") {
			GameObject BuildDestroLoad = Resources.Load ("Envi_Destructables/L_Building_Model_Destro") as GameObject;
			destructionObject = BuildDestroLoad;
		} else if (gameObject.tag == "Ruin") {
			GameObject RuinDestroLoad = Resources.Load ("Envi_Destructables/M_Rock_Thick_Destro") as GameObject;
			destructionObject = RuinDestroLoad;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BlowUp(){
		{
			Audio_Manager.inst.PlayOnce(AA.Chr_Robot_Damage_MetalOnRock_2, transform.position);
			Audio_Manager.inst.PlayOnce(AA.Env_Desert_DesertRock_Crumble_2, transform.position);
			GameObject.Destroy (this.gameObject);
			Instantiate (destructionObject, new Vector3 (transform.position.x, transform.position.y + 0, transform.position.z), transform.rotation);
		} 
	}
	void OnCollisionEnter(Collision col)
	{
		if (col.collider == mamaroFistCol && Ability_Manager.inst.GetSocket(Sockets.Melee).GetCoreCount() > 1) {
			//Debug.Log ("MYFACE");
			BlowUp();
			punchCount ++;
			Debug.Log(punchCount.ToString());
		}

	}

	void OnCollisionStay(Collision col)
	{
		if (col.collider == mamaroFistCol) {
			Debug.Log ("Ouch");
		}
	}
}
