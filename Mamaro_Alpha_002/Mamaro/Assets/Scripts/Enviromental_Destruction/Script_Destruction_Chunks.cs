using UnityEngine;
using System.Collections;

public class Script_Destruction_Chunks : MonoBehaviour {
	public GameObject mamaro;
	public BoxCollider mamaroFistCol;
	public Rigidbody rigBod;
	// Use this for initialization
	void Awake () {
		rigBod = GetComponent<Rigidbody> ();
		//rigBod.isKinematic = true;
		rigBod.velocity = Vector3.zero;
		rigBod.angularVelocity = Vector3.zero;
		mamaro = GameObject.FindGameObjectWithTag ("Player");
		mamaroFistCol = mamaro.GetComponentInChildren<BoxCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Terrain") {
			Debug.Log ("MYFACE");
			GameObject.Destroy(this.gameObject, 10);
		}

		if (col.collider == mamaroFistCol) {
			Debug.Log ("MYFACE");
			rigBod.isKinematic = false;
			GameObject.Destroy(this.gameObject, 1);
		}

		if (col.gameObject.tag == "Destro") {
			Debug.Log ("Buddy");
			rigBod.velocity = Vector3.zero;
			rigBod.angularVelocity = Vector3.zero;
		}
		
	}
}
