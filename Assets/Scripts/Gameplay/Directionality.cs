using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Directionality
{
    public int XSign;
    public int YSign;
    public int ZSign;

    public Directionality(int xSign, int ySign, int zSign)
    {
        this.XSign = xSign;
        this.YSign = ySign;
        this.ZSign = zSign;
    }

    public Vector3 GetMovementVector()
    {
        Vector3 movementVector = new Vector3(XSign, YSign, ZSign);
        return movementVector.normalized;
    }

    public override bool Equals(object obj)
    {
        if (obj is Directionality direction)
        {
            if (direction.XSign == this.XSign && direction.YSign == this.YSign && direction.ZSign == this.ZSign)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    public override int GetHashCode()
    {
        return (XSign + (YSign * 10) + (ZSign * 100)).GetHashCode();
    }
}
