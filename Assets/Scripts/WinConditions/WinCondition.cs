using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//could this be an interface??
//in addition all win conditions should be in one folder
public enum WinType
{
    COUNT,
    BUG_COLOR,
    BEE_PREVENT
}

public abstract class WinCondition : MonoBehaviour
{
    protected bool win = false;

    public WinType winType;

    public abstract bool CheckWin();
}
