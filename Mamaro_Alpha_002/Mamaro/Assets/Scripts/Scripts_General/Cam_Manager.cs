using UnityEngine;
using System.Collections;

/*
 * Use: To apply movement to the camera and effects
 * Has: Methods to Lerp to a position, Cam shake effect with three user defined levels of intensity.
 * Can: Adjust the speed in which the camera moves, An amplifier for intensity (Which is applied to
 * 		all three presets), the rate that the cam shake effect wears off.
*/

// GLOBAL: for passed parameter of shake func
public enum Shake {Small, Medium, Large};
public enum CamPos {Original, Malfunction};

public class Cam_Manager : MonoBehaviour 
{
	// static class instance
	public static Cam_Manager inst;
	
	// inspector assigned vars
	public Camera cam;
	public float camSpeed;
	[Range(0.1f, 0.5f)]
	public float intensityBuff = 0.25f;
	[Range(1.0f, 5.0f)]
	public float easeRate = 1.0f;
	[Range(0.1f, 3.0f)]
	public float smallShake = 0.5f, mediumShake = 0.9f, largeShake = 1.4f;
	public Vector3 camMalfuncPos;

	// private vars
	private float shake;
	private Vector3 currentPos;
	private Vector3 targetPos;
	private Vector3 startPos;
	private Vector3 originalPos;
	private bool isMoving = false;
	private float lerpInc;

	void Awake()
	{
		if (inst == null)
			inst = this;
	}

	void Start()
	{
		currentPos = transform.localPosition;
		originalPos = transform.localPosition;
	}

	void Update()
	{
		// testing /////////////////////////////////////////////////////////////
		if (Input.GetKeyDown(KeyCode.F1))								///////
			ShakeCam(Shake.Small);									   ///////
																	  ///////
		if (Input.GetKeyDown(KeyCode.F2))							 ///////
			ShakeCam(Shake.Medium);									///////
																   ///////
		if (Input.GetKeyDown(KeyCode.F3))						  ///////
			ShakeCam(Shake.Large);								 ///////
		///////////////////////////////////////////////////////////////

		// is currently moving to another position
		if(isMoving)
		{

			print ("hit");
			// apply appropriate ease interpolation
			LerpLin();
			currentPos = Vector3.Lerp(startPos, targetPos, lerpInc);

			// stop once target is reached
			if(currentPos == targetPos)
			{
				isMoving = false;
				lerpInc = 0.0f;
			}
		}

		// sets the object back to the currentPos every frame before shake
		cam.transform.localPosition = currentPos;


		//TODO add gate here for when paused to prevent the screen shake
		// shake is active
		if (shake > 0.0f) 
		{
			// reduce shake size over time
			shake -= Time.deltaTime * easeRate;
			
			// allow the object to continue moving from other scripts
			if (shake < 0.0f)
				shake = 0.0f;
			
			// apply shake
			Vector3 shakePos = Random.insideUnitSphere * shake * intensityBuff;
			Vector3 camPos = cam.transform.localPosition;
			cam.transform.localPosition = new Vector3 (camPos.x + shakePos.x, camPos.y + shakePos.y, camPos.z); //+ shakePos.z);
		}
	}

	// shakes GameObject at a predefined amount
	public void ShakeCam(Shake amount)
	{
		// set startPos if not already set
		if (shake == 0.0f)
			currentPos = cam.transform.localPosition;

		// set shake amount
		switch(amount)
		{
		case Shake.Small:
			shake = smallShake;
			break;
		case Shake.Medium:
			shake = mediumShake;
			break;
		case Shake.Large:
			shake = largeShake;
			break;
		default:
			Debug.LogError("Switch statement fell through");
			break;
		}
	}

	// lerps cam towards passed position
	public void LerpTo(CamPos pos)
	{
		// assign param
		if (pos == CamPos.Malfunction)
			targetPos = camMalfuncPos;
		else
			targetPos = originalPos;

		startPos = cam.transform.localPosition;
		isMoving = true;
	}
	
	// a linear interpolation
	private void LerpLin()
	{
		lerpInc += (Time.deltaTime * camSpeed);
	}
}
