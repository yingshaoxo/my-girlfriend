using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_script : MonoBehaviour
{
    public float speed = 40f;

    Rigidbody rigidbody;

    GameObject fire_target;

    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        fire_target = GameObject.Find("look_target");
        rigidbody = gameObject.GetComponent<Rigidbody>();

        direction = Vector3.Normalize(fire_target.transform.position-transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other) {
        Destroy(gameObject);
    }
}
