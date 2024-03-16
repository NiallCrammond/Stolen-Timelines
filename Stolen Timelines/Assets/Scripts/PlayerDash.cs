using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerMovement pm;

    public bool canDash;
    public bool isDashing;
    [Range(0,100)]
    public float dashingPower = 40.0f;
    [Range(0,1)]
    public float dashingTime = 1.0f;
    [Range(0,10)]
    public float dashCooldown = 10.0f;
    [HideInInspector]
    public float dashCooldownTimer = 0.0f;

    [Range(0, 1)]
    public float timeManipulation = 1.0f;

    [SerializeField]
    TrailRenderer tr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pm = GetComponent<PlayerMovement>();
        canDash = true;
        dashCooldownTimer = dashCooldown;
    }

    public void performDash()
    {
        StartCoroutine(Dash());
    }

    //dash coroutine
    public IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        Time.timeScale = timeManipulation;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0.0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0.0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        Time.timeScale = 1.0f;
        isDashing = false;
        dashCooldownTimer = 0f;
        while (dashCooldown > dashCooldownTimer)
        {
            dashCooldownTimer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        canDash = true;

    }
}