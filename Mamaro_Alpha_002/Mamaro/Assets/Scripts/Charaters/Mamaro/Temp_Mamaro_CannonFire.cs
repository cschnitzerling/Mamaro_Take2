using UnityEngine;
using System.Collections;

public class Temp_Mamaro_CannonFire : MonoBehaviour {

	public GameObject laserBeam;


	void OnEnable()
	{
		Instantiate(laserBeam, transform.position, transform.rotation);
		this.enabled = false;
	}

	void Start()
	{}
}
