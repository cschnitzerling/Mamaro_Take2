using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Comic_Camera_Move : MonoBehaviour {

	public List<Transform> targets = new List<Transform>();
	public Transform target, curPos;
	public int counter;
	public Lightanimation_Controller animMove;
	public Comic_Trigger pauser;
	public bool isPaused;
	// Use this for initialization
	void Awake () {
		pauser = GameObject.FindGameObjectWithTag ("AtEnd").GetComponent<Comic_Trigger> ();
		animMove = GameObject.FindGameObjectWithTag ("LightTrig").GetComponent<Lightanimation_Controller> ();

		AddTargets ();
	}

	public void AddTargets()
	{
		GameObject[] targo = GameObject.FindGameObjectsWithTag ("Targets");
		
		foreach (GameObject targ in targo)
			AddTarget (targ.transform);
		
	}

	public void AddTarget(Transform target)
	{
		targets.Add (target);
	}

	public void SortedList(){
		targets = targets.OrderBy(target => target.gameObject.name).ToList();
		}

	public void NewTarget()
	{
		if (!animMove.isIn) {
			if (target == null) {
				target = targets [0];
			} else {
				int Index = targets.IndexOf (target);
				if (Index < targets.Count - 1) {
					Index ++;
				} else {
					Index = 0;
				}
				//DeselectTarget();
				target = targets [Index];
			}
			//SelectTarget();
		}
	}


	// Update is called once per frame
	void Update () {
		isPaused = pauser.isPuased;
		if (target == null) {
			target = targets [0];
		}
		SortedList ();
		curPos = gameObject.transform;
		if (Input.GetKeyDown ("1")) {
			NewTarget();
		}
		if (target != null) {
			if (!animMove.isPlaying && !animMove.hasPressed){
				if(!isPaused){
				transform.position = Vector3.Lerp (curPos.position, target.position, .05f);
				}
		}
		}
	}
}
