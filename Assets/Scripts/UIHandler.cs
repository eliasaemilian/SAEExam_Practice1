using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public class UIEvent : UnityEvent<string>
{

}
public class UIHandler : MonoBehaviour
{
    [SerializeField] private GameObject PopUpItemPickup;
    private TextMeshProUGUI popUpText;
    public static UIEvent EnableItemPickup, DisableItemPickup;

    private void Awake()
    {
        popUpText = PopUpItemPickup.GetComponentInChildren<TextMeshProUGUI>();
        EnableItemPickup = new UIEvent();   
        DisableItemPickup = new UIEvent();
        EnableItemPickup.AddListener( EnableItemPopUp );
        DisableItemPickup.AddListener( DisableItemPopUp );
        PopUpItemPickup.SetActive( false );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EnableItemPopUp( string itemName )
    {
        popUpText.text = $"Press [ E ] to pickup {itemName}";
        PopUpItemPickup.SetActive( true );
    }

    private void DisableItemPopUp( string itemName )
    {
        PopUpItemPickup.SetActive( false );
    }
}
