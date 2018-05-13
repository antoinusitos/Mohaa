﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCamera: NetworkBehaviour
{
    private float verticalSpeed = 50.0f;
    private float horizontalSpeed = 150.0f;

    public Transform allBody = null;
    public Transform upperBodyFPS = null;
    public Transform upperBodyTPS = null;

    private float _currentAngle = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (isServer && !Data.GetInstance().DEBUG) return;

        float x = Input.GetAxis("Mouse X") * Time.deltaTime * horizontalSpeed;
        float z = Input.GetAxis("Mouse Y") * Time.deltaTime * verticalSpeed;
        if(z > 0 && _currentAngle < 89)
            _currentAngle += z;
        else if (z < 0 && _currentAngle > -89)
            _currentAngle += z;
        if (_currentAngle > 89) _currentAngle = 89;
        else if (_currentAngle < -89) _currentAngle = -89;

        Vector3 prevAngle = upperBodyFPS.rotation.eulerAngles;
        upperBodyFPS.rotation = Quaternion.Euler(-_currentAngle, prevAngle.y, prevAngle.z);
        upperBodyTPS.rotation = Quaternion.Euler(-_currentAngle, prevAngle.y, prevAngle.z);

        allBody.Rotate(0, x, 0);
    }
}