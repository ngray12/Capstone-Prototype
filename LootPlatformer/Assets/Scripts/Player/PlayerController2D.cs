using System;
using System.Collections;
using System.Collections.Generic;
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
        public bool facingRight = true;

        [Header("Jump Physics")]
        public float fallMultiplier = 2.5f;
        public float lowJumpMultiplier = 4f;

        [Header("Jump Assistance")]
        public float coyoteTime = 0.12f;
        public float jumpBufferTime = 0.12f;

        [Header("References")]
        public Rigidbody2D body;
        public BoxCollider2D groundCheck;
        public LayerMask groundMask;

        [Header("Encumbrance")]
        public LootSack lootSack;

        private bool grounded;
        private float coyoteCounter;
        private float jumpBufferCounter;
        private float xInput;
        private float encumbranceFactor = 0f;

        void Update()
        {
            xInput = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump"))
                jumpBufferCounter = jumpBufferTime;

            jumpBufferCounter -= Time.deltaTime;

            // Drop treasure
            if (Input.GetKeyDown(KeyCode.Q) && lootSack != null &&lootSack.HasTreasure())
            {
                Debug.Log("dropped treasure");
                lootSack.DropLastTreasure();
            }

            // Optional: update sack sway
            if (lootSack != null)
                lootSack.ApplySway();
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
            coyoteCounter = grounded ? coyoteTime : coyoteCounter - Time.fixedDeltaTime;
        }

        void MoveWithInput()
        {
            float minSpeedMult = 0.12f;
            float speedMult = Mathf.Lerp(1f, minSpeedMult, encumbranceFactor * encumbranceFactor);
            float jumpMult = speedMult;

            float currentMoveSpeed = moveSpeed * speedMult;
            float currentJumpSpeed = jumpSpeed * jumpMult;

            if (grounded)
                body.velocity = new Vector2(xInput * currentMoveSpeed, body.velocity.y);
            else
                body.velocity = new Vector2(
                    Mathf.Lerp(body.velocity.x, xInput * currentMoveSpeed, airControl),
                    body.velocity.y
                );

            // Jump
            if (jumpBufferCounter > 0 && coyoteCounter > 0)
            {
                body.velocity = new Vector2(body.velocity.x, currentJumpSpeed);
                jumpBufferCounter = 0;
            }

            // Flip sprite
            if (xInput > 0 && !facingRight) Flip();
            else if (xInput < 0 && facingRight) Flip();
        }

        void ApplyBetterJumpPhysics()
        {
            if (body.velocity.y < 0)
                body.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
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

        // Called by LootSack
        public void SetEncumbrance(float weight)
        {
            if (lootSack == null) return;
            encumbranceFactor = Mathf.Clamp01(weight / lootSack.maxEncumbrance);
        }
    }
}