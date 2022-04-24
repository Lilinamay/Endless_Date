using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform : MonoBehaviour
{
    public static platform LastPlatform;
    public bool becomeAva = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (becomeAva)
        {
            platform LastPlatform = this;
            becomeAva = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //platform last = platform.LastPlatform;
        //int randint = Random.Range(0, 3);
        if (other.CompareTag("end"))
        {
            Debug.Log("hit");
            GetComponentInParent<planeGenerate>().newplatform();
            gameObject.SetActive(false);
        }
    }


}
