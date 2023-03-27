using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurationManagement : MonoBehaviour
{
    public static ConfigurationContainer ActiveConfiguration { get; set; } = new ConfigurationContainer();
    public static string ConfigurationFilepath = "configurations.mimaconfig";
    public static string UnitFolderName = "Units";
    public static string StructureFolderName = "Structures";
    public static string GatherableFolderName = "Gatherables";
    public static string RootFolderName = "Runtime Data";

    public string MapFilePath = "defaultmap.mimamap";

    [SerializeReference]
    private MapLoader MapLoaderInstance;

    // TEMPORARY HACK: Just making convenient place for this
    public static Dictionary<string, UnitSkeleton> UnitSkeletons { get; private set; } = new Dictionary<string, UnitSkeleton>();
    public static Dictionary<string, StructureSkeleton> StructureSkeletons { get; private set; } = new Dictionary<string, StructureSkeleton>();
    public static Dictionary<string, GatherableSkeleton> GatherableSkeletons { get; private set; } = new Dictionary<string, GatherableSkeleton>();

    private void Awake()
    {
        string targetFilepath = Application.dataPath + "\\" + RootFolderName + "\\" + ConfigurationFilepath;
        ConfigurationContainer configuration = LoadFromConfiguration<ConfigurationContainer>(targetFilepath);
        ActiveConfiguration = configuration;

        LoadAllGatherables();
        LoadAllUnits();
        LoadAllStructures();

        string mapFilePath = Application.dataPath + "\\" + RootFolderName + "\\" + MapFilePath;
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

    private void LoadAllGatherables()
    {
        string gatherableDirectory = Application.dataPath + "\\" + RootFolderName + "\\" + GatherableFolderName;
        string[] gatherables = System.IO.Directory.GetFiles(gatherableDirectory, "*.gatherableconfig");
        foreach (string curGatherablePath in gatherables)
        {
            GatherableSkeleton thisGatherableSkeleton = GatherableSkeleton.LoadFromFile(curGatherablePath);
            GatherableSkeletons.Add(thisGatherableSkeleton.FriendlyName, thisGatherableSkeleton);
        }
    }

    private void LoadAllUnits()
    {
        string unitDirectory = Application.dataPath + "\\" + RootFolderName + "\\" + UnitFolderName;
        string[] units = System.IO.Directory.GetFiles(unitDirectory, "*.unitconfig");
        foreach (string curUnitPath in units)
        {
            UnitSkeleton thisUnitSkeleton = UnitSkeleton.LoadFromFile(curUnitPath);
            UnitSkeletons.Add(thisUnitSkeleton.FriendlyName, thisUnitSkeleton);
        }
    }

    private void LoadAllStructures()
    {
        string structureDirectory = Application.dataPath + "\\" + RootFolderName + "\\" + StructureFolderName;
        string[] structures = System.IO.Directory.GetFiles(structureDirectory, "*.structureconfig");
        foreach (string curStructurePath in structures)
        {
            StructureSkeleton thisStructureSkeleton = StructureSkeleton.LoadFromFile(curStructurePath);
            StructureSkeletons.Add(thisStructureSkeleton.FriendlyName, thisStructureSkeleton);
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
