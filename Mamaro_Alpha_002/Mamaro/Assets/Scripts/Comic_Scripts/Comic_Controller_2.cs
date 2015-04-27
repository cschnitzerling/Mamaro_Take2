using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Comic_Controller_2 : MonoBehaviour {

	public List<Comic_Target> targets;

	int curTarget;

	public Color fadeColor;

	float timerPause;
	float timerMove;
	float timerFade;

	public float fadeTime;

	bool isMove;

	bool isEnd;

	public string nextScene;

	public Material fadeMaterial;


	// Use this for initialization
	void Start () 
	{
		isMove = false;
		timerPause = targets[curTarget].pauseTime;
		transform.position =  targets[curTarget].GetPosition();
		transform.rotation =  targets[curTarget].GetRotation();

		fadeColor.a = 0;
		fadeMaterial.color = fadeColor;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isEnd)
		{
			timerFade += Time.deltaTime / fadeTime;

			fadeColor.a = timerFade;
			fadeMaterial.color = fadeColor;

			if (timerFade > 1)
			{
				Application.LoadLevel(nextScene);
				print("Finish");
			}
		}
		else
		{
			if (isMove)
			{
				timerMove += Time.deltaTime / targets[curTarget].moveTime;

				transform.position = Vector3.Lerp(targets[curTarget - 1].GetPosition(), targets[curTarget].GetPosition(), timerMove);
				transform.rotation = Quaternion.Lerp(targets[curTarget - 1].GetRotation(), targets[curTarget].GetRotation(), timerMove);

				if (timerMove > 1)
				{
					transform.position =  targets[curTarget].GetPosition();
					transform.rotation =  targets[curTarget].GetRotation();
					timerMove = 0;
					isMove = false;
				}

			}
			else
			{
				timerPause -= Time.deltaTime;
				if (timerPause <= 0)
				{
					NextTarget();
					isMove = true;
				}
			}
		}
	}

	public void NextTarget()
	{
		curTarget ++;

		if (curTarget == targets.Count)
		{
			OnEndComic();
		}
		else
		{
			timerPause = targets[curTarget].pauseTime;
		}

	}

	public void OnEndComic()
	{
		isEnd = true;
	}
}
