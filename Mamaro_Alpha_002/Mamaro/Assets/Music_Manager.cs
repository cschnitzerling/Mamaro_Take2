using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System.Collections.Generic;

public enum Area {Forest, City, Desert};

[System.Serializable]
public class Region
{
	public float size;
	public Area location;
	public Color col;
}

public class Music_Manager : MonoBehaviour 
{
	private Mamaro_Manager mama;

	// music mixer vars
	public AudioMixerSnapshot forestMain, forestDrum, cityMain, cityDrum, desertMain, desertDrum, mute;


	public float transitionSpeed = 1.0f;
	public float drumRange = 20.0f;
	public Area currentArea = Area.Forest;
	public AudioMixerSnapshot currentM;
	public AudioMixerSnapshot currentD;
	public Region[] TransRegion = new Region[2];
	public bool showGizmo = true;

	public List<GameObject> enemies = new List<GameObject>();
	public Vector3 startPos;

	public bool drumsOn = false;

	// Use this for initialization
	void Start () 
	{
		mama = Mamaro_Manager.inst;
		startPos = transform.position;

		TransitioMusic (Area.Forest);

		GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");
		foreach(GameObject g in temp)
		{
			if(g.name != "ShootPillarHold")
			enemies.Add(g);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Game_Manager.inst.isMalfunction || Game_Manager.inst.isPaused)
		{
			mute.TransitionTo(transitionSpeed);
		}
		else
		{
			// update area
			float dist = Vector3.Distance (startPos, transform.position);
			if(currentArea == Area.Forest && dist > TransRegion[0].size)
				TransitioMusic(Area.City);
			
			if(currentArea != Area.Forest && dist < TransRegion[0].size)
				TransitioMusic(Area.Forest);
			
			if (currentArea == Area.City && dist > TransRegion [1].size)
				TransitioMusic (Area.Desert);
			
			if (currentArea == Area.Desert && dist < TransRegion [1].size)
				TransitioMusic (Area.City);
			
			
			
			// stay with mamaro's current pos
			transform.position = mama.transform.position;
			
			// bring in drums when enemies are close
			int count = 0;
			foreach(GameObject g in enemies)
			{
				if(g != null)
				{
					if(Vector3.Distance(transform.position, g.transform.position) <= drumRange)
						count++;
				}
			}
			
			
			if (count > 0)
				currentD.TransitionTo (transitionSpeed);
			else
				currentM.TransitionTo (transitionSpeed);

		}
	}

	public void TransitionDrums(bool on)
	{
		if (on)
			currentD.TransitionTo(transitionSpeed);
		else 
			currentM.TransitionTo(transitionSpeed);
	}

	public void TransitioMusic(Area a)
	{
		switch (a) 
		{
		case Area.Forest:

			currentArea = Area.Forest;
			currentM = forestMain;
			currentD = forestDrum;

			break;

		case Area.City:

			currentArea = Area.City;
			currentM = cityMain;
			currentD = cityDrum;

			break;

		case Area.Desert:

			currentArea = Area.Desert;
			currentM = desertMain;
			currentD = desertDrum;

			break;
		default:
			break;
		}
	}

	public void OnDrawGizmos()
	{
		if(showGizmo)
		{
			Gizmos.color = TransRegion[0].col;
			Gizmos.DrawWireSphere(transform.position, TransRegion[0].size);
			
			Gizmos.color = TransRegion[1].col;
			Gizmos.DrawWireSphere(startPos, TransRegion[1].size);
		}
	}
}
