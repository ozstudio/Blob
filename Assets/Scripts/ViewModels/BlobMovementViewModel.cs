using System;
using UniRx;
using UnityEngine;

public class BlobMovementViewModel 
{
    public ReactiveProperty<Vector3> blobMovement;
    public ReactiveProperty<bool> isWall;

    private CharacterController characterController;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float ySpeed;
    private float jumpSpeed = 1f;
    private float gravityValue = -20.81f;
    public bool canJump = true;

    private float _moveSpeed;
    private float _maxSpeed;
    private float _minSpeed;
    private float _currentTemp;
    private float _prevTemp;
    private float _speedIncreaseFactor;
    private float _maxJumpHeight;
    public bool isTouchingWall;
    private Vector3 move;

    

    public BlobMovementViewModel(ReactiveProperty <float> blobTemp, CharacterController characterController)
    {
        blobMovement = new ReactiveProperty<Vector3>();
        isTouchingWall = false;

        
        
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


            //Add more gravity to push characterController to the ground
            //Zero gravity doesn't work well.
            if (characterController.isGrounded)
            {
                ySpeed = -1f;
            };

            //Send raycast to direction which Player is facing from middle 
            RaycastHit hitSide;
            if (Physics.Raycast(characterController.transform.position,
              characterController.transform
              .TransformDirection(Vector3.forward), out hitSide,

              //calculating raycast length.with this you can play to adjust how deep player sticks to the wall
              characterController.transform.localScale.x / 2 - 0.1f)
            )
            {
                //player can climb up/down verticaly only on walls with tag "VerticalWall"
                if (hitSide.transform.tag == "VerticalWall")
                {
                    isTouchingWall = true;
                }
            }
            else
            {
                isTouchingWall = false;
            }



            //check if characterController touch the ceiling
            if ((characterController.collisionFlags & CollisionFlags.Above) != 0)
            {
                // if characterController touching ceiling change characterController.move Y speed
                //you can play with number to increase or decrease gravity speed when touching ceilng
                ySpeed = gravityValue * Time.deltaTime * 5;
            }


            //stop characterController.move Y speed
            if (isTouchingWall)
            {
                ySpeed = 0;
              
            }
            else
            {
                ySpeed += gravityValue * Time.deltaTime;
               
            }
            
            characterController.Move(new Vector3(0, ySpeed, 0) * Time.deltaTime);


            //Prevent characterController from moving on Z axis.
            characterController.transform.position =
                new Vector3(characterController.transform.position.x,
                characterController.transform.position.y,0);
        });


        //set move speed according to blob temp
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
        CheckpointManager.Instance.OnGoToLastCheckpoint += CheckpointManagerOnGoToLastCheckpoint;
    }

    private void CheckpointManagerOnGoToLastCheckpoint(object sender, EventArgs e)
    {
        _moveSpeed = CheckpointManager.Instance.BlobMoveSpeedCheckpoint;
    }

    private void GameInput_OnMove(object sender, GameInput.MovementVectorEventArgs e)
    {
        PlayerMove(e.movementVector);
    }

    private void GameInput_OnJump(object sender, EventArgs e)
    {
        if (canJump)
        {
            PlayerJump();
        }

    }


    //!!! here check again!!! Think it is not working properly
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
        //Jump when characterController on "VerticalWall"(tag)
        if (isTouchingWall)
        {
            float jumpHeight = _maxJumpHeight * characterController.transform.localScale.x;
            ySpeed = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue * jumpSpeed);
            if(characterController.transform.rotation.y < 0)
            {
                characterController.Move(new Vector3(20f, ySpeed, 0) * Time.deltaTime);
            }
            if(characterController.transform.rotation.y > 0)
            {
                characterController.Move(new Vector3(-20f, ySpeed, 0) * Time.deltaTime);
            }

            isTouchingWall = false;

        }

        //Jump height according to the blob size.
        if (characterController.isGrounded)
        {
            float jumpHeight = _maxJumpHeight * characterController.transform.localScale.x;
            ySpeed = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue * jumpSpeed);
            characterController.Move(new Vector3(0, ySpeed, 0) * Time.deltaTime);

        }
       
    }

    private void PlayerMove(Vector2 movement)
    {
        
        groundedPlayer = characterController.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }




        //move player up/down on "VerticalWall"(tag)
        //check from which side and to which direction player facing
        if (isTouchingWall)
        {
            if (characterController.transform.rotation.y > 0)
            {
                if (movement.x < 0)
                {
                    move = new Vector3(0, -1, 0);
                }
                if (movement.x > 0)
                {
                    move = new Vector3(0, 1, 0);
                }

            }
            else
            {
                if (movement.x < 0)
                {
                    move = new Vector3(0, 1, 0);
                }
                if (movement.x > 0)
                {
                    move = new Vector3(0, -1, 0);
                }

            }
            

        }
        else
        {
            move = new Vector3(movement.x, 0, 0);

            //rotate player to stick direction
            characterController.transform.rotation = Quaternion.LookRotation(new Vector2(movement.x, 0));
        }
        characterController.stepOffset = BlobSpeed() / 200;

        if (move != Vector3.zero)
        {
            blobMovement.Value = move * BlobSpeed() * Time.deltaTime;

        }
    }
}
