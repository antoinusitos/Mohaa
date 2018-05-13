using System.Collections;
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
    public Transform leanTPS = null;
    public Transform leanFPS = null;

    private float _currentAngle = 0;

    public float leanAngle = 20.0f;

    [SyncVar]
    private bool _dead = true;

    public void SetCursorLocked(bool newState)
    {
        if (newState)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (isServer && !Data.GetInstance().DEBUG) return;

        if (_dead) return;

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

        Vector3 prevleanAngle = upperBodyFPS.rotation.eulerAngles;
        if (Input.GetKey(Data.GetInstance().leanLeft))
        {
            leanFPS.rotation = Quaternion.Euler(prevleanAngle.x, prevleanAngle.y, leanAngle);
            leanTPS.rotation = Quaternion.Euler(prevleanAngle.x, prevleanAngle.y, leanAngle);
        }
        else if (Input.GetKey(Data.GetInstance().leanRight))
        {
            leanFPS.rotation = Quaternion.Euler(prevleanAngle.x, prevleanAngle.y, -leanAngle);
            leanTPS.rotation = Quaternion.Euler(prevleanAngle.x, prevleanAngle.y, -leanAngle);
        }
        else
        {
            leanFPS.rotation = Quaternion.Euler(prevleanAngle.x, prevleanAngle.y, 0.0f);
            leanTPS.rotation = Quaternion.Euler(prevleanAngle.x, prevleanAngle.y, 0.0f);
        }
    }

    public void SetDead(bool newState)
    {
        _dead = newState;
    }
}
