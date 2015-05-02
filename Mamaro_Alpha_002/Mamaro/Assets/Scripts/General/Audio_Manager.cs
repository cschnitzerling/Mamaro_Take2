using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Enum = System.Enum;

/// <summary>
/// Loads and handles all audio assets
/// </summary>
public class Audio_Manager : MonoBehaviour 
{
	// static instance of Audio_Manager
	public static Audio_Manager inst;
	
	// storage for all loaded audio assets
	private Dictionary<AA, AudioClip> assets = new Dictionary<AA, AudioClip>();
	
	// tracking instantiated recursive aSources
	private Dictionary<string, AudioSource> trackedASources = new Dictionary<string, AudioSource>();
	
	// tracking one shot objects
	private List<AudioSource> oneShots = new List<AudioSource>();
	
	// the object which hears the audio
	public GameObject listenerObj;
	public float hearDist = 100.0f;
	public bool showGizmos = true;
	
	// Use this for initialization
	void Awake () 
	{
		// create static instance
		if(inst == null)
			inst = this;
		
		// occupy dictionary
		LoadAudio ();
	}
	
	void Update()
	{
		// check to destroy oneshots
		for(int i = 0; i < oneShots.Count; ++i)
		{
			// has stopped playing
			if(!oneShots[i].isPlaying)
			{
				Destroy(oneShots[i].gameObject);
				oneShots.Remove(oneShots[i]);
			}
		}
	}
	
	// Class Methods //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	/// plays audio at this pos once
	public void PlayOneShot(AA clip, float volume = 1.0f, bool threeD = false)
	{
		// check for errors in loading
		if(assets.ContainsKey(clip))
		{
			// instancitae an empty gameObject
			GameObject tempObj = new GameObject();
			tempObj.transform.position = listenerObj.transform.position;
			tempObj.AddComponent<AudioSource>();
			tempObj.name = "OneShot" + clip.ToString();
			
			// assign the audiosource
			AudioSource tempA = tempObj.GetComponent<AudioSource>();
			tempA.clip = assets[clip];
			tempA.volume = volume;
			
			// apply 3D sound if true
			if(threeD)
				tempA.spatialBlend = 1.0f;
			
			tempA.Play();
			
			// add to tracked oneShots
			oneShots.Add(tempA);
		}
		else
			Debug.Log("The requested audio does not exist: (" + clip.ToString() + "). Check that your enum is up to date.");
	}
	
	/// plays audio at specified position once
	public void PlayOneShot(AA clip, Vector3 pos, float volume = 1.0f, bool scaleVol = true, bool threeD = false)
	{
		// check for errors in loading
		if(assets.ContainsKey(clip))
		{
			// instancitae an empty gameObject
			GameObject tempObj = new GameObject();
			tempObj.transform.position = pos;
			tempObj.AddComponent<AudioSource>();
			tempObj.name = "OneShot" + clip.ToString();
			
			// assign the audiosource
			AudioSource tempA = tempObj.GetComponent<AudioSource>();
			tempA.clip = assets[clip];
			
			// scale vol in respects to dist/hearDist
			if(scaleVol)
				tempA.volume = VolumePerc(pos, volume);
			else
				tempA.volume = volume;
			
			// apply 3D sound if true
			if(threeD)
				tempA.spatialBlend = 1.0f;
			
			tempA.Play();
			
			// add to tracked oneShots
			oneShots.Add(tempA);
		}
		else
			Debug.Log("The requested audio does not exist: (" + clip.ToString() + "). Check that your enum is up to date.");
	}
	
	/// plays recursive music at specified position. Specefied key will be used for future access/destruction
	public void PlayLooped(AA clip, Vector3 pos, string key, float volume = 1.0f, bool scaleVol = true, bool threeD = false)
	{
		// key exists
		if(trackedASources.ContainsKey(key))
		{
			Debug.Log("Tracked Audio Source list already contains an element matching Key("
			          + key + "). Use a different key or destroy the existing element");
			//			// set up audio source
			//			trackedASources[key].transform.position = pos;
			//			trackedASources[key].clip = assets[clip];
			//			trackedASources[key].loop = true;
			//			trackedASources[key].volume = volume;
			//
			//			// scale vol in respects to dist/hearDist
			//			if(scaleVol)
			//				trackedASources[key].volume = VolumePerc(pos, volume);
			//			else
			//				trackedASources[key].volume = volume;
			//
			//			if(threeD)
			//				trackedASources[key].spatialBlend = 1.0f;
			//			else
			//				trackedASources[key].spatialBlend = 0.0f;
			//
			//			trackedASources[key].Play();
		}
		else
		{
			// check for errors in loading
			if(assets.ContainsKey(clip))
			{
				// instancitae an empty gameObject
				GameObject tempObj = new GameObject();
				tempObj.transform.position = pos;
				tempObj.AddComponent<AudioSource>();
				tempObj.name =  "AudioSource_" + key;
				
				// assign the audiosource
				AudioSource tempA = tempObj.GetComponent<AudioSource>();
				tempA.loop = true;
				tempA.clip = assets[clip];
				
				// scale vol in respects to dist/hearDist
				if(scaleVol)
					tempA.volume = VolumePerc(pos, volume);
				else
					tempA.volume = volume;
				
				if(threeD)
					tempA.spatialBlend = 1.0f;
				
				tempA.Play();
				
				// add to tracked Dict
				trackedASources.Add(key, tempA);
			}
			else
				Debug.Log("The requested audio does not exist: (" + clip.ToString() + "). Check that your enum is up to date.");
		}
	}
	
	/// return a float ranging from 0.0f to 1.0f in respects to dist percent
	public float VolumePerc(Vector3 soundPos, float vol)
	{
		float dist = Vector3.Distance (soundPos, listenerObj.transform.position);
		
		// out of hearing range
		if(dist >= hearDist)
			return 0.0f;
		else
		{
			return vol * (1.0f - dist / hearDist);
		}
	}
	
	/// Updates the tracked audio source's volume in respects to dist percent
	public void UpdateVol(string key, float startVol, Vector3 pos)
	{
		if(trackedASources.ContainsKey(key))
			trackedASources[key].volume = VolumePerc(pos, startVol);
		else
			Debug.Log("The key (" + key + ") does not exist in the tracked audio" +
			          "sources. The key may have been misspelt or the source has been destroyed.");
	}
	
	/// destroy the specefied tracked audio source
	public void DestroySource(string key)
	{
		// key exists
		if(trackedASources.ContainsKey(key))
		{
			Destroy(trackedASources[key].gameObject);
			trackedASources.Remove(key);
		}
		else
			Debug.Log("The key (" + key + ") does not exist in the tracked audio" +
			          "sources. The key may have been misspelt or the source has been destroyed.");
	}
	
	/// destroys all tracked audio sources
	public void DestroyAllSources()
	{
		List<string> tempL = new List<string>();
		
		// iterate the tracked Dict ::CAUTION:: can not delete items while iterating dict like this::
		foreach(KeyValuePair<string, AudioSource> entry in trackedASources)
		{
			tempL.Add(entry.Key);
		}
		
		// itterate and delete
		foreach(string s in tempL)
		{
			DestroySource(s);
		}
	}
	
	/// creates a default tracked audio source
	public void CreateSource(Vector3 pos, string key)
	{
		if(trackedASources.ContainsKey(key))
		{
			Debug.Log("Tracked Audio Source list already contains an element matching Key("
			          + key + "). Use a different key or destroy the existing element");
		}
		else
		{
			// instancitae an empty gameObject
			GameObject tempObj = new GameObject();
			tempObj.transform.position = pos;
			tempObj.AddComponent<AudioSource>();
			tempObj.name = "AudioSource_" + key;
			
			// assign the audiosource
			AudioSource tempA = tempObj.GetComponent<AudioSource>();
			
			// add to tracked Dict
			trackedASources.Add(key, tempA);
		}
	}
	
	/// loads all audio assets from the resources folder
	public void LoadAudio()
	{
		// iterate the enum 'AA'
		foreach(string name in Enum.GetNames(typeof(AA)))
		{
			// load the resource and add it
			AudioClip tempClip = Resources.Load("Audio/" + name) as AudioClip;
			AA tempAA = (AA)Enum.Parse(typeof(AA), name);
			assets.Add(tempAA, tempClip);
		}
	}
	
	/// draws the hearing range of the listener object
	private void OnDrawGizmos()
	{
		if(showGizmos)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(listenerObj.transform.position, hearDist);
		}
	}
	
	/// returns the specified audio clip from the asset list
	public AudioClip GetAsset(AA clip)
	{
		AudioClip tempC = null;
		
		if(assets.ContainsKey(clip))
			tempC = assets[clip];
		else
			Debug.Log("The requested audio does not exist: (" + clip.ToString() + "). Check that your enum is up to date.");
		
		return tempC;
	}
	
	/// returns the specefied audio source
	public AudioSource GetSource(string key)
	{
		AudioSource tempAS = null;
		
		if(trackedASources.ContainsKey(key))
			tempAS = trackedASources[key];
		else
			Debug.Log("The key (" + key + ") does not exist in the tracked audio" +
			          "sources. The key may have been misspelt or the source has been destroyed.");
		
		return tempAS;
	}
}