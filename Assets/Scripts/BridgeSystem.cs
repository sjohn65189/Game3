using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class BridgeSystem : MonoBehaviour
{
    public Transform targetPosition; //position the plank will land on the bridge

    private bool hasCollided = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasCollided)
        {
            MoveToTargetPosition();
            hasCollided=true;
        }
    }

    private void MoveToTargetPosition()
    {
        transform.position = targetPosition.position;
    }
}
