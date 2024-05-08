using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiClose : MonoBehaviour
{
    public Material material;
    public float maxFloatValue;
	public float minFloatValue;
    // Start is called before the first frame update
    void Start()
    {
        material.SetFloat("_VignettePower", maxFloatValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
