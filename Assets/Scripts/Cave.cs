using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cave : MonoBehaviour
{
	public GameObject CaveRoof;
	public GameObject CaveHidden;
	public GameObject player;
	private Collider2D collider;
	
	// Start is called before the first frame update
	void Start()
	{
		collider = GetComponent<Collider2D>();
	}

	// Update is called once per frame
	void Update()
	{
		if (collider.bounds.Contains(player.transform.position)) 
		{
			if (!CaveHidden.activeSelf || CaveRoof.activeSelf) 
			{
				CaveHidden.SetActive(true);
				CaveRoof.SetActive(false);
			}
		}
		else
		{
			if (CaveHidden.activeSelf || !CaveRoof.activeSelf) 
			{
				CaveHidden.SetActive(false);
				CaveRoof.SetActive(true);
			}
		}
	}
}
