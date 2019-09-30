using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;



public class PointerExitRemove : MonoBehaviour, IPointerExitHandler, IPointerClickHandler
{
    public void OnPointerExit(PointerEventData eventData)
    {
        this.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        this.gameObject.SetActive(false);
    }

}
