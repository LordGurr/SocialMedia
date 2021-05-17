using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Net;

public class SetHeight : MonoBehaviour
{
    public enum Mode
    {
        Normal,
        Profile,
        Upload
    }

    public Mode displayMode;
    [SerializeField] private UnityEvent ScreenLoaded;
    private RectTransform rectTransform;
    [SerializeField] private UiColors uicolors;
    private float height = 0;
    [SerializeField] private string[] urls;
    [SerializeField] private GameObject inlägg;
    [HideInInspector] public bool loading = true;
    private float screenStartPos = float.MaxValue;
    [SerializeField] private Animator loadingScreen;
    private float timeSinceLoad = 0;

    public enum Sort
    {
        FromBottom,
        FromTop
    }

    public Sort sortMode;

    // Start is called before the first frame update
    public void Reload(bool anim)
    {
        loading = true;
        timeSinceLoad = 0;
        if (anim && loadingScreen != null)
        {
            loadingScreen.SetBool("Loading", true);
        }
        StartCoroutine(startup(anim));
    }

    private IEnumerator startup(bool anim)
    {
        yield return null;
        //Debug.Log("HeightStarted");
        yield return null;
        rectTransform = gameObject.GetComponent<RectTransform>();
        if (anim)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        yield return null;
        yield return null;

        if (displayMode == Mode.Normal)
        {
            //List<string> newUrls = new List<string>();
            using (WebClient client = new WebClient())
            {
                string url = client.DownloadString("https://firebasestorage.googleapis.com/v0/b/mysocialmedia-1781d.appspot.com/o/TestImages.txt?alt=media&token=bf0886c9-b897-4231-b3c8-73474d91abc9");
                url += "\n" + client.DownloadString("https://inspirobot.me/api?generate=true");
                urls = url.Replace("\r", "").Split('\n');
            }
            for (int i = 0; i < urls.Length; i++)
            {
                GameObject senasteInlägg = Instantiate(inlägg, transform.position, transform.rotation);
                senasteInlägg.transform.parent = transform;
                //RectTransform child = senasteInlägg.GetComponent<RectTransform>();
                //child.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 500f);
                senasteInlägg.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        if (displayMode == Mode.Profile)
        {
            for (int i = gameObject.transform.childCount; i < urls.Length; i++)
            {
                GameObject senasteInlägg = Instantiate(inlägg, transform.position, transform.rotation);
                senasteInlägg.transform.parent = transform;
                //RectTransform child = senasteInlägg.GetComponent<RectTransform>();
                //child.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 500f);
                senasteInlägg.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        //int amountOfKids = gameObject.transform.childCount;
        //rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 320 * amountOfKids);           "https://cdn.glitch.com/360f5555-e790-490c-9f53-9625be6a98f5%2FLukas.jpg?v=1605209661970"
        int prevColor = 0;
        int currentColor = Random.Range(0, uicolors.theUiColors.Length);
        if (sortMode == Sort.FromTop)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                while (prevColor == currentColor)
                {
                    currentColor = Random.Range(0, uicolors.theUiColors.Length);
                }
                prevColor = currentColor;
                if (i >= urls.Length)
                {
                    transform.GetChild(i).GetComponent<SetInläggSize>().SetStuff(uicolors.theUiColors[currentColor], displayMode);
                }
                else
                {
                    //if (sortMode == Sort.FromTop)
                    //{
                    transform.GetChild(i).GetComponent<SetInläggSize>().SetStuff(uicolors.theUiColors[currentColor], urls[/*urls.Length - 1 - */i], displayMode);
                    //}
                    //else if (sortMode == Sort.FromBottom)
                    //{
                    //transform.GetChild(i).GetComponent<SetInläggSize>().SetStuff(uicolors.theUiColors[currentColor], urls[urls.Length - 1 - i], displayMode);
                    //}
                }
                while (!transform.GetChild(i).GetComponent<SetInläggSize>().finished)
                {
                    yield return null;
                }
            }
        }
        else if (sortMode == Sort.FromBottom)
        {
            for (int i = gameObject.transform.childCount - 1; i > -1; i--)
            {
                while (prevColor == currentColor)
                {
                    currentColor = Random.Range(0, uicolors.theUiColors.Length);
                }
                prevColor = currentColor;
                if (gameObject.transform.childCount - 1 - i >= urls.Length)
                {
                    transform.GetChild(i).GetComponent<SetInläggSize>().SetStuff(uicolors.theUiColors[currentColor], displayMode);
                }
                else
                {
                    //if (sortMode == Sort.FromTop)
                    //{
                    transform.GetChild(i).GetComponent<SetInläggSize>().SetStuff(uicolors.theUiColors[currentColor], urls[gameObject.transform.childCount - 1 - i], displayMode);
                    //}
                    //else if (sortMode == Sort.FromBottom)
                    //{
                    //transform.GetChild(i).GetComponent<SetInläggSize>().SetStuff(uicolors.theUiColors[currentColor], urls[urls.Length - 1 - i], displayMode);
                    //}
                }
                while (!transform.GetChild(i).GetComponent<SetInläggSize>().finished)
                {
                    yield return null;
                }
            }
        }
        if (ScreenLoaded != null)
        {
            yield return null;
            //Debug.Log("Event started");
            ScreenLoaded.Invoke();
        }
        StartCoroutine(setSize(false, anim));
    }

    public void Loaded()
    {
        loading = false;
        StartCoroutine(setSize(false, false));
    }

    public void setLike()
    {
        if (gameObject.transform.childCount != 0 && displayMode == Mode.Normal)
        {
            Debug.Log("Update like");
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<SetInläggSize>().setLike();
            }
        }
    }

    private IEnumerator setSize(bool StartEvent, bool anim)
    {
        while (true)
        {
            int i;
            for (i = 0; i < gameObject.transform.childCount; i++)
            {
                if (!transform.GetChild(i).GetComponent<SetInläggSize>().finished)
                {
                    break;
                }
            }
            if (i < gameObject.transform.childCount)
            {
                if (!transform.GetChild(i).GetComponent<SetInläggSize>().finished)
                {
                    //loadingScreen.transform.rotation = Quaternion.Euler(loadingScreen.transform.rotation.x, loadingScreen.transform.rotation.y, loadingScreen.transform.rotation.z + 50 * Time.deltaTime);
                    yield return null;
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }
        yield return null;
        if (displayMode == Mode.Normal)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 9000);
            height = 0;
            //yield return new WaitForSecondsRealtime(1);
            yield return null;
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                height += transform.GetChild(i).GetComponent<Image>().rectTransform.sizeDelta.y;
                height += 20;
            }
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
            rectTransform.localPosition = new Vector2(0, 0);
            //for (int i = 0; i < 5; i++)
            //{
            //    yield return null;
            //}
            screenStartPos = -rectTransform.anchoredPosition.y;
        }
        if (ScreenLoaded != null && StartEvent)
        {
            yield return null;
            Debug.Log("Event started");
            ScreenLoaded.Invoke();
        }
        if (displayMode == Mode.Normal)
        {
            if (anim && loadingScreen != null)
            {
                loadingScreen.SetBool("Loading", false);
            }
            loading = false;
        }
        Debug.Log("Applicaton loaded");
    }

    private void Update()
    {
        if (!loading && displayMode == Mode.Normal && Time.time > 5)
        {
            timeSinceLoad += Time.deltaTime;
            if (!loading && rectTransform.anchoredPosition.y + 500 < screenStartPos && timeSinceLoad > 5 && Application.internetReachability != NetworkReachability.NotReachable)
            {
                if (loadingScreen != null)
                {
                    if (!loadingScreen.GetBool("Loading"))
                    {
                        Debug.Log("Reload");
                        Reload(true);
                    }
                }
            }
        }
        else
        {
            timeSinceLoad = 0;
        }
    }

    public void ScrollUp()
    {
        StartCoroutine(ScrollUpRoutine());
    }

    private IEnumerator ScrollUpRoutine()
    {
        float yVelocity = 0.0f;
        float smoothTime = 0.2f;
        ScrollRect scroll = transform.parent.GetComponent<ScrollRect>();
        scroll.enabled = false;
        while (rectTransform.anchoredPosition.y - 10 > screenStartPos)
        {
            float amountToMoveY = Mathf.SmoothDamp(rectTransform.anchoredPosition.y, screenStartPos, ref yVelocity, smoothTime);
            //float amountToMoveX = Mathf.SmoothDamp(image.transform.localPosition.x, FirePoint.transform.position.x, ref xVelocity, smoothTime);
            rectTransform.anchoredPosition = new Vector3(rectTransform.anchoredPosition.x, amountToMoveY);
            //Debug.Log(Vector2.Distance(new Vector2(0, 2402), image.transform.localPosition));

            yield return null;
        }
        scroll.enabled = true;
    }
}