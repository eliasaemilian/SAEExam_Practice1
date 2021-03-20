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
    }

    private void OnDisable()
    {
        // design
        CheckForPlayerController();
        playerController.ItemPickupEvent.RemoveListener( GetPickedUpByPlayer );
    }

    private void GetPickedUpByPlayer(GameObject go)
    {
        bool valid = false;
        if ( go == this ) valid = true;
        else
        {           
            for ( int i = 0; i < children.Length; i++ )
            {
                if ( go == children[i].gameObject ) valid = true;
            }
        }

        if (valid)
        {
            Debug.Log( $"{name} got picked up!" );
            Destroy( gameObject );
        }
    }

    private void CheckForPlayerController()
    {
        if ( playerController == null )
        {
            playerController = FindObjectOfType<PlayerController>();
        }
    }
}
