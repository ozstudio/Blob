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
        blobTemp = new ReactiveProperty<float>(GlobalModel.Instance.StartBlobTemp);
        blobTemp.Subscribe(_ => �oolingDown(_));
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

        CheckCoolingDown();
    }

    private IEnumerator TempIncreaseCoroutine()
    {
        while (true)
        {
            Debug.Log("blobTemp: " + blobTemp.Value.ToString());
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
        if (tempIncreaseOnArea != null)
        {
            coroutineRunner.StopOneCoroutine(tempDecreaseOnArea);
            tempDecreaseOnArea = null;
        }
        CheckCoolingDown();
    }

    private IEnumerator TempDecreaseOnAreaCoroutine()
    {
        while (true)
        {
            Debug.Log("blobTemp: " + blobTemp.Value.ToString());
            blobTemp.Value -= GlobalModel.Instance.BlobCoolingSpeed;
            yield return new WaitForSeconds(GlobalModel.Instance.ModificationTime);
        }
    }

    private void CheckCoolingDown()
    {
        �oolingDown(blobTemp.Value);
    }

    public void �oolingDown(float temp)
    {
        if (!onHotColdArea)
        {
            if (temp > GlobalModel.Instance.VaporizationTemp && tempDecreaseFromHighTemp == null)
            {
                tempDecreaseFromHighTemp = TempDecreaseOnAreaCoroutine();
                coroutineRunner.RunCoroutine(tempDecreaseFromHighTemp);
            }
            else if(temp <= GlobalModel.Instance.VaporizationTemp && tempDecreaseFromHighTemp != null )
            {
                coroutineRunner.StopOneCoroutine(tempDecreaseFromHighTemp);
                tempDecreaseFromHighTemp = null;
            }
        }
    }
}
