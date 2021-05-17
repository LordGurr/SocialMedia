using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Menu : MonoBehaviour
{
    private GameObject prevMenu = null;

    public GameObject PrevMenu { set => prevMenu = value; }
    [SerializeField] public UnityEvent onLoad;
    [HideInInspector] public bool displayingImage = false;

    public void StepForward(GameObject nextMenu)
    {
        nextMenu.SetActive(true);
        nextMenu.GetComponent<Menu>().onLoad.Invoke();
        nextMenu.GetComponent<Menu>().PrevMenu = gameObject;
        gameObject.SetActive(false);
    }

    public void StepBack()
    {
        if (prevMenu != null)
        {
            prevMenu.SetActive(true);
            prevMenu.GetComponent<Menu>().onLoad.Invoke();
            gameObject.SetActive(false);
        }
    }

    public void Exit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Debug.Log("Cancelled pressed");
            if (prevMenu != null && !displayingImage)
            {
                StepBack();
            }
        }
    }

    public void goBackIfLoggedIn()
    {
        if (GameObject.FindGameObjectWithTag("Canvas").GetComponent<FirebaseInit>().checkIfLoggedIn())
        {
            StepBack();
        }
    }
}