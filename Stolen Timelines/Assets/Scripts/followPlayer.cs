using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{

   public GameObject player;

    private void Update()
    {
        gameObject.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y+7, player.transform.position.z);

    }
}
