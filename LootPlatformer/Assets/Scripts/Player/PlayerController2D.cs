using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CMIYC
{
    public class PlayerController2D : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed = 5.0f;
        public float jumpSpeed = 7.5f;
        [Range(0f, 1f)] public float groundDecay = 0.8f;
        [Range(0f, 1f)] public float airControl = 0.4f;
        private bool facingRight = true;

        [Header("Jump Physics")]
        public float fallMultiplier = 2.5f;     // Faster fall
        public float lowJumpMultiplier = 4f;    // Short hop control

        [Header("Jump Assistance")]
        public float coyoteTime = 0.12f;
        public float jumpBufferTime = 0.12f;

        [Header("References")]
        public Rigidbody2D body;
        public BoxCollider2D groundCheck;
        public LayerMask groundMask;

        [Header("Encumberance")]
        [SerializeField] private float totalEncumberance = 0f;
        public float maxEncumberance = 10f;

        public Transform lootSack;

        bool grounded;
        float coyoteCounter;
        float jumpBufferCounter;
        float xInput;

        void Update()
        {
            xInput = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump"))
                jumpBufferCounter = jumpBufferTime;

            jumpBufferCounter -= Time.deltaTime;
        }

        void FixedUpdate()
        {
            CheckGround();
            HandleCoyoteTime();
            MoveWithInput();
            ApplyBetterJumpPhysics();
            ApplyFriction();
        }

        void CheckGround()
        {
            grounded = Physics2D.OverlapAreaAll(
                groundCheck.bounds.min,
                groundCheck.bounds.max,
                groundMask
            ).Length > 0;
        }

        void HandleCoyoteTime()
        {
            if (grounded)
                coyoteCounter = coyoteTime;
            else
                coyoteCounter -= Time.fixedDeltaTime;
        }

        void MoveWithInput()
        {
            float t = Mathf.Clamp01(totalEncumberance / maxEncumberance);
            float minSpeedMult = 0.12f; // ~12% of base speed at max encumbrance
            float speedMult = Mathf.Lerp(1f, minSpeedMult, t * t); // squared for sharper slowdown
            float jumpMult = speedMult;

            float currentMoveSpeed = moveSpeed * speedMult;
            float currentJumpSpeed = jumpSpeed * jumpMult;

            // Then use these when moving:
            if (grounded)
                body.velocity = new Vector2(xInput * currentMoveSpeed, body.velocity.y);
            else
                body.velocity = new Vector2(
                    Mathf.Lerp(body.velocity.x, xInput * currentMoveSpeed, airControl),
                    body.velocity.y
                );

            // For jump:
            if (jumpBufferCounter > 0 && coyoteCounter > 0)
            {
                body.velocity = new Vector2(body.velocity.x, currentJumpSpeed);
                jumpBufferCounter = 0;
            }

            if (xInput > 0 && !facingRight)
            {
                Flip();
            }
            else if (xInput < 0 && facingRight)
            {
                Flip();
            }
        }

        void ApplyBetterJumpPhysics()
        {
            // If falling — make it faster
            if (body.velocity.y < 0)
                body.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;

            // If rising but jump not held — short hop
            else if (body.velocity.y > 0 && !Input.GetButton("Jump"))
                body.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

        void ApplyFriction()
        {
            if (grounded && xInput == 0)
                body.velocity = new Vector2(body.velocity.x * groundDecay, body.velocity.y);
        }

        void Flip()
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        public void AddEncumberance(float weight)
        {
            float encumbranceFactor = Mathf.Clamp01(1f - (weight / 100f));
            moveSpeed = moveSpeed * (0.2f + (0.8f * encumbranceFactor));
        }

        private void UpdateLootSackVisual()
        {
            if (lootSack != null)
            {
                float scaleMult = 1f + (totalEncumberance / maxEncumberance) * 0.5f;
                lootSack.localScale = new Vector3(scaleMult, scaleMult, 1f);
            }
        }
        
    }
}