using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalPatch : MonoBehaviour
{
    [SerializeReference]
    private Crystals CrystalsPrefab;

    [SerializeField]
    private int CountOfCrystalsToSpawn;

    [SerializeField]
    private float MaximumCrystalDistanceFromCenter;

    List<Crystals> SpawnedCrystals { get; set; } = new List<Crystals>();

    public void Awake()
    {
        for (int ii = 0; ii < CountOfCrystalsToSpawn; ii++)
        {
            Vector3 randomPoint = Random.insideUnitCircle * MaximumCrystalDistanceFromCenter;
            Vector3 randomPointOnPlane = transform.position + new Vector3(randomPoint.x, 0, randomPoint.y);
            Crystals newCrystals = Instantiate(CrystalsPrefab, randomPointOnPlane, Quaternion.identity, this.transform);
            SpawnedCrystals.Add(newCrystals);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, MaximumCrystalDistanceFromCenter);
    }
}
