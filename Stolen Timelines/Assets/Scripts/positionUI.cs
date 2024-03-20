using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positionUI : MonoBehaviour
{
  public Transform pos;


    private void Awake()
    {
        gameObject.transform.position = pos.position;
    }
}
