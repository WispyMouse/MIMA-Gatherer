using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherable : GameworldObject
{
    public GatherableSkeleton GatherableSkeletonData { get; private set; }

    [SerializeReference]
    private MeshRenderer ColorableModel;

    [SerializeReference]
    private Transform ScalableModel;

    public override string FriendlyName => GatherableSkeletonData.FriendlyName;
    public override string DisplayName => GatherableSkeletonData.ResourceName;

    public void AssignGatherableSkeleton(GatherableSkeleton data)
    {
        this.GatherableSkeletonData = data;

        this.ScalableModel.localScale = Vector3.one * data.ModelScale;

        if (ColorUtility.TryParseHtmlString(data.ModelColor, out Color parsedColor))
        {
            this.ColorableModel.material.color = parsedColor;
        }
    }

}
