using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    PlayerController playerController;



    float x;
    float y;

    [Header("Zoom")]
    [SerializeField] private float zoomValue; // How much to zoom
    [SerializeField] private float zoomMin, zoomMax; // The min and max zoom amounts
    [SerializeField] private float startPos; // At which distance the camera should be at on start
    private float cameraValue;

    [Header("Rotation")]
    [SerializeField] private float cameraSensitivity; // Rotate speed
    [SerializeField] private Transform rotateObject; // The object the camera rotates
    [SerializeField] private float minY, maxY; // How much the camera can move up and down
    float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        if(startPos < zoomMin || startPos > zoomMax)  // Makes sure that the start camera position is inside the min and max values
        {
            Debug.Log("Start position outside values.");
            float newPos = (zoomMin + zoomMax) / 2;
            transform.position = new Vector3(0, 0, -newPos);
        }

    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();
        HandleZoom();

    }

    private void HandleZoom() // Checks the distance between the camera and rotator and checks if the distance is within the min and max value.
    {
        float checkDistance = Vector3.Distance(transform.position, rotateObject.transform.position);

        if (checkDistance > zoomMax && cameraValue < 0)
            return;

        if (checkDistance < zoomMin && cameraValue > 0)
            return;

        transform.position += transform.forward * cameraValue;
    }

    private void HandleRotation() // Rotates the rotate object so that the camera orbits
    {
        rotateObject.transform.eulerAngles += Vector3.up * x * cameraSensitivity;

        xRotation += y * cameraSensitivity;

        xRotation = Mathf.Clamp(xRotation, minY, maxY); // Clamps the max and minimum Y value for looking up and down

        rotateObject.eulerAngles = new Vector3(xRotation, rotateObject.transform.eulerAngles.y, rotateObject.transform.eulerAngles.z);
    }

    public void OrbitPlayer(InputAction.CallbackContext context) // Reads the x and y position of the mouse delta
    {
        x = context.ReadValue<Vector2>().x;
        y = context.ReadValue<Vector2>().y;
    }

    public void ZoomCamera(InputAction.CallbackContext context) // Reads the scroll on the mouse wheel and clamps it between -1 to 1
    {
        cameraValue = context.ReadValue<Vector2>().y;
        cameraValue = Mathf.Clamp(cameraValue, -1, 1);
    }
}
