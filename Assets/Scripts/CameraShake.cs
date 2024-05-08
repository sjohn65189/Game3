using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	public Vector3 currentPos;
	public CameraFollow cameraFollow;
	public float intensity; // Intensity of camera shake
	public float speed = 1.0f; // Speed of camera shake
	
	// Start is called before the first frame update
	void Start()
	{
		intensity = 0f;
		currentPos = cameraFollow.cameraPos;
	}

	// Update is called once per frame
	void Update()
	{
		currentPos = cameraFollow.cameraPos;
		
		if (intensity > 0) 
		{
			ShakeCamera();
		}
	}
	
	void ShakeCamera()
	{
		Vector3 shakeOffset = Random.insideUnitSphere * intensity;
		shakeOffset.z = 0; // Ensure the shake doesn't affect the camera's z position

		transform.position += shakeOffset * Time.deltaTime * speed;
	}
}
