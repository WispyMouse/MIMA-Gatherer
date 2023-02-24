using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A globally accessible listener to inputs and delegates operations.
/// This MonoBehaviour sits on a GameObject that persists across scenes.
/// </summary>
public class InputManagement : MonoBehaviour
{
    [SerializeReference]
    private Camera MainGameplayCamera;

    [SerializeField]
    private LayerMask GameworldObjectLayerMask;

    [SerializeField]
    private LayerMask GroundLayerMask;

    [SerializeReference]
    private UnitEvent UnitSelectedEvent;

    private Unit SelectedUnit { get; set; }

    private HashSet<Directionality> CurrentInputDirectionalities { get; set; } = new HashSet<Directionality>();

    [SerializeField]
    private ConfiguredStatistic<float> CameraPanningMovementPerSecond = new ConfiguredStatistic<float>(10f, $"{nameof(InputManagement)}.{nameof(CameraPanningMovementPerSecond)}");

    public void Update()
    {
        HandleLeftClick();
        HandleRightClick();
        HandleCameraPanning();
    }

    void HandleLeftClick()
    {
        // If we're clicking on a GameworlObject, interact with it
        if (Input.GetMouseButtonDown(0))
        {
            SelectedUnit = null;

            Ray clickRaycast = MainGameplayCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(clickRaycast.origin, clickRaycast.direction, float.MaxValue, GameworldObjectLayerMask.value);
            foreach (RaycastHit hit in hits)
            {
                GameworldObject go = hit.rigidbody.GetComponent<GameworldObject>();

                if (go != null)
                {
                    go.Interact();

                    // TODO HACK: This should raise some generic selection instead of unboxing
                    if (go is Unit goAsUnit)
                    {
                        SelectedUnit = goAsUnit;
                        UnitSelectedEvent.Raise(goAsUnit);
                    }

                    break;
                }
            }
        }
    }

    void HandleRightClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray clickRaycast = MainGameplayCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(clickRaycast.origin, clickRaycast.direction, float.MaxValue, GroundLayerMask.value);
            foreach (RaycastHit hit in hits)
            {
                Vector3 positionHit = hit.point;
                
                if (SelectedUnit != null)
                {
                    SelectedUnit.SendTowardsPosition(positionHit);
                }
            }
        }
    }

    public void OnPanningStart(Directionality direction)
    {
        CurrentInputDirectionalities.Add(direction);
    }

    public void OnPanningEnd(Directionality direction)
    {
        CurrentInputDirectionalities.Remove(direction);
    }

    void PanCameraBasedOnInputDirections(float distanceAllowed)
    {
        Vector3 totalVector = Vector3.zero;
        foreach (Directionality curDirection in CurrentInputDirectionalities)
        {
            totalVector += curDirection.GetMovementVector();
        }

        if (totalVector.magnitude == 0)
        {
            // No input, or only canceled out inputs
            return;
        }

        totalVector.Normalize();

        MainGameplayCamera.transform.Translate(totalVector * distanceAllowed, Space.World);
    }

    void HandleCameraPanning()
    {
        if (Input.GetKey(KeyCode.W))
        {
            CurrentInputDirectionalities.Add(new Directionality(0, 0, 1));
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            CurrentInputDirectionalities.RemoveWhere(dir => dir.XSign == 0 && dir.YSign == 0 && dir.ZSign == 1);
        }

        if (Input.GetKey(KeyCode.A))
        {
            CurrentInputDirectionalities.Add(new Directionality(-1, 0, 0));
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            CurrentInputDirectionalities.RemoveWhere(dir => dir.XSign == -1 && dir.YSign == 0 && dir.ZSign == 0);
        }

        if (Input.GetKey(KeyCode.S))
        {
            CurrentInputDirectionalities.Add(new Directionality(0, 0, -1));
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            CurrentInputDirectionalities.RemoveWhere(dir => dir.XSign == 0 && dir.YSign == 0 && dir.ZSign == -1);
        }

        if (Input.GetKey(KeyCode.D))
        {
            CurrentInputDirectionalities.Add(new Directionality(1, 0, 0));
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            CurrentInputDirectionalities.RemoveWhere(dir => dir.XSign == 1 && dir.YSign == 0 && dir.ZSign == 0);
        }

        PanCameraBasedOnInputDirections(Time.deltaTime * CameraPanningMovementPerSecond.Value);
    }
}
