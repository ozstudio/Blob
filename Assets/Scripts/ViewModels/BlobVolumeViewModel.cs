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

    public void WaterTouchActions()
    {
        //Функция вызывается при касании с водой
        blobVolume.Value += GlobalModel.Instance.BlobVolumeFilled;
        CheckMaxBlobVolume();
    }

    private void CheckVaporizeTemp(float blobTemp)
    {
        //Функция вызывается каждый раз при изменении температуры. Проверяет, нужно ли уменьшать объем капли сейчас.
        if(blobTemp >= GlobalModel.Instance.VaporizationTemp && volumeDecreaseFromHighTemp == null && blobVolume.Value > 0)
        {
            volumeDecreaseFromHighTemp = VolumeDecreaseCoroutine();
            coroutineRunner.RunCoroutine(volumeDecreaseFromHighTemp);
        }
        else if ((blobTemp < GlobalModel.Instance.VaporizationTemp && volumeDecreaseFromHighTemp != null))
        {
            coroutineRunner.StopOneCoroutine(volumeDecreaseFromHighTemp);
            volumeDecreaseFromHighTemp = null;
        }
        CheckMinBlobVolume();
    }

    private void CheckMinBlobVolume()
    {
        //Проверка, что объем не стал меньше 0. Вызывается везде, где уменьшается температура
        if(blobVolume.Value <= 0)
        {
            blobVolume.Value = 0;//Конец игры
            if(volumeDecreaseFromHighTemp != null)
            {
                coroutineRunner.StopOneCoroutine(volumeDecreaseFromHighTemp);
                volumeDecreaseFromHighTemp = null;
            }
        }
    }

    private void CheckMaxBlobVolume()
    {
        //Проверка, что объем не стал больше 100. Вызывается везде, где увеличивается температура
        if (blobVolume.Value >= 100)
        {
            blobVolume.Value = 100;
        }
    }

    private void CheckSizeChangeFromVolume(float blobVolume)
    {
        //Функция для пересчета размера капли в зависимости от объема
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

    private IEnumerator VolumeDecreaseCoroutine()
    {
        //Корутина для уменьшения объема капли
        while (true)
        {
            //Debug.Log("blobVolume: " + blobVolume.Value.ToString());
            blobVolume.Value -= GlobalModel.Instance.VaporizationVolumeSpeed;
            yield return new WaitForSeconds(GlobalModel.Instance.ModificationTime);
        }
    }
}
