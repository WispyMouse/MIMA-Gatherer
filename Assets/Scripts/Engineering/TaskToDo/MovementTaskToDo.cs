using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementTaskToDo : TaskToDo
{
    public const float CloseEnoughToCorner = .01f;

    public Vector3 Destination { get; private set; }

    private Unit PerformingObjectAsUnit { get; set; }
    private NavMeshPath Navigation { get; set; }
    private int CurCornerIndex { get; set; }

    private ConfiguredStatistic<float> DurationBeforeSheeping { get; set; } = new ConfiguredStatistic<float>(.2f, $"{nameof(MovementTaskToDo)}.{nameof(DurationBeforeSheeping)}");
    private ConfiguredStatistic<float> DistanceThresholdForSheeping { get; set; } = new ConfiguredStatistic<float>(.2f, $"{nameof(MovementTaskToDo)}.{nameof(DistanceThresholdForSheeping)}");
    private ConfiguredStatistic<float> MaximumTimeToSheep { get; set; } = new ConfiguredStatistic<float>(.2f, $"{nameof(MovementTaskToDo)}.{nameof(MaximumTimeToSheep)}");
    private float TimeLeftForCurrentSheeping { get; set; } = 0;
    private bool IsSheeping { get; set; } = false;
    private float TimeSinceLastPastThresholdMovement { get; set; } = 0;
    private Vector3 RandomDirectionToWalk { get; set; }
    private Vector3 LastFramePosition { get; set; }

    public MovementTaskToDo(Unit performingObject, Vector3 destination) : base(performingObject, $"Moving to {destination.ToString()}")
    {
        this.Destination = destination;
        this.PerformingObjectAsUnit = performingObject;
    }

    public MovementTaskToDo(Unit performingObject, Vector3 destination, NavMeshPath calculatedPath) : this(performingObject, destination)
    {
        this.Navigation = calculatedPath;
    }

    public override void ProcessAndCalculate()
    {
        base.ProcessAndCalculate();

        if (Navigation != null)
        {
            // Already have a path, no work needs to be done yet
            return;
        }

        NavMeshPath pathToNearestThing = new NavMeshPath();
        if (!PerformingObjectAsUnit.Agent.CalculatePath(Destination, pathToNearestThing))
        {
            Debug.Log("Can't find it, boss!");
            RejectAsNotPossible();
            return;
        }

        Navigation = pathToNearestThing;
        LastFramePosition = PerformingObjectAsUnit.transform.position;
    }

    public override void Tick(float deltaTime)
    {
        if (Navigation == null)
        {
            return;
        }

        if (Vector3.Distance(PerformingObjectAsUnit.transform.position, LastFramePosition) <= DistanceThresholdForSheeping.Value * Time.deltaTime)
        {
            TimeSinceLastPastThresholdMovement += deltaTime;

            if (TimeSinceLastPastThresholdMovement > DurationBeforeSheeping.Value)
            {
                IsSheeping = true;
                Vector3 randomPosition = UnityEngine.Random.onUnitSphere;
                RandomDirectionToWalk = new Vector3(randomPosition.x, 0, randomPosition.z).normalized;
                TimeLeftForCurrentSheeping = UnityEngine.Random.Range(0, MaximumTimeToSheep.Value);
            }
        }
        else
        {
            TimeSinceLastPastThresholdMovement = 0;
        }

        if (IsSheeping)
        {
            PerformingObjectAsUnit.AttachedRigidbody.MovePosition(PerformingObjectAsUnit.transform.position + RandomDirectionToWalk * deltaTime);
            TimeLeftForCurrentSheeping -= deltaTime;
            if (TimeLeftForCurrentSheeping <= 0)
            {
                IsSheeping = false;
            }
            return;
        }

        float movementAllowed = deltaTime * PerformingObjectAsUnit.UnitSkeletonData.MovementPerSecond;
        float actualDistanceTraveled = 0;
        while (movementAllowed > 0)
        {
            if (Navigation == null)
            {
                TaskCompleted();
                break;
            }

            FollowPathToNextCorner(ref movementAllowed);
        }

        LastFramePosition = PerformingObjectAsUnit.transform.position;
    }

    void FollowPathToNextCorner(ref float movementAllowed)
    {
        if (Navigation == null)
        {
            movementAllowed = -1f;
            return;
        }

        if (CurCornerIndex < 0)
        {
            movementAllowed = -1f;
            return;
        }

        if (CurCornerIndex >= Navigation.corners.Length)
        {
            movementAllowed = -1f;
            return;
        }

        Vector3 startingPosition = PerformingObjectAsUnit.transform.position;
        Vector3 targetPosition = Navigation.corners[CurCornerIndex];
        Vector3 calculatedNewPosition = Vector3.MoveTowards(PerformingObjectAsUnit.transform.position, targetPosition, movementAllowed);

        float distanceToCorner = Vector3.Distance(PerformingObjectAsUnit.transform.position, targetPosition);

        PerformingObjectAsUnit.AttachedRigidbody.MovePosition(calculatedNewPosition);

        if (Vector3.Distance(PerformingObjectAsUnit.transform.position, targetPosition) < CloseEnoughToCorner)
        {
            CurCornerIndex++;
            if (Navigation.corners.Length <= CurCornerIndex)
            {
                Navigation = null;
            }
        }

        movementAllowed = Mathf.Max(0, movementAllowed - distanceToCorner);
    }

    public override string TaskDetails()
    {
        return String.Concat(base.TaskDetails(),
            $"{(IsSheeping ? "Sheeping...\n" : "")}",
            $"{(PerformingObjectAsUnit.HeldResources != null ? $"Carrying {PerformingObjectAsUnit.HeldResources.Cost} of {PerformingObjectAsUnit.HeldResources.Resource}\n" : "")}");
    }
}
