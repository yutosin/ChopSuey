using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountWinCondition : WinCondition
{
    public int count;
    
    public override bool CheckWin()
    {
        if (count == GameController.SharedInstance.score)
            win = true;
        else
            win = false;
        return win;
    }
}
