using System.Collections;
using UnityEngine;

public class HookOkayu : MonoBehaviour
{
    public Transform playerPosition;
    public Transform hookPoint;
    public float ropeLength = 5f;
    public float maxPullDistance = 5f;
    public float grabDistance = 2f;
    public float followSpeed = 10f;
    public KeyCode grabKey = KeyCode.E;
    public KeyCode releaseKey = KeyCode.R;

    private GameObject grabbedObject = null;
    private bool isGrabbing = false;

    private LineRenderer lineRenderer;

    private int layerIndex;

    void Start()
    {
        layerIndex = LayerMask.NameToLayer("Objects");

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    void LateUpdate()
    {
        if (isGrabbing && grabbedObject != null)
        {
            lineRenderer.SetPosition(0, playerPosition.position);
            lineRenderer.SetPosition(1, grabbedObject.transform.position);
        }
        else
        {
            lineRenderer.SetPosition(0, playerPosition.position);
            lineRenderer.SetPosition(1, playerPosition.position);
        }
    }

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
        // Определяем направление Raycast в зависимости от направления взгляда игрока
        Vector2 direction = hookPoint.right;
        if (playerPosition.localScale.x < 0) // Если игрок смотрит налево
        {
            direction = -hookPoint.right;
        }

        RaycastHit2D hit = Physics2D.Raycast(hookPoint.position, direction, grabDistance);
        if (hit.collider != null && hit.collider.gameObject.layer == layerIndex)
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
        Vector2 playerPos = playerPosition.position;
        Vector2 objectPosition = grabbedObject.transform.position;

        Vector2 directionToHook = (playerPos - objectPosition).normalized;
        float distance = Vector2.Distance(playerPos, objectPosition);

        Rigidbody2D grabbedRb = grabbedObject.GetComponent<Rigidbody2D>();

        // Проверка на превышение максимальной дистанции
        if (distance >= maxPullDistance)
        {
            ReleaseObject();
            Debug.Log("Объект отцепился: превышено максимальное расстояние");
            return;
        }

        // Если объект находится дальше длины верёвки, подтягиваем его
        if (distance > ropeLength)
        {
            Vector2 targetPosition = playerPos - directionToHook * ropeLength;
            Vector2 pullForce = (targetPosition - objectPosition) * followSpeed;

            grabbedRb.AddForce(pullForce, ForceMode2D.Force);
        }

        // Добавляем демпфирование для уменьшения раскачивания
        Vector2 velocity = grabbedRb.velocity;
        Vector2 dampingForce = -velocity * 0.8f; // Коэффициент демпфирования (можно настроить)
        grabbedRb.AddForce(dampingForce, ForceMode2D.Force);

        
    }
}