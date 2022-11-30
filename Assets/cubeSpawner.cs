using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeSpawner : MonoBehaviour
{
    ObjectPooler objectPooler;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }
    private void FixedUpdate()
    {
        objectPooler.SpawnFromPool("Cube", transform.position, Quaternion.identity);

        objectPooler.SpawnFromPool("Sphere", transform.position, Quaternion.identity);

        objectPooler.SpawnFromPool("Cylinder", transform.position, Quaternion.identity);


    }
}
