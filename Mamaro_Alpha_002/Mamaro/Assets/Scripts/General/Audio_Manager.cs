using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Enum = System.Enum;

/// <summary>
/// This is intended to be used with the 'Enum Generator Version 1.0'
/// YOU WILL HAVE TO AA ALL TEXT "AA" WITH THE NAME OF THE ENUM GENERATED.
/// This should be attached to the main camera (where the recursive music will play from)
/// 
/// Please feel free to change any audio functions to suit as the included is just for generic and basic use.
/// 
/// </summary>

[RequireComponent(typeof(AudioSource))]
public class Audio_Manager : MonoBehaviour 
{
	// static instance of Audio_Manager
	public static Audio_Manager inst;

	// storage for all loaded audio assets
	private Dictionary<AA, AudioClip> assets = new Dictionary<AA, AudioClip>();

	// tracking instantiated recursive aSources
	private Dictionary<string, AudioSource> trackedASources = new Dictionary<string, AudioSource>();

	// the attached aSource
	private AudioSource aSource;

	// Use this for initialization
	void Awake () 
	{
		// create static instance
		if(inst == null)
			inst = this;

		// occupy dictionary
		LoadAudio ();

		// assign audioSource
		aSource = GetComponent<AudioSource>();
	}

	/// plays requested audio clip at this position
	public void PlayOnce(AA clip)
	{
		// check if the clip exists (error prompting)
		if (assets.ContainsKey(clip))
			aSource.PlayOneShot(assets[clip]);
		else
			Debug.LogError("The requested audio does not exist: " + clip.ToString());
	}

	/// plays audio at specified position once
	public void PlayOnce(AA clip, Vector3 pos)
	{
		// check for errors in loading
		if(assets.ContainsKey(clip))
			AudioSource.PlayClipAtPoint(assets[clip], pos);
		else
			Debug.LogError("The requested audio does not exist: " + clip.ToString());
	}

	/// plays recursive audio using attached audio source
	public void PlayRecursive(AA clip)
	{
		// set aSource to loop
		aSource.loop = true;

		// play clip
		aSource.clip = assets[clip];
		aSource.Play();
	}
	
	/// instanciates a recursive audio source at the specified position. specefied key will be used for future access/destruction. spacial blend defaults to 1.0f(3D) set to 0.0f for 2D(audio heard every where).min dist determines how far you can hear the audio, default is 1.0f.
	public void PlayRecursive(AA clip, Vector3 pos, string key, float spatialBlend = 1.0f, float minDist = 0.5f)
	{
		// key exists
		if(trackedASources.ContainsKey(key))
		{
			// set up audio source
			trackedASources[key].transform.position = pos;
			trackedASources[key].clip = assets[clip];
			trackedASources[key].loop = true;
			trackedASources[key].Play();
		}
		else
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
			tempA.Play();
			
			// set spatial blend and min distance
			tempA.spatialBlend = Mathf.Clamp(spatialBlend, 0.0f, 1.0f);
			tempA.minDistance = minDist;
			
			// add to tracked Dict
			trackedASources.Add(key, tempA);
		}
	}

	/// sets play on an existing tracked recursive audio source.
	public void PlayRecursive(string key)
	{
		// key exists
		if(trackedASources.ContainsKey(key))
			trackedASources[key].Play();
		else
			Debug.LogError("The key (" + key + ") does not exist. The audio source may have been destroyed or check key spelling.");
	}


	/// pauses the attached recursive audio souce 
	public void PauseRecursive()
	{
		aSource.Pause();
	}

	/// pauses the tracked recursive audio source
	public void PauseRecursive(string key)
	{
		// key exists
		if(trackedASources.ContainsKey(key))
			trackedASources[key].Pause();
		else
			Debug.LogError("The key (" + key + ") does not exist. The audio source may have been destroyed or check key spelling.");
	}

	/// un-pause the attached recursive audio souce 
	public void UnPauseRecursive()
	{
		aSource.UnPause();
	}
	
	/// un-pauses the tracked recursive audio source
	public void UnPauseRecursive(string key)
	{
		// key exists
		if(trackedASources.ContainsKey(key))
			trackedASources[key].UnPause();
		else
			Debug.LogError("The key (" + key + ") does not exist. The audio source may have been destroyed or check key spelling.");
	}

	/// stops the attached recursive audio source
	public void StopRecursive()
	{
		aSource.Stop();
	}

	/// stops the tracked recursive audio source
	public void StopRecursive(string key)
	{
		// key exists
		if(trackedASources.ContainsKey(key))
			trackedASources[key].Stop();
		else
			Debug.LogError("The key (" + key + ") does not exist. The audio source may have been destroyed or check key spelling.");
	}

	/// destroy the tracked recursive audio source
	public void DestroyRecursive(string key)
	{
		// key exists
		if(trackedASources.ContainsKey(key))
		{
			Destroy(trackedASources[key].gameObject);
			trackedASources.Remove(key);
		}
		else
			Debug.LogError("The key (" + key + ") does not exist. The audio source may have been destroyed or check key spelling.");
	}

	/// destroys all tracked recursive audio sources bar the one attached.
	public void DestroyAllRecursive()
	{
		List<string> tempL = new List<string>();

		// itterate the tracked Dict ::CAUTION:: can not delete items while itterating dict like this::
		foreach(KeyValuePair<string, AudioSource> entry in trackedASources)
		{
			tempL.Add(entry.Key);
		}

		// itterate and delete
		foreach(string s in tempL)
		{
			DestroyRecursive(s);
		}
	}

	/// returns the world position of the tracked recursive audio source
	public Vector3 GetRecursivePos(string key)
	{
		return trackedASources[key].transform.position;
	}

	/// returns true if the attached audio source is playing (returns false if paused)
	public bool IsPlaying()
	{
		return aSource.isPlaying;
	}

	/// returns true if the tracked recusrive audio source is playing (returns false if paused)
	public bool IsPlaying(string key)
	{
		return trackedASources[key].isPlaying;
	}

	/// returns true if the tracked recursive audio source exists
	public bool RecursiveExists(string key)
	{
		return trackedASources.ContainsKey(key);
	}

	/// returns the string name of the currently assigned clip
	public string GetClipPlaying()
	{
		// make  sure an audio clip is actually assigned
		if(aSource.clip != null)
			return aSource.clip.name;
		else
		{
			Debug.LogError("There was no audio clip assigned to the requested audioSource");
			return "Error";
		}

	}

	/// returns the string name of the currently assigned clip
	public string GetClipPlaying(string key)
	{
		// key exists
		if(trackedASources.ContainsKey(key))
		{
			// make  sure an audio clip is actually assigned
			if(trackedASources[key].clip != null)
				return trackedASources[key].clip.name;
			else
			{
				Debug.LogError("There was no audio clip assigned to the requested audioSource");
				return "Error";
			}
		}
		else
		{
			Debug.LogError("Key (" + ") does not exist");
			return "Error";
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
}
