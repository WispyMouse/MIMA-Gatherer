using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectedGameworldObjectPanel : MonoBehaviour
{
    [SerializeReference]
    private TMP_Text DisplayText;

    [SerializeReference]
    private GameObject PanelToggle;

    GameworldObject SelectedGameworldObject { get; set; } = null;

    private void Awake()
    {
        OnDismiss();
    }

    public void OnGameworldObjectSelected(GameworldObject selected)
    {
        PanelToggle.SetActive(true);
        DisplayText.text = $"{selected.DisplayName}\n{selected.GetCurrentTask()}";
        SelectedGameworldObject = selected;
    }

    public void OnDismiss()
    {
        PanelToggle.SetActive(false);
        SelectedGameworldObject = null;
    }

    private void Update()
    {
        if (SelectedGameworldObject == null)
        {
            return;
        }

        // TODO HACK: This should not update every frame, should only do so when there is an update to be had.
        // This is doing this now because the current task might change, and we aren't raising an event to signal a needed change yet
        DisplayText.text = $"{SelectedGameworldObject.DisplayName}\n{SelectedGameworldObject.GetCurrentTask()}";
    }
}
