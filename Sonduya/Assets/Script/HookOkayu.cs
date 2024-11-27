using System.Collections;
using UnityEngine;

public class HookOkayu : MonoBehaviour
{
    public Transform hookPoint; 
    public float ropeLength = 5f;  
    public float maxPullDistance = 8f;  
    public float followSpeed = 10f; 
    public KeyCode grabKey = KeyCode.E;
    public KeyCode releaseKey = KeyCode.R;

    private GameObject grabbedObject = null;
    private bool isGrabbing = false;

    void Update()
    {
        if (Input.GetKeyDown(grabKey))
        {
            TryGrabObject();
        }

        if (Input.GetKeyDown(releaseKey))
        {
            ReleaseObject();
        }

        if (isGrabbing && grabbedObject != null)
        {
            HandleRope();
        }
    }

    void TryGrabObject()
    {
        RaycastHit2D hit = Physics2D.Raycast(hookPoint.position, hookPoint.right, maxPullDistance);
        if (hit.collider != null)
        {
            Rigidbody2D hitRb = hit.collider.GetComponent<Rigidbody2D>();
            if (hitRb != null)
            {
                grabbedObject = hit.collider.gameObject;
                isGrabbing = true;
            }
        }
    }

    void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject = null;
            isGrabbing = false;
            Debug.Log("Объект отцепился");
        }
    }

    void HandleRope()
    {
        Vector2 hookPosition = hookPoint.position;

        Vector2 directionToHook = (hookPosition - (Vector2)grabbedObject.transform.position).normalized;
        float distance = Vector2.Distance(hookPosition, grabbedObject.transform.position);

        if (distance > ropeLength)
        {
            Rigidbody2D grabbedRb = grabbedObject.GetComponent<Rigidbody2D>();

            Vector2 targetPosition = hookPosition - directionToHook * ropeLength;
            grabbedRb.velocity = (targetPosition - (Vector2)grabbedObject.transform.position) * followSpeed;
        }
        else if(distance > ropeLength * 2)
        {
            ReleaseObject();
        }
    }
}