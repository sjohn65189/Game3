using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static List<string> collectedItems = new List<string>(); // Static list to keep track of collected items across all instances

    public string itemName; //Name of Item

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {  
            if (Input.GetKeyDown(KeyCode.G)) 
            {
                CollectItem();
            }
        }
    }

    void CollectItem() {
        //check if item has already been collected
        if (!collectedItems.Contains(itemName))
        {
            // Add the item to the list of collected items
            collectedItems.Add(itemName);

            // Disable the collected item's renderer and collider
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;

            // Update the UI to display the collected items
            UpdateCollectedItemsUI();
        }
    }

    static void UpdateCollectedItemsUI()
    {
        // Print the names of collected items to the console for demonstration
        Debug.Log("Collected Items:");
        foreach (var item in collectedItems)
        {
            Debug.Log("- " + item);
        }
    }
}
