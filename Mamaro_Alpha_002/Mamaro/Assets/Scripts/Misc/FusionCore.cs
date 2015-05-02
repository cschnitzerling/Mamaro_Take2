using UnityEngine;
using System.Collections;

public class FusionCore : MonoBehaviour {

	GameObject ui;

	public RectTransform buttonX;
	public RectTransform buttonY;

	public float intSize = 0.8f;
	public float pickupTime;
	public float previousTime;
	public float timerPickup;

	// audio vars
	private Audio_Manager am;
	private string keyHum = "Hum";
	[Range(0.0f, 1.0f)]
	public float humVolume = 1.0f;
	[Range(0.0f, 1.0f)]
	public float pickUpVolume = 1.0f;
	[Range(0.0f, 1.0f)]
	public float destroyVolume = 1.0f;

	// Use this for initialization
	void Start () 
	{
		keyHum += GetInstanceID().ToString();

		am = Audio_Manager.inst;
		ui = Pickup_UI.inst.gameObject;

		buttonX = Pickup_UI.inst.buttonX;
		buttonY = Pickup_UI.inst.buttonY;

		ui.SetActive (false);
		timerPickup = 0;

		am.PlayLooped(AA.Env_Powercore_idle_1, transform.position, keyHum, humVolume);
	}
	
	// Update is called once per frame
	void Update () 
	{
		// update looped pos / vol
		am.UpdateVol(keyHum, humVolume, transform.position);

		//////////////////////////////////////////////////////////////////////////////
		/// 
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.LoadLevel(0);
		}
		/////////////////////////////////////////////////////////////////////////////////


		//Reset if Button Released for a single frame.
		if (timerPickup > 0 && timerPickup == previousTime)
		{
			timerPickup = 0;
			buttonX.localScale = Vector3.one * intSize;
			buttonY.localScale = Vector3.one * intSize;
		}

		previousTime = timerPickup;
	}

	public void CollectCore()
	{
		timerPickup += Time.deltaTime / pickupTime;
		buttonY.localScale = Vector3.one * (intSize + timerPickup);

		if (timerPickup > 1f)
		{
			Game_Manager.inst.coreDestroyed = false;
			Mamaro_Manager.inst.OnCorePickUp();
			am.PlayOneShot(AA.Chr_mamaro_powerup_pick_up, transform.position,  pickUpVolume);
			RemoveObject();
		}
	}

	public void DestroyCore()
	{
		timerPickup += Time.deltaTime / pickupTime;
		buttonX.localScale = Vector3.one * (intSize + timerPickup);

		if (timerPickup > 1f)
		{
			// initiate mal mode
			Mamaro_Manager.inst.isMalfunctioning = true;
			Game_Manager.inst.MalfunctionMode(true);
			Game_Manager.inst.coreDestroyed = true;

			Lucy_Manager.inst.UpgradeFear();
			am.PlayOneShot(AA.Env_General_ElectricalExplosion, transform.position, destroyVolume);
			RemoveObject();
		}
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.tag == "Player")
		{
			MamaroController.inst.fusionCores.Add(this); 
			ui.SetActive(true);
		}

	}

	void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player")
		{
			MamaroController.inst.fusionCores.Remove(this); 
			if (MamaroController.inst.fusionCores.Count == 0)
			{
				ui.SetActive(false);
			}
		}
	}

	void RemoveObject()
	{
		MamaroController.inst.fusionCores.Remove(this); 
		buttonX.localScale = Vector3.one * intSize;
		buttonY.localScale = Vector3.one * intSize;
		if (MamaroController.inst.fusionCores.Count == 0)
		{
			ui.SetActive(false);
		}

		// destroy looped audio
		am.DestroySource(keyHum);

		Destroy(gameObject);
	}
}
