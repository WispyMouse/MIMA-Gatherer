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
    }

    public override void Tick(float deltaTime)
    {
        if (Navigation == null)
        {
            return;
        }

        float movementAllowed = deltaTime * PerformingObjectAsUnit.UnitSkeletonData.MovementPerSecond;
        while (movementAllowed > 0)
        {
            if (Navigation == null)
            {
                TaskCompleted();
                break;
            }

            movementAllowed = FollowPathToNextCorner(movementAllowed);
        }
    }

    /// <summary>
    /// Instructs the Unit to move down the path it's on.
    /// </summary>
    /// <param name="maxDistance">The maximum amount of distance that can be covered. If the distance to the next corner is less than this, will only go to the next available corner.</param>
    /// <returns>The amount of space left after reaching the farthest point that could be reached, or after reaching the next corner.</returns>
    float FollowPathToNextCorner(float maxDistance)
    {
        if (Navigation == null)
        {
            return -1f;
        }

        if (CurCornerIndex < 0)
        {
            return -1f;
        }

        if (CurCornerIndex >= Navigation.corners.Length)
        {
            return -1f;
        }

        Vector3 targetPosition = Navigation.corners[CurCornerIndex];
        Vector3 calculatedNewPosition = Vector3.MoveTowards(PerformingObjectAsUnit.transform.position, targetPosition, maxDistance);

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

        return Mathf.Max(0, maxDistance - distanceToCorner);
    }

}
