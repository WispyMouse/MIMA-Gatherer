using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Structure : GameworldObject
{
    [SerializeReference]
    private StructureEvent StructureSelectedEvent;

    public StructureSkeleton StructureSkeletonData { get; private set; }

    [SerializeReference]
    private MeshRenderer ColorableModel;

    [SerializeReference]
    private Transform ScalableModel;

    public override string FriendlyName => StructureSkeletonData.FriendlyName;
    public override string DisplayName => StructureSkeletonData.StructureName;

    public void AssignStructureSkeleton(StructureSkeleton data)
    {
        this.StructureSkeletonData = data;

        this.ScalableModel.localScale = Vector3.one * data.ModelScale;

        if (ColorUtility.TryParseHtmlString(data.ModelColor, out Color parsedColor))
        {
            this.ColorableModel.material.color = parsedColor;
        }
    }

    private void Awake()
    {

    }

    public override void Interact()
    {
        base.Interact();

        StructureSelectedEvent.Raise(this);
    }
}
