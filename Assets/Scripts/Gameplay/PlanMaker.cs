using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanMaker : MonoBehaviour
{
    [SerializeReference]
    private Structure StructurePlanPF;

    [SerializeField]
    private LayerMask GroundMask;

    private Structure StructurePlanActive { get; set; }

    public void OnStructurePlanButtonPressed(StructureSkeleton skeleton)
    {
        ClearPlan();

        StructurePlanActive = Instantiate(StructurePlanPF);
        StructurePlanActive.AssignStructureSkeleton(skeleton);
        StructurePlanActive.SetIsPossiblePlan();
    }

    public void OnDismiss()
    {
        ClearPlan();
    }

    private void ClearPlan()
    {
        if (StructurePlanActive != null)
        {
            Destroy(StructurePlanActive.gameObject);
        }

        StructurePlanActive = null;
    }

    private void Update()
    {
        if (StructurePlanActive == null)
        {
            return;
        }

        RaycastHit hit;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.MaxValue, GroundMask.value))
        {
            return;
        }

        StructurePlanActive.transform.position = hit.point;

        if (Input.GetMouseButtonDown(0))
        {
            StructurePlanActive.StartThePlan();
            StructurePlanActive = null;
            OnDismiss();
        }
    }
}
