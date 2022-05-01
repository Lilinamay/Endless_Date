using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowerProperty : MonoBehaviour
{
    public TextAsset inkFlowerFile;
    float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("end"))
        {

            gameObject.SetActive(false);
            Debug.Log("deactivate flower");
            GetComponentInParent<FlowerManager>().generateFlowerTime();
        }
    }
}
