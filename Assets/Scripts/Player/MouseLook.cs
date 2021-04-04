﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 100f;

    public Transform playerBody;

    float xRotation = 0f;

    private float sideRecoil = 0;
    private float upRecoil = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void AddRecoil(float up, float side)
    {
        sideRecoil = side;
        upRecoil = up;
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime + sideRecoil;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime + upRecoil;


            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -75f, 75f);

            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
