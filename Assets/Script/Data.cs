using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerMovement
{
    RUNNING,
    WALKING,
    CROUCH,
}

[System.Serializable]
public struct SPlayerMovementTweak
{
    public EPlayerMovement movement;
    public float speed;
}

public class Data : MonoBehaviour
{
    public bool DEBUG = false;

    public KeyCode forwardKeycode = KeyCode.Z;
    public KeyCode backwardKeycode = KeyCode.S;
    public KeyCode rightKeycode = KeyCode.Q;
    public KeyCode leftKeycode = KeyCode.D;
    public KeyCode fireKeycode = KeyCode.Mouse0;
    public KeyCode reload = KeyCode.R;

    private static Data _instance = null;
    public static Data GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        _instance = this;
    }
}
