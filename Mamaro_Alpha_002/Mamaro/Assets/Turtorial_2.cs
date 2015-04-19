using UnityEngine;
using System.Collections;

public class Turtorial_2 : MonoBehaviour {

	public GameObject tt;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
		{
			tt.SetActive (true);
			Destroy (gameObject);
		}
	}
}
