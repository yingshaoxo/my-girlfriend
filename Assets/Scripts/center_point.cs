using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class center_point : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector3(0,0,0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        // center_mouse_point.transform.rotation = Camera.main.transform.rotation;
        // center_mouse_point.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, Camera.main.nearClipPlane));
    }
}
