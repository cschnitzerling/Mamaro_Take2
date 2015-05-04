using UnityEngine;
using System.Collections;

public class AnimatorAudio : MonoBehaviour 
{
	[Range(0.0f, 1.0f)]
	public float StepVolume = 1.0f;
	[Range(0.0f, 1.0f)]
	public float soundVariation = 0.2f; 
	private string keyStep = "Step";

	private Audio_Manager am;
	private int stepCount = 1;
	private float originalPitch;


	private void Start()
	{
		am = Audio_Manager.inst;

		keyStep += this.GetInstanceID().ToString();

		am.CreateSource(transform.position, keyStep);
		am.GetSource(keyStep).volume = StepVolume;
		originalPitch = am.GetSource(keyStep).pitch;
	}

	public void PlayAudio()
	{
		stepCount++;

		// stop audio if overlapping
		if(am.GetSource(keyStep).isPlaying)
			am.GetSource(keyStep).Stop();

		// cycle between left and right foot step audio
		if(stepCount % 2 == 0)
		{
			am.GetSource(keyStep).clip = am.GetClip(AA.Chr_Mamaro_Movement_Footstep_1);
			float pitch = am.GetSource(keyStep).pitch;
			am.GetSource(keyStep).pitch = Random.Range(originalPitch - originalPitch * soundVariation, originalPitch + originalPitch * soundVariation);
			am.GetSource(keyStep).Play();
		}
		else
		{
			am.GetSource(keyStep).clip = am.GetClip(AA.Chr_Mamaro_Movement_Footstep_2);
			float pitch = am.GetSource(keyStep).pitch;
			am.GetSource(keyStep).pitch = Random.Range(originalPitch - originalPitch * soundVariation, originalPitch + originalPitch * soundVariation);
			am.GetSource(keyStep).Play();
		}
	}
}
