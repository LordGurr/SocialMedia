using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadPage : MonoBehaviour
{
    private GameObject theObject;

    public void Reload(GameObject objectToReload)
    {
        objectToReload.SetActive(false);
        theObject = objectToReload;
        StartCoroutine(wait());
    }

    private IEnumerator wait()
    {
        yield return new WaitForSecondsRealtime(2f);
        theObject.SetActive(true);
    }
}