using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class BlobMovementViewModel 
{
    
    public IObservable<Vector2> movement { get; private set; }
    public IObservable <long> playerJump { get; private set; }




    public ReactiveCommand Jump = new ReactiveCommand();

    private GameInput _gameInput;
    private float _moveSpeed;
    private float _maxSpeed;
    private float _minSpeed;
    private float _currentTemp;
    private float _prevTemp;
    private Rigidbody _rigidbody;
    private float _speedIncreaseFactor;
    private Transform _raycastPoint;
    private float _minDistanceToGround = 0.1f;
    private float _maxJumpHeight;
    public UnityEngine.UI.Button _jumpBtn;



    public BlobMovementViewModel(ReactiveProperty <float> blobTemp)
    {
        _jumpBtn = GameObject.FindGameObjectWithTag("JumpBtn").GetComponent<Button>();
        _maxJumpHeight = GlobalModel.Instance.MaxJumpHeight;
        _speedIncreaseFactor = GlobalModel.Instance.SpeedIncreaseFactor;
        _maxSpeed = GlobalModel.Instance.StartBlobSpeed * 2;
        _minSpeed = GlobalModel.Instance.StartBlobSpeed / 2;
        _moveSpeed = GlobalModel.Instance.StartBlobSpeed;
        _currentTemp = GlobalModel.Instance.StartBlobTemp;
       
        _prevTemp = _currentTemp;

        _raycastPoint = 
            GameObject.FindGameObjectWithTag("CheckGroundPoint").transform;

        _gameInput = 
            GameObject.FindGameObjectWithTag("GameInputJoystick").GetComponent<GameInput>();

        _rigidbody = 
            GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();

        


        movement = Observable.EveryFixedUpdate()
            .Select(_ =>
            {
                var x = _gameInput._fixedJoystick.Horizontal;
                var y = _gameInput._fixedJoystick.Vertical;

                Vector2 inputVector = _gameInput.GetMovementVectorNormalized();
                Vector2 moveDir = new Vector2(inputVector.x, _rigidbody.velocity.y);

                return new Vector2(moveDir.x * BlobSpeed(), _rigidbody.velocity.y);

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

        _jumpBtn.OnClickAsObservable().Subscribe(_ => PlayerJump());

       
        playerJump = Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.K));

            playerJump.Subscribe(_ => PlayerJump());
    }


    float BlobSpeed()
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

    void PlayerJump()
    {

        float jumpHeight = _maxJumpHeight * _rigidbody.transform.localScale.x;
        if (CheckIfGrounded())
        {
              _rigidbody.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2 * Physics.gravity.y), ForceMode.Impulse);

        }

    }

    bool CheckIfGrounded()
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
    }

}
