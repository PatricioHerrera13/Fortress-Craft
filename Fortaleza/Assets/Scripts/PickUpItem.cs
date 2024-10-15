using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public GameObject HandPoint;
    private GameObject pickedItem = null;

    public string pickedItemType = "";
    public float throwForce = 15f;
    public float throwAngle = 45f;

    private Vector3 lastMoveDirection = Vector3.zero;
    private Vector3 lastPosition = Vector3.zero;

    public float HoldTime = 2;
    private bool StartTimer;

    void Update()
    {
        DetectMovement();

        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (pickedItem == null)
            {
                TryPickUpItem();
            }
            else
            {
                ReleaseItem();
            }
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            
            StartTimer = true;
            StartCoroutine(HoldTimer());
            
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            StartTimer = false;
        }

    }

    IEnumerator HoldTimer()
    {
        Debug.Log("Empezó Timer!");
        yield return new WaitForSeconds(HoldTime);
        if (!StartTimer)
        {
            Debug.Log("No se pudo");
        }
        else
        {
            Debug.Log("Lanzamiento!");
            ThrowItem();
        }
    }

    private void DetectMovement()
    {
        if (transform.position != lastPosition)
        {
            lastMoveDirection = (transform.position - lastPosition).normalized;
            lastPosition = transform.position;
        }
    }

    private void TryPickUpItem()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider other in colliders)
        {
            if (other.CompareTag("Item"))
            {
                Item itemComponent = other.GetComponent<Item>();
                if (itemComponent != null)
                {
                    other.GetComponent<Rigidbody>().useGravity = false;
                    other.GetComponent<Rigidbody>().isKinematic = true;
                    other.transform.position = HandPoint.transform.position;
                    other.gameObject.transform.SetParent(HandPoint.transform);

                    pickedItem = other.gameObject;
                    pickedItemType = itemComponent.itemType;
                    break;
                }
            }
        }
    }

    private void ReleaseItem()
    {
        if (pickedItem != null)
        {
            pickedItem.GetComponent<Rigidbody>().useGravity = true;
            pickedItem.GetComponent<Rigidbody>().isKinematic = false;
            pickedItem.transform.SetParent(null);
            pickedItemType = "";
            pickedItem = null;
        }
    }

    private void ThrowItem()
    {
        if (pickedItem != null)
        {
            pickedItem.transform.SetParent(null);
            Rigidbody itemRb = pickedItem.GetComponent<Rigidbody>();
            itemRb.useGravity = true;
            itemRb.isKinematic = false;

            Vector3 throwDirection = lastMoveDirection;
            if (throwDirection == Vector3.zero)
            {
                throwDirection = transform.forward;
            }

            throwDirection = (throwDirection + Vector3.up * Mathf.Tan(throwAngle * Mathf.Deg2Rad)).normalized;
            itemRb.AddForce(throwDirection * throwForce, ForceMode.VelocityChange);

            pickedItem = null;
            pickedItemType = "";
        }
    }

    public string GetPickedItemType()
    {
        return pickedItemType;
    }
}
