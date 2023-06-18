using System;
using UniRx;
using UnityEngine;

public class BlobView : MonoBehaviour
{
    public BlobTempViewModel blobTempViewModel { get; private set; }
    public BlobVolumeViewModel blobVolumeViewModel { get; private set; }
    public BlobMovementViewModel blobMovementViewModel { get; private set; }

    private Vector3 blobStartScale;


    void Awake()
    {
        blobStartScale = transform.localScale;
        blobTempViewModel = new BlobTempViewModel();

        blobMovementViewModel = new BlobMovementViewModel(blobTempViewModel.blobTemp);


        blobVolumeViewModel = new BlobVolumeViewModel(blobTempViewModel.blobTemp);


        blobVolumeViewModel.blobSize.Subscribe(_ => ChangeBlobSize(_));
        blobMovementViewModel.movement.Subscribe(_ => BlobMovement(_));
        



    }

    private void BlobMovement(Vector2 _)
    {
        transform.GetComponent<Rigidbody>().velocity = _;
    }

   

    private void ChangeBlobSize(float blobSize)
    {
        transform.localScale = new Vector3(blobStartScale.x * blobSize, blobStartScale.y * blobSize, blobStartScale.z * blobSize);
    }
}
