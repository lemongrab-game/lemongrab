using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class GeneratedOnClick : MonoBehaviour, IPointerClickHandler
{
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if(GetComponent<Button>().interactable == true)
        {
            Text text = GetComponentInChildren<Text>();
            if (text != null)
            {
                gameObject.SendMessageUpwards("SetCurrentAction", text.text);
            }
        }
    }

}
