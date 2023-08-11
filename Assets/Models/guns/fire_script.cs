using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire_script : MonoBehaviour
{
    public GameObject bullet_object;
    private bool in_fire = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            in_fire = true;
        }

        if (Input.GetMouseButtonUp(0)) {
            in_fire = false;
        }

        StartCoroutine(fire());
    }

    IEnumerator fire()
    {
        while (true)
        {
            if (in_fire == true) {
                Instantiate(bullet_object, transform.position, bullet_object.transform.rotation);
                float random_delay = Random.Range(0.8f, 2.0f);
                yield return new WaitForSeconds(random_delay);
            } else {
                break;
            }
        }
    }
}
