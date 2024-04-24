using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;

    public Transform orientation;

    private Rigidbody rb;
    private AudioSource audioSource;

    private bool readyToJump = true;
    private bool isWalking = false;
    private const float footstepThreshold = 0.1f;


    [Header("Audio")]
    public AudioClip jump;
    public AudioClip walk;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        CheckMovement();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
    }

    private void CheckMovement()
    {
        // Check if the player is grounded and moving
        if (grounded && rb.velocity.magnitude > footstepThreshold)
        {
            if (!isWalking)
            {
                isWalking = true;
                PlayFootstepSound();
            }
        }
        else
        {
            if (isWalking)
            {
                isWalking = false;
                StopFootstepSound();
            }
        }

        // Check for jump input
        if (Input.GetKeyDown(jumpKey) && grounded && readyToJump)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Apply force based on ground or air movement
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
{
    Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

    // Limit velocity if needed
    if (flatVel.magnitude > moveSpeed)
    {
        Vector3 limitedVel = flatVel.normalized * moveSpeed;
        rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
    }
}

    private void PlayFootstepSound()
    {
        if (walk != null && audioSource != null)
        {
            audioSource.clip = walk;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void StopFootstepSound()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with a ground object (you may need to adjust the tag or layer)
        if (collision.gameObject.CompareTag("trampoline"))
        {
            // Play the landing sound
            if (jump != null && audioSource != null)
            {
                audioSource.PlayOneShot(jump);
            }
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        if (jump != null && audioSource != null)
        {
            audioSource.PlayOneShot(jump);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
