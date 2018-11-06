using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPortal : MonoBehaviour
{
    public float PortalSpeed;
    public float PortalDistance;
    public bool RotateClockwise;
    public Transform RotateObjectRef;

    // Stored value of the original height
    private float OriginalHeight;

    private void Start ()
    {
        // Store the original height for later use
        OriginalHeight = this.transform.position.y;

        // Calculate the position of the protal related with the 
        // orbit reference object while remain the original height
        Vector3 initialTransform = 
            (this.transform.position - RotateObjectRef.position).normalized 
            * PortalDistance + RotateObjectRef.position;
        initialTransform.y = OriginalHeight;
        this.transform.position = initialTransform;
	}

    private void Update ()
    {
        // Base on the rotation direction, change the speed accordingly
        float rotateSpeed = PortalSpeed;
        if (RotateClockwise == false)
        {
            rotateSpeed = -PortalSpeed;
        }

        // Apply the transform
        this.transform.RotateAround(RotateObjectRef.position, 
            Vector3.up, rotateSpeed * Time.deltaTime);
	}
}
