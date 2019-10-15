using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generate_random_cubes_into_map : MonoBehaviour
{
    public GameObject cube;

    private int min = 0;
    private int max = 1000;

    // Start is called before the first frame update
    void Start()
    {
        put_cubes_in();
    }

    Vector3 GeneratedPosition()
    {
        float scale = 1f;

        float moveAreaX = gameObject.GetComponent<MeshFilter>().mesh.bounds.size.x / 2;
        float moveAreaY = gameObject.GetComponent<MeshFilter>().mesh.bounds.size.y / 2;
        float moveAreaZ = gameObject.GetComponent<MeshFilter>().mesh.bounds.size.z / 2;
        Vector3 center = gameObject.GetComponent<MeshFilter>().mesh.bounds.center;

        float x = center.x + Random.Range(-moveAreaX*scale, moveAreaX*scale);
        float y = center.y + Random.Range(-moveAreaY*scale, moveAreaY*scale);
        float z = center.z + Random.Range(-moveAreaZ*scale, moveAreaZ*scale);

        return new Vector3(x,y,z);
    }

    void put_cubes_in()
    {
        for(int i = 0; i < max; i++)
        {
            GameObject new_obj = Instantiate(cube, GeneratedPosition(), Quaternion.identity);
            float length = Random.Range(1, 5);
            float height = Random.Range(1, 10);
            new_obj.transform.localScale =  new Vector3(length, length, height); // change its local scale in x y z form
        }
    }
}
