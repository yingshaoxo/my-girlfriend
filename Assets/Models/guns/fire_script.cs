using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire_script : MonoBehaviour
{
    public GameObject bullet_object;
    private bool in_fire = false;

    private IEnumerator fire_function;

    private ParticleSystem muzzle_flash_effect_particle;

    // Start is called before the first frame update
    void Start()
    {
        fire_function = fire();

        muzzle_flash_effect_particle = GameObject.Find("muzzle_flash").GetComponentInChildren<ParticleSystem>();
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

        // rotate_gun();
    }

    void rotate_gun() {
        Quaternion targetRotation =  Quaternion.Euler(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);
        gameObject.transform.rotation = targetRotation;
    }

    IEnumerator fire()
    {
        while (true)
        {
            if (in_fire == true) {
                rotate_gun();
                Instantiate(bullet_object, transform.position, bullet_object.transform.rotation);
                muzzle_flash_effect_particle.Play();
                float random_delay = Random.Range(0.1f, 0.3f);
                yield return new WaitForSeconds(random_delay);
            } else {
                break;
            }
        }
    }
}
