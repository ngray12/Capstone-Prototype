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

        [Header("Jump Assistance")]
        public float coyoteTime = 0.12f;      // Time after leaving ground to still jump
        public float jumpBufferTime = 0.12f;  // Time before landing to store a jump

        [Header("References")]
        public Rigidbody2D body;
        public BoxCollider2D groundCheck;
        public LayerMask groundMask;

        bool grounded;
        float coyoteCounter;
        float jumpBufferCounter;
        float xInput;

        void Update()
        {
            // Horizontal input — raw for snappy control
            xInput = Input.GetAxisRaw("Horizontal");

            // Jump input — buffer the press
            if (Input.GetButtonDown("Jump"))
                jumpBufferCounter = jumpBufferTime;

            // Reduce jump buffer over time
            jumpBufferCounter -= Time.deltaTime;
        }

        void FixedUpdate()
        {
            CheckGround();
            HandleCoyoteTime();
            MoveWithInput();
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
            if (grounded)
            {
                // Full control on ground
                body.velocity = new Vector2(xInput * moveSpeed, body.velocity.y);
            }
            else
            {
                // Air control — gradual steering
                float targetX = xInput * moveSpeed;
                body.velocity = new Vector2(
                    Mathf.Lerp(body.velocity.x, targetX, airControl),
                    body.velocity.y
                );
            }

            // Jumping — only if coyote time or grounded, and jump was buffered
            if (jumpBufferCounter > 0 && coyoteCounter > 0)
            {
                body.velocity = new Vector2(0f, jumpSpeed); // Zero X on launch for snappy jumps
                jumpBufferCounter = 0; // Reset buffer
            }
        }

        void ApplyFriction()
        {
            if (grounded && xInput == 0)
            {
                body.velocity = new Vector2(body.velocity.x * groundDecay, body.velocity.y);
            }
        }
    }
}