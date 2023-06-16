using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class BlobVolumeViewModel
{
    public ReactiveProperty<float> blobVolume { get; private set; }

    public ReactiveProperty<float> blobSize { get; private set; }

    private IEnumerator volumeDecreaseFromHighTemp;

    private CoroutineRunner coroutineRunner;

    public BlobVolumeViewModel(ReactiveProperty<float> blobTemp)
    {
        coroutineRunner = CoroutineRunner.Instance;
        blobSize = new ReactiveProperty<float>(1f);
        blobVolume = new ReactiveProperty<float>(GlobalModel.Instance.StartBlobVolume);
        blobVolume.Subscribe(_ => CheckSizeChangeFromVolume(_));
        blobTemp.Subscribe(_ => CheckVaporizeTemp(_));
    }

    private void CheckVaporizeTemp(float blobTemp)
    {
        if(blobTemp >= GlobalModel.Instance.VaporizationTemp && volumeDecreaseFromHighTemp == null && blobVolume.Value > 0)
        {
            volumeDecreaseFromHighTemp = VolumeIncreaseCoroutine();
            coroutineRunner.RunCoroutine(volumeDecreaseFromHighTemp);
        }
        else if ((blobTemp < GlobalModel.Instance.VaporizationTemp && volumeDecreaseFromHighTemp != null) || blobVolume.Value <= 0)
        {
            blobVolume.Value = 0;
            coroutineRunner.StopOneCoroutine(volumeDecreaseFromHighTemp);
            volumeDecreaseFromHighTemp = null;
        }
    }

    private void CheckSizeChangeFromVolume(float blobVolume)
    {
        if (blobVolume >= GlobalModel.Instance.MaxVolumeForBlobSize)
        {
            blobSize.Value = GlobalModel.Instance.MaxBlobSize;
        }
        else if (blobVolume <= GlobalModel.Instance.MinVolumeForBlobSize)
        {
            blobSize.Value = GlobalModel.Instance.MinBlobSize;
        }
        else
        {
            blobSize.Value = blobVolume / GlobalModel.Instance.StartBlobVolume;
        }        
    }

    private IEnumerator VolumeIncreaseCoroutine()
    {
        while (true)
        {
         //   Debug.Log("blobVolume: " + blobVolume.Value.ToString());
            blobVolume.Value -= GlobalModel.Instance.VaporizationVolumeSpeed;
            yield return new WaitForSeconds(GlobalModel.Instance.ModificationTime);
        }
    }
}
