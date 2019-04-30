using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeController : FlyController
{
    protected override bool BoundCheck()
    {
        if (base.BoundCheck())
        {
            GameController.SharedInstance.UpdateScore(1, FlyType.NONE);
            return true;
        }
        
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HitZone"))
        {
            GameController.SharedInstance.UpdateScore(10, FlyType.BEE);
            Destroy(this.gameObject);
        }
    }
}
