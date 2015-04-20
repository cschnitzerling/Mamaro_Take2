using UnityEngine;
using System.Collections;

public class Pickup_UI : MonoBehaviour {

	public static Pickup_UI inst;

	public RectTransform buttonX;
	public RectTransform buttonY;

	void Awake()
	{
		inst = this;

		gameObject.SetActive(false);
	}

}
