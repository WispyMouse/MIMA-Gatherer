using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamesManagement : MonoBehaviour
{
    public static NamesManagement Instance { get; private set; }
 
    public string NamesFilepath = "names.txt";

    public List<string> Names { get; set; } = new List<string>();


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        string targetFilepath = Application.dataPath + "\\" + ConfigurationManagement.RootFolderName + "\\" + ConfigurationManagement.UnitFolderName + "\\" + NamesFilepath;
        Names = ConfigurationManagement.LoadFromConfiguration<List<string>>(targetFilepath);
    }

    public static string GetRandomName()
    {
        return Instance.Names[Random.Range(0, Instance.Names.Count)];
    }
}
