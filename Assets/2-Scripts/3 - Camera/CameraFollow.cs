using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CameraFollow : MonoBehaviour
{

    public GameObject cameraFollow;
    public float cameraMoveSpeed = 120.0f;
    public float lowClampAngle = 80.0f, highClampAngle = 80.0f;
    public float inputSensitivity = 150.0f;
    private float mouseX, mouseY, finalInputX, finalInputZ, smoothX, smoothY;
    private float rotX = 0.0f, rotY = 0.0f;
    private Quaternion localRotation;
    public UiController uicontroller;
    private Player player;

    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;

        player = GameController.controller.player.GetComponent<Player>();

        //Cursor.lockState = CursorLockMode.Locked;      
    }


    void Update()
    {
        if (uicontroller.camXSpeed!=0 && uicontroller.camYSpeed!=0)
        {
            rotateCamera();
        }

    }

    void LateUpdate()
    {
        updateCamera();
    }

    void rotateCamera()
    {
        if (player.target == null)
        {
            //Set up the rotation of the controller sticks or mouse
            float horizontalInput = Input.GetAxis("RightStickHorizontal");
            float vertivalInput = Input.GetAxis("RightStickVertical");
            mouseX = Input.GetAxis("Mouse X");
            mouseY = -Input.GetAxis("Mouse Y");
            finalInputX = mouseX + horizontalInput;
            finalInputZ = mouseY + vertivalInput;

            rotY += finalInputX * inputSensitivity * Time.deltaTime;
            rotX += finalInputZ * inputSensitivity * Time.deltaTime;

            rotX = Mathf.Clamp(rotX, -lowClampAngle, highClampAngle);


            localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
            transform.rotation = localRotation; //normal rotation
        }
        else //If player is locked on a target
        {
            Vector3 targetPos = player.target.transform.position;
            targetPos.y = this.transform.position.y;

            var rotation = Quaternion.LookRotation(targetPos - this.transform.position);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5);           
        }
    }

    void updateCamera()
    {
        //set the target object to follow
        Transform target = cameraFollow.transform;

        //move towards the target
        float step = cameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
