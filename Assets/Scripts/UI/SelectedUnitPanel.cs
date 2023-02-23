using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectedUnitPanel : MonoBehaviour
{
    [SerializeReference]
    private TMP_Text UnitText;

    Unit SelectedUnit { get; set; } = null;

    private void Awake()
    {
        OnDismiss();
    }

    public void OnUnitSelected(Unit selected)
    {
        UnitText.gameObject.SetActive(true);
        UnitText.text = $"{selected.name}\n{selected.GetCurrentTask()}";
        SelectedUnit = selected;
    }

    public void OnDismiss()
    {
        UnitText.gameObject.SetActive(false);
        SelectedUnit = null;
    }

    private void Update()
    {
        if (SelectedUnit == null)
        {
            return;
        }

        // TODO HACK: This should not update every frame, should only do so when there is an update to be had.
        UnitText.text = $"{SelectedUnit.name}\n{SelectedUnit.GetCurrentTask()}";
    }
}
