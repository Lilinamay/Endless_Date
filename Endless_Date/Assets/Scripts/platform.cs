using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//platform property
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
        if (becomeAva)                          //if become avaliable, this game object become the last platform
        {
            platform LastPlatform = this;
            becomeAva = false;
        }
    }

    private void OnTriggerEnter(Collider other)   //if hit the end, disable displatform and call function to create new platform
    {
        if (other.CompareTag("end"))
        {
            //Debug.Log("hit");
            GetComponentInParent<planeGenerate>().newplatform();
            gameObject.SetActive(false);
        }
    }


}
