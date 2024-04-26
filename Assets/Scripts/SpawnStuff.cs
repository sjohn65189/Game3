using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStuff : MonoBehaviour
{
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        // go = game object
        var go = Instantiate(prefab);
        var go1 = Instantiate(prefab);
        var go2 = Instantiate(prefab);
        Destroy(go, 5.0f);
        Destroy(go1, 6.0f);
        Destroy(go2, 7.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
