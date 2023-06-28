using System;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class BlobMovementViewModel 
{
    public ReactiveProperty<Vector3> blobMovement;

    private CharacterController characterController;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float ySpeed;
    private float jumpSpeed = 1f;
    private float gravityValue = -20.81f;

    private float _moveSpeed;
    private float _maxSpeed;
    private float _minSpeed;
    private float _currentTemp;
    private float _prevTemp;
    private float _speedIncreaseFactor;
    private float _maxJumpHeight;



    public BlobMovementViewModel(ReactiveProperty <float> blobTemp, CharacterController characterController)
    {
        blobMovement = new ReactiveProperty<Vector3>();
        this.characterController = characterController;

        _maxJumpHeight = GlobalModel.Instance.MaxJumpHeight;
        _speedIncreaseFactor = GlobalModel.Instance.SpeedIncreaseFactor;
        _maxSpeed = GlobalModel.Instance.StartBlobSpeed * 2;
        _minSpeed = GlobalModel.Instance.StartBlobSpeed / 2;
        _moveSpeed = GlobalModel.Instance.StartBlobSpeed;
        _currentTemp = GlobalModel.Instance.StartBlobTemp;

        _prevTemp = _currentTemp;

        GameInput.Instance.OnJump += GameInput_OnJump;
        GameInput.Instance.OnMove += GameInput_OnMove;


        var t = Observable.EveryUpdate().Subscribe(_ => {
            ySpeed += gravityValue * Time.deltaTime;
            if (characterController.isGrounded)
            {
                ySpeed = 0;
            }

            characterController.Move(new Vector3(0, ySpeed, 0) * Time.deltaTime);
            GameObject.FindGameObjectWithTag("Player").transform.position =
                new Vector3(characterController.transform.position.x,
                characterController.transform.position.y,0);
        });

        blobTemp.Subscribe(blobTemp =>
        {
            _currentTemp = blobTemp;

            if (_currentTemp > _prevTemp)
            {
                _moveSpeed += _speedIncreaseFactor;
            }

            if (_currentTemp < _prevTemp)
            {
                _moveSpeed -= _speedIncreaseFactor;

            }

        });    
    }

    private void GameInput_OnMove(object sender, GameInput.MovementVectorEventArgs e)
    {
        PlayerMove(e.movementVector);
    }

    private void GameInput_OnJump(object sender, EventArgs e)
    {
        PlayerJump();
    }

    private float BlobSpeed()
    {
        if (_moveSpeed >= _maxSpeed)
        {
            _moveSpeed = _maxSpeed;
        }
        if (_moveSpeed <= _minSpeed)
        {
            _moveSpeed = _minSpeed;
        }
        return _moveSpeed;

    }

    

    private void PlayerJump()
    {

        if (characterController.isGrounded)
        {
            float jumpHeight = _maxJumpHeight * characterController.transform.localScale.x;
            ySpeed = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue * jumpSpeed);
        } else
        {
            return;
        }


        characterController.Move(new Vector3(0, ySpeed, 0) * Time.deltaTime);

        //groundedPlayer = characterController.isGrounded;
        //if (groundedPlayer && playerVelocity.y < 0)
        //{
        //    playerVelocity.y = 0f;
        //}

        //float jumpHeight = _maxJumpHeight * characterController.transform.localScale.x;
        //if (groundedPlayer)
        //{
        //    playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        //}

        //playerVelocity.y += gravityValue * Time.deltaTime;
        //characterController.Move(playerVelocity * Time.deltaTime);
    }

    private void PlayerMove(Vector2 movement)
    {
        groundedPlayer = characterController.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(movement.x, playerVelocity.y, 0);
        characterController.Move(move * Time.deltaTime * BlobSpeed());

        if (move != Vector3.zero)
        {
            blobMovement.Value = move;
        }
    }

    /*private bool CheckIfGrounded()
    {
        bool grounded = false;
        RaycastHit hit;
        Ray ray = new Ray(_raycastPoint.position, -_rigidbody.transform.up);
        Debug.DrawRay(_raycastPoint.position, -_rigidbody.transform.up, Color.red);

        if (Physics.Raycast(ray, out hit, _minDistanceToGround))
        {

            if (hit.collider != null)
            {
                grounded = true;
            }
            else
            {
                grounded = false;
            }
        }
        return grounded;
    }*/

}
