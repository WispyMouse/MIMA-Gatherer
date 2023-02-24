using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameplayMap
{
    public string MapName;

    public List<StartingResource> ResourceNodes = new List<StartingResource>();
    public List<StartingStructure> Structures = new List<StartingStructure>();

    public List<StartingInventoryElement> StartingInventory = new List<StartingInventoryElement>();
}
