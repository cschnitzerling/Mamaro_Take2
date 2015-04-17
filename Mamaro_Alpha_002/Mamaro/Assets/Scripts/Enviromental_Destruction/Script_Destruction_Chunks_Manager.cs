using UnityEngine;
using System.Collections;

public class Script_Destruction_Chunks_Manager : MonoBehaviour {
	public GameObject mamaro;
	public BoxCollider mamaroFistCol;
	public Rigidbody rigBod;
	public Script_Destruction_Chunks[] chunklets = new Script_Destruction_Chunks[0];
	public int fistCount;
	// Use this for initialization
	void Awake () {
		rigBod = GetComponent<Rigidbody> ();
		mamaro = GameObject.FindGameObjectWithTag ("Player");
		mamaroFistCol = mamaro.GetComponentInChildren<BoxCollider> ();
		chunklets = GetComponentsInChildren<Script_Destruction_Chunks> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnCollisionEnter(Collision col)
	{
		
		if (col.collider == mamaroFistCol) {
			Debug.Log ("MYFACE");
			GameObject.Destroy(this.gameObject, 1);
		}

		
	}
}