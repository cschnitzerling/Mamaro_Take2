using UnityEngine;
using System.Collections;

public class HUDButtonScript : MonoBehaviour {
	public bool mouseOver;
	public HUDScript hudScriptPuased;
	public bool puaseActive, MenuisActive, OptionsActive, isDrag;
	public Vector3 mousePosition, sliderCurPos, newSliderPos, SliderPosX, sliderViewPos, localisedSliderOffset,fullScreen, parentScale;
	public float sliderOffset, distanceCheck;
	// Use this for initialization
	void Awake(){
		isDrag = false;
		}
	void Start () {
			Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			hudScriptPuased = transform.GetComponentInParent<HUDScript> ();
	}
	public void OnMouseEnter(){
		mouseOver = true;
	}

	public void OnMouseOver(){
		mouseOver = true;
	}

	public void OnMouseExit(){
		mouseOver = false;
	}


	// Update is called once per frame
	void Update () {
		sliderCurPos = new Vector3 (0, 0, 0);
		mousePosition = Input.mousePosition;
		mousePosition = Camera.main.ScreenToViewportPoint(mousePosition);
		sliderViewPos =  Camera.main.WorldToViewportPoint(transform.localPosition);
		parentScale = Camera.main.WorldToViewportPoint(transform.parent.localScale);

		sliderCurPos.x = mousePosition.x;
		mousePosition.x = Mathf.Clamp (mousePosition.x, 0.33f, 0.587f);
		sliderCurPos.x = Mathf.Clamp(sliderCurPos.x, 0.33f, 0.587f);
		sliderOffset = ((fullScreen.x - parentScale.x));
		fullScreen = Camera.main.ScreenToViewportPoint(new Vector3(Screen.width,Screen.height,10));
		distanceCheck = sliderOffset - mousePosition.x;

				puaseActive = hudScriptPuased.isPause;
				MenuisActive = hudScriptPuased.menuActive;
				OptionsActive = hudScriptPuased.OptionsActive;


				if (mouseOver) {
						if (mouseOver && Input.GetMouseButton (0) && transform.name == "VolumeSliderButton" && OptionsActive) {
								isDrag = true;
								Debug.Log ("VolumeSliderButton");
								if (!isDrag || !mouseOver) {
										transform.localPosition = sliderCurPos;
								}
								if (isDrag) {
					transform.localPosition = (sliderCurPos * 4) + new Vector3 (-1.321101f, 0, 0);
					//transform.localPosition = (sliderCurPos);
										sliderCurPos = transform.localPosition;
								} else {
										isDrag = false;
										mouseOver = false;
								}
						} else {
								isDrag = false;
								mouseOver = false;
						}
						if (Input.GetMouseButtonDown (0)) {
								if (transform.name == "MenuButton" && !puaseActive && !MenuisActive) {
										Debug.Log ("OverMouseMenu");
										mouseOver = false;
										puaseActive = true;
										hudScriptPuased.isPause = true;
				}
								if(transform.name == "ResumeButton" && puaseActive) {
										Debug.Log ("OverMouseResume");
										mouseOver = false;
										hudScriptPuased.isPause = false;
										puaseActive = false;
				}

				if(transform.name == "OptionButton" && puaseActive) {
					Debug.Log ("OverMouseOption");
					mouseOver = false;
					OptionsActive = true;
					hudScriptPuased.OptionsActive = true;
				}

								if (transform.name == "GoBackStartHUDButton" && puaseActive) {
										Application.LoadLevel ("StartScreen");
								}

				if(transform.name == "BackButton" && puaseActive) {
					Debug.Log ("OverMouseOption");
					mouseOver = false;
					OptionsActive = false;
					hudScriptPuased.OptionsActive = false;
				}
						}
				}

		}
}


