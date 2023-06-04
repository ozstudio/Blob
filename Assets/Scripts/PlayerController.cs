using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody),typeof(SphereCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private FixedJoystick _fixedJoystick;
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _jumpForce;


    private void FixedUpdate()
    {
        _rigidbody.velocity = 
           new  Vector3(_fixedJoystick.Horizontal
           * _moveSpeed,_rigidbody.velocity.y,0);
    }
    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            
            Jump();
        }
    }

    public void Jump()
    {
        _rigidbody.AddForce(_jumpForce * Vector3.up,ForceMode.Force);
    }
}
