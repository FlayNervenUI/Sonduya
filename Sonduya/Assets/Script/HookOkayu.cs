using System.Collections;
using UnityEngine;

public class HookOkayu : MonoBehaviour
{
    public Transform playerPosition;
    public Transform hookPoint;
    public float ropeLength = 5f;
    public float maxPullDistance = 8f;
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
        RaycastHit2D hit = Physics2D.Raycast(hookPoint.position, hookPoint.right, maxPullDistance);
        if (hit.collider != null && hit.collider.gameObject.layer == layerIndex)
        {
            Rigidbody2D hitRb = hit.collider.GetComponent<Rigidbody2D>();
            if (hitRb != null)
            {
                grabbedObject = hit.collider.gameObject;
                isGrabbing = true;
            }
        }
        else
        {
            return;
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
        if (distance > maxPullDistance)
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
        Vector2 dampingForce = -velocity * 0.9f; // Коэффициент демпфирования (можно настроить)
        grabbedRb.AddForce(dampingForce, ForceMode2D.Force);

        // Если объект висит в воздухе, подтягиваем его вертикально, чтобы он не "зависал"
        if (distance < ropeLength)
        {
            Vector2 upwardPull = new Vector2(0, followSpeed);
            grabbedRb.AddForce(upwardPull, ForceMode2D.Force);
        }
    }
}

//3
/*void HandleRope()
{
    Vector2 playerPos = playerPosition.position;
    Vector2 objectPosition = grabbedObject.transform.position;

    Vector2 directionToHook = (playerPos - objectPosition).normalized;
    float distance = Vector2.Distance(playerPos, objectPosition);

    if (distance > maxPullDistance)
    {
        ReleaseObject();
        Debug.Log("Объект отцепился: превышено максимальное расстояние");
        return;
    }

    Rigidbody2D grabbedRb = grabbedObject.GetComponent<Rigidbody2D>();

    if (distance > ropeLength)
    {
        Vector2 targetPosition = playerPos - directionToHook * ropeLength;
        Vector2 pullForce = (targetPosition - objectPosition) * followSpeed;
        grabbedRb.AddForce(pullForce, ForceMode2D.Force);
    }
}*/

//2
/*void HandleRope()
{
    // Преобразуем позицию hookPoint в Vector2
    Vector2 hookPosition = hookPoint.position;

    // Рассчитываем направление к объекту и расстояние
    Vector2 directionToHook = (hookPosition - (Vector2)grabbedObject.transform.position).normalized;
    float distance = Vector2.Distance(hookPosition, grabbedObject.transform.position);

    Rigidbody2D grabbedRb = grabbedObject.GetComponent<Rigidbody2D>();

    // Если объект дальше длины верёвки, подтягиваем его
    if (distance > ropeLength)
    {
        // Рассчитываем целевую позицию объекта
        Vector2 targetPosition = hookPosition - directionToHook * ropeLength;

        // Применяем силу, чтобы подтянуть объект, вместо жесткого задания скорости
        Vector2 pullForce = (targetPosition - (Vector2)grabbedObject.transform.position) * followSpeed;
        grabbedRb.AddForce(pullForce, ForceMode2D.Force);
    }
    else
    {
        // Если объект находится в пределах длины верёвки, он падает под действием гравитации
        grabbedRb.velocity = grabbedRb.velocity; // Сохраняем естественное поведение
    }
}*/

//1
/*void HandleRope()
{
    Vector2 hookPosition = playerPosition.position;

    Vector2 directionToHook = (hookPosition - (Vector2)grabbedObject.transform.position).normalized;
    float distance = Vector2.Distance(hookPosition, grabbedObject.transform.position);
    if(distance > maxPullDistance)
    {
        ReleaseObject();
    }
    else if (distance > ropeLength)
    {
        Rigidbody2D grabbedRb = grabbedObject.GetComponent<Rigidbody2D>();

        Vector2 targetPosition = hookPosition - directionToHook * ropeLength;
        grabbedRb.velocity = (targetPosition - (Vector2)grabbedObject.transform.position) * followSpeed;
    }
    else if(distance > ropeLength * 2)
    {
        ReleaseObject();
    }
}*/