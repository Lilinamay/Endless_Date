using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//plane generate manager to spawn platforms when game starts, and function to generate new platform during game
public class planeGenerate : MonoBehaviour
{
    public List<GameObject> plane;
    int randP;
    float Timer = 0;
    [SerializeField] float maxTime = 3;
    [SerializeField] int planeNum;      //amount of platforms on screen
    int startMutipler;
    Vector3 myVector = new Vector3(20.0f, 0.0f, 0.0f);



    void Start()
    {
        
        startMutipler = planeNum - 2;  //mutipler to calculate space between platforms
        for(int i = 0; i < planeNum; i++)
        {
            bool valid = false;
            while (valid == false)
            {
                int randint = Random.Range(0, plane.Count);
                if (plane[randint].active == false)
                {
                    valid = true;
                    plane[randint].SetActive(true);     //choose a random platform that is not active
                    plane[randint].transform.position = transform.position;
                   
                    if (i == 0)
                    {
                        plane[randint].transform.localPosition = transform.localPosition - myVector* startMutipler;  //if is the first platform,set its original starting point
                    }
                    else
                    {
                        plane[randint].transform.localPosition = platform.LastPlatform.transform.localPosition + myVector;      //others add space on top
                    }
                    platform.LastPlatform = plane[randint].GetComponent<platform>();
                    //Debug.Log(plane[randint].gameObject.name);
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void newplatform()       //generate a random unactive platform from the list and put in position
    {
        
        bool valid = false;
        while (valid == false)
        {
            int randint = Random.Range(0, plane.Count);
            if (plane[randint].active == false)
            {
                valid = true;
                plane[randint].SetActive(true);
                platform.LastPlatform = plane[randint].GetComponent<platform>();
                plane[randint].transform.position = transform.position;
                plane[randint].transform.localPosition = -myVector * startMutipler;//new Vector3(-160, 0, 0); 20 *
            }
            
        }
    }
}
