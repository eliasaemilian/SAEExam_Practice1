using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    [SerializeField] private Item item;

    private PlayerController playerController;
    private Collider[] children;
    // Start is called before the first frame update
    void Start()
    {
       children  = GetComponentsInChildren<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        // signup
        CheckForPlayerController();
        playerController.ItemPickupEvent.AddListener( GetPickedUpByPlayer );
        playerController.ItemSeenEvent.AddListener( GetSeenByPlayer );
    }

    private void OnDisable()
    {
        // design
        CheckForPlayerController();
       // if ( playerController.ItemPickupEvent != null ) playerController.ItemPickupEvent.RemoveListener( GetPickedUpByPlayer );
    }

    private void GetSeenByPlayer(GameObject go)
    {
        if ( CheckIfValid( go ) )
        {
            UIHandler.EnableItemPickup.Invoke( item.ItemName );
        }
    }

    private void GetPickedUpByPlayer(GameObject go)
    {
        if (CheckIfValid(go))
        {
            Debug.Log( $"{name} got picked up!" );
            Destroy( gameObject );
            UIHandler.DisableItemPickup.Invoke( "" );

            CheckForPlayerController();
            playerController.ValidItemIsAddedToInventory.Invoke( item );
        }
    }

    private bool CheckIfValid(GameObject go)
    {
        if ( go == this ) return true;
        else
        {
            for ( int i = 0; i < children.Length; i++ )
            {
                if ( go == children[i].gameObject ) return true;
            }
        }

        return false;
    }

    private void CheckForPlayerController()
    {
        if ( playerController == null )
        {
            playerController = FindObjectOfType<PlayerController>();
        }
    }
}
