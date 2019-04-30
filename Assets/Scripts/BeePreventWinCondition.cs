using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeePreventWinCondition : WinCondition
{
	public int beeCount;

	public override bool CheckWin()
	{
		if (GameController.SharedInstance.beeScore == beeCount)
			win = true;
		else
			win = false;
		return win;
	}
}
