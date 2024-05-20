using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Unity.VisualScripting;
using NavMeshPlus.Components;

public class MessageSign : MonoBehaviour
{
    public Tilemap groundTilemap;
    public Tilemap groundOverlayTilemap;
    public Tilemap colliderTilemap;
    public GameObject messageUI; // Reference to the UI element that will display the message

    private TextMeshProUGUI messageText; // Reference to the TextMeshProUGUI component in the UI
    private NavMeshSurface navMeshSurface; //reference to the NavMeshSurface component
    private Rigidbody2D colliderRigidbody; // Reference to the Rigidbody2D component

    private void Start()
    {
        // Ensure the message UI is initially inactive
        messageUI.SetActive(false);
        // Get the TextMeshProUGUI component from the message UI
        messageText = messageUI.GetComponentInChildren<TextMeshProUGUI>();

        colliderRigidbody = GetComponent<Rigidbody2D>();
        navMeshSurface = FindObjectOfType<NavMeshSurface>();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Set the message text and activate the message UI
            messageUI.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Deactivate the message UI
            messageUI.SetActive(false);
        }
    }
}
