using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurationManagement : MonoBehaviour
{
    public static ConfigurationContainer ActiveConfiguration { get; set; } = new ConfigurationContainer();
    public string ConfigurationFilepath = "configurations.mimaconfig";
    public string UnitFolderName = "Units";

    public string MapFilePath = "defaultmap.mimamap";

    [SerializeReference]
    private MapLoader MapLoaderInstance;

    // TEMPORARY HACK: Just making convenient place for this
    public static Dictionary<string, UnitSkeleton> UnitSkeletons { get; private set; } = new Dictionary<string, UnitSkeleton>();

    private void Awake()
    {
        string targetFilepath = Application.dataPath + "\\" + ConfigurationFilepath;
        ConfigurationContainer configuration = LoadFromConfiguration<ConfigurationContainer>(targetFilepath);
        ActiveConfiguration = configuration;

        LoadAllUnits();

        string mapFilePath = Application.dataPath + "\\" + MapFilePath;
        GameplayMap map = LoadFromConfiguration<GameplayMap>(mapFilePath);

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

    private void LoadAllUnits()
    {
        string unitDirectory = Application.dataPath + "\\" + UnitFolderName;
        string[] units = System.IO.Directory.GetFiles(unitDirectory, "*.unitconfig");
        foreach (string curUnitPath in units)
        {
            UnitSkeleton thisUnitSkeleton = UnitSkeleton.LoadFromFile(curUnitPath);
            UnitSkeletons.Add(thisUnitSkeleton.FriendlyName, thisUnitSkeleton);
        }
    }

#nullable enable
    public static string LoadStringFromConfiguration(string path)
    {
        if (!System.IO.File.Exists(path))
        {
            Debug.LogError($"File at path [{path}] does not exist, could not load configuration");
            return string.Empty;
        }

        string allText = System.IO.File.ReadAllText(path);
        return allText;
    }

    public static T? LoadFromConfiguration<T>(string path) where T : new()
    {
        if (!System.IO.File.Exists(path))
        {
            Debug.LogError($"File at path [{path}] does not exist, could not load configuration");
            return default(T);
        }

        string allText = System.IO.File.ReadAllText(path);
        T configuration = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(allText);
        
        if (configuration == null)
        {
            Debug.LogError($"File at path [{path}] does exist, but could not be deserialized in to {nameof(T)}");
            return default(T);
        }

        return configuration;
    }
#nullable restore
}
