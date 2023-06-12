using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdAreaViewModel
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<BlobView>().blobTempViewModel.ColdAreaTouchActions();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<BlobView>().blobTempViewModel.ColdAreaEndTouchActions();
        }
    }
}
