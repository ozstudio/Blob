using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class BlobVolumeViewModel
{
    public ReactiveProperty<float> blobVolume { get; private set; } = new ReactiveProperty<float>();

    public ReactiveProperty<float> blobSize { get; private set; } = new ReactiveProperty<float>();

    private IEnumerator volumeDecreaseFromHighTemp;

    private CoroutineRunner coroutineRunner;

    public BlobVolumeViewModel(ReactiveProperty<float> blobTemp)
    {
        coroutineRunner = CoroutineRunner.Instance;
        blobSize.Value = GlobalModel.Instance.StartBlobSize;
        blobVolume.Value = GlobalModel.Instance.StartBlobVolume;
        blobVolume.Subscribe(_ => CheckSizeChangeFromVolume(_));
        CheckpointManager.Instance.OnGoToLastCheckpoint += CheckpointManager_OnGoToLastCheckpoint;
        blobTemp.Subscribe(_ => CheckVaporizeTemp(_));
    }

    private void CheckpointManager_OnGoToLastCheckpoint(object sender, System.EventArgs e)
    {
        blobVolume.Value = CheckpointManager.Instance.BlobVolumeCheckpoint;
    }

    public void WaterTouchActions()
    {
        //������� ���������� ��� ������� � �����
        blobVolume.Value += GlobalModel.Instance.BlobVolumeFilled;
        CheckMaxBlobVolume();
    }

    private void CheckVaporizeTemp(float blobTemp)
    {
        //������� ���������� ������ ��� ��� ��������� �����������. ���������, ����� �� ��������� ����� ����� ������.
        if(blobTemp >= GlobalModel.Instance.VaporizationTemp && blobTemp < GlobalModel.Instance.BoilingTemp && volumeDecreaseFromHighTemp == null && blobVolume.Value > 0)
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
        //��������, ��� ����� �� ���� ������ 0. ���������� �����, ��� ����������� �����������
        if(blobVolume.Value <= 0)
        {
            //blobVolume.Value = GlobalModel.Instance.StartBlobVolume;//����� ����
            if(volumeDecreaseFromHighTemp != null)
            {
                coroutineRunner.StopOneCoroutine(volumeDecreaseFromHighTemp);
                volumeDecreaseFromHighTemp = null;
            }
            GameManager.Instance.TryGameOver();
        }
    }

    private void CheckMaxBlobVolume()
    {
        //��������, ��� ����� �� ���� ������ 100. ���������� �����, ��� ������������� �����������
        if (blobVolume.Value >= 100)
        {
            blobVolume.Value = 100;
        }
    }

    private void CheckSizeChangeFromVolume(float blobVolume)
    {
        //������� ��� ��������� ������� ����� � ����������� �� ������
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
        //�������� ��� ���������� ������ �����
        while (true)
        {
            blobVolume.Value -= GlobalModel.Instance.VaporizationVolumeSpeed;
            yield return new WaitForSeconds(GlobalModel.Instance.ModificationTime);
        }
    }
}
