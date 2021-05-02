using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeePreventWinCondition : WinCondition
{
	public int beeCount;
	
	//We gotta refactor the bee prevent win condition...is it reliant on score? or nah?
	public override bool CheckWin()
	{
		if (GameController.SharedInstance.beeScore < beeCount)
			win = true;
		else
			win = false;
		return win;
	}
}
