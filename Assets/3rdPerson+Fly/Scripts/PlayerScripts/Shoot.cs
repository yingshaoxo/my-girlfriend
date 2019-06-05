using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject ball;
    public GameObject plants;
    public float speed = 15;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.J)) {
            shoot();
        } else if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.K)) {
            put_in_plants();
        }
    }

    void shoot() {
        Vector3 front_position = transform.position + transform.forward + new Vector3(0f,0.8f,0f);
        GameObject new_obj  = Instantiate(
            ball, 
            front_position, 
            Quaternion.identity
        );
        // change the color of a ball
        //new_obj.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_Color", new Color(Random.value,Random.value,Random.value));
        new_obj.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        // change the weight 
        new_obj.GetComponent<Rigidbody>().mass = 100;
        // push the ball forward
        new_obj.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0,0,speed));
    }

    void put_in_plants() {
        Vector3 front_position = transform.position + (transform.forward*20) + new Vector3(0f,0.01f,0f);
        GameObject new_obj  = Instantiate(
            plants, 
            front_position, 
            Quaternion.identity
        );
        //new_obj.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0,0,speed));
    }
}