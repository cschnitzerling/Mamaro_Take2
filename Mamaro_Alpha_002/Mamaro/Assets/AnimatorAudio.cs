using UnityEngine;
using System.Collections;

public class AnimatorAudio : MonoBehaviour 
{
	[Range(0.0f, 1.0f)]
	public float StepVolume = 1.0f;

	private Audio_Manager am;
	private int stepCount = 1;

	private void Start()
	{
		am = Audio_Manager.inst;
	}

	public void PlayAudio()
	{
		stepCount++;

		// cycle between left and right foot step audio
		if(stepCount % 2 == 0)
			am.PlayOneShot(AA.Chr_Mamaro_Movement_Standalone_Footstep_1, StepVolume);
		else
			am.PlayOneShot(AA.Chr_Mamaro_Movement_Standalone_Footstep_2, StepVolume);
	}
}
