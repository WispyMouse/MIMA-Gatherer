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

    public void Update()
    {
        // If we're clicking on a GameworlObject, interact with it
        if (Input.GetMouseButtonDown(0))
        {
            Ray clickRaycast = MainGameplayCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(clickRaycast.origin, clickRaycast.direction, float.MaxValue, GameworldObjectLayerMask.value);
            foreach (RaycastHit hit in hits)
            {
                GameworldObject go = hit.rigidbody.GetComponent<GameworldObject>();

                if (go != null)
                {
                    go.Interact();
                    break;
                }
            }
        }
    }
}
