using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public GameObject HandPoint;

    private GameObject pickedItem = null;



    void Update()
    {
        if (pickedItem != null)
        {
            if (Input.GetKey("r"))
            {
                pickedItem.GetComponent<Rigidbody>().useGravity = true;

                pickedItem.GetComponent<Rigidbody>().isKinematic = false;

                pickedItem.gameObject.transform.SetParent(null);

                pickedItem = null;
            }
        }

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            if (Input.GetKey("e") && pickedItem == null)
            {
                other.GetComponent<Rigidbody>().useGravity = false;

                other.GetComponent<Rigidbody>().isKinematic = true;

                other.transform.position = HandPoint.transform.position;

                other.gameObject.transform.SetParent(HandPoint.gameObject.transform);

                pickedItem = other.gameObject;
            }
        }

    }


}
