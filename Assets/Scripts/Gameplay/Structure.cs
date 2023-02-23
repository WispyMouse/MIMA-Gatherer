using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : GameworldObject
{
    [SerializeReference]
    private Unit UnitPrefab;

    [SerializeField]
    private ConfiguredStatistic<float> DistanceToSpawnUnit { get; set; } = new ConfiguredStatistic<float>(1f, $"{nameof(Structure)}.{nameof(DistanceToSpawnUnit)}");

    private void Awake()
    {
        DistanceToSpawnUnit.LoadFromConfiguration(ConfigurationManagement.ActiveConfiguration);
    }

    public override void Interact()
    {
        int amountOfCrystals = InventoryManagement.ActiveInventoryInstance.GetGatherableCount(nameof(Crystals)).Count;
        if (amountOfCrystals < UnitPrefab.CrystalUnitCost.Value)
        {
            Debug.Log($"Insufficient crystals. Requires {UnitPrefab.CrystalUnitCost} crystals.");
            return;
        }

        InventoryManagement.ChangeResource(nameof(Crystals), -UnitPrefab.CrystalUnitCost.Value);

        StartCoroutine(SpawnUnitAfterWaiting(UnitPrefab));

        base.Interact();
    }

    IEnumerator SpawnUnitAfterWaiting(Unit toSpawn)
    {
        yield return new WaitForSeconds(toSpawn.ProductionTimeSeconds.Value);

        Unit newUnit = Instantiate(UnitPrefab);

        Vector2 randomOffset = Random.insideUnitCircle.normalized * DistanceToSpawnUnit.Value;
        newUnit.transform.position = transform.position + new Vector3(randomOffset.x, 0, randomOffset.y);

        yield break;
    }
}
