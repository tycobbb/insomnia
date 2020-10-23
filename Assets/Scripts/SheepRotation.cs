using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepRotation : MonoBehaviour
{
    // Start is called before the first frame update
    protected void Update()
    {
        var camera = Camera.main;
        if (Vector3.Distance(transform.position, camera.transform.position) < 10.0f)
        {
            transform.forward = camera.transform.forward * -1;
        }
    }
}
