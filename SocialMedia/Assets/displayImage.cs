using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class displayImage : MonoBehaviour
{
    [SerializeField] private UnityEvent ButtonPressed;
    public Menu currentMenu;
    //[SerializeField] private Animator animator;

    private enum Moving
    {
        Up,
        Down
    }

    public void buttonPressed()
    {
        Transform image = transform.GetChild(0);
        if (image.localPosition.y > 350)
        {
            //image.localPosition = new Vector2(0, 0);
            //animator.enabled = true;
            //animator.SetTrigger("moveUp");
            StartCoroutine(anim(image, Moving.Up));
        }
        else if (image.localPosition.y < -350)
        {
            //animator.enabled = true;
            //animator.SetTrigger("moveDown");
            StartCoroutine(anim(image, Moving.Down));
        }
    }

    private IEnumerator anim(Transform image, Moving moving)
    {
        ScrollRect scroll = gameObject.GetComponent<ScrollRect>();
        scroll.enabled = false;
        //animator.enabled = false;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        float yVelocity = 0.0f;
        float smoothTime = 0.05f;
        ButtonPressed.Invoke();
        if (moving == Moving.Up)
        {
            while (image.transform.localPosition.y < rectTransform.rect.height)
            {
                float amountToMoveY = Mathf.SmoothDamp(image.transform.localPosition.y, rectTransform.rect.height + 2, ref yVelocity, smoothTime);
                //float amountToMoveX = Mathf.SmoothDamp(image.transform.localPosition.x, FirePoint.transform.position.x, ref xVelocity, smoothTime);
                image.transform.localPosition = new Vector3(0f, amountToMoveY);
                //Debug.Log(Vector2.Distance(new Vector2(0, 2402), image.transform.localPosition));
                setElasticity();
                yield return null;
            }
        }
        else if (moving == Moving.Down)
        {
            while (image.transform.localPosition.y > -rectTransform.rect.height)
            {
                float amountToMoveY = Mathf.SmoothDamp(image.transform.localPosition.y, -(rectTransform.rect.height + 2), ref yVelocity, smoothTime);
                //float amountToMoveX = Mathf.SmoothDamp(image.transform.localPosition.x, FirePoint.transform.position.x, ref xVelocity, smoothTime);
                image.transform.localPosition = new Vector3(0f, amountToMoveY);
                //Debug.Log(Vector2.Distance(new Vector2(0, -2402), image.transform.localPosition));
                setElasticity();
                yield return null;
            }
        }
        currentMenu.displayingImage = false;
        image.localPosition = new Vector2(0, 0);
        GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(GameObject.FindGameObjectWithTag("Canvas").transform.childCount - 1).gameObject.SetActive(false);
    }

    public void setElasticity()
    {
        ScrollRect scroll = gameObject.GetComponent<ScrollRect>();
        Transform theDisplayImage = transform.GetChild(0);

        //scroll.elasticity = Mathf.Abs(theDisplayImage.localPosition.y / 1000) + 0.1f;
        float alpha = theDisplayImage.localPosition.y;
        if (alpha < 0)
        {
            alpha *= -1;
        }
        alpha = 1 - (alpha / 1500);
        theDisplayImage.GetComponent<Image>().color = new Color(theDisplayImage.GetComponent<Image>().color.r, theDisplayImage.GetComponent<Image>().color.g, theDisplayImage.GetComponent<Image>().color.b, /*Mathf.Abs*/(alpha));
        gameObject.GetComponent<Image>().color = new Color(gameObject.GetComponent<Image>().color.r, gameObject.GetComponent<Image>().color.g, gameObject.GetComponent<Image>().color.b, /*Mathf.Abs*/(alpha));
        //Debug.Log(Mathf.Abs(alpha));
    }

    public void onLoad()
    {
        Transform theDisplayImage = transform.GetChild(0);
        theDisplayImage.GetComponent<Image>().color = new Color(theDisplayImage.GetComponent<Image>().color.r, theDisplayImage.GetComponent<Image>().color.g, theDisplayImage.GetComponent<Image>().color.b, /*Mathf.Abs*/(1));
        gameObject.GetComponent<Image>().color = new Color(gameObject.GetComponent<Image>().color.r, gameObject.GetComponent<Image>().color.g, gameObject.GetComponent<Image>().color.b, /*Mathf.Abs*/(1));
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Debug.Log("Cancelled pressed");
            Transform image = transform.GetChild(0);
            StartCoroutine(anim(image, Moving.Up));
        }
    }
}