using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private RectTransform loadingBar;
    [SerializeField] private TMP_Text downloadSizeText;

    [SerializeField] Scene s;

    private IEnumerator Start()
    {
        loadingBar.anchorMax = new Vector2(0, 1);

        var loadDownloadSize = Addressables.GetDownloadSizeAsync("enemy");
        yield return loadDownloadSize;
        var downloadSize = Mathf.FloorToInt(loadDownloadSize.Result / 1024f);

        var loadHandle = Addressables.DownloadDependenciesAsync("enemy", true);

        while (!loadHandle.IsDone)
        {
            downloadSizeText.text = $"{Mathf.FloorToInt(loadHandle.GetDownloadStatus().DownloadedBytes / 1024f)} / {downloadSize}"; ;
            // Barre de chargement basée sur le PercentComplete mis à disposition par Unity
            loadingBar.anchorMax = new Vector2(Mathf.Lerp(0, 1, loadHandle.PercentComplete), 1);
            yield return null;
        }

        loadingBar.anchorMax = Vector2.one;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    private void Update()
    {
        loadingText.color = new Color(1, 1, 1, Mathf.PingPong(Time.time * 2f, 1));
    }
}