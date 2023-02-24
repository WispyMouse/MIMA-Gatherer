using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenPanningSensor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Directionality Directionality;

    [SerializeReference]
    private DirectionalityEvent PanningStartEvent;
    [SerializeReference]
    private DirectionalityEvent PanningEndEvent;
    [SerializeField]
    private ConfiguredStatistic<float> Width = new ConfiguredStatistic<float>(20f, $"{nameof(ScreenPanningSensor)}.{nameof(Width)}.Side");


    void Start()
    {
        if (Directionality.XSign != 0)
        {
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Width.Value);
        }
        else if (Directionality.ZSign != 0)
        {
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Width.Value);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PanningStartEvent.Raise(Directionality);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PanningEndEvent.Raise(Directionality);
    }
}
