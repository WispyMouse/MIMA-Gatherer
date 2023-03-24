using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GetResourcesFromStructureTaskToDo : LocationBasedTaskToDo
{
    public Unit PerformingObjectAsUnit { get; private set; }
    public string ResourceToGather { get; private set; }
    public Structure SourceStructure { get; private set; }
    public int Count { get; private set; }

    public GetResourcesFromStructureTaskToDo(Unit performingObject, string resourceToGather, int count) : base(performingObject)
    {
        this.PerformingObjectAsUnit = performingObject;
        this.ResourceToGather = resourceToGather;
        this.Count = count;
    }

    public GetResourcesFromStructureTaskToDo(Unit performingObject, Structure sourceStructure, string resourceToGather, int count) : base(performingObject, sourceStructure.transform.position, $"Gathering {count} {ConfigurationManagement.GatherableSkeletons[resourceToGather].FriendlyName} from {sourceStructure.StructureSkeletonData.FriendlyName}")
    {
        this.PerformingObjectAsUnit = performingObject;
        this.ResourceToGather = resourceToGather;
        this.SourceStructure = sourceStructure;
        this.Count = count;
    }

    public override void ProcessAndCalculate()
    {
        base.ProcessAndCalculate();

        if (SourceStructure == null)
        {
            Structure[] allTargets = GameObject.FindObjectsOfType<Structure>();
            List<Structure> consideredStructures = new List<Structure>();

            foreach (Structure target in allTargets)
            {
                if (target.StructureSkeletonData.AcceptedResources == null)
                {
                    continue;
                }

                if (target.IsPlan)
                {
                    continue;
                }

                if (target.StructureSkeletonData.AcceptedResources.Contains(ResourceToGather))
                {
                    consideredStructures.Add(target);
                }
            }

            NavMeshPath foundPath;
            SourceStructure = PerformingObjectAsUnit.GetNearestObject<Structure>(out foundPath, consideredStructures);
            AlreadyCalculatedPath = foundPath;

            if (SourceStructure == null)
            {
                Debug.Log($"No one accepts the resource we're looking for: {ResourceToGather}");
            }

            this.TargetPosition = SourceStructure.transform.position;
        }
    }

    public override void TaskCompleted()
    {
        base.TaskCompleted();

        InventoryManagement.ChangeResource(ResourceToGather, -Count);
        PerformingObjectAsUnit.StartHoldingResources(ResourceToGather, Count);
    }
}
