using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConfiguredStatistic<T>
{
    public T Value;
    public string ConfigurationPath;

    public ConfiguredStatistic()
    {

    }

    public ConfiguredStatistic(T valueOfConfiguration, string configurationPath)
    {
        this.Value = valueOfConfiguration;
        this.ConfigurationPath = configurationPath;
    }

    public void LoadFromConfiguration(ConfigurationContainer configuration)
    {
        string foundValue;
        if (configuration.Configuration.TryGetValue(ConfigurationPath, out foundValue))
        {
            T deserializedValue = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(foundValue);

            if (deserializedValue != null)
            {
                Value = deserializedValue;
            }
            else
            {
                Debug.LogError($"Configuration at path {ConfigurationPath} found a value, but it could not be deserialized into {nameof(T)}");
            }
        }
        else
        {
            Debug.LogError($"Unable to find a configuration at path {ConfigurationPath}");
        }
    }
}
