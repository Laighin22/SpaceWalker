using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItemRotator : MonoBehaviour
{
    private float rotateSpeed = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if (isActiveAndEnabled)
        {
            transform.Rotate(new Vector3(0, -90, 0) * Time.deltaTime * rotateSpeed);
        }
    }
}
