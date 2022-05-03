using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//platform movement
public class planeProperty : MonoBehaviour
{
    float speed = 5f;
    
    void Start()
    {
    }

    
    void Update()
    {
        transform.position += new Vector3(1, 0, 0)*Time.deltaTime* speed;
    }


}
