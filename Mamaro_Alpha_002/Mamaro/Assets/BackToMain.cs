using UnityEngine;
using System.Collections;

public class BackToMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void LoadMainMenu()
	{
		Application.LoadLevel("StartScreen");
	}
}
