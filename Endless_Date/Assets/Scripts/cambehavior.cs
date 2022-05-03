using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//allow user to change their camera view angle using their WASD/arrow keys
public class cambehavior : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] float xLimit = 60f;
    //[SerializeField] float yboLimit = -30;
    [SerializeField] float yLimit = 60f;
    public int invertX = 1;     //allow player to invert their control
    public int invertY = 1;
    float moveZ;
    float moveY;
    float xRotation = 0;
    float yRotation = 0;
    Quaternion origin;

    void Start()
    {
        origin = transform.rotation;
    }


    void Update()
    {
        moveZ = Input.GetAxis("Vertical") * speed * Time.deltaTime*-1 * invertY;        //vertical rotation with clamp
        yRotation += moveZ;
        yRotation = Mathf.Clamp(yRotation, -xLimit, xLimit);
        moveY = Input.GetAxis("Horizontal") * speed * Time.deltaTime * invertX;         //horizontal rotation with clamp
        xRotation += moveY;
        xRotation = Mathf.Clamp(xRotation, -yLimit, yLimit);
        transform.rotation = origin* Quaternion.Euler(yRotation, xRotation, 0);

    }
}
