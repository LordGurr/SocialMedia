using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using UnityEngine.Events;

public class SetInläggSize : MonoBehaviour
{
    private RawImage rawImage;
    private RectTransform rectTransform;
    [HideInInspector] public bool finished = false;
    private bool likeHasBeenPressed = false;
    private SetHeight.Mode myMode;
    [SerializeField] private Animator likeButton;
    [SerializeField] private UnityEvent ButtonPressed;
    private Sprite sprite;

    public void SetStuff(Color color, string url, SetHeight.Mode parentMode)
    {
        finished = false;
        myMode = parentMode;
        if (myMode == SetHeight.Mode.Normal)
        {
            Image myImage = gameObject.GetComponent<Image>();
            myImage.color = color;
            rectTransform = gameObject.GetComponent<RectTransform>();
            rawImage = transform.GetChild(2).GetChild(0).GetComponent<RawImage>();
            StartCoroutine(SetPicture(rawImage, url));
        }
        else if (myMode == SetHeight.Mode.Profile)
        {
            Image myImage = gameObject.GetComponent<Image>();
            myImage.color = color;
            RawImage squareImage = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RawImage>();
            StartCoroutine(SetPicture(squareImage, url));
        }
    }

    public void SetStuff(Color color, Texture2D texture, SetHeight.Mode parentMode)
    {
        finished = false;
        myMode = parentMode;
        if (myMode == SetHeight.Mode.Normal || myMode == SetHeight.Mode.Upload)
        {
            Image myImage = gameObject.GetComponent<Image>();
            myImage.color = color;
            rectTransform = gameObject.GetComponent<RectTransform>();
            rawImage = transform.GetChild(2).GetChild(0).GetComponent<RawImage>();
            rawImage.texture = texture;
            sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
            //Debug.Log(texture.name);
            if (myMode == SetHeight.Mode.Normal)
            {
                theRestNormal(rawImage, sprite.rect.size.x, sprite.rect.size.y);
            }
            if (myMode == SetHeight.Mode.Upload)
            {
                theRestUpload(rawImage, sprite.rect.size.x, sprite.rect.size.y);
            }
        }
        else if (myMode == SetHeight.Mode.Profile)
        {
            Image myImage = gameObject.GetComponent<Image>();
            myImage.color = color;
            RawImage squareImage = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RawImage>();
            squareImage.texture = texture;
            sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
            if (myMode == SetHeight.Mode.Profile)
            {
                theRestProfile(rawImage, sprite.rect.size.x, sprite.rect.size.y);
            }
        }

        //rawImage.texture = texture;
        //sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        ////Debug.Log(texture.name);
        //if (myMode == SetHeight.Mode.Normal)
        //{
        //    theRestNormal(rawImage, sprite.rect.size.x, sprite.rect.size.y);
        //}
        //else if (myMode == SetHeight.Mode.Profile)
        //{
        //    theRestProfile(rawImage, sprite.rect.size.x, sprite.rect.size.y);
        //}
    }

    public void setLike()
    {
        if (likeHasBeenPressed)
        {
            likeButton.SetBool("LikePushed", true);
        }
    }

    public void SetStuff(Color color, SetHeight.Mode parentMode)
    {
        finished = false;
        myMode = parentMode;
        Image myImage = gameObject.GetComponent<Image>();
        myImage.color = color;
        Image squareImage = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>();
        //if (squareImage.sprite.rect.size.x > squareImage.sprite.rect.size.y /*&& myMode == SetHeight.Mode.Profile*/)
        //{
        //    Debug.Log("WidePicture set in profile");
        //    RectTransform squareRect = squareImage.gameObject.GetComponent<RectTransform>();
        //    squareRect.anchoredPosition = new Vector2(0, 0);
        //    squareRect.anchorMax = new Vector2(0.5f, 1);
        //    squareRect.anchorMin = new Vector2(0.5f, 0);
        //    squareRect.sizeDelta = new Vector2(2000, 312);
        //    squareRect.offsetMin = new Vector2(squareRect.offsetMin.x, 0);
        //    squareRect.offsetMax = new Vector2(squareRect.offsetMax.x, 0);
        //}
        finished = true;
    }

    private void theRestNormal(RawImage rawImage, float width, float height)
    {
        //Debug.Log((image.transform.parent.GetComponent<RectTransform>().rect.width) + "    " + gameObject.name);
        float inläggSize = (((rawImage.transform.parent.GetComponent<RectTransform>().rect.width) / (width / height)) + 210); //image.size.delta.x/ //Förrut 800

        TextMeshProUGUI username = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        username.text = ("Posted by " + UnityEngine.Random.Range(0f, 5000f));
        TextMeshProUGUI title = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        title.text = ("PlaceHolderText");
        Debug.Log("Inläggsize: " + inläggSize + " width: " + width + " Height: " + height);
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, inläggSize);
        finished = true;
    }

    private void theRestUpload(RawImage rawImage, float width, float height)
    {
        //Debug.Log((image.transform.parent.GetComponent<RectTransform>().rect.width) + "    " + gameObject.name);
        float inläggSize = (((rawImage.transform.parent.GetComponent<RectTransform>().rect.width) / (width / height))); //image.size.delta.x/ //Förrut 800

        TextMeshProUGUI username = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        username.text = ("Posted by " + UnityEngine.Random.Range(0f, 5000f));
        TextMeshProUGUI title = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        title.text = ("PlaceHolderText");
        Debug.Log("Inläggsize: " + inläggSize + " width: " + width + " Height: " + height);
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, inläggSize);
        finished = true;
    }

    private void theRestProfile(RawImage rawImage, float width, float height)
    {
        if (rawImage.texture.width > rawImage.texture.height /*&& myMode == SetHeight.Mode.Profile*/)
        {
            Debug.Log("WidePicture set in profile");
            RectTransform squareRect = rawImage.gameObject.GetComponent<RectTransform>();
            squareRect.anchoredPosition = new Vector2(0, 0);
            squareRect.anchorMax = new Vector2(0.5f, 1);
            squareRect.anchorMin = new Vector2(0.5f, 0);
            squareRect.sizeDelta = new Vector2(2000, 312);
            squareRect.offsetMin = new Vector2(squareRect.offsetMin.x, 0);
            squareRect.offsetMax = new Vector2(squareRect.offsetMax.x, 0);
            float inläggSize = (((squareRect.rect.height) / (height / width)));
            squareRect.sizeDelta = new Vector2(inläggSize, squareRect.sizeDelta.y);
        }
        else
        {
            RectTransform squareRect = rawImage.gameObject.GetComponent<RectTransform>();
            float inläggSize = (((squareRect.rect.width) / (width / height)));
            squareRect.sizeDelta = new Vector2(squareRect.sizeDelta.x, inläggSize);
        }
        finished = true;
    }

    private IEnumerator SetPicture(RawImage rawImage, string url)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log("Loading image error  " + uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);

                rawImage.texture = texture;
                sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
                //Debug.Log(texture.name);
                if (myMode == SetHeight.Mode.Normal)
                {
                    theRestNormal(rawImage, sprite.rect.size.x, sprite.rect.size.y);
                }
                else if (myMode == SetHeight.Mode.Profile)
                {
                    theRestProfile(rawImage, sprite.rect.size.x, sprite.rect.size.y);
                }
            }
        }
    }

    public void setDisplayImage()
    {
        Image displayImage = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(GameObject.FindGameObjectWithTag("Canvas").transform.childCount - 1).GetChild(0).GetComponent<Image>();
        //Texture2D newTexture = new Texture2D(rawImage.texture.width, rawImage.texture.height, TextureFormat.RGBA32, false);
        //newTexture.ReadPixels(new Rect(0, 0, rawImage.texture.width, rawImage.texture.height), 0, 0);
        //newTexture.Apply();
        displayImage.sprite = sprite; //Sprite.Create(newTexture, new Rect(0.0f, 0.0f, rawImage.texture.width, rawImage.texture.height), new Vector2(0.5f, 0.5f), 100f);
        if (myMode == SetHeight.Mode.Normal)
        {
            Menu currentMenu = GameObject.FindGameObjectWithTag("NormalView").GetComponent<Menu>();
            currentMenu.displayingImage = true;
            GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(GameObject.FindGameObjectWithTag("Canvas").transform.childCount - 1).gameObject.SetActive(true);
            GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(GameObject.FindGameObjectWithTag("Canvas").transform.childCount - 1).GetComponent<displayImage>().currentMenu = currentMenu;
            GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(GameObject.FindGameObjectWithTag("Canvas").transform.childCount - 1).GetComponent<Menu>().onLoad.Invoke();
        }
        else if (myMode == SetHeight.Mode.Profile)
        {
            Menu currentMenu = GameObject.FindGameObjectWithTag("Profile").GetComponent<Menu>();
            currentMenu.displayingImage = true;
            GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(GameObject.FindGameObjectWithTag("Canvas").transform.childCount - 1).gameObject.SetActive(true);
            GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(GameObject.FindGameObjectWithTag("Canvas").transform.childCount - 1).GetComponent<displayImage>().currentMenu = currentMenu;

            GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(GameObject.FindGameObjectWithTag("Canvas").transform.childCount - 1).GetComponent<Menu>().onLoad.Invoke();
        }
        else if (myMode == SetHeight.Mode.Upload)
        {
            Menu currentMenu = GameObject.FindGameObjectWithTag("Upload").GetComponent<Menu>();
            currentMenu.displayingImage = true;
            GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(GameObject.FindGameObjectWithTag("Canvas").transform.childCount - 1).gameObject.SetActive(true);
            GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(GameObject.FindGameObjectWithTag("Canvas").transform.childCount - 1).GetComponent<displayImage>().currentMenu = currentMenu;
            GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(GameObject.FindGameObjectWithTag("Canvas").transform.childCount - 1).GetComponent<Menu>().onLoad.Invoke();
        }
    }

    public void likeClicked()
    {
        TextMeshProUGUI likes = likeButton.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        likeButton.SetTrigger("Like" + UnityEngine.Random.Range(1, 6));
        if (!likeHasBeenPressed)
        {
            likes.text = ("Likes: " + 1);
            likeHasBeenPressed = true;
            likeButton.SetBool("LikePushed", true);
        }
        else
        {
            likes.text = ("Likes: " + 0);
            likeHasBeenPressed = false;
            likeButton.SetBool("LikePushed", false);
        }
        ButtonPressed.Invoke();
    }
}