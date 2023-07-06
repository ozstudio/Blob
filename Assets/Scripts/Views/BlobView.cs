using System;
using UniRx;
using UnityEngine;

public class BlobView : MonoBehaviour
{
    public BlobTempViewModel blobTempViewModel { get; private set; }
    public BlobVolumeViewModel blobVolumeViewModel { get; private set; }
    public BlobMovementViewModel blobMovementViewModel { get; private set; }

    private Vector3 blobStartScale;
    CharacterController characterController;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        blobStartScale = transform.localScale;

        blobTempViewModel = new BlobTempViewModel();

        blobMovementViewModel = new BlobMovementViewModel(blobTempViewModel.blobTemp, characterController);        

        blobVolumeViewModel = new BlobVolumeViewModel(blobTempViewModel.blobTemp);


        blobVolumeViewModel.blobSize.Subscribe(_ => ChangeBlobSize(_));
       // blobMovementViewModel.blobMovement.Subscribe(_ => BlobMovement(_));
        
        



    }

    
    //private void BlobMovement(Vector3 _)
    //{
    //       //gameObject.transform.forward =_;
    //     // characterController.Move(_ * Time.deltaTime);

    //}



    private void ChangeBlobSize(float blobSize)
    {
        transform.localScale = new Vector3(blobStartScale.x * blobSize, blobStartScale.y * blobSize, blobStartScale.z * blobSize);
    }
}
