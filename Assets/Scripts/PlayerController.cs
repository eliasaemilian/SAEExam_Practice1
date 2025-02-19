﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[System.Serializable]
public class PlayerEvent : UnityEvent<GameObject>
{

}

public class ItemEvent : UnityEvent<Item>
{

}
public class PlayerController : MonoBehaviour
{
    [SerializeField] float rotationPower = 3f;
    [SerializeField] float walkSpeed = 1f;
    [SerializeField] float sprintSpeed = 3f;
    [SerializeField] float pickupRange = 50f;
    [SerializeField] string itemTag = "Item";
    [SerializeField] Transform followTransform;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rigidbody;

    public PlayerEvent ItemPickupEvent = new PlayerEvent();
    public PlayerEvent ItemSeenEvent = new PlayerEvent();

    public ItemEvent ValidItemIsAddedToInventory = new ItemEvent();

    Vector2 moveInput;
    Vector2 lookInput;
    float sprintInput;

    private bool haltMovement;
    private bool encumbered;
    public static Inventory inventory = new Inventory();

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        inventory.MaxWeight = 100;
        ValidItemIsAddedToInventory.AddListener( AddItemToInventory );
        UIHandler.InventoryOpened.AddListener( FreezePlayer );
        UIHandler.InventoryClosed.AddListener( UnfreezePlayer );
    }

    private void FreezePlayer()
    {
        haltMovement = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void UnfreezePlayer()
    {
        haltMovement = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void FixedUpdate()
    {
        if ( haltMovement ) return;

        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        lookInput = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
        if ( !encumbered ) sprintInput = Input.GetAxis("Sprint");

        UpdateFollowTargetRotation();

        float speed = 0;

        speed = Mathf.Lerp(walkSpeed, sprintSpeed, sprintInput);
        Vector3 movement = (transform.forward * moveInput.y * speed) + (transform.right * moveInput.x * speed);
        rigidbody.velocity = new Vector3(movement.x, rigidbody.velocity.y, movement.z);

        animator.SetFloat("MovementSpeed", moveInput.y * (speed / walkSpeed));

        //only rotate the player when moving, allows user to look at the player when idle
        if (moveInput.magnitude > 0.01f)
        {
            //Set the player rotation based on the look transform
            transform.rotation = Quaternion.Euler(0, followTransform.eulerAngles.y, 0);
            //reset the y rotation of the look transform
            followTransform.localEulerAngles = new Vector3(followTransform.localEulerAngles.x, 0, 0);
        }
    }

    private void Update()
    {
        CheckForItemInLineOfSight();
    }

    private void CheckForItemInLineOfSight()
    {
        RaycastHit hit;
        if ( Physics.Raycast( transform.position, transform.forward, out hit, pickupRange ) )
        {
            if ( hit.collider.tag == itemTag )
            {
                Debug.Log( "Seeing " + hit.collider.gameObject.name );

                ItemSeenEvent.Invoke( hit.collider.gameObject );

                if ( Input.GetKeyUp( KeyCode.E ) )
                {
                    // call item event to add to inventory
                    ItemPickupEvent.Invoke( hit.collider.gameObject );

                    // play Pickup Animation
                    PlayPickupAnim();
                }

            }

        }
        else UIHandler.DisableItemPickup.Invoke( null );
    }

    private void AddItemToInventory( Item item )
    {
        if ( CheckIfMaxWeightIsReached() ) encumbered = true;
        else
        {
            inventory.AddItemToInventory(item);

            encumbered = false;
        }

    }

    private bool CheckIfMaxWeightIsReached() 
    {
        if ( inventory.MaxWeight <= 0 ) return false;
        if ( inventory.CarriedWeight > inventory.MaxWeight ) return true;
        else return false;
    }

    private void PlayPickupAnim()
    {
        haltMovement = true;
        animator.SetBool( "pickup", true);
        StartCoroutine( PickupAnimation() );
    }

    IEnumerator PickupAnimation()
    {
        yield return new WaitForSeconds( 2f );
        animator.SetBool( "pickup", false );

        haltMovement = false;
    }

    private void UpdateFollowTargetRotation()
    {
        //Update follow target rotation
        followTransform.rotation *= Quaternion.AngleAxis(lookInput.x * rotationPower, Vector3.up);
        followTransform.rotation *= Quaternion.AngleAxis(lookInput.y * rotationPower, Vector3.right);

        var angles = followTransform.localEulerAngles;
        angles.z = 0;
        var angle = angles.x;

        //Clamp the Up/Down rotation
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }
        followTransform.localEulerAngles = angles;
    }


}
