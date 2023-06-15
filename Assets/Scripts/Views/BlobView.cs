using UniRx;
using UnityEngine;

public class BlobView : MonoBehaviour
{
    public BlobTempViewModel blobTempViewModel { get; private set; }
    public BlobVolumeViewModel blobVolumeViewModel { get; private set; }
    private Vector3 blobStartScale;

    void Awake()
    {
        blobStartScale = transform.localScale;
        blobTempViewModel = new BlobTempViewModel();
        blobVolumeViewModel = new BlobVolumeViewModel(blobTempViewModel.blobTemp);
        blobVolumeViewModel.blobSize.Subscribe(_ => ChangeBlobSize(_));
    }

    private void ChangeBlobSize(float blobSize)
    {
        transform.localScale = new Vector3(blobStartScale.x * blobSize, blobStartScale.y * blobSize, blobStartScale.z * blobSize);
    }
}
