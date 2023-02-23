using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurationManagement : MonoBehaviour
{
    public static ConfigurationContainer ActiveConfiguration { get; set; } = new ConfigurationContainer();
    public string ConfigurationFilepath = "configurations.mimaconfig";

    private void Awake()
    {
        string targetFilepath = Application.dataPath + "\\" + ConfigurationFilepath;

        if (!System.IO.File.Exists(targetFilepath))
        {
            return;
        }

        string allText = System.IO.File.ReadAllText(targetFilepath);
        ConfigurationContainer configuration = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigurationContainer>(allText);
        ActiveConfiguration = configuration;
    }
}
