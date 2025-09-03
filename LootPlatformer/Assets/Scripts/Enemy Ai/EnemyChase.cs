using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;


public class EnemyChase : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float jumpForce = 5f;
    public float maxJumpHeight = 2f;       // max vertical distance it can clear
    public Transform player;
    public LayerMask groundMask;

    [Header("Ground & Front Checks")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public Transform frontCheck;

    [Header("Lookahead")]
    public float lookDistance = 2f;        // how far ahead to detect obstacles
    public float jumpCooldown = 0.2f;      // delay between jumps
    public float slopeCheckDistance = 0.2f;

    private Rigidbody2D body;
    private bool facingRight = true;
    private bool active = true;
    private float nextJumpTime = 0f;

    private void Awake()
    {
        GameManager gameManager = GameManager.Instance;
        
    }
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        
    }

    void FixedUpdate()
    {
        if (!active || player == null) return;

        float dir = player.position.x - transform.position.x;
        float moveX = Mathf.Sign(dir) * moveSpeed;

        bool onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);

        // Check for slopes or edges to avoid sticking
        RaycastHit2D slopeCheck = Physics2D.Raycast(frontCheck.position, Vector2.down, slopeCheckDistance, groundMask);
        if (slopeCheck.collider == null && onGround)
        {
            moveX *= 0.5f; // slow down to avoid falling
        }

        // Decide if we should jump
        if (onGround && ShouldJump(dir) && Time.time >= nextJumpTime)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            nextJumpTime = Time.time + jumpCooldown;
        }

        // Adjust horizontal speed (slightly slower in air for control)
        float speedFactor = onGround ? 1f : 0.85f;
        body.velocity = new Vector2(moveX * speedFactor, body.velocity.y);

        // Flip sprite
        if (dir > 0 && !facingRight) Flip();
        else if (dir < 0 && facingRight) Flip();
    }

    bool ShouldJump(float dir)
    {
        Vector2 origin = frontCheck.position;
        float[] heights = { 0f, 0.5f, 1f }; // ray heights above frontCheck

        // Check for obstacles
        foreach (float h in heights)
        {
            Vector2 rayStart = origin + Vector2.up * h;
            RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.right * Mathf.Sign(dir), lookDistance, groundMask);
            if (hit.collider != null)
            {
                float heightDiff = hit.collider.bounds.max.y - transform.position.y;
                if (heightDiff > 0.1f && heightDiff <= maxJumpHeight)
                    return true; // obstacle is jumpable
            }
        }

        // Check for gaps ahead
        Vector2 gapOrigin = origin + Vector2.right * Mathf.Sign(dir) * 0.5f;
        RaycastHit2D groundAhead = Physics2D.Raycast(gapOrigin, Vector2.down, 2f, groundMask);
        if (groundAhead.collider == null && Mathf.Abs(dir) > 0.5f)
            return true; // gap is far enough to warrant jump

        return false;
    }

    public bool IsTooFarFrom(Transform target, float maxDistance)
    {
        if (target == null) return false;
        return Vector2.Distance(transform.position, target.position) > maxDistance;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void StopChase()
    {
        active = false;
        body.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!active) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player caught! Game Over!");
            StopChase();
            GameManager.Instance.EndScene();
        }
    }
}


