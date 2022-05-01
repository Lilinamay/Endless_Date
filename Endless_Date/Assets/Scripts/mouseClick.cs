using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseClick : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    [SerializeField] LayerMask mask;
    [SerializeField] float distance;            //distance limit on how far you can detect
    public TextAsset myinkFlowerFile;
    [SerializeField] GameObject menu;
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, distance, mask))              //if click on clickable layer
        {
            if (Input.GetMouseButtonDown(0))
            {
                print(hit.collider.name);
                myinkFlowerFile = hit.collider.GetComponent<flowerProperty>().inkFlowerFile;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menu.active)
            {
                menu.SetActive(true);
            }
            else
            {
                menu.SetActive(false);
            }
        }
    }
}
