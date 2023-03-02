using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Structure : GameworldObject
{
    [SerializeReference]
    private StructureEvent StructureSelectedEvent;

    private void Awake()
    {

    }

    public override void Interact()
    {
        base.Interact();

        StructureSelectedEvent.Raise(this);
    }
}
