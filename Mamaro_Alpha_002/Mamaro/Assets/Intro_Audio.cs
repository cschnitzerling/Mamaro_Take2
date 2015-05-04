using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class Intro_Audio : MonoBehaviour {

	public AudioMixerSnapshot off;
	public float transSpeed = 3.5f;
	public Transform target;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (transform.position == target.position)
			off.TransitionTo (transSpeed);
	}
}
