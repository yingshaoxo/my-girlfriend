using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire_script : MonoBehaviour
{
    public GameObject bullet_object;
    private bool in_fire = false;

    private IEnumerator fire_function;

    // Start is called before the first frame update
    void Start()
    {
        fire_function = fire();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            in_fire = true;

            StartCoroutine(fire_function);
        }

        if (Input.GetMouseButtonUp(0)) {
            in_fire = false;

            StopCoroutine(fire_function);
        }
    }

    IEnumerator fire()
    {
        while (true)
        {
            if (in_fire == true) {
                Instantiate(bullet_object, transform.position, bullet_object.transform.rotation);
                float random_delay = Random.Range(0.1f, 0.3f);
                yield return new WaitForSeconds(random_delay);
            } else {
                break;
            }
        }
    }
}
