using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    [SerializeReference]
    private Structure StructurePrefab;
    [SerializeReference]
    private Gatherable GatherablePrefab;
    [SerializeReference]
    private Unit UnitPrefab;

    public GameplayMap LoadedMap { get; private set; }
    List<GameworldObject> SpawnedObjects { get; set; } = new List<GameworldObject>();

    public void LoadMap(GameplayMap map)
    {
        LoadedMap = map;

        foreach (StartingGatherable sr in map.ResourceNodes)
        {
            Gatherable newResource = Instantiate(GatherablePrefab, sr.Position, Quaternion.identity);
            newResource.AssignGatherableSkeleton(ConfigurationManagement.GatherableSkeletons[sr.FriendlyName]);
            SpawnedObjects.Add(newResource);
        }

        foreach (StartingStructure ss in map.Structures)
        {
            Structure structure = Instantiate(StructurePrefab, ss.Position, Quaternion.identity);
            structure.AssignStructureSkeleton(ConfigurationManagement.StructureSkeletons[ss.FriendlyName]);
            SpawnedObjects.Add(structure);
        }

        foreach (StartingUnit su in map.Units)
        {
            Unit unit = Instantiate(UnitPrefab, su.Position, Quaternion.identity);
            unit.AssignUnitSkeleton(ConfigurationManagement.UnitSkeletons[su.FriendlyName]);
            SpawnedObjects.Add(unit);
        }
    }

    public void ClearMap()
    {
        LoadedMap = null;

        for (int ii = 0; ii < SpawnedObjects.Count; ii++)
        {
            Destroy(SpawnedObjects[ii].gameObject);
        }
        SpawnedObjects.Clear();
    }
}
