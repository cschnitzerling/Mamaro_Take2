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

	
	// private vars
	public int spareCores = 0;
	public int selectedSocket = 0;

	public Vector3 disabledScale, enabledScale;

	
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
		print ("Left");
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
		print ("Right");
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
				}
				else if(sockets[selectedSocket].oppositeSocket.GetCoreCount() > 0)
				{
					// add from opposite
					sockets[selectedSocket].oppositeSocket.RemoveCore();
					sockets[selectedSocket].AddCore();
				}
			}
	}

//	int GetOpposite(int curSocket)
//	{
//		switch (curSocket)
//		{
//		case 0:
//			return 1;
//		case 1:
//			return 0;
//		case 2:
//			return 3;
//		case 3:
//			return 2;
//		}
//
//		Debug.Log("GetOpposite out of range > 3" + curSocket);
//		return 0;
//	}
	
	
	
	public void SocketRemove()
	{
		// check for empty
		if(sockets[selectedSocket].GetCoreCount() > 0)
		{
			// remove core and reset sHangTimer
			sockets[selectedSocket].RemoveCore();
			spareCores++;
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
