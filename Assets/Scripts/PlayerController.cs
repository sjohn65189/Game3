using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public PlayerInputActions input;
    public float moveSpeed = 5f;
    public Transform movePoint;

    public LayerMask whatStopsMovement;
    
    void Start()
    {
        movePoint.parent = null;
    }

    void Awake()
    {
        input = new PlayerInputActions();
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    void Update()
    {
        // Move player towards movepoint
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            // Check movement in order (up, down, left, right)
            if(input.PlayerActions.MoveUp.IsPressed())
            {
                // If no object in front of us, move
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, 0f, 1f), .2f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(0f, 0f, 1f);
                }
            }
            else if(input.PlayerActions.MoveDown.IsPressed())
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, 0f, -1f), .2f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(0f, 0f, -1f);
                }
            }
            else if(input.PlayerActions.MoveLeft.IsPressed())
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(-1f, 0f, 0f), .2f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(-1f, 0f, 0f);
                }
            }
            else if(input.PlayerActions.MoveRight.IsPressed())
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(1f, 0f, 0f), .2f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(1f, 0f, 0f);
                }
            }
        }
    }
}
