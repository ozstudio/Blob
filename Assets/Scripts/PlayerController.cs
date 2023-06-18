using UniRx;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
   
    //[SerializeField]
   // float _speedIncreaseFactor = 1f;
    [SerializeField]
    Transform _raycastPoint;
    //[SerializeField]
    //float _minDistanceToGround = 0.1f;
    //[SerializeField]
    private GameInput _gameInput;

    //private float _moveSpeed;
    //private float _maxSpeed;
    //private float _minSpeed;
    //private float _currentTemp;
    //private float _prevTemp;
    //private Rigidbody _rigidbody;





    public float maxHeight = 3f;
    public BlobView blobV;


    private void Start()
    {
       
        //_maxSpeed = GlobalModel.Instance.StartBlobSpeed * 2;
        //_minSpeed = GlobalModel.Instance.StartBlobSpeed / 2;
        //_moveSpeed = GlobalModel.Instance.StartBlobSpeed;
        //_currentTemp = GlobalModel.Instance.StartBlobTemp;
        //_prevTemp = _currentTemp;
        Invoke("GetBlowV", 1f);
      
    }


    void GetBlowV()
    {
        //blobV.blobTempViewModel.blobTemp.Subscribe(blobTemp =>
        //{
        //    _currentTemp = blobTemp;

        //    if (_currentTemp > _prevTemp)
        //    {
        //        _moveSpeed += _speedIncreaseFactor;
        //    }

        //    if (_currentTemp < _prevTemp)
        //    {
        //        _moveSpeed -= _speedIncreaseFactor;

        //    }

        //});
    }
    private void FixedUpdate()
    {
        //Vector2 inputVector = _gameInput.GetMovementVectorNormalized();
        //Vector2 moveDir = new Vector2(inputVector.x, _rigidbody.velocity.y);
        //_rigidbody.velocity = new Vector2(moveDir.x * BlobSpeed(),_rigidbody.velocity.y);
    }

    //float BlobSpeed()
    //{
    //    if (_moveSpeed >= _maxSpeed)
    //    {
    //        _moveSpeed = _maxSpeed;
    //    }
    //    if (_moveSpeed <= _minSpeed)
    //    {
    //        _moveSpeed = _minSpeed;
    //    }
    //    return _moveSpeed;

    //}
    private void Update()
    {
        if (_gameInput._actions.Player.Jump.WasPressedThisFrame() || Input.GetKeyDown(KeyCode.K))
        {
            PlayerJump();
        }


    }

    

    public void PlayerJump()
    {
        
        //float jumpHeight = maxHeight * gameObject.transform.localScale.x;
        //if (CheckIfGrounded())
        //{
        //  //  _rigidbody.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2 * Physics.gravity.y), ForceMode.Impulse);
            
        //}

    }

    //bool CheckIfGrounded()
    //{
    //    bool grounded = false;
    //    RaycastHit hit;
    //    Ray ray = new Ray(_raycastPoint.position, -transform.up);
    //    Debug.DrawRay(_raycastPoint.position, -transform.up, Color.red);

    //    if (Physics.Raycast(ray, out hit, _minDistanceToGround))
    //    {

    //        if (hit.collider != null)
    //        {
    //            grounded = true;
    //        }
    //        else
    //        {
    //            grounded = false;
    //        }
    //    }
    //    return grounded;
    //}


}
