using UnityEngine;
using System.Collections;

public class Script_Destruction_Manager : MonoBehaviour {
	public GameObject destructionObject;
	public GameObject mamaro;
	//public BoxCollider mamaroFistCol;
	public Script_Destruction_Chunks[] chunklets = new Script_Destruction_Chunks[0];
	public int punchCount;

	// audio vars
	private Audio_Manager am;
	[Range(0.0f, 1.0f)]
	public float hitVolume = 1.0f;
	[Range(0.0f, 1.0f)]
	public float crumbleVolume = 1.0f;


	// Use this for initialization
	void Awake () 
	{
		mamaro = GameObject.FindGameObjectWithTag ("Player");
		//mamaroFistCol = mamaro.GetComponentInChildren<BoxCollider> ();

		if (gameObject.tag == "Wall") {
			GameObject WallDestroLoad = Resources.Load ("Envi_Destructables/Kelpi_Wall_Destro") as GameObject;
			destructionObject = WallDestroLoad;
		} else if (gameObject.tag == "Build") {
			GameObject BuildDestroLoad = Resources.Load ("Envi_Destructables/Kelpi_Building_Destro") as GameObject;
			destructionObject = BuildDestroLoad;
		} else if (gameObject.tag == "Ruin") {
			GameObject RuinDestroLoad = Resources.Load ("Envi_Destructables/M_Rock_Thick_Destro") as GameObject;
			destructionObject = RuinDestroLoad;
		}
	}

	void Start()
	{

		am = Audio_Manager.inst;
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void BlowUp(){
		{
			am.PlayOneShot(AA.Chr_Robot_Damage_MetalOnRock_1, transform.position, hitVolume);
			am.PlayOneShot(AA.Env_Desert_DesertRock_Crumble_2, transform.position, crumbleVolume);
			GameObject.Destroy (this.gameObject);
			GameObject destroInst = Instantiate (destructionObject, new Vector3 (transform.position.x, transform.position.y + 0, transform.position.z), transform.rotation) as GameObject;
			//destroInst.transform.localScale = gameObject.GetComponentInChildren<Transform>().transform.localScale;
		} 
	}
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Fist" && Ability_Manager.inst.GetSocket(Sockets.Melee).GetCoreCount() > 1) {
			//Debug.Log ("MYFACE");
			BlowUp();
			punchCount ++;
			Debug.Log(punchCount.ToString());
		}

	}

	void OnTriggerStay(Collider col)
	{

		if (col.tag == "Fist") {
			Debug.Log ("Ouch");
		}
	}
}
