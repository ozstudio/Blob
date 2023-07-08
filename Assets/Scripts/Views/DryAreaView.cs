using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryAreaView : MonoBehaviour
{
    private bool isMustSlide = false;
    private CharacterController _playerColl;
    [SerializeField]
    private float speed = 0.05f;
    [SerializeField]
    int chMoveDir;
    private BlobView _player;


    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<BlobView>();
        
    }


    private void Update()
    {
        //   print(180 - gameObject.transform.eulerAngles.z);

        if (isMustSlide)
        {
            Slide();
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            isMustSlide = true;
            _player.blobMovementViewModel.canJump = false;
           _playerColl = other.GetComponent<CharacterController>();
          
        }
    }
   
    private void OnTriggerExit(Collider other)
    {
        isMustSlide = false;
        _player.blobMovementViewModel.canJump = true;
    }
    
    void Slide()
    {
        _playerColl.Move(new Vector3(speed * Time.deltaTime * chMoveDir, 0, 0));
       
    }



}
