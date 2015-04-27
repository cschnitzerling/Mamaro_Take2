using UnityEngine;
using System.Collections;

public class AnimatorAudio : MonoBehaviour {

	public void PlayAudio()
	{
		Audio_Manager.inst.PlayOnce(AA.Chr_Mamaro_Movement_Standalone_Footstep_2,Camera.main.transform.position);
	}
}
