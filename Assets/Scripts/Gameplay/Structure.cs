using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : GameworldObject
{
    [SerializeReference]
    private Unit UnitPrefab;

    [SerializeField]
    private float DistanceToSpawnUnit = 1f;

    public override void Interact()
    {
        int amountOfCrystals = InventoryManagement.ActiveInventoryInstance.GetGatherableCount(nameof(Crystals)).Count;
        if (amountOfCrystals < UnitPrefab.CrystalUnitCost)
        {
            Debug.Log($"Insufficient crystals. Requires {UnitPrefab.CrystalUnitCost} crystals.");
            return;
        }

        InventoryManagement.ChangeResource(nameof(Crystals), -UnitPrefab.CrystalUnitCost);

        StartCoroutine(SpawnUnitAfterWaiting(UnitPrefab));

        base.Interact();
    }

    IEnumerator SpawnUnitAfterWaiting(Unit toSpawn)
    {
        yield return new WaitForSeconds(toSpawn.ProductionTimeSeconds);

        Unit newUnit = Instantiate(UnitPrefab);

        Vector2 randomOffset = Random.insideUnitCircle.normalized * DistanceToSpawnUnit;
        newUnit.transform.position = transform.position + new Vector3(randomOffset.x, 0, randomOffset.y);

        yield break;
    }
}
