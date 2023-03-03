using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ResourceChangeData
{
    public string FriendlyName;
    public int OldAmount;
    public int NewAmount;
    public int Delta
    {
        get
        {
            return NewAmount - OldAmount;
        }
    }
}
