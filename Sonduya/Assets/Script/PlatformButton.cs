using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformButton : MonoBehaviour
{
    public GameObject ObjectToDisable;
    
    private int layerIndex;


    // Start is called before the first frame update
    void Start()
    {
        layerIndex = LayerMask.NameToLayer("Objects");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == layerIndex) //collision.CompareTag("movableBox") Если будет надобность в том, какие объекты могут взаимодействовать с кнопками
        {
            if(ObjectToDisable != null)
            {
                ObjectToDisable.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == layerIndex) //collision.CompareTag("movableBox") Если будет надобность в том, какие объекты могут взаимодействовать с кнопками
        {
            if (ObjectToDisable != null)
            {
                ObjectToDisable.SetActive(true);
            }
        }
    }
}
