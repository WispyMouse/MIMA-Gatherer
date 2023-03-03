using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameplayMap
{
    public string MapName;

    public List<StartingGatherable> ResourceNodes = new List<StartingGatherable>();
    public List<StartingStructure> Structures = new List<StartingStructure>();
    public List<StartingUnit> Units = new List<StartingUnit>();

    public List<StartingInventoryElement> StartingInventory = new List<StartingInventoryElement>();
}
