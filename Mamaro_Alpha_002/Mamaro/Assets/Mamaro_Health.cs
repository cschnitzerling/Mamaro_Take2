using UnityEngine;
using System.Collections;

public class Mamaro_Health : MonoBehaviour {

	private Vector3 meterVec = Vector3.one;
	public RectTransform meter;
	public Material color;
	public GameObject healthUpParticles;

	private int previousHealth;	// for checking if the health has gone up or down

	// Use this for initialization
	void Start () 
	{
		// set pHealth to starting val
		previousHealth = 100;
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

		// check for health change

		// particle effect at the relative end of health bar
		float effectPos = meter.rect.height * Mamaro_Manager.inst.health / Mamaro_Manager.inst.maxHealth;

		// health has increased
		if(previousHealth < Mamaro_Manager.inst.health)
		{
			// show health increase particles
			healthUpParticles.transform.localPosition = new Vector3(0, effectPos, 0);
			healthUpParticles.SetActive(true);
		}
		// has decreased
		else if(previousHealth < Mamaro_Manager.inst.health)
		{
			//TODO optional: show decrease effect
		}

		// update pHealth to current
		previousHealth = Mamaro_Manager.inst.health;
	}
}
