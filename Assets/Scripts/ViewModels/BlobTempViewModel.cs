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
        //Увеличение температуры
        tempIncreaseOnArea = TempIncreaseCoroutine();
        coroutineRunner.RunCoroutine(tempIncreaseOnArea);

    }

    public void HotAreaEndTouchActions()
    {
        onHotColdArea = false;
        //Завершение увеличения температуры
        if (tempIncreaseOnArea != null)
        {
            coroutineRunner.StopOneCoroutine(tempIncreaseOnArea);
        }
        OnBlobTempChange();
    }

    private IEnumerator TempIncreaseCoroutine()
    {
        //Корутина для увеличения температуры
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
        //Уменьшение температуры
        tempDecreaseOnArea = TempDecreaseOnAreaCoroutine();
        coroutineRunner.RunCoroutine(tempDecreaseOnArea);

    }

    public void ColdAreaEndTouchActions()
    {
        onHotColdArea = false;
        //Завершение уменьшения температуры
        if (tempDecreaseOnArea != null)
        {
            coroutineRunner.StopOneCoroutine(tempDecreaseOnArea);
            tempDecreaseOnArea = null;
        }
        OnBlobTempChange();
    }

    private IEnumerator TempDecreaseOnAreaCoroutine()
    {
        //Корутина для уменьшения температуры
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
        //Функция, которая вызывается каждый раз, когда меняется температура
        СoolingDown(blobTemp.Value);
    }

    public void СoolingDown(float temp)
    {
        //Проверка, нужно ли уменьшать температуру, если она больше стартовой температуры(25)
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
        //Проверка, что температура не стала меньше 0. Вызывается везде, где уменьшается температура
        if (blobTemp.Value <= 0)
        {
            blobTemp.Value = 0;//Конец игры
            if (tempDecreaseOnArea != null)
            {
                coroutineRunner.StopOneCoroutine(tempDecreaseOnArea);
                tempDecreaseOnArea = null;
            }
        }
    }
    private void CheckMaxBlobTemp()
    {
        //Проверка, что температура не стала больше 100. Вызывается везде, где увеличивается температура
        if (blobTemp.Value >= 100)
        {
            blobTemp.Value = 100;//Конец игры
            if (tempIncreaseOnArea != null)
            {
                coroutineRunner.StopOneCoroutine(tempIncreaseOnArea);
                tempIncreaseOnArea = null;
            }
        }
    }
}
