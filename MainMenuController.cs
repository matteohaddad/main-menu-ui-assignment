using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    public UIDocument levelSelectDoc;
    public UIDocument settingsDoc;
    public UIDocument helpDoc;  // 👈 Add this in the Inspector!

    private VisualElement mainRoot;
    private VisualElement levelRoot;
    private VisualElement settingsRoot;
    private VisualElement helpRoot;

    private void OnEnable()
    {
        // Initialize root UIs
        mainRoot = GetComponent<UIDocument>().rootVisualElement;
        levelRoot = levelSelectDoc.rootVisualElement;
        settingsRoot = settingsDoc.rootVisualElement;
        helpRoot = helpDoc.rootVisualElement;

        // Hide all secondary screens
        levelRoot.style.display = DisplayStyle.None;
        settingsRoot.style.display = DisplayStyle.None;
        helpRoot.style.display = DisplayStyle.None;

        // Main menu buttons
        mainRoot.Q<Button>("levelSelectButton")?.RegisterCallback<ClickEvent>(_ => ShowLevelSelect());
        mainRoot.Q<Button>("settingsButton")?.RegisterCallback<ClickEvent>(_ => ShowSettings());
        mainRoot.Q<Button>("helpButton")?.RegisterCallback<ClickEvent>(_ => ShowHelp());
        mainRoot.Q<Button>("quitButton")?.RegisterCallback<ClickEvent>(_ => OnQuit());

        // Level select buttons
        levelRoot.Q<Button>("level1Button")?.RegisterCallback<ClickEvent>(_ => LoadLevel("level1"));
        levelRoot.Q<Button>("level2Button")?.RegisterCallback<ClickEvent>(_ => LoadLevel("level2"));
        levelRoot.Q<Button>("backButton")?.RegisterCallback<ClickEvent>(_ => ShowMainMenu());

        // Settings back
        settingsRoot.Q<Button>("settingsBackButton")?.RegisterCallback<ClickEvent>(_ => ShowMainMenu());

        // Help back
        helpRoot.Q<Button>("helpBackButton")?.RegisterCallback<ClickEvent>(_ => ShowMainMenu());

        // Fullscreen toggle
        var fullscreenToggle = settingsRoot.Q<Toggle>("fullscreenToggle");
        if (fullscreenToggle != null)
        {
            fullscreenToggle.value = Screen.fullScreen;
            fullscreenToggle.RegisterValueChangedCallback(evt =>
            {
                Screen.fullScreen = evt.newValue;
                Debug.Log("Fullscreen set to: " + Screen.fullScreen);
            });
        }

        // Volume slider
        var volumeSlider = settingsRoot.Q<Slider>("volumeSlider");
        if (volumeSlider != null)
        {
            volumeSlider.lowValue = 0;
            volumeSlider.highValue = 100;
            volumeSlider.value = AudioListener.volume * 100f;

            volumeSlider.RegisterValueChangedCallback(evt =>
            {
                float volume = evt.newValue / 100f;
                AudioListener.volume = volume;
                Debug.Log("Volume set to: " + volume);
            });
        }
    }

    private void ShowMainMenu()
    {
        levelRoot.style.display = DisplayStyle.None;
        settingsRoot.style.display = DisplayStyle.None;
        helpRoot.style.display = DisplayStyle.None;
        mainRoot.style.display = DisplayStyle.Flex;
    }

    private void ShowLevelSelect()
    {
        mainRoot.style.display = DisplayStyle.None;
        settingsRoot.style.display = DisplayStyle.None;
        helpRoot.style.display = DisplayStyle.None;
        levelRoot.style.display = DisplayStyle.Flex;
    }

    private void ShowSettings()
    {
        mainRoot.style.display = DisplayStyle.None;
        levelRoot.style.display = DisplayStyle.None;
        helpRoot.style.display = DisplayStyle.None;
        settingsRoot.style.display = DisplayStyle.Flex;
        Debug.Log("Settings Opened");
    }

    private void ShowHelp()
    {
        Debug.Log("Help screen opened");
        mainRoot.style.display = DisplayStyle.None;
        levelRoot.style.display = DisplayStyle.None;
        settingsRoot.style.display = DisplayStyle.None;
        helpRoot.style.display = DisplayStyle.Flex;
    }

    private void LoadLevel(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
            SceneManager.LoadScene(sceneName);
        else
            Debug.LogWarning($"Scene '{sceneName}' not found or not added to build settings.");
    }

    private void OnQuit()
    {
        Debug.Log("Quit requested");
        Application.Quit();
    }
}
