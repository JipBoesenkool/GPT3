using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{

    public float movementSpeed;
    public float turnSpeed;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var v = -Input.GetAxis("Vertical"); // use the same axis that move back/forth
        var h = -Input.GetAxis("Horizontal"); // use the same axis that turns left/right

        if (Input.GetMouseButton(0))
        {
            //var mousePos = Input.mousePosition;
            //mousePos.x -= Screen.width / 2;
            //mousePos.y -= Screen.height / 2;

            v = -Input.GetAxis("Mouse Y"); // use the same axis that move back/forth
            h = -Input.GetAxis("Mouse X"); // use the same axis that turns left/right
        }

        Vector3 rot = transform.rotation.eulerAngles;
        rot.x += v * turnSpeed;
        rot.y += -h * turnSpeed;

        float angle = rot.x;
        if (angle > 270f)
        {
            angle -= 360f;
        }

        //Debug.Log("angle: " + angle.ToString());
        if (angle < -89f || angle > 89f)
        {
            rot = transform.rotation.eulerAngles;
        }
        transform.rotation = Quaternion.Euler(rot);

        transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }
}
