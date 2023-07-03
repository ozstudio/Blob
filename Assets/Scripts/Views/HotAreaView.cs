using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HotAreaView : MonoBehaviour
{
    private HotAreaViewModel hotAreaViewModel;
    // Start is called before the first frame update
    void Awake()
    {
        hotAreaViewModel = new HotAreaViewModel();
    }

    private void OnTriggerEnter(Collider other)
    {
        hotAreaViewModel?.OnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        hotAreaViewModel?.OnTriggerExit(other);
    }


}
