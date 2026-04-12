using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using Yarn.Unity;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement Instance { get; private set; }

    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text progressText;

    [Header("Settings")]
    [Tooltip("Minimum time (seconds) the loading screen stays visible.")]
    [SerializeField] private float minimumLoadTime = 1f;

    [Header("Testing")]
    [Tooltip("Enable to allow test scene loading with a key press.")]
    [SerializeField] private bool enableTestMode = false;
    [Tooltip("Key to press to trigger the test scene load.")]
    [SerializeField] private Key testKey = Key.L;
    [Tooltip("Scene name to load when the test key is pressed.")]
    [SerializeField] public int sceneIndex;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }

    #region Testing mode

    private void Update()
    {
        if (enableTestMode && Keyboard.current != null && Keyboard.current[testKey].wasPressedThisFrame)
        {
            if (sceneIndex >= 0)
            {
                Debug.Log($"[SceneManagement] Test loading scene index: {sceneIndex}");
                TestLoadScene();
            }
            else
            {
                Debug.LogWarning("[SceneManagement] Test scene index is invalid.");
            }
        }
    }

    void TestLoadScene()
    {
        //increment it by 1 to load the next scene in the build settings, or set it to a specific index for testing
        sceneIndex++;

        // Wrap around if it exceeds the total number of scenes in build settings
        if (sceneIndex >= 0)
        {
            //get total number of scenes in build settings
            int totalScenes = SceneManager.sceneCountInBuildSettings;
            if (sceneIndex >= totalScenes)
            {
                sceneIndex = 0; // Reset to first scene if we exceed the count
            }
        }

        if (sceneIndex >= 0)
        {
            Debug.Log($"[SceneManagement] Test loading scene index: {sceneIndex}");
            LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogWarning("[SceneManagement] Test scene index is invalid.");
        }
    }

    #endregion Testing mode

    #region Yarn commands

    // Usage in Yarn: <<load_scene "SceneName">>
    [YarnCommand("load_scene")]
    public static void YarnLoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("[SceneManagement] Yarn command load_scene called with empty scene name.");
            return;
        }

        if (Instance == null)
        {
            Debug.LogError("[SceneManagement] No SceneManagement instance available to load scene.");
            return;
        }

        Debug.Log($"[SceneManagement] Yarn requested loading scene: {sceneName}");
        Instance.LoadScene(sceneName);
    }

    // Usage in Yarn: <<load_scene_index "2">>  (index passed as string)
    [YarnCommand("load_scene_index")]
    public static void YarnLoadSceneIndex(string indexString)
    {
        if (string.IsNullOrEmpty(indexString))
        {
            Debug.LogWarning("[SceneManagement] Yarn command load_scene_index called with empty index.");
            return;
        }

        if (!int.TryParse(indexString, out int index))
        {
            Debug.LogWarning($"[SceneManagement] load_scene_index expects an integer index, got '{indexString}'.");
            return;
        }

        if (Instance == null)
        {
            Debug.LogError("[SceneManagement] No SceneManagement instance available to load scene by index.");
            return;
        }

        Debug.Log($"[SceneManagement] Yarn requested loading scene index: {index}");
        Instance.LoadScene(index);
    }

    #endregion Yarn commands

    #region Exposed Methods

    /// <summary>
    /// Using two overloads to allow loading scenes by name or build index, both showing the loading screen and progress.
    /// </summary>
    /// <param name="sceneName"></param>
    // Loads a scene asynchronously by name, showing the loading screen.

    public void LoadScene(string sceneName)
    {
        Debug.Log($"[SceneManagement] Loading scene: {sceneName}");
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    // Loads a scene asynchronously by build index, showing the loading screen.
    public void LoadScene(int sceneIndex)
    {
        Debug.Log($"[SceneManagement] Loading scene index: {sceneIndex}");
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    #endregion Exposed Methods

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        yield return StartCoroutine(LoadSceneRoutine(SceneManager.LoadSceneAsync(sceneName)));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        yield return StartCoroutine(LoadSceneRoutine(SceneManager.LoadSceneAsync(sceneIndex)));
    }

    private IEnumerator LoadSceneRoutine(AsyncOperation operation)
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        operation.allowSceneActivation = false;

        float elapsed = 0f;

        while (!operation.isDone)
        {
            elapsed += Time.unscaledDeltaTime;

            // Unity reports progress up to 0.9, then waits for allowSceneActivation
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (progressBar != null)
                progressBar.value = progress;

            if (progressText != null)
                progressText.text = $"{progress * 100f:F0}%";

            // Activate scene once loading is done and minimum time has passed
            if (operation.progress >= 0.9f && elapsed >= minimumLoadTime)
            {
                if (progressBar != null)
                    progressBar.value = 1f;

                if (progressText != null)
                    progressText.text = "100%";

                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }
}
