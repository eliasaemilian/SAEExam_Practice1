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
    [SerializeField] private GameObject InventoryContainer;
    [SerializeField] private Transform ItemContainer;
    [SerializeField] private GameObject SelectedItemDescription;

    // SELECTED ITEM PANEL
    [SerializeField] private TextMeshProUGUI UI_ItemName;
    [SerializeField] private TextMeshProUGUI UI_ItemDescription;
    [SerializeField] private TextMeshProUGUI UI_ItemValue;
    [SerializeField] private TextMeshProUGUI UI_ItemWeight;

    [SerializeField] private GameObject ItemEntryPrefab;

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
        InventoryContainer.SetActive( false );
        PlayerController.inventory.InventoryChanged.AddListener( GenerateInventory );
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (InventoryContainer.activeSelf ) InventoryContainer.SetActive( false );
            else InventoryContainer.SetActive( true );
        }
    }

    private void CreateNewItemEntry(Item item)
    {
        GameObject entry = Instantiate( ItemEntryPrefab );
        entry.transform.SetParent(ItemContainer);
        entry.GetComponentInChildren<TextMeshProUGUI>().text = item.ItemName;

    }

    private void GenerateInventory()
    {
        for ( int i = 0; i < ItemContainer.childCount; i++ )
        {
            Destroy(ItemContainer.GetChild( i ).gameObject);

        }

        for ( int i = 0; i < PlayerController.inventory.Items.Count; i++ )
        {
            CreateNewItemEntry( PlayerController.inventory.Items[i] );
        }
    }

    private void FillSelectedItemPanel( Item item )
    {
        UI_ItemName.text = item.ItemName;
        UI_ItemDescription.text = item.Description;
        UI_ItemWeight.text = $"Weight: {item.Weight}";
        UI_ItemValue.text = $"Value: {item.Value}";
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
