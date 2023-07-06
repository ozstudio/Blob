using System.Collections;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class BlobTempViewModel
{
    public ReactiveProperty<float> blobTemp { get; private set; }

    private bool onHotColdArea = false;

    private CoroutineRunner coroutineRunner;

    private IEnumerator tempIncreaseOnArea;
    private IEnumerator tempDecreaseOnArea;
    private IEnumerator tempDecreaseFromHighTemp;

    public BlobTempViewModel()
    {
        coroutineRunner = CoroutineRunner.Instance;
        GlobalModel model = GlobalModel.Instance;
        float startBlobTemp = GlobalModel.Instance.StartBlobTemp;
        blobTemp = new ReactiveProperty<float>(GlobalModel.Instance.StartBlobTemp);
        blobTemp.Subscribe(_ => OnBlobTempChange());
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
            //Debug.Log("blobTemp: " + blobTemp.Value.ToString());
            blobTemp.Value -= GlobalModel.Instance.BlobCoolingSpeed;
            yield return new WaitForSeconds(GlobalModel.Instance.ModificationTime);
        }
    }

    private void OnBlobTempChange()
    {
        CheckMinBlobTemp();
        CheckMaxBlobTemp();
        //�������, ������� ���������� ������ ���, ����� �������� �����������
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
            blobTemp.Value = 0;//����� ����
            if (tempDecreaseOnArea != null)
            {
                coroutineRunner.StopOneCoroutine(tempDecreaseOnArea);
                tempDecreaseOnArea = null;
            }
        }
    }
    private void CheckMaxBlobTemp()
    {
        //��������, ��� ����������� �� ����� ������ 100. ���������� �����, ��� ������������� �����������
        if (blobTemp.Value >= 100)
        {
            blobTemp.Value = 100;//����� ����
            if (tempIncreaseOnArea != null)
            {
                coroutineRunner.StopOneCoroutine(tempIncreaseOnArea);
                tempIncreaseOnArea = null;
            }
        }
    }
}
