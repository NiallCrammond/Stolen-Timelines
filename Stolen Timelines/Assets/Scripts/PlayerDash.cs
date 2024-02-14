using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerMovement pm;

    public bool canDash;
    public bool isDashing;
    public float dashingPower = 40.0f;
    private float dashingTime = 0.2f;
    public float dashCooldown = 5.0f;

    [SerializeField]
    TrailRenderer tr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pm = GetComponent<PlayerMovement>();
        canDash = true;
    }

    public void performDash(Vector2 inputVec, float dashInput)
    {
        StartCoroutine(Dash());
    }

    //dash coroutine
    public IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0.0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0.0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

    }
}