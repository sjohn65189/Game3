using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AreaPlankCounter : MonoBehaviour
{
	public GameObject PlankCounterUI;
	public GameObject outPoint;
	public GameObject inPoint;
	private Collider2D collider;
	private bool shouldMoveIn;
	private float moveSpeed = 500f;
	private GameObject player;
	
	private void Start() 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		collider = GetComponent<Collider2D>();
		
		if (collider.bounds.Contains(player.transform.position))
		{
			shouldMoveIn = true;
		}
		else 
		{
			shouldMoveIn = false;
		}
	}
	
	private void Update() 
	{
		if (collider.bounds.Contains(player.transform.position)) 
		{
			shouldMoveIn = true;
		}
		else 
		{
			shouldMoveIn = false;
		}
		
		if (shouldMoveIn && PlankCounterUI.transform.position != inPoint.transform.position) 
		{
			PlankCounterUI.transform.position = Vector3.MoveTowards(PlankCounterUI.transform.position, inPoint.transform.position, moveSpeed * Time.deltaTime);
		}
		else if (!shouldMoveIn && PlankCounterUI.transform.position != outPoint.transform.position) 
		{
			PlankCounterUI.transform.position = Vector3.MoveTowards(PlankCounterUI.transform.position, outPoint.transform.position, moveSpeed * Time.deltaTime);
		}
	}
}
