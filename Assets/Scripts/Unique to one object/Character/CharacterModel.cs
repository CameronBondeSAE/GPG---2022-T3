using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    public float speed = 10f;
    public float jumpHeight = 5f;
    public Vector2 movementDirection;
    public float exitVehicleForce = 100f;

    [SerializeField]
    private float turnSpeed = 15f;

    public float interactDistance = 1f;

    [Header("Setup")]
    public Rigidbody rb;

    public Vector3 interactRayOffset = new Vector3(0, 0.5f, 0);
    public float onGroundDrag = 5f;

    [Header("Info. Don't edit")]
    public bool onGround = true;

    public bool inVehicle = false;
    // public IVehicle IVehicleReference;
    public Vector3 lookMovementDirection;


    public event Action JumpEvent;
    public event Action LandedEvent;
    public event Action<bool> OnGroundEvent;

    public event Action<bool> GetInVehicleEvent;
    public event Action<bool> CryingEvent;

    [Header("Cry Variables")]
    public GameObject grass;

    public float maxDistance = 1.8f;
    public float cryTimer = 3f;
    public Vector3 heightOffset = new Vector3(0, 0.5f, 0);
    float cryTimerValue;

    public IPickupable holdingObject;
    private GameObject holdingObjectGO;

    [SerializeField]
    Transform holdingMount;

    [SerializeField]
    float throwForce = 3f;

    // TrailerModel trailer;

    [SerializeReference]
    IPickupable pickupableNearby;

    Coroutine cryCoroutine;
    [SerializeField]
    private bool isCrying;

    void FixedUpdate()
    {
        if (!onGround)
        {
            rb.drag = 0f;
            return;
        }
        else
        {
            rb.drag = onGroundDrag;
        }

        Vector3 movementDirectionFinal = new Vector3(movementDirection.x, 0, movementDirection.y);
        rb.AddForce(movementDirectionFinal * speed, ForceMode.Acceleration);
        lookMovementDirection = movementDirectionFinal;
        lookMovementDirection.y = 0; // Don't look up and down

        // Don't go crazy if you're not moving at all
        if (lookMovementDirection.magnitude > 0.01f)
        {
            // Don't insta look using transform.LookAt, instead Lerp slowlyish so it looks better
            // This calculates the WORLD SPACE rotation to look at a point FROM 0,0,0
            Quaternion toRotation = Quaternion.LookRotation(lookMovementDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);

            // Old line, don't use, too crappy looking
            // transform.LookAt(transform.position + lookMovementDirection);
        }
    }

    public void MoveDirection(Vector2 direction)
    {
        if (inVehicle)
        {
            // IVehicleReference.Accelerate(direction.y);
            // IVehicleReference.Steer(direction.x);
        }
        else
        {
            movementDirection = direction;
        }
    }

    public void Jump()
    {
        if (!onGround)
            return;

        rb.drag = 0f;
        rb.AddForce(0, jumpHeight, 0, ForceMode.VelocityChange);
        JumpEvent?.Invoke();
    }

    public void Interact()
    {
	    RaycastHit hit = CheckWhatsInFrontOfMe();

        // Vehicles?
        // Review: This won't work if it has multiple IVehicle
        // IVehicleReference = hit.collider.gameObject.GetComponentInParent<IVehicle>();
        // if (IVehicleReference != null)
        // {
            // if (!inVehicle)
                // GetInVehicle();
        // }

        // Review: This won't work if it has multiple IInteractables
        // IInteractable interactable = hit.collider.gameObject.GetComponentInParent<IInteractable>();
        // if (interactable != null)
        // {
            // interactable.Interact();
        // }
    }


    public void PickUpCheck()
    {
        //Always check if something is in front in case player wants to place entities on a trailer
        RaycastHit hit = CheckWhatsInFrontOfMe();

        if (hit.collider != null)
        {
            // if (hit.collider.gameObject.GetComponentInParent<TrailerModel>() != null)
            // {
                // trailer = hit.collider.gameObject.GetComponentInParent<TrailerModel>();
            // }

            if (hit.collider.gameObject.GetComponentInParent<IPickupable>() != null)
            {
                pickupableNearby = hit.collider.gameObject.GetComponentInParent<IPickupable>();
            }
        }
        else
        {
            // trailer = null;
            pickupableNearby = null;
        }

        // Already holding something, so drop it
        // if (holdingObject != null && !inVehicle && trailer == null)
        // {
            // Drop(true);
            // return;
        // }

        //Holding something and in front of a trailer, place it on trailer
        // if (trailer != null && holdingObject != null)
        // {
        //     //TODO: Might not need this if we use a single point for a spawn area
        //     //for (int i = 0; i < trailer.mounts.Length; i++)
        //     {
        //         //TODO: Trailer mounts no longer have child GO - need to do physics check?
        //         //if (trailer.mounts[i].childCount == 0)
        //         {
        //             //Hack: Drop has a throw feature
        //             //holdingObjectGO.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //             trailer.objectsInTrailer.Add(holdingObjectGO);
        //             holdingObjectGO.transform.position = trailer.mounts[0].transform.position;
        //             holdingObjectGO.transform.rotation = trailer.mounts[0].rotation;
        //             //holdingObjectGO = null;
        //             Drop(false);
        //             return;
        //         }
        //     }
        // }

        //Not holding anything but in front of a trailer, grab whatever is on the trailer
        //TODO: NOT WORKING
        // else if (trailer != null && holdingObject == null && trailer.objectsInTrailer.Count > 0)
        // {
        //     holdingObjectGO = trailer.objectsInTrailer[0];
        //     trailer.objectsInTrailer.Remove(holdingObjectGO);
        //     //pickupableNearby = holdingObjectGO.GetComponentInParent<IPickupable>();
        //     Pickup(holdingObjectGO);
        //     return;
        // }


        // HACK: Hardcoded to tractor. Should be able to use interface
        // Note: Characters only need to tell tractors to DETACH. Tractors auto attach on contact
        // TractorModel tractorModel = IVehicleReference as TractorModel;
        // if (inVehicle && tractorModel.hasAttachment)
        // {
        //     tractorModel.Detach();
        //     return;
        // }

        // Pickup? NOT in vehicle, NOT holding something already
        //Hack: Bug allowing player to pick up trailer
        if (pickupableNearby != null)
        {
            Pickup(hit.collider.gameObject);
        }
    }

    private void Pickup(GameObject pickupGO)
    {
        holdingObjectGO = pickupGO;
        holdingObject = pickupGO.GetComponent<IPickupable>();
        // holdingObject.PickUp();
        holdingObjectGO.transform.parent = holdingMount;
        holdingObjectGO.transform.localPosition = Vector3.zero;
        holdingObjectGO.transform.rotation = holdingMount.rotation;
    }
    
    
    RaycastHit CheckWhatsInFrontOfMe()
    {
        // Check what's in front of me. TODO: Make it scan the area or something less precise
        RaycastHit hit;
        // Ray        ray = new Ray(transform.position + transform.TransformPoint(interactRayOffset), transform.forward);
        // NOTE: TransformPoint I THINK includes the main position, so you don't have to add world position to the final
        Vector3 transformPoint = transform.TransformPoint(interactRayOffset);
        Debug.Log(transformPoint);
        Ray ray = new Ray(transformPoint, transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.green, 2f);

        // if (Physics.Raycast(ray, out hit, interactDistance))
        Physics.SphereCast(ray, 0.5f, out hit, interactDistance);

        return hit;
    }

    void OnCollisionEnter(Collision other)
    {
        onGround = true;
        LandedEvent?.Invoke();
    }

    void OnCollisionExit(Collision other)
    {
        onGround = false;
        OnGroundEvent?.Invoke(false);
    }

    void OnCollisionStay(Collision other)
    {
        onGround = true;
        OnGroundEvent?.Invoke(true);
    }
}