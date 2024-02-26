using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : MonoBehaviour
{

    private Rigidbody2D rb;

    public bool isSliding = false;
    public BoxCollider2D regularColl;
    public BoxCollider2D slideColl;

    // temp vars for sprite render (should be decided by animController)
    public SpriteRenderer regularSprite;
    public SpriteRenderer slideSprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void prefromSlide(Vector2 inputVec, float slideForce)
    {


        isSliding = true;     
       
        regularColl.enabled = false;
        slideColl.enabled = true;

        regularSprite.enabled = false;
        slideSprite.enabled = true;

        rb.AddForce(inputVec * slideForce);

        //StartCoroutine("stopSlide");
    }

    public void stopSlide()
    {
        isSliding = false;
        regularColl.enabled = true;

        regularSprite.enabled = true;
        slideSprite.enabled = false;

        slideColl.enabled = false;

    }
}
