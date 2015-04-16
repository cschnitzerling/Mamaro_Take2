using UnityEngine;
using System.Collections;

public class CamaraSwitch : MonoBehaviour {
	public GameObject Target1, Target2, Target3, player;
	public float currentMaxDis, currentMinDis;
	public Transform currentTarget;
	public Vector3 currentLocation;
	public CamaraFollow camAdjust;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		camAdjust = Camera.main.GetComponent<CamaraFollow> ();
		currentMaxDis = camAdjust.maxDistance;
		currentMinDis = camAdjust.minDistance;
		currentTarget = player.transform;
		currentLocation = Camera.main.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
				if (Input.GetKey ("e")) {
						//Book ();
				}
	}

	public void TargetSwitch1(){
				camAdjust.switched 		= true;
				camAdjust.target = Target1.transform;
				camAdjust.minDistance = 50;
				camAdjust.maxDistance = 100;
		}
	public void TargetSwitch2(){
		camAdjust.switched = true;
		camAdjust.target = Target2.transform;
		camAdjust.minDistance = 500;
		camAdjust.maxDistance = 550;
	}
	public void TargetSwitch3(){
		camAdjust.switched = true;
		camAdjust.target = Target3.transform;
		camAdjust.minDistance = 500;
		camAdjust.maxDistance = 550;
	}

	public void CloseUp(){
		camAdjust.minDistance = 0;
		camAdjust.maxDistance = 1;
	}
	

	public void Reset(){
		camAdjust.switched = false;
		camAdjust.target = currentTarget;
		camAdjust.minDistance = currentMinDis;
		camAdjust.maxDistance = currentMaxDis;
		Camera.main.transform.position = currentLocation;
}
	
}
