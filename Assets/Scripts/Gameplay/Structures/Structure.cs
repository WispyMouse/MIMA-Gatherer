using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Structure : GameworldObject
{
    [SerializeReference]
    private StructureEvent StructureSelectedEvent;
    [SerializeReference]
    private StructureEvent StructureFinishedPlanEvent;

    public StructureSkeleton StructureSkeletonData { get; private set; }

    [SerializeReference]
    private MeshRenderer ColorableModel;

    [SerializeReference]
    private Transform ScalableModel;

    public override string FriendlyName => StructureSkeletonData.FriendlyName;
    public override string DisplayName => StructureSkeletonData.StructureName;

    public bool IsPlan { get; set; } = false;
    public bool PlanIsReadyForWork { get; set; } = false;

    /// <summary>
    /// A count of how many workers are actively focused on finishing this plan.
    /// </summary>
    public int WorkersAssigned { get; set; }
    /// <summary>
    /// A count of how many workers this Structure desires to finish this plan.
    /// </summary>
    public int WorkersDesired { get; set; }

    /// <summary>
    /// A count of how many worker-seconds have been put in to finishing this plan.
    /// </summary>
    public float ProductionProgressSeconds { get; set; } = 0;

    public Dictionary<string, int> HeldResources { get; private set; } = new Dictionary<string, int>();
    public Dictionary<string, int> AccountedForResources { get; private set; } = new Dictionary<string, int>();

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

    public void SetIsPossiblePlan()
    {
        IsPlan = true;
    }

    public void StartThePlan()
    {
        PlanIsReadyForWork = true;
    }

    public void PutTimeIntoPlan()
    {
        if (!IsPlan)
        {
            return;
        }

        ProductionProgressSeconds += Time.deltaTime;

        if (ProductionProgressSeconds >= StructureSkeletonData.ProductionTimeSeconds)
        {
            FinishPlan();
        }
    }

    void FinishPlan()
    {
        IsPlan = false;
        WorkersAssigned = 0;
        WorkersDesired = 0;
        StructureFinishedPlanEvent.Raise(this);
    }

    public override string GetCurrentTask()
    {
        if (IsPlan)
        {
            return $"{ProductionProgressSeconds / StructureSkeletonData.ProductionTimeSeconds}% complete, {WorkersDesired} workers desired, {WorkersAssigned} workers assigned";
        }

        return base.GetCurrentTask();
    }

    public void ContributeResource(string resource, int count)
    {
        if (!IsPlan)
        {
            return;
        }

        if (HeldResources.ContainsKey(resource))
        {
            HeldResources[resource] += count;
        }
        else
        {
            HeldResources.Add(resource, count);
        }
    }

    public bool CanAffordToFinalize()
    {
        foreach (ResourceCost cost in StructureSkeletonData.Costs)
        {
            
        }

        return true;
    }

    public void AccountForResource(string resource, int amount)
    {
        if (AccountedForResources.ContainsKey(resource))
        {
            AccountedForResources[resource] += amount;
        }
        else
        {
            AccountedForResources.Add(resource, amount);
        }
    }

    public void CancelResourceAccount(string resource, int amount)
    {
        AccountedForResources[resource] -= amount;
    }

    public bool IsResourceNeeded(string resource, out int required)
    {
        int held = 0;
        required = 0;

        if (!HeldResources.TryGetValue(resource, out held))
        {
            return false;
        }

        foreach (ResourceCost curCost in StructureSkeletonData.Costs)
        {
            if (curCost.Resource.Equals(resource, System.StringComparison.InvariantCultureIgnoreCase))
            {
                required = Mathf.Max(0, curCost.Cost - held);
                return required > 0;
            }
        }

        return false;
    }
}
