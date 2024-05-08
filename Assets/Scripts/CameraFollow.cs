using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform player; // Player transform to follow
	public float smoothSpeed = 0.125f; // Adjust the speed of camera follow
	public Vector3 cameraPos;
	
	private Vector3 offset;
	private Vector3 velocity = Vector3.zero;

	void Start()
	{
		offset = transform.position - player.position;
	}

	void LateUpdate()
	{
		Vector3 targetPos = player.position + offset;
		Vector3 smoothedPos = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothSpeed);
		transform.position = smoothedPos;
		cameraPos = transform.position;
	}
}