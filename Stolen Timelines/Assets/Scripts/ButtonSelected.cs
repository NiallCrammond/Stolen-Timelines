using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelected : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool onPointerStay = false;
    bool enlarge = false;

    Vector3 originalScale;
    

    void Start()
    {
        originalScale = transform.localScale;
    }
    void Update()
    {
        if (onPointerStay)
        {


            if (enlarge)
            {
                transform.localScale = transform.localScale + new Vector3(0.01f, 0.01f, 0.01f);
                if (transform.localScale.x > 1)
                {
                    enlarge = false;
                }
            }
            else
            {
                transform.localScale = transform.localScale - new Vector3(0.01f, 0.01f, 0.01f);
                if (transform.localScale.x < 0.5)
                {
                    enlarge = true;
                }
            }
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        enlarge = true;
        onPointerStay = true;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerStay = false;
        transform.localScale = originalScale;
    }
}
