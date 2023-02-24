using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    [SerializeReference]
    private Structure StructurePrefab;
    [SerializeReference]
    private Crystals ResourcePrefab;

    public GameplayMap LoadedMap { get; private set; }
    List<GameworldObject> SpawnedObjects { get; set; } = new List<GameworldObject>();

    public void LoadMap(GameplayMap map)
    {
        LoadedMap = map;

        foreach (StartingResource sr in map.ResourceNodes)
        {
            Crystals newResource = Instantiate(ResourcePrefab, sr.Position, Quaternion.identity);
            SpawnedObjects.Add(newResource);
        }

        foreach (StartingStructure ss in map.Structures)
        {
            Structure structure = Instantiate(StructurePrefab, ss.Position, Quaternion.identity);
            SpawnedObjects.Add(structure);
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
