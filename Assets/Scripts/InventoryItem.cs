using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryItem : MonoBehaviour
{
    public Item Item;
    TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }


    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var wordIndex = 0;
            if ( text != null ) wordIndex = TMP_TextUtilities.FindIntersectingWord( text, Input.mousePosition, null );
            else return;

            if ( wordIndex != -1 )
            {
                UIHandler.UIItemGotSelected.Invoke( Item );

            }
        }

    }

}
