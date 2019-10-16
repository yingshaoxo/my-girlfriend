using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube_collision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter (Collision col)
    {
        if(col.gameObject.name.Contains("Cube"))
        {
            Destroy(col.gameObject, 3);
            Destroy(gameObject);
        }
    }
}
