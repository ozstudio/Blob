using UniRx;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;

    [SerializeField]
    private FixedJoystick _fixedJoystick;

    [SerializeField]
    float _speedIncreaseFactor = 1f;
    [SerializeField]
    Transform _raycastPoint;
    [SerializeField]
    float _minDistanceToGround = 0.1f;

    private float _moveSpeed;
    private float _maxSpeed;
    private float _minSpeed;
    private float _currentTemp;
    private float _prevTemp;
    



    public float maxHeight = 3f;
    public BlobView blobV;


    private void Start()
    {
       
        _maxSpeed = GlobalModel.Instance.StartBlobSpeed * 2;
        _minSpeed = GlobalModel.Instance.StartBlobSpeed / 2;
        _moveSpeed = GlobalModel.Instance.StartBlobSpeed;
        _currentTemp = GlobalModel.Instance.StartBlobTemp;
        _prevTemp = _currentTemp;
        Invoke("GetBlowV", 1f);
    }


    void GetBlowV()
    {
        blobV.blobTempViewModel.blobTemp.Subscribe(blobTemp =>
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


    private void FixedUpdate()
    {
      
        if (_fixedJoystick.Horizontal > 0)
        {
            _rigidbody.velocity =
           new Vector3(1 * BlobSpeed(), _rigidbody.velocity.y, 0);
        }
        if (_fixedJoystick.Horizontal < 0)
        {
            _rigidbody.velocity =
          new Vector3(-1 * BlobSpeed(), _rigidbody.velocity.y, 0);

        }
       if(_fixedJoystick.Horizontal == 0)
        {
            _rigidbody.velocity = new Vector2(0,_rigidbody.velocity.y);
        }

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
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.K))
        {

            PlayerJump();
        }
    }

    public void PlayerJump()
    {
        float jumpHeight = maxHeight * gameObject.transform.localScale.x;
        if (CheckIfGrounded())
        {
           
            _rigidbody.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2 * Physics.gravity.y), ForceMode.Impulse);
        }

    }

    bool CheckIfGrounded()
    {
        bool grounded = false;
        RaycastHit hit;
        Ray ray = new Ray(_raycastPoint.position, -transform.up);
        Debug.DrawRay(_raycastPoint.position, -transform.up, Color.red);

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
