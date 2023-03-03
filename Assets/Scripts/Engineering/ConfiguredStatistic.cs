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

    public ConfiguredStatistic(T defaultUnconfiguredValue, string configurationPath)
    {
        this._value = defaultUnconfiguredValue;
        this.ConfigurationPath = configurationPath;
    }

    protected void LoadFromConfiguration(ConfigurationContainer configuration)
    {
        string foundValue;
        if (configuration.Configuration.TryGetValue(ConfigurationPath, out foundValue))
        {
            if (typeof(T) == typeof(string))
            {
                _value = (T)(object)foundValue;
            }
            else
            {
                _value = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(foundValue);
            }            

            if (_value == null)
            {
                Debug.LogError($"Found configuration at path {ConfigurationPath} but it could not be boxed in to type {typeof(T).Name}");
            }
        }
        else
        {
            Debug.Log($"Unable to find a configuration at path {ConfigurationPath}");
        }

        ValueLoaded = true;
    }
}
