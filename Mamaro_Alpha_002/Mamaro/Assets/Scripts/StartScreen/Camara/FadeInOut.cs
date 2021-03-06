﻿using UnityEngine;
using System.Collections;

public class FadeInOut : MonoBehaviour {
	public float fadeSpeed = 1.5f; 

	public bool sceneStarting = true, washStarting = false, sceneEnding = false, nextScene = false, sceneReseting = false, nextSceneTwo = false;
	private MeshFilter layerMesh;
	// Use this for initialization
	void Awake () 
	{
		layerMesh = transform.GetComponent<MeshFilter> ();
		//layerMesh.
		//guiTexture.pixelInset = new Rect (0.0f, 0.0f, Screen.width, Screen.height);
	}

	void Start()
	{
		Time.timeScale = 1.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (washStarting) {
			StartWash();
		}

		if (sceneEnding) 
		{
			EndScene();
		}

		if (sceneStarting) 
		{
			StartScene();
		}

		if(nextScene)
		{
			StartNextScene();
		}

		if(sceneReseting)
		{
			ResetScene();
		}

		if(nextSceneTwo)
		{
			StartNextSceneTwo();
		}
	}

	void FadeToClear()
	{
		layerMesh.GetComponent<Renderer>().material.color = Color.Lerp (layerMesh.GetComponent<Renderer>().material.color, Color.clear, fadeSpeed * Time.deltaTime);
		//guiTexture.color = Color.Lerp (guiTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
	}

	void FadetoBlack()
	{
		layerMesh.GetComponent<Renderer>().material.color = Color.Lerp (layerMesh.GetComponent<Renderer>().material.color, Color.black, fadeSpeed * Time.deltaTime);
		//guiTexture.color = Color.Lerp (guiTexture.color, Color.black, fadeSpeed * Time.deltaTime);
	}

	void WhiteWash()
	{
		layerMesh.GetComponent<Renderer>().material.color = Color.Lerp (layerMesh.GetComponent<Renderer>().material.color, Color.white, fadeSpeed * Time.deltaTime);
		//guiTexture.color = Color.Lerp (guiTexture.color, Color.black, fadeSpeed * Time.deltaTime);
	}

	void StartWash()
	{
		layerMesh.GetComponent<Renderer>().enabled = true;
		WhiteWash ();
		//3d Based Fade in out
		if (layerMesh.GetComponent<Renderer>().material.color.a <= 0.05f) 
		{
			layerMesh.GetComponent<Renderer>().material.color = Color.clear;
			sceneStarting = false;
		}
		//Gui Based fade in out
		/*
		if (guiTexture.color.a <= 0.05f) 
		{
			guiTexture.color = Color.clear;
			guiTexture.enabled = false;
			sceneStarting = false;
		}
		*/
	}

	void StartScene()
	{
		FadeToClear ();
		//3d Based Fade in out
		if (layerMesh.GetComponent<Renderer>().material.color.a <= 0.05f) 
		{
			layerMesh.GetComponent<Renderer>().material.color = Color.clear;
			layerMesh.GetComponent<Renderer>().enabled = false;
			sceneStarting = false;
		}
		//Gui Based fade in out
		/*
		if (guiTexture.color.a <= 0.05f) 
		{
			guiTexture.color = Color.clear;
			guiTexture.enabled = false;
			sceneStarting = false;
		}
		*/
	}

	public void StartNextScene()
	{
		layerMesh.GetComponent<Renderer>().enabled = true;
		//guiTexture.enabled = true;
		WhiteWash ();
		
		if (layerMesh.GetComponent<Renderer>().material.color.a >= 0.99f) 
		{
			Application.LoadLevel("Level");
		}
		
		/*
		if (guiTexture.color.a <= 0.05f) 
		{
			Application.LoadLevel(0);
		}
		*/
	}

	public void StartNextSceneTwo()
	{
		layerMesh.GetComponent<Renderer>().enabled = true;
		//guiTexture.enabled = true;
		WhiteWash ();
		
		if (layerMesh.GetComponent<Renderer>().material.color.a >= 0.99f) 
		{
			Application.LoadLevel("Level");
		}
		
		/*
		if (guiTexture.color.a <= 0.05f) 
		{
			Application.LoadLevel(0);
		}
		*/
	}

	public void ResetScene()
	{
		layerMesh.GetComponent<Renderer>().enabled = true;
		//guiTexture.enabled = true;
		WhiteWash ();
		
		if (layerMesh.GetComponent<Renderer>().material.color.a >= 0.99f) 
		{
			Application.LoadLevel(Application.loadedLevel);
		}
		
		/*
		if (guiTexture.color.a <= 0.05f) 
		{
			Application.LoadLevel(0);
		}
		*/
	}

	public void EndScene()
	{
		layerMesh.GetComponent<Renderer>().enabled = true;
		//guiTexture.enabled = true;
		WhiteWash ();

		if (layerMesh.GetComponent<Renderer>().material.color.a >= 0.99f) 
		{
			Application.LoadLevel("Credits");
		}

		/*
		if (guiTexture.color.a <= 0.05f) 
		{
			Application.LoadLevel(0);
		}
		*/
	}

	void OnCollisionEnter()
	{
		//EndScene ();
	}
}
