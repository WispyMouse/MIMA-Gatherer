using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceLabel : MonoBehaviour
{
    public ResourceCost RepresentedCost { get; set; }

    [SerializeReference]
    private TMP_Text ResourceNameLabel;
    [SerializeReference]
    private TMP_Text ResourceCostLabel;

    public void SetResourceCost(ResourceCost value)
    {
        this.RepresentedCost = value;
        ResourceNameLabel.text = value.Resource;
        ResourceCostLabel.text = $"x{value.Cost.ToString()}";
    }
}
