using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class BridgeSystem : MonoBehaviour
{
    public Vector3Int targetTilePosition; //position the plank will land on the bridge
    public Tilemap groundTilemap;
    public Tilemap groundOverlayTilemap;
    public float rotationAngle; // The angle by which you want to rotate the item

    private bool hasCollided = false;
    private NavMeshSurface navMeshSurface; //reference to the NavMeshSurface component
    private Rigidbody2D plankRigidbody; // Reference to the Rigidbody2D component

    private void Start()
    {
        // Get the Rigidbody2D component attached to this GameObject
        plankRigidbody = GetComponent<Rigidbody2D>();

        //find navMeshSurface in scene
        navMeshSurface = FindObjectOfType<NavMeshSurface>();
    }
    private void Update()
    {
        // Check if the "G" key is pressed and the collision flag is set
        if (Input.GetKeyDown(KeyCode.G) && hasCollided)
        {
            
            MoveToTargetTile();

            // Rebuild the NavMesh after moving the item
            if (navMeshSurface != null)
            {
                navMeshSurface.BuildNavMesh();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasCollided)
        {
            hasCollided=true;
            
        }
    }

    private void MoveToTargetTile()
    {
        // Move the item to the target tile position
        Vector3 targetWorldPosition = groundTilemap.GetCellCenterWorld(targetTilePosition);
        transform.position = targetWorldPosition;

        // Rotate the item along z axis
        transform.eulerAngles = new Vector3(0f, 0f, rotationAngle);

    }
}
