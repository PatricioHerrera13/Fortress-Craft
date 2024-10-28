using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PointerController : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public RectTransform safeZone;
    public float moveSpeed = 100f;
    public TanqueComb tanqueComb; // Referencia al TanqueComb para finalizar el QTE

    private RectTransform pointerTransform;
    private Vector3 targetPosition;

    void Start()
    {
        pointerTransform = GetComponent<RectTransform>();
        targetPosition = pointB.position;
    }

    void Update()
    {
        pointerTransform.position = Vector3.MoveTowards(pointerTransform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(pointerTransform.position, pointA.position) < 0.1f)
        {
            targetPosition = pointB.position;
        }
        else if (Vector3.Distance(pointerTransform.position, pointB.position) < 0.1f)
        {
            targetPosition = pointA.position;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckSuccess();
        }
    }

    void CheckSuccess()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(safeZone, pointerTransform.position, null))
        {
            Debug.Log("Success!");
            tanqueComb.CompleteQTE(true);
        }
        else
        {
            Debug.Log("Fail!");
            tanqueComb.CompleteQTE(false);
        }
    }
}
