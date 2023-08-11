using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser_script : MonoBehaviour
{
    public Camera playerCamera;
    public Transform laserOrigin;
    public float gunRange = 50f;
    public float fireRate = 0.2f;
    public float laserDuration = 0.05f;
 
    LineRenderer laserLine;
    float fireTimer;

    public RaycastHit hit_point;
 
    void Start()
    {
    }

    void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
        laserLine.SetWidth(0.001f, 0.001f);
    }
 
    void Update()
    {
        fireTimer += Time.deltaTime;
        if((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J)) && (fireTimer > fireRate))
        {
            fireTimer = 0;
            laserLine.SetPosition(0, laserOrigin.position);
            Vector3 camera_center_point = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            Vector3 ray_start_point = camera_center_point + (playerCamera.transform.forward * 5);
            if(Physics.Raycast(ray_start_point, playerCamera.transform.forward, out hit_point, gunRange))
            {
                laserLine.SetPosition(1, hit_point.point);
                // Destroy(hit.transform.gameObject);
            }
            else
            {
                hit_point.point = ray_start_point + (playerCamera.transform.forward * gunRange);
                laserLine.SetPosition(1, hit_point.point);
            }
            StartCoroutine(ShootLaser());
        }
    }
 
    IEnumerator ShootLaser()
    {
        laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;
    }
}
