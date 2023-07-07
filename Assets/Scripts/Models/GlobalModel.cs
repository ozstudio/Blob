using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class GlobalModel : MonoBehaviour
{
    //����� �������� �������, �� ������� ���������� �������� �� ������ ��������
    [SerializeField] private float modificationTime;

    [Header("----------------------")]
    //��������� ����������� �����
    [SerializeField] private float startBlobTemp;
    //�������� ���������� ����� � �������
    [SerializeField] private float blobHeatingSpeed;
    //�������� ���������� ����� � �������
    [SerializeField] private float blobCoolingSpeed;
    //����������� ��������� �����
    [SerializeField] private float vaporizationTemp;
    //�������� ��������� ����� � �������
    [SerializeField] private float vaporizationVolumeSpeed;
    //����������� ������� �����
    [SerializeField] private float boilingTemp;
    //��������� ����� �����
    [SerializeField] private float startBlobVolume;
    //��������� ������ �����
    [SerializeField] private float startBlobSize;

    [SerializeField] private float speedIncreaseFactor;
    [SerializeField] private float maxJumpHeight;

    //��������� �������� �����
    [SerializeField] private float startBlobSpeed;

    //������������ �������� �����
    [SerializeField] private float maxBlobSpeed;
    //����������� �������� �����
    [SerializeField] private float minBlobSpeed;
    //����������� ����������
    [SerializeField] private float freezingTemp;
    //������������ ������ ����� (1.5)
    [SerializeField] private float maxBlobSize;
    //����������� ������ ����� (0.5)
    [SerializeField] private float minBlobSize;
    //�������� ������ ����� ��� ������������ ������� (75)
    [SerializeField] private float maxVolumeForBlobSize;
    //�������� ������ ����� ��� ����������� ������� (25)
    [SerializeField] private float minVolumeForBlobSize;
    //������������ ����� ����� �� ����
    [SerializeField] private float blobVolumeFilled;

    [Header("----------------------")]

    //���-�� ������ �� ������
    [SerializeField] private int livesCount;

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
    public float StartBlobSize{ get { return startBlobSize; } }
    public float StartBlobSpeed { get { return startBlobSpeed; } }
    public float MaxBlobSpeed { get { return maxBlobSpeed; } }
    public float MinBlobSpeed { get { return minBlobSpeed; } }
    public float FreezingTemp { get { return freezingTemp; } }
    public float MaxBlobSize { get { return maxBlobSize; } }
    public float MinBlobSize { get { return minBlobSize; } }
    public float MaxVolumeForBlobSize { get { return maxVolumeForBlobSize; } }
    public float MinVolumeForBlobSize { get { return minVolumeForBlobSize; } }
    public float BlobVolumeFilled { get { return blobVolumeFilled; } }


    public int LivesCount { get { return livesCount; } }

    public static GlobalModel Instance;

    private void Awake()
    {
        Instance = this;
    }


}
