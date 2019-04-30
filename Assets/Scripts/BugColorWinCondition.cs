using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugColorWinCondition : WinCondition
{
	public int blackFlyCount, blueFlyCount, redFlyCount;
	
	public override bool CheckWin()
	{
		if (GameController.SharedInstance.blackScore >= blackFlyCount  
		    && GameController.SharedInstance.blueScore >= blueFlyCount
		    && GameController.SharedInstance.redScore >= redFlyCount)
			win = true;
		else
			win = false;
		return win;
	}
}
