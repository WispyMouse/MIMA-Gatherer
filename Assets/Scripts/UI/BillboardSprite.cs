using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    private void Update()
    {
        transform.localRotation = Camera.main.transform.localRotation;
    }
}
