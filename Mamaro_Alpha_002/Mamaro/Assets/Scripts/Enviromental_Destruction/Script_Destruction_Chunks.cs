using UnityEngine;
using System.Collections;

public class Script_Destruction_Chunks : MonoBehaviour {
	public GameObject mamaro;
	public BoxCollider mamaroFistCol;
	public Rigidbody rigBod;
	public GameObject colDust;
	public AudioClip rockClash;

	// Use this for initialization
	void Awake () {
		rigBod = GetComponent<Rigidbody> ();
		//rigBod.isKinematic = true;
		rigBod.velocity = Vector3.zero;
		rigBod.angularVelocity = Vector3.zero;
		mamaro = GameObject.FindGameObjectWithTag ("Player");
		mamaroFistCol = mamaro.GetComponentInChildren<BoxCollider> ();
	}
	void Start(){
		GameObject colDustGrab = Resources.Load ("Envi_Destructables/Rock_Hit_Particles") as GameObject;
		colDust = colDustGrab;

		AudioClip rockClashGrab = Resources.Load ("Audio/Chr_Robot_General_FootStomp_2") as AudioClip;
		rockClash = rockClashGrab;
	}
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter(Collision col)
	{
		Collide ();
		if (col.gameObject.tag == "Terrain") {
			//Debug.Log ("MYFACE");
			GameObject.Destroy(this.gameObject, 10);
		}

		if (col.collider == mamaroFistCol) {
			//Debug.Log ("MYFACE");
			rigBod.isKinematic = false;
			GameObject.Destroy(this.gameObject);
		}

		if (col.gameObject.tag == "Destro") {
			//Debug.Log ("Buddy");
			rigBod.velocity = Vector3.zero;
			rigBod.angularVelocity = Vector3.zero;
		}
		if (col.gameObject.tag == "Chunks" && rigBod.isKinematic == false) {
			Collide();
			rigBod.velocity = Vector3.zero;
			rigBod.angularVelocity = Vector3.zero;
		}

		
	}

	public void Collide(){
		//AudioSource.PlayClipAtPoint (rockClash, Camera.main.transform.position, .5f);
		Instantiate (colDust, transform.position, transform.rotation);
		rigBod.isKinematic = false;
		GameObject.Destroy(this.gameObject, 6);
	}
}
