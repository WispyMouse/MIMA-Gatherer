using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCounter
{
    public int Count { get; set; } = 0;

    public InventoryCounter(int startingCount)
    {
        Count = startingCount;
    }
}
