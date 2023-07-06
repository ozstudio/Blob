using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointViewModel
{
    public static CheckpointView lastCheckpoint { get; private set; }

    public void OnTriggerEnter(Collider other, CheckpointView checkpointView)
    {
        if (other.tag == "Player")
        {
            lastCheckpoint = checkpointView;
        }
    }


}
