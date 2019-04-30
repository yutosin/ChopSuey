using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeController : FlyController
{
    protected override bool BoundCheck()
    {
        if (base.BoundCheck())
        {
            GameController.SharedInstance.UpdateScore(1, FlyType.BEE);
            return true;
        }
        
        return false;
    }
}
