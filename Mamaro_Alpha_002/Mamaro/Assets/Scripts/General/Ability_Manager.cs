using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Ability_Manager : MonoBehaviour 
{
	// static access instance
	public static Ability_Manager inst;
	
	// inspector assigned vars
	public AbilitySocket[] sockets = new AbilitySocket[4];
	public Text spareCoreDisplayText;


	// audio vars
	private Audio_Manager am;
	[Range(0.0f, 1.0f)]
	public float beepVolume = 1.0f;

	// private vars
	public int spareCores = 0;
	public int selectedSocket = 0;

	public Vector3 disabledScale, enabledScale;

	public UI_CoreChageSprites[] coreChangeSprites = new UI_CoreChageSprites[6];

	public float coreChangeTime;
	
	
	void Awake()
	{
		if (inst == null)
		{
			inst = this;
		}
	}
	
	// Use this for initialization
	void Start()
	{
		am = Audio_Manager.inst;

		// set all cores to deactive
		SetSockets(0,0,0,0,2);
		sockets[0].oppositeSocket = sockets[1];
		sockets[1].oppositeSocket = sockets[0];
		sockets[2].oppositeSocket = sockets[3];
		sockets[3].oppositeSocket = sockets[2];

		sockets[0].SetSelected(true);
	}
	
	void Update()
	{
		// update spare core text
		UpdateSparetext();
	}
	
	// sets each socket to the desired amount
	public void SetSockets(int melee, int speed, int ranged, int shield, int spare)
	{
		sockets[0].SetActiveCores(melee);
		sockets[1].SetActiveCores(speed);
		sockets[2].SetActiveCores(ranged);
		sockets[3].SetActiveCores(shield);
		spareCores = spare;
	}
	
	public AbilitySocket GetSocket(Sockets type)
	{
		switch (type)
		{
		case Sockets.Melee:
			return sockets[0];
			//break;
		case Sockets.Ranged:
			return sockets[1];
			//break;
		case Sockets.Speed:
			return sockets[2];
			//break;
		case Sockets.Shield:
			return sockets[3];
			//break;
		}
		Debug.Log("Get Socket <" + type.ToString() + "> Does Not Exist");
		return null;
	}
	
	// keeps spare core text updated
	private void UpdateSparetext()
	{
		// add '0' in front if less than 10
		if(spareCores < 10)
			spareCoreDisplayText.text = "0" + spareCores.ToString();
		else
			spareCoreDisplayText.text = spareCores.ToString();
	}
	
	//Public Function to Allow Controller Input
	public void SelectSocketLeft()
	{
		selectedSocket -= 1;
		if (selectedSocket < 0)
		{
			selectedSocket = 3;
		}

		for (int i = 0; i < sockets.Length; i ++)
		{
			sockets[i].SetSelected(false);
		}

		sockets[selectedSocket].SetSelected(true);

	}
	
	///Public Function to Select the next Socket to the left
	public void SelectSocketRight()
	{
		selectedSocket += 1;
		if (selectedSocket > 3)
		{
			selectedSocket = 0;
		}

		for (int i = 0; i < sockets.Length; i ++)
		{
			sockets[i].SetSelected(false);
		}
		
		sockets[selectedSocket].SetSelected(true);
	}
	
	public void SocketAdd()
	{
			// check if already full
			if(sockets[selectedSocket].GetCoreCount() < 4)
			{
				// add from spares
				if(spareCores > 0)
				{
					sockets[selectedSocket].AddCore();
					spareCores--;
					am.PlayOneShot(AA.Chr_Lucy_Foley_Laptop_PowerOn, beepVolume);
					ActivateChangeSprite(selectedSocket, true);
				}
				else if(sockets[selectedSocket].oppositeSocket.GetCoreCount() > 0)
				{
					// add from opposite
					sockets[selectedSocket].oppositeSocket.RemoveCore();
					sockets[selectedSocket].AddCore();
					ActivateChangeSprite(selectedSocket, false);
				}
			}
	}

	public void SocketRemove()
	{
		// check for empty
		if(sockets[selectedSocket].GetCoreCount() > 0)
		{
			// remove core and reset sHangTimer
			sockets[selectedSocket].RemoveCore();
			spareCores++;
			am.PlayOneShot(AA.Chr_Lucy_Foley_Laptop_PowerOn, beepVolume);

			ActivateChangeSprite(selectedSocket, true);
		}
	}

	public void ActivateChangeSprite (int socketNum, bool fromSpare)
	{
		switch (socketNum)
		{
		case 0:
			if (fromSpare)
			{
				coreChangeSprites[1].TurnOn(coreChangeTime);
			}
			else
			{
				coreChangeSprites[0].TurnOn(coreChangeTime);
			}
			break;
		case 1:
			if (fromSpare)
			{
				coreChangeSprites[2].TurnOn(coreChangeTime);
			}
			else
			{
				coreChangeSprites[0].TurnOn(coreChangeTime);
			}
			break;
		case 2:
			if (fromSpare)
			{
				coreChangeSprites[5].TurnOn(coreChangeTime);
			}
			else
			{
				coreChangeSprites[3].TurnOn(coreChangeTime);
			}
			break;
		case 3:
			if (fromSpare)
			{
				coreChangeSprites[4].TurnOn(coreChangeTime);
			}
			else
			{
				coreChangeSprites[3].TurnOn(coreChangeTime);
			}
			break;
		}
	}

	
	/// adds core to spare cores
	public void AddSpareCore()
	{
		//TODO apply some particles or visual notice
		//TODO play adding audio
		spareCores++;
	}

	/// returns spare core count
	public int GetSpareCount()
	{
		return spareCores;
	}
}
