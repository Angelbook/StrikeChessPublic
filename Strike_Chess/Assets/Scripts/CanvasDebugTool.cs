using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasDebugTool : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Name: " + eventData.pointerCurrentRaycast.gameObject.name);
        Debug.Log("Tag: " + eventData.pointerCurrentRaycast.gameObject.tag);
        Debug.Log("GameObject: " + eventData.pointerCurrentRaycast.gameObject);
    }
}
