using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {       
    }

    // Update is called once per frame
    public float burnDuration = 5.0f; // ����� �������
    private bool isBurning = false;

    void Update()
    {
        if (isBurning)
        {
            Ignite();
            StartCoroutine(Ignite());
            // ������ �������
            burnDuration -= Time.deltaTime;
            if (burnDuration <= 0)
            {
                Destroy(gameObject); // ���������� ������ ����� �������
            }
        }
    }

    public IEnumerator Ignite()
    {
        if (isBurning)
        {
            isBurning = true;
            // �������� ���������� �������, ����� � �.�.
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Color initialColor = spriteRenderer.color;
            Color newColor = Color.black;
            float duration = 5f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                spriteRenderer.color = Color.Lerp(initialColor, newColor, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            spriteRenderer.color = newColor;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fire"))
        {
            isBurning = true;
        }
    }    

}
