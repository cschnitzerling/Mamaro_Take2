using UnityEngine;
using System.Collections;

public class End_Scene_TimeScale : MonoBehaviour {
	public AudioSource musicVol;
	public MeshRenderer fadeAlph;
	public float alpha;
	// Use this for initialization
	void Start () {
		musicVol = GetComponent<AudioSource> ();
		fadeAlph = GameObject.FindGameObjectWithTag ("FadeScene").GetComponent<MeshRenderer> ();
		Time.timeScale = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		alpha = fadeAlph.material.color.a - 1;
		musicVol.volume = -alpha;
	
	}
}
