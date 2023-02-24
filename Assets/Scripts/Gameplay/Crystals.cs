using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystals : GameworldObject
{
    public string ResourceName = nameof(Crystals);

    public ConfiguredStatistic<float> TimeToGatherResource { get; set; } = new ConfiguredStatistic<float>(1f, $"{nameof(Crystals)}.{nameof(TimeToGatherResource)}");
    public ConfiguredStatistic<int> AmountOfResourcesGathered { get; set; } = new ConfiguredStatistic<int>(15, $"{nameof(Crystals)}.{nameof(AmountOfResourcesGathered)}");

    private void Awake()
    {
    }
}
