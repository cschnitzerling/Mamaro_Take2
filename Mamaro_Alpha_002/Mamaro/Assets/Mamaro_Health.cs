using UnityEngine;
using System.Collections;

public class Mamaro_Health : MonoBehaviour {

	private Vector3 meterVec = Vector3.one;
	public RectTransform meter;
	public Material color;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		meterVec.y = ((float)Mamaro_Manager.inst.health / (float)Mamaro_Manager.inst.maxHealth);
		meter.localScale = meterVec;

		if (Mamaro_Manager.inst.health  < 20)
		{
			color.color = Color.red;
		}
		else if(Mamaro_Manager.inst.health  > 40)
		{
			color.color = Color.yellow;
		}
		else 
		{
			color.color = Color.yellow;
		}
	}
}
