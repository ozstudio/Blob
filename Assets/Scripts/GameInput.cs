using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    
    public FixedJoystick _fixedJoystick;

    public PlayerInputActions _actions;


    private void Awake()
    {
        _actions = new PlayerInputActions();
        _actions.Player.Enable();
        
    }

    private Rigidbody _rigidbody;
    private void Start()
    {
        _rigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = new Vector2(0,0);

        if (_fixedJoystick.Horizontal > 0)
        {
            inputVector.x = 1;
        }
        if (_fixedJoystick.Horizontal < 0)
        {
            inputVector.x = -1;
        }
        if (_fixedJoystick.Horizontal == 0)
        {
            inputVector.x = 0;
        }

       
        inputVector = inputVector.normalized;

        return inputVector;
    }





   
}
