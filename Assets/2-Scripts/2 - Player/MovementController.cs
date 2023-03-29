using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
    public float movementSpeed = 5f, rotationSpeed = 0.3f, allowRotation = 0.1f, gravity = 5f;
    private float inputX, inputZ, speed;
    private Camera cam;
    private CharacterController characterController;
    private Vector3 desiredMoveDirection;
    private GameObject target;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
    }


    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        InputDecider();
        MovementManager();
    }

    void InputDecider()
    {
        speed = new Vector2(inputX, inputZ).sqrMagnitude;

        if (speed > allowRotation)
        {
            RotationManager();
        }
        else desiredMoveDirection = Vector3.zero;

    }

    void RotationManager()
    {
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * inputZ + right * inputX;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed);
    }

    void MovementManager()
    {
        Vector3 moveDirection = desiredMoveDirection * movementSpeed * Time.deltaTime;
        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection);
    }
}
