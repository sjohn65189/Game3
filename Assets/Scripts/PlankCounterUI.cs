using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AreaPlankCounter : MonoBehaviour
{
	public GameObject PlankCounterUI;
	public GameObject yetiClaw;
	public GameObject outPoint;
	public GameObject inPoint;
	private Collider2D collider;
	private bool shouldMoveIn;
	private float moveSpeed = 500f;
	private GameObject player;
	private bool thisMoving = false;
	
	private enum State
	{
		Idle,
		BothMovingIn,
		BothMovingOut,
		ClawMovingIn,
		ClawMovingOut
	}
	
	private State currentState = State.Idle;
	
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
		
		switch (currentState)
		{
			case State.Idle:
				// Move in Yeti Claw & Counter
				if (shouldMoveIn && PlankCounterUI.transform.position != inPoint.transform.position && yetiClaw.transform.position != inPoint.transform.position) 
				{
					currentState = State.BothMovingIn;
				}
				// Move out Yeti Claw
				if (shouldMoveIn && PlankCounterUI.transform.position == inPoint.transform.position && yetiClaw.transform.position != outPoint.transform.position) 
				{
					currentState = State.ClawMovingOut;
				}
				// Move in Yeti Claw
				if (!shouldMoveIn && PlankCounterUI.transform.position != outPoint.transform.position && yetiClaw.transform.position != inPoint.transform.position) 
				{
					currentState = State.ClawMovingIn;
				}
				// Move out Yeti Claw & Counter
				if (!shouldMoveIn && PlankCounterUI.transform.position != outPoint.transform.position && yetiClaw.transform.position == inPoint.transform.position) 
				{
					currentState = State.BothMovingOut;
				}
				break;
				
			case State.BothMovingIn:
				if (PlankCounterUI.transform.position == inPoint.transform.position && yetiClaw.transform.position == inPoint.transform.position) 
				{
					currentState = State.Idle;
				}
				else 
				{
					yetiClaw.transform.position = Vector3.MoveTowards(yetiClaw.transform.position, inPoint.transform.position, moveSpeed * Time.deltaTime);
					counterIn();
				}
				break;
				
			case State.BothMovingOut:
				if (PlankCounterUI.transform.position == outPoint.transform.position && yetiClaw.transform.position == outPoint.transform.position) 
				{
					currentState = State.Idle;
				}
				else 
				{
					yetiClaw.transform.position = Vector3.MoveTowards(yetiClaw.transform.position, outPoint.transform.position, moveSpeed * Time.deltaTime);
					counterOut();
				}
				break;
				
			case State.ClawMovingIn:
				if (yetiClaw.transform.position == inPoint.transform.position) 
				{
					currentState = State.Idle;
				}
				else 
				{
					yetiClaw.transform.position = Vector3.MoveTowards(yetiClaw.transform.position, inPoint.transform.position, moveSpeed * Time.deltaTime);
				}
				break;
				
			case State.ClawMovingOut:
				if (yetiClaw.transform.position == outPoint.transform.position) 
				{
					currentState = State.Idle;
				}
				else 
				{
					yetiClaw.transform.position = Vector3.MoveTowards(yetiClaw.transform.position, outPoint.transform.position, moveSpeed * Time.deltaTime);
				}
				break;
			
		}
	}
	
	private void counterIn() 
	{
		PlankCounterUI.transform.position = Vector3.MoveTowards(PlankCounterUI.transform.position, inPoint.transform.position, moveSpeed * Time.deltaTime);
	}
	
	private void counterOut() 
	{
		PlankCounterUI.transform.position = Vector3.MoveTowards(PlankCounterUI.transform.position, outPoint.transform.position, moveSpeed * Time.deltaTime);
	}
}
