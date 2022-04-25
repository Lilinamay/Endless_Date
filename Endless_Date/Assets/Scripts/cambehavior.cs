using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cambehavior : MonoBehaviour
{
    [SerializeField] Camera Cam;
    [SerializeField] float speed = 5;
    [SerializeField] float xLimit = 60f;
    [SerializeField] float yboLimit = -30;
    [SerializeField] float yLimit = 60f;
    public float moveZ;
    public float moveY;
    public float xRotation = 0;
    public float yRotation = 0;
    Quaternion origin;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //moveZ = Input.GetAxis("Mouse X") * speed * Time.deltaTime; 
        moveZ = Input.GetAxis("Vertical") * speed * Time.deltaTime*-1;

        yRotation += moveZ;
        yRotation = Mathf.Clamp(yRotation, -xLimit, xLimit);
        //moveZ = Input.GetAxis("Mouse Y") * speed * Time.deltaTime;
        moveY = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        xRotation += moveY;
        xRotation = Mathf.Clamp(xRotation, -yLimit, yLimit);
        transform.rotation = origin* Quaternion.Euler(yRotation, xRotation, 0);

    }
}
