using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class bullet_script : MonoBehaviour
{
    public float speed = 40f;

    float start_time;

    Rigidbody rigidbody;

    GameObject fire_target;

    Vector3 direction;


    public GameObject explosion_object;

    // Start is called before the first frame update
    void Start()
    {
        start_time = Time.time;

        fire_target = GameObject.Find("look_target");
        rigidbody = gameObject.GetComponent<Rigidbody>();

        direction = Vector3.Normalize(fire_target.transform.position-transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed);

        if ((Time.time - start_time) >= 3) {
            // destroy the object after 5 seconds
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        Destroy(gameObject);

        GameObject a_explosion = Instantiate(explosion_object, transform.position, transform.rotation);
        Destroy(a_explosion, 2f);
    }
}
