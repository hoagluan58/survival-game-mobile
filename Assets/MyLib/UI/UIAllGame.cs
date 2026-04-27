using DG.Tweening;
using NFramework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIAllGame : SingletonMono<UIAllGame>
{
    private bool isLoading = false;
    [SerializeField] private Image _loadingScene = default;
    [SerializeField] private GameObject _preventTouchUI = default;


    private void OnEnable()
    {
        EventManager.OnGameInitDone += DoEndFade;
        EventManager.OnLoadingLoadScene += OnLoadingLoadScene;
    }

    private void OnDisable()
    {
        EventManager.OnGameInitDone -= DoEndFade;
        EventManager.OnLoadingLoadScene -= OnLoadingLoadScene;
    }

    private void OnLoadingLoadScene(bool isLoad)
    {
        isLoading = isLoad;
    }

    public void DisableTouchUI()
    {
        _preventTouchUI.gameObject.SetActive(true);
    }

    public void EnableTouchUI()
    {
        _preventTouchUI.gameObject.SetActive(false);
    }

    public void LoadScene(int id)
    {
        StartCoroutine(IELoadScene(id));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(CRLoadScene(sceneName));
    }

    public void DoEndFade()
    {
        _loadingScene.DOFade(0f, 0.25f)
        .OnComplete(() =>
        {
            _loadingScene.gameObject.SetActive(false);
        });
    }

    private IEnumerator CRLoadScene(string sceneName, bool isEnableTouchUI = true)
    {
        _loadingScene.gameObject.SetActive(true);

        var fadeDuration = 0.25f;
        var color = _loadingScene.color;
        color.a = 0f;
        _loadingScene.color = color;
        _loadingScene.DOFade(1f, fadeDuration);

        yield return new WaitForSeconds(fadeDuration);

        yield return SceneUtils.CRLoadSceneAsync(sceneName, true, true);
        if (isLoading)
        {
            isLoading = false;
            StartCoroutine(CRContinueLoad());
        }
        else
        {
            StartCoroutine(CRContinueLoad());
        }

        IEnumerator CRContinueLoad()
        {
            var endFadeDuration = 0.25f;
            DoEndFade();
            yield return new WaitForSeconds(endFadeDuration);
            if (isEnableTouchUI)
            {
                EnableTouchUI();
            }
        }
    }

    private AsyncOperation async;
    private IEnumerator IELoadScene(int id, bool isEnableTouchUI = true)
    {
        _loadingScene.gameObject.SetActive(true);
        Color color = _loadingScene.color;
        color.a = 0f;
        _loadingScene.color = color;
        _loadingScene.DOFade(1f, 0.25f);

        yield return new WaitForSeconds(0.25f);

        async = SceneManager.LoadSceneAsync(id);
        async.allowSceneActivation = false;

        yield return new WaitUntil(() => async.progress == 0.9f);

        // Debug.LogErrorFormat("+===============load: {0}", isLoading);
        if (isLoading)
        {
            isLoading = false;
            // Debug.LogErrorFormat("======has ad");
            async.allowSceneActivation = true;
            StartCoroutine(ContinueLoad(isEnableTouchUI));
        }
        else
        {
            // Debug.LogErrorFormat("======no ad");
            async.allowSceneActivation = true;
            StartCoroutine(ContinueLoad(isEnableTouchUI));
        }
    }

    IEnumerator ContinueLoad(bool isEnableTouchUI = true)
    {
        yield return new WaitUntil(() => async.isDone);
        DoEndFade();
        yield return new WaitForSeconds(0.25f);

        if (isEnableTouchUI)
            EnableTouchUI();
    }
}
