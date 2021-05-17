using Firebase;
using Firebase.Analytics;
using Firebase.Auth;
using Firebase.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FirebaseInit : MonoBehaviour
{
    private TMP_InputField regEmail;
    private TMP_InputField regPassword;
    private TMP_InputField regVerifiedPassword;

    private TMP_InputField loginEmail;
    private TMP_InputField loginPassword;

    [SerializeField] private Image verify;
    [SerializeField] private Image theEmail;
    [SerializeField] private Image thePassword;

    [SerializeField] private Image theLoginEmail;
    [SerializeField] private Image theLoginPassword;

    [SerializeField] private GameObject EmailInUse;
    [SerializeField] private GameObject LoginError;

    [SerializeField] private GameObject loginButton;
    [SerializeField] private GameObject logOutButton;

    [SerializeField] private SetHeight feed;
    [SerializeField] private SetHeight profile;
    [SerializeField] private GameObject profileMenu;
    [SerializeField] private Animator loadingScreen;
    [SerializeField] private GameObject reloadButton;

    [SerializeField] private GameObject loading;
    private Texture2D texture;

    [SerializeField] private Texture2D defaultTexture;
    private string uploadImageTitle;

    // Start is called before the first frame update
    private void Awake()
    {
        StartCoroutine(LoadEverything());
    }

    public IEnumerator LoadEverything()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            reloadButton.SetActive(false);

            var dependencyStatus = FirebaseApp.CheckAndFixDependenciesAsync();
            yield return new WaitUntil(() => dependencyStatus.IsCompleted);
            //if (task == DependencyStatus.Available)
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

            regEmail = theEmail.GetComponent<TMP_InputField>();
            regPassword = thePassword.GetComponent<TMP_InputField>();
            regVerifiedPassword = verify.GetComponent<TMP_InputField>();

            loginEmail = theLoginEmail.GetComponent<TMP_InputField>();
            loginPassword = theLoginPassword.GetComponent<TMP_InputField>();

            StartCoroutine(fixDependencies());
        }
        else
        {
            Debug.Log("Error. Check internet connection!");
            reloadButton.SetActive(true);
        }
    }

    private async void ActualyFixDependencies()
    {
        var dependencyResult = FirebaseApp.CheckAndFixDependenciesAsync();
        //if (dependencyResult == DependencyStatus.Available)
        //{
        //}
        if (dependencyResult.IsFaulted || dependencyResult.Exception != null)
        {
            Debug.Log("Authentication failed: " + dependencyResult.Exception);
        }
    }

    private IEnumerator fixDependencies()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventTutorialBegin);
        // = FirebaseApp.CheckAndFixDependenciesAsync();
        //yield return new WaitUntil(() => dependencyResult.IsCompleted);f

        Debug.Log("Authentication online");

        checkIfLoggedIn();
        //if (dependencyResult == DependencyStatus.Available)
        //{
        //    app
        //}
        profileMenu.SetActive(true);
        loadingScreen.SetBool("Loading", true);
        profile.Reload(false);
        //while (profile.loading || feed.loading)
        //{
        //}
        yield return null;
        feed.Reload(false);
        loading.SetActive(true);
        while (profile.loading || feed.loading)
        {
            yield return null;
        }
        yield return null;
        profileMenu.SetActive(false);
        loadingScreen.SetBool("Loading", false);
    }

    public void search(string text)
    {
        Debug.Log("Searched");
        Firebase.Analytics.FirebaseAnalytics.LogEvent(
  Firebase.Analytics.FirebaseAnalytics.EventSearch,
  new Firebase.Analytics.Parameter[] {
    new Firebase.Analytics.Parameter(
      Firebase.Analytics.FirebaseAnalytics.ParameterSearchTerm, text),
  }
);
    }

    public void registerNewUser(Menu menu)
    {
        StartCoroutine(RegisterUser(menu));
    }

    private IEnumerator RegisterUser(Menu menu)
    {
        if (regEmail.text == null || regEmail.text == "")
        {
            theEmail.color = new Color(1, 0.5f, 0.5f, /*Mathf.Abs*/1);
        }
        if (regPassword.text == null || regPassword.text == "")
        {
            thePassword.color = new Color(1, 0.5f, 0.5f, /*Mathf.Abs*/1);
            verify.color = new Color(1, 0.5f, 0.5f, /*Mathf.Abs*/1);
        }
        if (regEmail.text != null && regPassword.text != null && regPassword.text == regVerifiedPassword.text && regEmail.text != "" && regPassword.text != "")
        {
            var auth = FirebaseAuth.DefaultInstance;
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(regEmail.text, regPassword.text);
            yield return new WaitUntil(() => registerTask.IsCompleted);
            if (registerTask.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
            }
            else if (registerTask.IsFaulted)
            {
                theEmail.color = new Color(1, 0.5f, 0.5f, /*Mathf.Abs*/1);
                EmailInUse.SetActive(true);

                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + registerTask.Exception);
            }
            else
            {
                // Firebase user has been created.
                Firebase.Auth.FirebaseUser newUser = registerTask.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1}), {2}",
                    newUser.DisplayName, newUser.UserId, newUser.Email);
                menu.StepBack();
                using (WebClient client = new WebClient())
                {
                    string users = client.DownloadString(Link("users"));
                    StartCoroutine(UploadDocRoutine(users + "\n" + newUser.UserId, "users"));
                }
            }
        }
        checkIfLoggedIn();
    }

    public void setPassword(string text)
    {
        //regPassword.text = text;
        if (regPassword.text != null && regPassword.text != "")
        {
            thePassword.color = new Color(1, 1, 1, /*Mathf.Abs*/1);
        }
        if (regPassword.text != regVerifiedPassword.text)
        {
            verify.color = new Color(1, 0.5f, 0.5f, /*Mathf.Abs*/1);
        }
        else
        {
            verify.color = new Color(1, 1, 1, /*Mathf.Abs*/1);
        }
    }

    public void setVerify(string text)
    {
        //regVerifiedPassword.text = text;
        if (regPassword.text != regVerifiedPassword.text)
        {
            verify.color = new Color(1, 0.5f, 0.5f, /*Mathf.Abs*/1);
        }
        else
        {
            verify.color = new Color(1, 1, 1, /*Mathf.Abs*/1);
        }
    }

    public void setEmail(string text)
    {
        //regEmail.text = text;
        if (regEmail.text != null && regEmail.text != "")
        {
            theEmail.color = new Color(1, 1, 1, /*Mathf.Abs*/1);
            EmailInUse.SetActive(false);
        }
    }

    public bool checkIfLoggedIn()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            loginButton.SetActive(false);
            logOutButton.SetActive(true);
            Debug.Log("Logged in" + FirebaseAuth.DefaultInstance.CurrentUser.DisplayName);
            return true;
        }
        else
        {
            loginButton.SetActive(true);
            logOutButton.SetActive(false);
            return false;
        }
    }

    public void setEmailLogin(string text)
    {
        //loginEmail.text = text;

        if (loginEmail.text != null)
        {
            LoginError.SetActive(false);
            theLoginEmail.color = new Color(1, 1, 1, /*Mathf.Abs*/1);
        }
    }

    public void setPasswordLogin(string text)
    {
        //loginPassword.text = text;

        if (loginPassword.text != null && loginPassword.text != "")
        {
            LoginError.SetActive(false);
            theLoginPassword.color = new Color(1, 1, 1, /*Mathf.Abs*/1);
        }
    }

    public void logOut()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        checkIfLoggedIn();
    }

    public void startLogin(Menu menu)
    {
        StartCoroutine(logIn(menu));
    }

    private IEnumerator logIn(Menu menu)
    {
        if (loginEmail == null || loginEmail.text == "")
        {
            theLoginEmail.color = new Color(1, 0.5f, 0.5f, /*Mathf.Abs*/1);
        }
        if (loginPassword == null || loginPassword.text == "")
        {
            theLoginPassword.color = new Color(1, 0.5f, 0.5f, /*Mathf.Abs*/1);
        }
        if (loginEmail != null && loginPassword != null && loginEmail.text != "" && loginPassword.text != "")
        {
            var auth = FirebaseAuth.DefaultInstance;
            var loginTask = auth.SignInWithEmailAndPasswordAsync(loginEmail.text, loginPassword.text);
            yield return new WaitUntil(() => loginTask.IsCompleted);
            if (loginTask.Exception != null)
            {
                Debug.LogWarning("Login failed: " + loginTask.Exception);
                LoginError.SetActive(true);
            }
            else
            {
                checkIfLoggedIn();

                Debug.Log("Login succeded with" + loginTask.Result.Email);
                menu.StepBack();
            }
        }
        checkIfLoggedIn();
    }

    public void resetLogins()
    {
        theLoginPassword.color = new Color(1, 1, 1, /*Mathf.Abs*/1);
        theLoginEmail.color = new Color(1, 1, 1, /*Mathf.Abs*/1);
        verify.color = new Color(1, 1, 1, /*Mathf.Abs*/1);
        thePassword.color = new Color(1, 1, 1, /*Mathf.Abs*/1);
        theEmail.color = new Color(1, 1, 1, /*Mathf.Abs*/1);

        regEmail.text = null;
        regPassword.text = null;
        regVerifiedPassword.text = null;

        loginEmail.text = null;
        loginPassword.text = null;
        LoginError.SetActive(false);
        EmailInUse.SetActive(false);
    }

    public void ChooseImage(SetInläggSize setInläggSize)
    {
        if (checkIfLoggedIn())
        {
            string filePath;
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
            {
                Debug.Log("Image path: " + path);
                if (path != null)
                {
                    // Create Texture from selected image
                    texture = NativeGallery.LoadImageAtPath(path, 5000);
                    if (texture == null)
                    {
                        Debug.Log("Couldn't load texture from " + path);
                        return;
                    }
                    Debug.Log("Loaded texture from: " + path);
                    setInläggSize.SetStuff(new Color(0.1176471f, 0.1529412f, 0.1803922f, 0), texture, SetHeight.Mode.Upload);
                    //// Assign texture to a temporary quad and destroy it after 5 seconds
                    //GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    //quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
                    //quad.transform.forward = Camera.main.transform.forward;
                    //quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

                    //Material material = quad.GetComponent<Renderer>().material;
                    //if (!material.shader.isSupported) // happens when Standard shader is not included in the build
                    //    material.shader = Shader.Find("Legacy Shaders/Diffuse");

                    //material.mainTexture = texture;

                    //Destroy(quad, 5f);

                    //// If a procedural texture is not destroyed manually,
                    //// it will only be freed after a scene change
                    //Destroy(texture, 5f);
                    //Firebase.Storage.StorageReference storage_ref = storage.Reference();

                    //Firebase.Storage.StorageReference rivers_ref = storage_ref.Child(FirebaseAuth.DefaultInstance.CurrentUser.DisplayName+"/rivers.jpg");

                    // Upload the file to the path "images/rivers.jpg"

                    // Metadata contains file metadata such as size, content-type, and download URL.
                    //Firebase.Storage.StorageMetadata metadata = task.Result;
                    //string download_url = metadata.DownloadUrl.ToString();
                    //Debug.Log("Finished uploading...");
                    //Debug.Log("download url = " + download_url);
                }
            });
            Debug.Log("Permission result: " + permission);
        }
    }

    public void UploadImage()
    {
        if (texture != null && uploadImageTitle != "" && uploadImageTitle != null)
        {
            StartCoroutine(UploadImageRoutine(uploadImageTitle));
        }
    }

    public void UpdateTitle(string text)
    {
        uploadImageTitle = text;
    }

    public void onLoadUpload(SetInläggSize setInläggSize)
    {
        setInläggSize.SetStuff(new Color(0.1176471f, 0.1529412f, 0.1803922f, 0), defaultTexture, SetHeight.Mode.Upload);
        texture = null;
    }

    private IEnumerator UploadImageRoutine(string text)
    {
        Firebase.Storage.FirebaseStorage storage = Firebase.Storage.FirebaseStorage.DefaultInstance;
        //Firebase.Storage.StorageReference storage_ref = storage.GetReferenceFromUrl("/" + FirebaseAuth.DefaultInstance.CurrentUser.DisplayName + "/" + Guid.NewGuid() + ".png");
        //Firebase.Storage.StorageReference user_ref = storage_ref.Child(FirebaseAuth.DefaultInstance.CurrentUser.DisplayName);
        string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        string savePath = $"gs://mysocialmedia-1781d.appspot.com/{userId}/pictures/{Guid.NewGuid()}.png";
        Debug.Log(savePath);
        Firebase.Storage.StorageReference storage_ref = storage.GetReferenceFromUrl(savePath);
        var bytes = duplicateTexture(texture).EncodeToPNG();

        var metadataChange = new MetadataChange()
        {
            ContentEncoding = "image/png",
            CustomMetadata = new Dictionary<string, string>()
            {
                {"Titeln", text }
            },
        };
        var uploadTask = storage_ref.PutBytesAsync(bytes, metadataChange);
        yield return new WaitUntil(() => uploadTask.IsCompleted);
        if (uploadTask.Exception != null)
        {
            Debug.LogError("Failed to upload: " + uploadTask.Exception);
        }
        else
        {
            Debug.Log("Verkar onekligen ha fungerat");
            /*using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
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
            }*/
        }
    }

    public string Link(string path)
    {
        Firebase.Storage.FirebaseStorage storage = Firebase.Storage.FirebaseStorage.DefaultInstance;
        Firebase.Storage.StorageReference storage_ref = storage.GetReference(path);
        return storage_ref.GetDownloadUrlAsync().Result.AbsoluteUri;
    }

    private IEnumerator UploadDocRoutine(string text, string path)
    {
        Firebase.Storage.FirebaseStorage storage = Firebase.Storage.FirebaseStorage.DefaultInstance;
        //Firebase.Storage.StorageReference storage_ref = storage.GetReferenceFromUrl("/" + FirebaseAuth.DefaultInstance.CurrentUser.DisplayName + "/" + Guid.NewGuid() + ".png");
        //Firebase.Storage.StorageReference user_ref = storage_ref.Child(FirebaseAuth.DefaultInstance.CurrentUser.DisplayName);
        string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        string savePath = $"gs://mysocialmedia-1781d.appspot.com/{path}.txt";
        Debug.Log(savePath);
        Firebase.Storage.StorageReference storage_ref = storage.GetReferenceFromUrl(savePath);
        var bytes = Encoding.Unicode.GetBytes(text);

        var metadataChange = new MetadataChange()
        {
            ContentEncoding = "text/plain",
            CustomMetadata = new Dictionary<string, string>()
            {
                {"Titeln",text.Length < 30? text.Split()[0]: text.Substring(0,30).Split()[0] }
            },
        };
        var uploadTask = storage_ref.PutBytesAsync(bytes, metadataChange);
        yield return new WaitUntil(() => uploadTask.IsCompleted);
        if (uploadTask.Exception != null)
        {
            Debug.LogError("Failed to upload: " + uploadTask.Exception);
        }
        else
        {
            Debug.Log("Verkar onekligen ha fungerat");
            /*using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
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
            }*/
        }
    }

    private Texture2D duplicateTexture(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
}