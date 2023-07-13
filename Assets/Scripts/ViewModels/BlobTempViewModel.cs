using System;
using System.Collections;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class BlobTempViewModel
{
    public ReactiveProperty<float> blobTemp { get; private set; } = new ReactiveProperty<float>();   

    

    private bool onHotColdArea = false;
    private float startBlobTemp;
    private float minBlobTemp;
    private float maxBlobTemp;

    private CoroutineRunner coroutineRunner;

    private IEnumerator tempIncreaseOnArea;
    private IEnumerator tempDecreaseOnArea;
    private IEnumerator tempDecreaseFromHighTemp;

    private bool isGameEnd = false;

    public BlobTempViewModel()
    {
        coroutineRunner = CoroutineRunner.Instance;
        GlobalModel model = GlobalModel.Instance;
        startBlobTemp = GlobalModel.Instance.StartBlobTemp;
        minBlobTemp = GlobalModel.Instance.FreezingTemp;
        maxBlobTemp = GlobalModel.Instance.BoilingTemp;
        blobTemp.Value = GlobalModel.Instance.StartBlobTemp;

        blobTemp.Subscribe(_ => OnBlobTempChange());
        CheckpointManager.Instance.OnGoToLastCheckpoint += CheckpointManager_OnGoToLastCheckpoint; 
    }

    public void Update()
    {
        if (isGameEnd)
        {
            isGameEnd = false;
            GameManager.Instance.TryGameOver();
        }
    } 

    private void CheckpointManager_OnGoToLastCheckpoint(object sender, System.EventArgs e)
    {
        blobTemp.Value = CheckpointManager.Instance.BlobTempCheckpoint;
    }

    public void HotAreaTouchActions()
    {
        onHotColdArea = true;
        //���������� �����������
        tempIncreaseOnArea = TempIncreaseCoroutine();
        coroutineRunner.RunCoroutine(tempIncreaseOnArea);

    }

    public void HotAreaEndTouchActions()
    {
        onHotColdArea = false;
        //���������� ���������� �����������
        if (tempIncreaseOnArea != null)
        {
            coroutineRunner.StopOneCoroutine(tempIncreaseOnArea);
        }
        OnBlobTempChange();
    }

    private IEnumerator TempIncreaseCoroutine()
    {
        //�������� ��� ���������� �����������
        while (true)
        {
            //Debug.Log("blobTemp: " + blobTemp.Value.ToString());
            blobTemp.Value += GlobalModel.Instance.BlobHeatingSpeed;
            yield return new WaitForSeconds(GlobalModel.Instance.ModificationTime);
        }
    }

    public void ColdAreaTouchActions()
    {
        onHotColdArea = true;
        //���������� �����������
        tempDecreaseOnArea = TempDecreaseOnAreaCoroutine();
        coroutineRunner.RunCoroutine(tempDecreaseOnArea);

    }

    public void ColdAreaEndTouchActions()
    {
        onHotColdArea = false;
        //���������� ���������� �����������
        if (tempDecreaseOnArea != null)
        {
            coroutineRunner.StopOneCoroutine(tempDecreaseOnArea);
            tempDecreaseOnArea = null;
        }
        OnBlobTempChange();
    }

    private IEnumerator TempDecreaseOnAreaCoroutine()
    {
        //�������� ��� ���������� �����������
        while (true)
        {
            blobTemp.Value -= GlobalModel.Instance.BlobCoolingSpeed;
            yield return new WaitForSeconds(GlobalModel.Instance.ModificationTime);
        }
    }

    private void OnBlobTempChange()
    {
        //�������, ������� ���������� ������ ���, ����� �������� �����������
        CheckMinBlobTemp();
        CheckMaxBlobTemp();
        �oolingDown(blobTemp.Value);
    }

    public void �oolingDown(float temp)
    {
        //��������, ����� �� ��������� �����������, ���� ��� ������ ��������� �����������(25)
        if (!onHotColdArea)
        {
            if (temp > GlobalModel.Instance.StartBlobTemp && tempDecreaseFromHighTemp == null)
            {
                tempDecreaseFromHighTemp = TempDecreaseOnAreaCoroutine();
                coroutineRunner.RunCoroutine(tempDecreaseFromHighTemp);
            }
            else if(temp <= GlobalModel.Instance.StartBlobTemp && tempDecreaseFromHighTemp != null )
            {
                coroutineRunner.StopOneCoroutine(tempDecreaseFromHighTemp);
                tempDecreaseFromHighTemp = null;
            }
        }
        else if(tempDecreaseFromHighTemp != null)
        {
            coroutineRunner.StopOneCoroutine(tempDecreaseFromHighTemp);
            tempDecreaseFromHighTemp = null;
        }
    }

    private void CheckMinBlobTemp()
    {
        //��������, ��� ����������� �� ����� ������ 0. ���������� �����, ��� ����������� �����������
        if (blobTemp.Value <= 0)
        {
            onHotColdArea = false;
            //����� ����
            if (tempDecreaseOnArea != null)
            {
                coroutineRunner.StopOneCoroutine(tempDecreaseOnArea);
                tempDecreaseOnArea = null;
            }
            isGameEnd = true;
        }
    }
    private void CheckMaxBlobTemp()
    {
        //��������, ��� ����������� �� ����� ������ 100. ���������� �����, ��� ������������� �����������
        if (blobTemp.Value >= GlobalModel.Instance.BoilingTemp)
        {
            onHotColdArea = false;
            //����� ����
            if (tempIncreaseOnArea != null)
            {
                coroutineRunner.StopOneCoroutine(tempIncreaseOnArea);
                tempIncreaseOnArea = null;
            }
            isGameEnd = true;
        }
    }
}
