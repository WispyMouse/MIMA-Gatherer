using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceCost
{
    public string Resource;
    public int Cost;

    public ResourceCost()
    {

    }

    public ResourceCost(string resource, int cost)
    {
        this.Resource = resource;
        this.Cost = cost;
    }
}
