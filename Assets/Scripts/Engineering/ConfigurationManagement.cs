using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurationManagement : MonoBehaviour
{
    public static ConfigurationContainer ActiveConfiguration { get; set; } = new ConfigurationContainer();
    public string ConfigurationFilepath = "configurations.mimaconfig";

    public string MapFilePath = "defaultmap.mimamap";

    [SerializeReference]
    private MapLoader MapLoaderInstance;

    private void Awake()
    {
        string targetFilepath = Application.dataPath + "\\" + ConfigurationFilepath;

        if (!System.IO.File.Exists(targetFilepath))
        {
            Debug.LogError($"Configuration file does not exist at {targetFilepath}");
            return;
        }

        string allText = System.IO.File.ReadAllText(targetFilepath);
        ConfigurationContainer configuration = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigurationContainer>(allText);
        ActiveConfiguration = configuration;

        string mapFilePath = Application.dataPath + "\\" + MapFilePath;

        if (!System.IO.File.Exists(mapFilePath))
        {
            Debug.LogError($"Map file does not exist at {mapFilePath}");
            return;
        }

        string allMapText = System.IO.File.ReadAllText(mapFilePath);
        GameplayMap map = Newtonsoft.Json.JsonConvert.DeserializeObject<GameplayMap>(allMapText);

        if (map == null)
        {
            Debug.LogError($"Unable to deserialize found map text as a {nameof(GameplayMap)}");
            return;
        }

        if (MapLoaderInstance == null)
        {
            Debug.LogError($"Map loader instance was null, needs to be set in the editor");
            return;
        }

        MapLoaderInstance.LoadMap(map);
    }

    private void Start()
    {
        InventoryManagement.Grant(MapLoaderInstance.LoadedMap.StartingInventory);
    }
}
