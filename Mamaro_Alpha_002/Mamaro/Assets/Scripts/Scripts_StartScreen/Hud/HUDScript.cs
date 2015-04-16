using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class HUDScript : MonoBehaviour {
	public bool isPause, menuActive, OptionsActive;
	public HUDButtonScript[] childbuttons = new HUDButtonScript[0];
	public Transform pauseMenu, OptionsMenu, hudMenuButton;
	// Use this for initialization
	void Awake(){
		childbuttons = new HUDButtonScript[this.transform.GetComponentsInChildren<HUDButtonScript>().Length];
		for(int i =0; i < this.transform.GetComponentsInChildren<HUDButtonScript>().Length; i ++){
			childbuttons[i] = this.transform.GetComponentsInChildren<HUDButtonScript>()[i];
		}
				isPause = false;
		}

	void Start () {
		pauseMenu = this.transform.GetChild (0);
		OptionsMenu = this.transform.GetChild (2);
		hudMenuButton = this.transform.GetChild (1).GetChild (0);
	}
	

	
	// Update is called once per frame
	void Update () {
		//print position of mouse

		if (!isPause) {
						childbuttons [2].mouseOver = false;
				}

		if (isPause && !OptionsActive) {
						childbuttons [2].mouseOver = false;
						menuActive = true;
						Time.timeScale = 0;
						pauseMenu.gameObject.SetActive (true);
						OptionsMenu.gameObject.SetActive (false);
						hudMenuButton.gameObject.SetActive (false);
						Debug.Log (Time.timeScale.ToString ());
				}else if(OptionsActive && isPause){
				pauseMenu.gameObject.SetActive (false);
				this.transform.GetChild(2).gameObject.SetActive(true);
			}else {
			OptionsActive = false;
			this.transform.GetChild (2).gameObject.SetActive (false);
			Time.timeScale = 1;
			Debug.Log (Time.timeScale.ToString ());
			this.transform.GetChild(0).gameObject.SetActive(false);
			this.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
			menuActive = false;
		}

				if (Input.GetKeyDown (KeyCode.Escape)) {
						isPause = !isPause;
				}
		}

	}
