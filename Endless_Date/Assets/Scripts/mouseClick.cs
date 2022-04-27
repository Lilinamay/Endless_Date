using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseClick : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    [SerializeField] LayerMask mask;
    [SerializeField] float distance;            //distance limit on how far you can detect
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, distance, mask))
        {
            if (Input.GetMouseButtonDown(0))
                print(hit.collider.name);
        }
    }
}
