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

    [SerializeField]
    private UnitEvent UnitSelectedEvent;

    private Unit SelectedUnit { get; set; }

    public void Update()
    {
        HandleLeftClick();
        HandleRightClick();
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
}
