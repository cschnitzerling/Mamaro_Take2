using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Script_QuickTime : MonoBehaviour 
{
	// static instance var
	public static Script_QuickTime inst;

	// static access
	private Mamaro_Manager mamaro;

	// inspector assigned vars
	public Image imageButton;
	public Sprite spriteButtonUp, spriteButtonDown;
	[Range(1, 9)]
	public float altRate = 5.0f;
	[Range(0.5f, 2.0f)]
	public float difficultyOffset = 1.0f;
	public float resistRate = 1.0f;

	// private vars
	public float longestBlink = 1.0f;
	public float timerButton = 0.0f;
	public float malPercent = 0.0f;

	void Awake()
	{
		if(inst == null)
			inst = this;
	}

	// Use this for initialization
	void Start () 
	{
		mamaro = Mamaro_Manager.inst;
		timerButton = longestBlink;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// indicate user interaction
		AltImage();

		// check if reached 100 percent
		if(malPercent >= 100.0f)
		{
			malPercent = 100.0f;
			//TODO play 'lost his shit matrix gargle' audio
			//TODO begin ending cutscene
		}
		else
		{
			// increase malfunction percent in respects to total cores
			malPercent += Time.deltaTime * mamaro.GetTotalCores() * difficultyOffset;
		}
	}

	/// receives input
	public void Resist()
	{
		// shouldn't fall below zero
		if(malPercent - resistRate <= 0.0f)
			malPercent = 0.0f;
		else
			malPercent -= resistRate;
	}

	/// alternates the sprite of the button image
	private void AltImage()
	{
		// changes between sprites based on altRate to percent
		timerButton -= Time.deltaTime;
		if(timerButton <= longestBlink * (altRate / 10))
		{
			// check which image is current
			if(imageButton.sprite == spriteButtonUp)
				imageButton.sprite = spriteButtonDown;
			else
				imageButton.sprite = spriteButtonUp;
			
			timerButton = longestBlink;
		}
	}
}
