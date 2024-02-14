using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollw : MonoBehaviour
{
    [SerializeField]
    public float followSpeed = 6f;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float yOffset = 1f;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, - 10f);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed*Time.deltaTime);
    }
}
