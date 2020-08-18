using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateText : MonoBehaviour
{
    private bool isTriggered;
    private GameObject myPrefab; 

    public void MakeText(GameObject prefab)
    {
        if(myPrefab != prefab && myPrefab != null)
        {
            Destroy(myPrefab);
            isTriggered = false;
        }

        if (isTriggered == false)
        {
            myPrefab = Instantiate(prefab);
            isTriggered = true;
        }

    }
}
