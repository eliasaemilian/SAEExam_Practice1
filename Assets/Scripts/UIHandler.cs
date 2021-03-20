using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public class UIEvent : UnityEvent<Item>
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
    public static UIEvent UIItemGotSelected;


    public static UnityEvent InventoryOpened, InventoryClosed;

    private void Awake()
    {
        popUpText = PopUpItemPickup.GetComponentInChildren<TextMeshProUGUI>();
        EnableItemPickup = new UIEvent();   
        DisableItemPickup = new UIEvent();
        UIItemGotSelected = new UIEvent();
        InventoryOpened = new UnityEvent();
        InventoryClosed = new UnityEvent();
        UIItemGotSelected.AddListener( FillSelectedItemPanel );
        EnableItemPickup.AddListener( EnableItemPopUp );
        DisableItemPickup.AddListener( DisableItemPopUp );
        PopUpItemPickup.SetActive( false );
        InventoryContainer.SetActive( false );
        PlayerController.inventory.InventoryChanged.AddListener( GenerateInventory );
        ClearSelectedItemPanel();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if ( InventoryContainer.activeSelf )
            {
                InventoryContainer.SetActive( false );
                InventoryClosed.Invoke();
            }
            else
            {
                InventoryContainer.SetActive( true );
                InventoryOpened.Invoke();
            }
        }

    }


    private void CreateNewItemEntry(Item item)
    {
        GameObject entry = Instantiate( ItemEntryPrefab );
        entry.transform.SetParent(ItemContainer);
        entry.GetComponentInChildren<TextMeshProUGUI>().text = item.ItemName;
        entry.GetComponent<InventoryItem>().Item = item;

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

    private void ClearSelectedItemPanel()
    {
        UI_ItemName.text = "";
        UI_ItemDescription.text = "";
        UI_ItemWeight.text = "";
        UI_ItemValue.text = "";
    }

    private void EnableItemPopUp( Item item )
    {
        popUpText.text = $"Press [ E ] to pickup {item.ItemName}";
        PopUpItemPickup.SetActive( true );
    }

    private void DisableItemPopUp( Item item )
    {
        PopUpItemPickup.SetActive( false );
    }
}
