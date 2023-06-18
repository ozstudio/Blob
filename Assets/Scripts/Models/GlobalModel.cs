using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class GlobalModel : MonoBehaviour
{
    //Общий параметр времени, за которое изменяется значение на нужную велечину
    [SerializeField] private float modificationTime;

    [Header("----------------------")]
    //Стартовая температура капли
    [SerializeField] private float startBlobTemp;
    //Скорость нагревания капли в секунду
    [SerializeField] private float blobHeatingSpeed;
    //Скорость охлаждения капли в секунду
    [SerializeField] private float blobCoolingSpeed;
    //Температура испарения капли
    [SerializeField] private float vaporizationTemp;
    //Скорость испарения капли в секунду
    [SerializeField] private float vaporizationVolumeSpeed;
    //Температура кипения капли
    [SerializeField] private float boilingTemp;
    //Стартовый объем капли
    [SerializeField] private float startBlobVolume;

    [SerializeField] private float speedIncreaseFactor;
    [SerializeField] private float maxJumpHeight;

    //Стартовая скорость капли
    [SerializeField] private float startBlobSpeed;

    //Максимальная скорость капли
    [SerializeField] private float maxBlobSpeed;
    //Минимальная скорость капли
    [SerializeField] private float minBlobSpeed;
    //Температура замерзания
    [SerializeField] private float freezingTemp;
    //Максимальный размер капли (1.5)
    [SerializeField] private float maxBlobSize;
    //Минимальный размер капли (0.5)
    [SerializeField] private float minBlobSize;
    //Значение объема капли при максимальном размере (75)
    [SerializeField] private float maxVolumeForBlobSize;
    //Значение объема капли при минимальном размере (25)
    [SerializeField] private float minVolumeForBlobSize;
    //Восполняемый объем капли от воды
    [SerializeField] private float blobVolumeFilled;

    public float ModificationTime { get { return modificationTime; } }
    public float StartBlobTemp { get { return startBlobTemp; } }
    public float BlobHeatingSpeed { get { return blobHeatingSpeed; } }
    public float BlobCoolingSpeed { get { return blobCoolingSpeed; } }
    public float VaporizationTemp { get { return vaporizationTemp; } }
    public float VaporizationVolumeSpeed { get { return vaporizationVolumeSpeed; } }

    public float SpeedIncreaseFactor { get { return speedIncreaseFactor; } }
    public float MaxJumpHeight { get { return maxJumpHeight; } }
    public float BoilingTemp { get { return boilingTemp; } }
    public float StartBlobVolume { get { return startBlobVolume; } }
    public float StartBlobSpeed { get { return startBlobSpeed; } }
    public float MaxBlobSpeed { get { return maxBlobSpeed; } }
    public float MinBlobSpeed { get { return minBlobSpeed; } }
    public float FreezingTemp { get { return freezingTemp; } }
    public float MaxBlobSize { get { return maxBlobSize; } }
    public float MinBlobSize { get { return minBlobSize; } }
    public float MaxVolumeForBlobSize { get { return maxVolumeForBlobSize; } }
    public float MinVolumeForBlobSize { get { return minVolumeForBlobSize; } }
    public float BlobVolumeFilled { get { return blobVolumeFilled; } }

    public static GlobalModel Instance;

    private void Awake()
    {
        Instance = this;
    }


}
