using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    Camera playerCamera;

    Vector3 moveDirection;
    Vector2 movement;

    [SerializeField] private float movementSpeed = 8f;  

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main.GetComponent<Camera>();
    }

    private void Update()
    {
        HandleMovement();

    }

    private void HandleMovement() // Moves the player in the direction the camera is facing 
    {


        moveDirection = new Vector3(movement.x, 0, movement.y);
        moveDirection = playerCamera.transform.forward * moveDirection.z + playerCamera.transform.right * moveDirection.x;
        moveDirection.y = 0;
        moveDirection.Normalize();

        characterController.Move(moveDirection * Time.deltaTime * movementSpeed);
    }

    public void Move(InputAction.CallbackContext context) // Gets the value of the direction pressed
    {
        movement = context.ReadValue<Vector2>();
    }


}
