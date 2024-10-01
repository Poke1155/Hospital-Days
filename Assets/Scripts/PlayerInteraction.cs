using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // Todo: Inventory implementation
    // logic for methods, reference to class
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private Transform objectGrabPoint;
    [SerializeField] private LayerMask interactLayerMask;
    private IInteractable interactableObject;
    private ObjectGrabbable objectGrabbable;
    private float interactRange = 1000f;
    
    [SerializeField] private Inventory inventory;
    void Update()
    {
        // adding the layer mask here creates null errors when you interact with stuff not in layer, is fine to have??
        // physics raycast every frame to try and incorporate UI elements to interactions
        Physics.Raycast(cameraPosition.position, cameraPosition.forward, out RaycastHit raycastHit, interactRange, interactLayerMask);
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objectGrabbable == null)
            {
                if (raycastHit.transform.TryGetComponent(out interactableObject))
                {
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable)) 
                    { 
                        objectGrabbable.Grab(objectGrabPoint); 
                    }
                    else 
                    {
                            interactableObject.Interact(interactableObject.GetTransform()); 
                    } 
                } 
            } else 
            {
                objectGrabbable.Drop();
                objectGrabbable = null;
            }
        } 
        // on drop button
        if (Input.GetKeyDown(KeyCode.Q) && objectGrabbable)
        {
            // null check in case something that is grabbable can't be put in inventory/doesn't have an item reference
            if (objectGrabbable != null && objectGrabbable.GetInventoryItem != null)
            {
                inventory.AddItem(objectGrabbable.GetInventoryItem);   
                objectGrabbable.Remove();
                objectGrabbable = null;
            }
        }
    }
    // public IInteractable GetInteractableObject()
    // {
    //     List<IInteractable> interactables = new List<IInteractable>();
    //     Collider[] colliders = Physics.OverlapSphere(transform.position, interactRange);
    //     foreach (Collider collider in colliders)
    //     {
    //         if (collider.TryGetComponent(out IInteractable interactable))
    //         {
    //             interactables.Add(interactable);   
    //         }
    //     }
    //
    //     IInteractable closestInterable = null;
    //     foreach (IInteractable interactable in interactables)
    //     {
    //         if (closestInterable == null)
    //         {
    //             closestInterable = interactable;
    //         }
    //         else if (Vector3.Distance(transform.position, interactable.GetTransform().position) 
    //                  < Vector3.Distance(transform.position, closestInterable.GetTransform().position))
    //         {
    //             closestInterable = interactable;
    //         }
    //     }
    //
    //     return closestInterable;
    // }
}
