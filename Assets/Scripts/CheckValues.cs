using UniRx;
using TMPro;
using UnityEngine;

public class CheckValues : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tempText;
    [SerializeField] private TextMeshProUGUI volumeText;
    [SerializeField] private TextMeshProUGUI sizeText;

    [SerializeField] private BlobView blob;

    private void Start()
    {
        blob.blobTempViewModel.blobTemp.Subscribe(_ =>
        {
            tempText.text = "Temp: " + _.ToString();
        });
        blob.blobVolumeViewModel.blobVolume.Subscribe(_ =>
        {
            volumeText.text = "Volume: " + _.ToString();
        });
        blob.blobVolumeViewModel.blobSize.Subscribe(_ =>
        {
            sizeText.text = "Size: " + _.ToString();
        });
    }
}
