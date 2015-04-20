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

	// tracking one shot objects
	private List<AudioSource> oneShots = new List<AudioSource>();

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

	/// plays requested audio clip at this position
	public void PlayOnce(AA clip)
	{
		// check for errors in loading
		if (assets.ContainsKey(clip))
			aSource.PlayOneShot(assets[clip]);
		else
			Debug.LogError("The requested audio does not exist: (" + clip.ToString() + "). Check that your enum is up to date.");
	}

	/// plays audio at specified position once
	public void PlayOnce(AA clip, Vector3 pos)
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
			tempA.spatialBlend = 1.0f;
			tempA.clip = assets[clip];
			tempA.Play();

			
			// add to tracked oneShots
			oneShots.Add(tempA);
		}
		else
			Debug.LogError("The requested audio does not exist: (" + clip.ToString() + "). Check that your enum is up to date.");
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

	/// plays recursive music at specified position. Specefied key will be used for future access/destruction
	public void PlayRecursive(AA clip, Vector3 pos, string key)
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
				tempA.Play();
				
				// add to tracked Dict
				trackedASources.Add(key, tempA);
			}
			else
				Debug.LogError("The requested audio does not exist: (" + clip.ToString() + "). Check that your enum is up to date.");
		}
	}

	/// plays an existing recursive audio source
	public void PlayRecursive(string key)
	{
		// key exists
		if(trackedASources.ContainsKey(key))
			trackedASources[key].Play();
		else
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
	}
	
	/// pauses the attached recursive audio source 
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
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
	}

	/// un-pause the attached recursive audio source 
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
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
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
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
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
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
	}

	/// destroys all tracked recursive audio sources bar the one attached.
	public void DestroyAllRecursive()
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
			DestroyRecursive(s);
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

	// Getters ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// returns the number of audio assets that have been loaded
	public int GetAssetCount()
	{
		return assets.Count;
	}

	/// returns the number of tracked audio sources
	public int GetRecursiveCount()
	{
		return trackedASources.Count;
	}

	/// returns a string array of the existing tracked audio keys
	public string[] GetActiveKeys()
	{
		List<string> tempS = new List<string>();

		// retrieve all keys
		foreach(string s in trackedASources.Keys)
		{
			tempS.Add(s);
		}

		return tempS.ToArray();
	}

	/// returns true if the tracked recursive audio source exists
	public bool RecursiveExists(string key)
	{
		return trackedASources.ContainsKey(key);
	}

	/// returns the world position of the tracked recursive audio source
	public Vector3 GetRecursivePos(string key)
	{
		// key exists
		if (trackedASources.ContainsKey (key))
			return trackedASources[key].transform.position;
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
			return Vector3.zero; // error return
		}
	}
	
	/// returns true if the attached audio source is playing (returns false if paused)
	public bool IsPlaying()
	{
		return aSource.isPlaying;
	}
	
	/// returns true if the tracked recursive audio source is playing (returns false if paused)
	public bool IsPlaying(string key)
	{
		// key exists
		if (trackedASources.ContainsKey (key))
			return trackedASources[key].isPlaying;
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
			return false; // error return
		}
	}
	
	/// Returns the main audio source *(It's recommended that you use the getters and setters provided to alter the audio source)*
	public AudioSource GetAudioSource()
	{
		return aSource;
	}
	
		/// Returns the audio source *(It's recommended that you use the getters and setters provided to alter the audio source)*
	public AudioSource GetAudioSource(string key)
	{
				// key exists
		if (trackedASources.ContainsKey (key))
			return trackedASources[key];
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
			return null; // error return
		}
	}

	/// returns the main audio source currently assigned audio clip
	public AudioClip GetCurrentClip()
	{
		// make  sure an audio clip is actually assigned
		if(aSource.clip != null)
		{
			return aSource.clip;
		}
		else
		{
			Debug.LogError("There was no audio clip assigned to the requested audioSource");
			return null;
		}
	}
	
	/// returns the audio source currently assigned audio clip
	public AudioClip GetCurrentClip(string key)
	{
		// key exists
		if (trackedASources.ContainsKey (key))
		{
			// make  sure an audio clip is actually assigned
			if(trackedASources[key].clip != null)
			{
				return trackedASources[key].clip;
			}
			else
			{
				Debug.LogError("There was no audio clip assigned to the requested audioSource");
				return null;
			}
		}
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
			return null; // error return
		}
	}

	/// returns true if the main audio source is set to Mute
	public bool GetMute()
	{
		return aSource.mute;
	}

	/// returns true if the audio source is set to Mute
	public bool GetMute(string key)
	{
		// key exists
		if (trackedASources.ContainsKey (key))
			return trackedASources[key].mute;
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
			return false; // error return
		}
	}

	/// returns true if the main audio source is set to Bypass Effects
	public bool GetBypassEffects()
	{
		return aSource.bypassEffects;
	}

	/// returns true if the audio source is set to Bypass Effects
	public bool GetBypassEffects(string key)
	{
		// key exists
		if (trackedASources.ContainsKey(key))
			return trackedASources[key].bypassEffects;
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
			return false; // error return
		}
	}

	/// returns true if the main audio source is set to Bypass Listener Effects
	public bool GetBypassListenerEffects()
	{
		return aSource.bypassListenerEffects;
	}

	/// returns true if the audio source is set to Bypass Listener Effects
	public bool GetBypassListenerEffects(string key)
	{
		// key exists
		if (trackedASources.ContainsKey(key))
			return trackedASources[key].bypassListenerEffects;
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
			return false; // error return
		}
	}

	/// returns true if the main audio source is set to Bypass Reverb Zones
	public bool GetBypassReverbZones()
	{
		return aSource.bypassReverbZones;
	}

	/// returns true if the audio source is set to Bypass Reverb Zones
	public bool GetBypassReverbZones(string key)
	{
		// key exists
		if (trackedASources.ContainsKey(key))
			return trackedASources[key].bypassReverbZones;
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
			return false; // error return
		}
	}

	/// returns true if the main audio source is set to Play On Awake
	public bool GetPlayOnAwake()
	{
		return aSource.playOnAwake;
	}

	/// returns true if the audio source is set to Play On Awake
	public bool GetPlayOnAwake(string key)
	{
		// key exists
		if (trackedASources.ContainsKey(key))
			return trackedASources[key].playOnAwake;
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
			return false; // error return
		}
	}

	/// returns true is the main audio source is set to Loop
	public bool GetLoop()
	{
		return aSource.loop;
	}

	/// returns true is the audio source is set to Loop
	public bool GetLoop(string key)
	{
		// key exists
		if (trackedASources.ContainsKey(key))
			return trackedASources[key].loop;
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
			return false; // error return
		}
	}

	/// returns the main audio source Priority value
	public int GetPriority()
	{
		return aSource.priority;
	}

	/// returns the audio source Priority value
	public int GetPriority(string key)
	{
		// key exists
		if (trackedASources.ContainsKey(key))
			return trackedASources[key].priority;
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
			return 0; // error return
		}
	}

	/// returns the main audio source Volume level
	public float GetVolume()
	{
		return aSource.volume;
	}

	/// returns the audio source Volume level
	public float GetVolume(string key)
	{
		// key exists
		if (trackedASources.ContainsKey(key))
			return trackedASources[key].volume;
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
			return 0.0f; // error return
		}
	}

	/// returns the main audio source Pitch value
	public float GetPitch()
	{
		return aSource.pitch;
	}

	/// returns the audio source Pitch value
	public float GetPitch(string key)
	{
		// key exists
		if (trackedASources.ContainsKey(key))
			return trackedASources[key].pitch;
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
			return 0.0f; // error return
		}
	}

	/// returns the main audio source Stereo Pan value
	public float GetStereoPan()
	{
		return aSource.panStereo;
	}

	/// returns the audio source Stereo Pan value
	public float GetStereoPan(string key)
	{
		// key exists
		if (trackedASources.ContainsKey(key))
			return trackedASources[key].panStereo;
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
			return 0.0f; // error return
		}
	}

	/// returns the main audio source Spatial Blend value
	public float GetSpacialBlend()
	{
		return aSource.spatialBlend;
	}

	/// returns the audio source Spatial Blend value
	public float GetSpacialBlend(string key)
	{
		// key exists
		if (trackedASources.ContainsKey(key))
			return trackedASources[key].spatialBlend;
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
			return 0.0f; // error return
		}
	}

	/// returns the main audio source Reverb Zone Mix value
	public float GetReverbZoneMix()
	{
		return aSource.reverbZoneMix;
	}
	
	/// returns the audio source Reverb Zone Mix value
	public float GetReverbZoneMix(string key)
	{
		// key exists
		if (trackedASources.ContainsKey(key))
			return trackedASources[key].reverbZoneMix;
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
			return 0.0f; // error return
		}
	}

	/// returns the main audio source Doppler Level value
	public float GetDopplerLevel()
	{
		return aSource.dopplerLevel;
	}

	/// returns the main audio source Doppler Level value
	public float GetDopplerLevel(string key)
	{
		// key exists
		if (trackedASources.ContainsKey(key))
			return trackedASources[key].dopplerLevel;
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
			return 0.0f; // error return
		}
	}

	/// returns the main audio source Volume Rolloff mode
	public AudioRolloffMode GetVolumeRolloff()
	{
		return aSource.rolloffMode;
	}

	/// returns the audio source Volume Rolloff mode
	public AudioRolloffMode GetVolumeRolloff(string key)
	{
		// key exists
		if (trackedASources.ContainsKey(key))
			return trackedASources[key].rolloffMode;
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
			return AudioRolloffMode.Logarithmic; // error return
		}
	}

	/// returns the main audio source Min Distance value
	public float GetMinDist()
	{
		return aSource.minDistance;
	}

	/// returns the audio source Min Distance value
	public float GetMinDist(string key)
	{
		// key exists
		if (trackedASources.ContainsKey(key))
			return trackedASources[key].minDistance;
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
			return 0.0f; // error return
		}
	}

	/// returns the main audio source Spread value
	public float GetSpread()
	{
		return aSource.spread;
	}

	/// returns the audio source Spread value
	public float GetSpread(string key)
	{
		// key exists
		if (trackedASources.ContainsKey(key))
			return trackedASources[key].spread;
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
			return 0.0f; // error return
		}
	}

	/// returns the main audio source Max Distance value
	public float GetMaxDist()
	{
		return aSource.maxDistance;
	}

	/// returns the audio source Max Distance value
	public float GetMaxDist(string key)
	{
		// key exists
		if (trackedASources.ContainsKey(key))
			return trackedASources[key].maxDistance;
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
			return 0.0f; // error return
		}
	}

	// Setters ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// sets the position of the tracked recursive audio source
	public void SetRecursivePos(string key, Vector3 pos)
	{
		// key exists
		if (trackedASources.ContainsKey (key))
			trackedASources [key].transform.position = pos;
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
		}
	}

	/// sets the position of the tracked recursive audio source
	public void SetRecursivePos(string key, float x, float y, float z)
	{
		// key exists
		if (trackedASources.ContainsKey (key))
			trackedASources[key].transform.position = new Vector3(x, y, z);
		else
		{
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
		}
	}

	/// manually sets the desired audio clip to the main audio source
	public void SetClip(AudioClip clip)
	{
		aSource.clip = clip;
	}

	/// manually sets the desired audio clip to the audio source
	public void SetClip(string key, AudioClip clip)
	{
		// key exists
		if (trackedASources.ContainsKey(key))
			trackedASources[key].clip = clip;
		else
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
	}

	/// sets the main audio source mute setting
	public void SetMute(bool mute)
	{
		aSource.mute = mute;
	}

	/// sets the audio source mute setting
	public void SetMute(string key, bool mute)
	{
		// key exists
		if (trackedASources.ContainsKey (key))
			trackedASources[key].mute = mute;
		else
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
	}

	/// sets the main audio source Bypass Effects setting
	public void SetBypassEffects(bool on)
	{
		aSource.bypassEffects = on;
	}

	/// sets the audio source Bypass Effects setting
	public void SetBypassEffects(string key, bool on)
	{
		// key exists
		if (trackedASources.ContainsKey (key))
			trackedASources[key].bypassEffects = on;
		else
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
	}

	/// sets the main audio source Bypass Listener Effects
	public void SetBypassListenerEffects(bool on)
	{
		aSource.bypassListenerEffects = on;
	}

	/// sets the audio source Bypass Listener Effects
	public void SetBypassListenerEffects(string key, bool on)
	{
		// key exists
		if (trackedASources.ContainsKey (key))
			trackedASources[key].bypassListenerEffects = on;
		else
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
	}

	/// sets the main audio source Bypass Reverb Zones setting
	public void SetBypassReverbZones(bool on)
	{
		aSource.bypassReverbZones = on;
	}

	/// sets the audio source Bypass Reverb Zones setting
	public void SetBypassReverbZones(string key, bool on)
	{
		// key exists
		if (trackedASources.ContainsKey (key))
			trackedASources[key].bypassReverbZones = on;
		else
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
	}

	/// sets the main audio source Play On Awake setting
	public void SetPlayOnAwake(bool on)
	{
		aSource.playOnAwake = on;
	}

	/// sets the audio source Play On Awake setting
	public void SetPlayOnAwake(string key, bool on)
	{
		// key exists
		if (trackedASources.ContainsKey (key))
			trackedASources[key].playOnAwake = on;
		else
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
	}

	/// sets the main audio source Loop setting
	public void SetLoop(bool on)
	{
		aSource.loop = on;
	}

	/// sets the audio source Loop setting
	public void SetLoop(string key, bool on)
	{
		// key exists
		if (trackedASources.ContainsKey (key))
			trackedASources[key].loop = on;
		else
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
	}

	/// Sets the priority of the main audio source ranging from 0 to 256 (128 is standard)
	public void SetPriority(int level)
	{
		aSource.priority = Mathf.Clamp(level, 0, 256);
	}
	
	/// Sets the priority of the tracked audio source ranging from 0 to 256 (128 is standard)
	public void SetPriority(string key, int level)
	{
		// key exists
		if(trackedASources.ContainsKey(key))
			trackedASources[key].priority = Mathf.Clamp(level, 0, 256);
		else
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
	}

	/// sets the volume of the main audio source (1.0 is full volume)
	public void SetVolume(float level)
	{ 
		aSource.volume = Mathf.Clamp(level, 0.0f, 1.0f);
	}

	/// sets the volume of the main audio source (1.0 is full volume)
	public void SetVolume(string key, float level)
	{
		// key exists
		if(trackedASources.ContainsKey(key))
			trackedASources[key].volume = Mathf.Clamp(level, 0.0f, 1.0f);
		else
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
	}

	/// sets the pitch ranging from -3 to 3 (1 is standard pitch)
	public void SetPitch(float pitch)
	{
		aSource.pitch = Mathf.Clamp(pitch, -3.0f, 3.0f);
	}

	/// sets the pitch ranging from -3 to 3 (1 is standard pitch)
	public void SetPitch(string key, float pitch)
	{
		// key exists
		if(trackedASources.ContainsKey(key))
			trackedASources[key].pitch = Mathf.Clamp(pitch, -3.0f, 3.0f);
		else
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
	}

	/// sets the main audio source Stereo Pan level (0.0 : middle) (-1 : left) (1 : right)
	public void SetStereoPan(float level)
	{
		aSource.panStereo = Mathf.Clamp(level, -1.0f, 1.0f);
	}

	/// sets the audio source Stereo Pan level (0.0 : middle) (-1 : left) (1 : right)
	public void SetStereoPan(string key, float level)
	{
		// key exists
		if (trackedASources.ContainsKey (key))
			trackedASources [key].panStereo = Mathf.Clamp (level, -1.0f, 1.0f);
		else
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
	}

	/// sets the main audio source Spatial Blend level (0.0 : 2D) (1.0 : 3D)
	public void SetSpatialBlend(float level)
	{
		aSource.spatialBlend = Mathf.Clamp(level, 0.0f, 1.0f);
	}

	/// sets the audio source Spatial Blend level (0.0 : 2D) (1.0 : 3D)
	public void SetSpatialBlend(string key, float level)
	{
		// key exists
		if (trackedASources.ContainsKey (key))
			trackedASources[key].spatialBlend = Mathf.Clamp(level, 0.0f, 1.0f);
		else
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
	}

	/// sets the main audio source Reverb Zone Mix level range from 0.0 to 1.1
	public void SetReverbZoneMix(float level)
	{
		aSource.reverbZoneMix = Mathf.Clamp(level, 0.0f, 1.1f);
	}

	/// sets the audio source Reverb Zone Mix level range from 0.0 to 1.1
	public void SetReverbZoneMix(string key, float level)
	{
		// key exists
		if (trackedASources.ContainsKey (key))
			trackedASources[key].reverbZoneMix = Mathf.Clamp(level, 0.0f, 1.1f);
		else
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				 "sources. The key may have been misspelt or the source has been destroyed.");
	}

	/// sets the main audio source Doppler Level ranging from 0.0 to 5.0 (1.0 is standard)
	public void SetDopplerLevel(float level)
	{
		aSource.dopplerLevel = Mathf.Clamp(level, 0.0f, 5.0f);
	}

	/// sets the audio source Doppler Level ranging from 0.0 to 5.0 (1.0 is standard)
	public void SetDopplerLevel(string key, float level)
	{
		// key exists
		if (trackedASources.ContainsKey (key))
			trackedASources[key].dopplerLevel = Mathf.Clamp(level, 0.0f, 5.0f);
		else
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
	}

	/// sets the main audio source Volume Rolloff mode
	public void SetVolumeRolloff(AudioRolloffMode mode)
	{
		aSource.rolloffMode = mode;
	}

	/// sets the audio source Volume Rolloff mode
	public void SetVolumeRolloff(string key, AudioRolloffMode mode)
	{
		// key exists
		if (trackedASources.ContainsKey (key))
			trackedASources[key].rolloffMode = mode;
		else
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
	}

	/// sets the main audio source Min Distance value from 0 to infinite
	public void SetMinDist(float dist)
	{
		aSource.minDistance = Mathf.Clamp(dist, 0.0f, Mathf.Infinity);
	}

	/// sets the audio source Min Distance value 0 to infinite
	public void SetMinDist(string key, float dist)
	{
		// key exists
		if (trackedASources.ContainsKey (key))
			trackedASources[key].minDistance = Mathf.Clamp(dist, 0.0f, Mathf.Infinity);
		else
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
	}

	/// sets the main audio source Spread value from 0.0 to 360.0 (0 is standard)
	public void SetSpread(int value)
	{
		aSource.spread = Mathf.Clamp(value, 0, 360);
	}

	/// sets the audio source Spread value from 0.0 to 360.0 (0 is standard)
	public void SetSpread(string key, int value)
	{
		// key exists
		if (trackedASources.ContainsKey(key))
			trackedASources[key].spread = Mathf.Clamp(value, 0, 360);		
		else
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
	}

	/// sets the main audio source Max Distance value from 1.1 to infinite (500 is standard)
	public void SetMaxDist(float value)
	{
		aSource.maxDistance = Mathf.Clamp(value, 1.1f, Mathf.Infinity);
	}

	/// sets the audio source Max Distance value from 1.1 to infinite (500 is standard)
	public void SetMaxDist(string key, float value)
	{
		// key exists
		if (trackedASources.ContainsKey (key))
			trackedASources[key].maxDistance = Mathf.Clamp(value, 1.1f, Mathf.Infinity);	
		else
			Debug.LogError("The key (" + key + ") does not exist in the tracked audio" +
				"sources. The key may have been misspelt or the source has been destroyed.");
	}
}