using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConfiguredStatistic<T>
{
    public T Value
    {
        get
        {
            if (ValueLoaded == false)
            {
                LoadFromConfiguration(ConfigurationManagement.ActiveConfiguration);
            }

            return _value;
        }
    }
    private T _value { get; set; }
    public string ConfigurationPath;
    public bool ValueLoaded { get; set; } = false;

    public ConfiguredStatistic()
    {

    }

    public ConfiguredStatistic(T valueOfConfiguration, string configurationPath)
    {
        this._value = valueOfConfiguration;
        this.ConfigurationPath = configurationPath;
    }

    public void LoadFromConfiguration(ConfigurationContainer configuration)
    {
        string foundValue;
        if (configuration.Configuration.TryGetValue(ConfigurationPath, out foundValue))
        {
            _value = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(foundValue);

            if (_value != null)
            {
                ValueLoaded = true;
                return;
            }
            else
            {
                Debug.LogError($"Found configuration at path {ConfigurationPath} but it could not be boxed in to type {typeof(T).Name}");
            }
        }
        else
        {
            Debug.LogError($"Unable to find a configuration at path {ConfigurationPath}");
        }
    }
}
