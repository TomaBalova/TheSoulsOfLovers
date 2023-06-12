using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour, IDataPersistence
{
    private GameData gameData;

    [Header("General")]
    public DataPersistenceManager dataPersistenceManager;
    public GameObject mainMenu, settings;
    public Button LoadGameButton;

    [Header("Volume settings")]
    [SerializeField] private TextMeshProUGUI volumePercent = null;
    [SerializeField] private Slider volumeSlider = null;

    [Header("Graphics settings")]
    [SerializeField] private TextMeshProUGUI brightnessPercent = null;
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private Toggle fullScreenToggle = null;
    [SerializeField] private PostProcessProfile postProcessProfile;

    [Header("Resolution settings")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Resolution[] resolutions;

    private bool defIsFullScreen;
    private float defBrightnessLevel;
    private float defVolumeLevel;
    private float stoppedTime = 0.000000000001f;
    private int defResolutionInd = 0;
    private AutoExposure exposure;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;


        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                defResolutionInd = i;
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = defResolutionInd;
        resolutionDropdown.RefreshShownValue();

        postProcessProfile.TryGetSettings(out exposure);

        
        if (UnityEngine.PlayerPrefs.GetFloat("MusicVolume", -1) != -1)
            SetVolume(UnityEngine.PlayerPrefs.GetFloat("MusicVolume"));

        if (UnityEngine.PlayerPrefs.GetFloat("Brightness", -1) != -1) 
            SetBrightness(UnityEngine.PlayerPrefs.GetFloat("Brightness"));

        if (UnityEngine.PlayerPrefs.GetInt("FullScreen", -1) != -1)
            SetFullScreen((UnityEngine.PlayerPrefs.GetInt("FullScreen")==1 ? true : false));

        if (UnityEngine.PlayerPrefs.GetInt("Resolution", -1) != -1)
            SetResolution(UnityEngine.PlayerPrefs.GetInt("Resolution"));
    }

    private void Update()
    {
        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null && Input.GetAxis("Vertical") != 0)
        {
            if (mainMenu != null && mainMenu.activeInHierarchy) SelectFirstButton(mainMenu.transform);
            if (settings != null && settings.activeInHierarchy) SelectFirstButton(settings.transform);
        }
    }
    public void BackToMainMenu()
    {
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void StartNewGame()
    {
        gameData = new GameData();
        dataPersistenceManager.SaveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }
    public void LoadGame()
    {

        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    public void OpenCloseSettings()
    {
        if (!settings.activeInHierarchy)
        {
            settings.SetActive(true);
            mainMenu.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(true);
            settings.SetActive(false);
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }


    public void SetVolume(float value)
    {
        AudioListener.volume = (float)value / 100;
        volumePercent.text = value.ToString() + "%";
        if (volumeSlider.value != value)
            volumeSlider.value = value;
    }
    public void SetBrightness(float brightness)
    {
        exposure.keyValue.value = (float)brightness / 100;
        brightnessPercent.text = brightness.ToString() + "%";
        if (brightnessSlider.value != brightness)
            brightnessSlider.value = brightness;
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        if (fullScreenToggle.isOn != isFullScreen)
            fullScreenToggle.isOn = isFullScreen;
    }
    public void SetResolution(int resInd)
    {
        if (resInd >= resolutions.Length || resInd < 0)
            return;
        Resolution res = resolutions[resInd];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        if (resolutionDropdown.value != resInd)
            resolutionDropdown.value = resInd;
    }

    public void ResetSettings()
    {
        SetVolume(defVolumeLevel);
        SetBrightness(defBrightnessLevel);
        SetFullScreen(defIsFullScreen);
        SetResolution(defResolutionInd);
    }

    public void SaveSettings()
    {
        defVolumeLevel = volumeSlider.value;
        defBrightnessLevel = brightnessSlider.value;
        defIsFullScreen = fullScreenToggle.isOn;
        defResolutionInd = resolutionDropdown.value;

        if(UnityEngine.PlayerPrefs.GetFloat("MusicVolume", -1) != defVolumeLevel)
            UnityEngine.PlayerPrefs.SetFloat("MusicVolume", defVolumeLevel);

        if (UnityEngine.PlayerPrefs.GetFloat("Brightness", -1) != defBrightnessLevel)
            UnityEngine.PlayerPrefs.SetFloat("Brightness", defBrightnessLevel);

        if (UnityEngine.PlayerPrefs.GetInt("FullScreen", -1) != (defIsFullScreen ? 1 : 0))
            UnityEngine.PlayerPrefs.SetInt("FullScreen", (defIsFullScreen ? 1 : 0));

        if (UnityEngine.PlayerPrefs.GetInt("Resolution", -1) != defResolutionInd)
            UnityEngine.PlayerPrefs.SetInt("Resolution", defResolutionInd);

        OpenCloseSettings();
    }


    public void LoadData(GameData gameData)
    {
        this.gameData = gameData;
        GameData emptyGD = new GameData();

        if (LoadGameButton == null)
        {
            List<Button> btns = returnButtonsInPage(mainMenu.transform).ToList();
            LoadGameButton = btns.Find(x => x.name == "LoadGameBtn");
        }

        if (GameData.GetJsonFromGameData(this.gameData) == GameData.GetJsonFromGameData(emptyGD))
            LoadGameButton.interactable = false;
        else
            LoadGameButton.interactable = true;
    }
    public void SaveData(ref GameData gameData)
    {
        gameData = this.gameData;
    }


    private Button[] returnButtonsInPage(Transform page)
    {
        Transform folderWithBtns = page.Find("Buttons");
        if (folderWithBtns == null)
            return null;
        return folderWithBtns.GetComponentsInChildren<Button>(true);
    }
    public void SelectFirstButton(Transform page)
    {
        Button[] btns = returnButtonsInPage(page);
        if (btns[0].interactable)
            btns[0].Select();
        else
            btns[1].Select();
    }

    public List<string> getListOfScenes()
    {
        var sceneNumber = SceneManager.sceneCountInBuildSettings;
        string[] arrayOfNames;
        arrayOfNames = new string[sceneNumber];
        for (int i = 0; i < sceneNumber; i++)
        {
            arrayOfNames[i] = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
        }
        return arrayOfNames.ToList();
    }
}