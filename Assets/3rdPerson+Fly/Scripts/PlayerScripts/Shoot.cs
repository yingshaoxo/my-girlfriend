using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject ball;
    public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            shoot();
        } else if (Input.GetKeyDown(KeyCode.J)) {
            shoot();
        }
    }

    void shoot() {
        Vector3 front_position = transform.position + transform.forward + new Vector3(0f,0.8f,0f);
        GameObject new_obj  = Instantiate(
            ball, 
            front_position, 
            Quaternion.identity
        );
        new_obj.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0,0,speed));
    }
}