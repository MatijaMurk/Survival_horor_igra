using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSway : MonoBehaviour
{
    [Header("Position")]
    public float amount;
    public float smoothAmount;
    public float maxAmount;
    private float movementX;
    private float movementY;

    private Vector3 initialPosition;

    [Header("Rotation")]
    public float rotationAmount = 25f;
    public float maxRotationAmount = 50f;
    public float smoothRotation = 12f;
    float tiltX;
    float tiltY;
    private Quaternion initialRotation;

    [Space]

    public bool rotationX = true;
    public bool rotationY = true;
    public bool rotationZ = true;




    private float InputX;
    private float InputY;



    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.localPosition; 
        initialRotation=transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateSway();
        MoveSway();
    }

    private void CalculateSway()
    {
        InputX = -Mouse.current.delta.x.ReadValue();
        InputY = -Mouse.current.delta.y.ReadValue();

    }

    private void MoveSway()
    {
        movementX = Mathf.Clamp(InputX * amount, -maxAmount, maxAmount);
        movementY = Mathf.Clamp(InputY * amount, -maxAmount, maxAmount);

        Vector3 finalPos = new Vector3(movementX, movementY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + initialPosition, Time.deltaTime * smoothAmount);
    }

    private void TiltSway()
    {
        tiltY = Mathf.Clamp(InputX*rotationAmount, -maxRotationAmount, maxRotationAmount);
        tiltX = Mathf.Clamp(InputY*rotationAmount, -maxRotationAmount, maxRotationAmount);

        Quaternion finalRotation = Quaternion.Euler(rotationX ? tiltX: 0f, rotationY ? tiltY : 0f, rotationZ ? tiltY : 0f);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRotation * initialRotation, Time.deltaTime * smoothRotation);

    }
}
