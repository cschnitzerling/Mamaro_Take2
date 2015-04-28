using UnityEngine;
using System.Collections;

public class BackToMain : MonoBehaviour 
{
	public void LoadMainMenu()
	{
		Application.LoadLevel("SplashScreen");
	}
}
