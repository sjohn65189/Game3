using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class BridgeSystem : MonoBehaviour
{
    public Vector2 targetPosition; //position the plank will land on the bridge

    private bool hasCollided = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasCollided)
        {
            MoveToTargetPosition();
            hasCollided=true;
        }
    }

    private void MoveToTargetPosition()
    {
        transform.position = targetPosition;
    }
}
