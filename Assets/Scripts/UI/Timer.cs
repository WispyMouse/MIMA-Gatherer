using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float Duration
    {
        get
        {
            return _Duration;
        }
        set
        {
            _Duration = value;
            UpdateGraphic();
        }
    }
    private float _Duration { get; set; }
    public float CurrentValue
    {
        get
        {
            return _CurrentValue;
        }
        set
        {
            _CurrentValue = value;
            UpdateGraphic();
        }
    }
    private float _CurrentValue { get; set; }

    [SerializeReference]
    private Image CurrentValueGraphic;

    public float Scale
    {
        get
        {
            return _Scale;
        }
        set
        {
            _Scale = value;
            UpdateGraphic();
        }
    }
    private float _Scale { get; set; } = 1f;

    public string TimerFrontColor
    {
        get
        {
            return _TimerFrontColor;
        }
        set
        {
            _TimerFrontColor = value;
            UpdateGraphic();
        }
    }
    private string _TimerFrontColor { get; set; } = "#FFFFFF";

    private void Start()
    {
        UpdateGraphic();
    }

    private void UpdateGraphic()
    {
        if (Duration <= 0)
        {
            Debug.LogError($"Duration of timer is 0, cannot update graphic.");
            return;
        }

        CurrentValueGraphic.fillAmount = CurrentValue / Duration;
        transform.localScale = Vector3.one * Scale;

        if (ColorUtility.TryParseHtmlString(TimerFrontColor, out Color parsedColor))
        {
            CurrentValueGraphic.color = parsedColor;
        }
    }

    public void Dismiss()
    {
        TimerManagement.ReturnTimerToPool(this);
    }
}
