using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Malfunction_UI : MonoBehaviour {

	public SpriteRenderer imageAButton;
	public Sprite spriteButUp;
	public Sprite spriteButDown;
	[Range(0.1f, 100.0f)]
	public float butFlashRate = 50.0f;
	
	private float altTimer = 0.0f;
	
	
	public void AlternateImages(SpriteRenderer i, Sprite a, Sprite b, float rate)
	{
		altTimer += Time.deltaTime;
		if(altTimer >= (100.0f - rate) / 100.0f)
		{
			altTimer = 0.0f;
			
			// change to the other sprite
			if(i.sprite == a)
				i.sprite = b;
			else
				i.sprite = a;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		AlternateImages(imageAButton,spriteButUp,spriteButDown,butFlashRate);
	}
}
