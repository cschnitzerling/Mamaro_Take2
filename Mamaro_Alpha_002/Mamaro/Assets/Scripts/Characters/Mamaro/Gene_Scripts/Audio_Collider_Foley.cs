using UnityEngine;
using System.Collections;

public class Audio_Collider_Foley : MonoBehaviour {
	public AudioClip hitSound, lucyGasp1, lucyGasp2, lucyGasp3, lucyGasp4;
	public AudioClip lucyGasp;
	public bool playSound, lucyGasping;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void HitFoley(){
		if(playSound){
			AudioSource.PlayClipAtPoint (hitSound, Camera.main.transform.position, 1f);
			LucyScreamSwitch();
			if(lucyGasping){
				//AudioSource.PlayClipAtPoint (lucyGasp, Camera.main.transform.position, .5f);
			lucyGasping = false;
			}
		}
	}
	
	void LucyScreamSwitch(){
		int dice = Random.Range(0,5);
		if(dice == 1){
			lucyGasp = lucyGasp1;
		} else if(dice == 2){
			lucyGasp = lucyGasp2;
		}
		else if(dice == 3){
			lucyGasp = lucyGasp3;
		}
		else if(dice == 4){
			lucyGasp = lucyGasp4;
		}
	}
}
