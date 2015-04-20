using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Comic_Camera_Move : MonoBehaviour {

	public List<Transform> targets = new List<Transform>();
	public Transform target, curPos;
	public int counter;
	// Use this for initialization
	void Awake () {
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
		if (target == null) {
			target = targets [0];
		} else {
			int Index = targets.IndexOf(target);
			if(Index < targets.Count - 1)
			{
				Index ++;
			}else{
				Index = 0;
			}
			//DeselectTarget();
			target = targets[Index];
		}
		//SelectTarget();
	}


	// Update is called once per frame
	void Update () {
		if (target == null) {
			target = targets [0];
		}
		SortedList ();
		curPos = gameObject.transform;
		if (Input.GetKeyDown ("1")) {
			NewTarget();
		}
		if (target != null) {
			transform.position = Vector3.Lerp (curPos.position, target.position, .05f);
		}
	}
}
