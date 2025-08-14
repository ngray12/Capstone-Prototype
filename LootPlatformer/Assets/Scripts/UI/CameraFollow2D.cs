using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;      
    public float smoothTime = 0.2f; 
    public Vector3 offset;        

    [Header("Bounds")]
    public Vector2 minBounds;        
    public Vector2 maxBounds;   
    

    private Vector3 velocity = Vector3.zero;
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null)
            return;

  
        Vector3 targetPosition = target.position + offset;

        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = camHalfHeight * cam.aspect;

        targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y + camHalfHeight, maxBounds.y - camHalfHeight);
        targetPosition.z = offset.z;


        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
    private void OnDrawGizmos()
    {
        if (cam == null) cam = GetComponent<Camera>();
        Gizmos.color = Color.red;

        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = camHalfHeight * cam.aspect;

        
        Vector3 bottomLeft = new Vector3(minBounds.x + camHalfWidth, minBounds.y + camHalfHeight, 0f);
        Vector3 topLeft = new Vector3(minBounds.x + camHalfWidth, maxBounds.y - camHalfHeight, 0f);
        Vector3 topRight = new Vector3(maxBounds.x - camHalfWidth, maxBounds.y - camHalfHeight, 0f);
        Vector3 bottomRight = new Vector3(maxBounds.x - camHalfWidth, minBounds.y + camHalfHeight, 0f);

        
        Gizmos.DrawLine(bottomLeft, topLeft);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
    }

}
