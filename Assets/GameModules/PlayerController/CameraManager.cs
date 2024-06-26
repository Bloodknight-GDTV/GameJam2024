using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour
{
    // Reference to the InputReader scriptable object
    [SerializeField] private InputReader input;

    Vector3 cameraVelocity = Vector3.zero;
    
    [Header("Targets & Transforms")]
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform cameraPivot;           // Pivot point of the camera (object transform)
    [SerializeField] private Transform cameraTransform;       // Camera object transform
   
    Vector2 cameraDirection;
    [Header("Pitch Variables (Up/Down)")]
    [SerializeField] private float cameraPitchSpeed = 1.0f;
    [SerializeField] private float minPitchAngle = -35.0f;
    [SerializeField] private float maxPitchAngle = 35.0f;
    float cameraPitch;
    
    [Header("Yaw Variables (Left/Right)")]
    [SerializeField] private float cameraYawSpeed = 1.0f; 
    float cameraYaw;            
    
    [Header("Camera Smoothing value")]
    [SerializeField] private float cameraSmoothTime = 0.2f;   // Camera Smooth Time for SmoothDamp() function
    
    private float startingPosition;
    //[Header("Camera Collision checking values")]
   // [SerializeField] private float cameraCollisionRadius = 2.0f;
   // [SerializeField] private float cameraCollisionOffset = 0.2f;
   // [SerializeField] private float minCollisionOffset = 0.2f;
   // [SerializeField] private LayerMask collisionLayers;


    private void Awake()
    {
        input.CameraEvent += HandleCamera;
        startingPosition = cameraTransform.position.z;
    }
    
    void HandleCamera(Vector2 dir)
    {
        cameraDirection = dir;
    }
    
    void CameraMove()
    {
        // do camera movement
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, 
            ref cameraVelocity, cameraSmoothTime);
        transform.position = targetPosition;
    }

    private void Update()
    {
        HandleAllCameraMovement();
    }

    public void HandleAllCameraMovement()
    {
        CameraMove();
        //FollowTarget();
        //RotateCamera();
        RotateCamera();
        //HandleCameraCollisions();
    }
    
    void RotateCamera()
    {
        //cameraDirection
        Vector3 rotation;
        cameraYaw += (cameraDirection.x * cameraYawSpeed);
        cameraPitch += (cameraDirection.y * cameraPitchSpeed);
        cameraPitch = Mathf.Clamp(cameraPitch, minPitchAngle, maxPitchAngle);
        
        rotation = Vector3.zero;
        rotation.y = cameraYaw;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;
        
        rotation = Vector3.zero;
        rotation.x = cameraPitch;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }
    
    #region Unused functions
   
    /*
    void HandleCameraCollisions()
    {
        float targetPosition = startingPosition;
        
        RaycastHit hit;
        
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        
        if(Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, 
               Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition -= distance - cameraCollisionOffset;
        }
        
        if(Mathf.Abs(targetPosition) < minCollisionOffset)
        {
            targetPosition -= minCollisionOffset;
        }
        
        Vector3 currentCameraPosition = Vector3.zero;
        currentCameraPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = currentCameraPosition;
    }
    void HandleZoom()
         {
             // Do zoom in/out
         }
         
    */

    #endregion
    
}
