using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using NavMeshPlus.Components;

public class ItemInventory : MonoBehaviour
{
    public Image[] itemSlots; // Array of Image components representing inventory slots
    public Sprite itemSprite; // The sprite of the item to be picked up
    public Tilemap groundTilemap;
    public Tilemap groundOverlayTilemap;
    public AudioClip pickupSound; // Add this field to hold the pickup sound
    public AudioSource audioSource; // Add this field to reference the AudioSource component

    private Rigidbody2D Rigidbody; // Reference to the Rigidbody2D component
    private NavMeshSurface navMeshSurface; //reference to the NavMeshSurface component
    private bool hasCollided = false;
    private void Start()
    {
        // Get the Rigidbody2D component attached to this GameObject
        Rigidbody = GetComponent<Rigidbody2D>();

        //find navMeshSurface in scene
        navMeshSurface = FindObjectOfType<NavMeshSurface>();
    }
    private void Update()
    {
        // Check if the "G" key is pressed and the collision flag is set
        if (Input.GetKeyDown(KeyCode.G) && hasCollided)
        {
            // Play the pickup sound
            if (pickupSound != null && audioSource != null)
            {
                audioSource.clip = pickupSound;
                audioSource.Play();
            }
            AddItem(itemSprite);
            hasCollided = false;


            // Rebuild the NavMesh after moving the item
            if (navMeshSurface != null)
            {
                navMeshSurface.BuildNavMesh();
            }
        }
    }
    // Add an item to the inventory
    public void AddItem(Sprite itemSprite)
    {
        // Find an empty slot in the inventory
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].sprite == null) // Check if the slot is empty
            {
                // Set the sprite of the empty slot to the item sprite
                itemSlots[i].sprite = itemSprite;
                return; // Exit the loop after adding the item
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasCollided)
        {
            hasCollided = true;
           
            // Optionally, destroy the item object after pickup
            Destroy(gameObject);
        }
    }
}
