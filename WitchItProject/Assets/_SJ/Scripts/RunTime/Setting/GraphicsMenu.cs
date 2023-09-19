using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GraphicsMenu : MonoBehaviour
{

    //public Dropdown resolutionDropdown;
    //public Dropdown qualityDropdown;
    //public Dropdown textureDropdown;
    //public Dropdown aaDropdown;
    //public Slider volumeSlider;

    // 오디오 조절 
    public AudioMixer audioMixer;

    // FPS 출력 
    public TMP_Text fpsText;
    private RectTransform fpsRect = default;    
    private float width;
    private float height;
    private int size = 15;
    private Color color = Color.black;

    // 해상도 조절
    public Toggle fullScreenToggle;
    public TMP_Dropdown resoultionsDropdown;
    private List<Resolution> resolutions =  default;
    private int resolutionNum = default;
    private FullScreenMode screenMode; 
    //
    
    private void Start()
    {       
        Init();
    }

    private void Update()
    {
        SetFpsText();
    }

   public void Init()
    {

        resolutions = new List<Resolution>();
        fpsRect = fpsText.GetComponent<RectTransform>();

        // { 해상도 초기화 
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRate == 60)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }
        
        resoultionsDropdown.options.Clear();

        int optionNum = 0;

        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = item.width + "x" + item.height + " " + item.refreshRate + "hz";
            resoultionsDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
            {
                resoultionsDropdown.value = optionNum;
            }
            optionNum++;
        }
        resoultionsDropdown.RefreshShownValue();

        Screen.SetResolution(resolutions[resoultionsDropdown.value].width, resolutions[resoultionsDropdown.value].height,
         FullScreenMode.FullScreenWindow, 60);
        //fullScreenToggle.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
        // } 해상도 초기화 
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }
    public void SetFpsText()
    {
        // 게임중 해상도가 변경되면 그에 대응해서 위치 이동?? 
        // 다만 그런 경우 앵커로 해결 하면 되지 않나?
        width = Screen.width - (fpsRect.rect.width/2);
        height = Screen.height - (fpsRect.rect.height/2);
        Rect rect = new Rect(width, height, Screen.width,Screen.height);
        //

        float fps = 1.0f / Time.deltaTime;
        float ms = Time.deltaTime * 1000.0f;

        fpsText.text = string.Format("{0:N0} FPS {1:N1}ms", fps, ms);
        fpsText.fontSize = size;
        fpsText.color = color;

        fpsText.rectTransform.position = rect.position;
    }       // SetFpsText()

    #region DropDownOptionChange
    public void DropdownOptionChange(int idx)
    {
        resolutionNum = idx;
    }

    public void AnitiAliasingOptionChange(int idx)
    {
        resolutionNum = idx;
    }

    #endregion

    #region CheckBoxChange
    public void OnFpsText(bool isFps)
    {
        fpsText.enabled = isFps;
    }

    public void SetVsnc(bool isVsnc)
    {        
        QualitySettings.vSyncCount = isVsnc ? 1 : 0;
    }
    #endregion

    public void SetResoultion()
    {
        Screen.SetResolution(resolutions[resolutionNum].width,
        resolutions[resolutionNum].height, screenMode, resolutions[resolutionNum].refreshRate);
    }

   

    public void SetFullScreen(bool isFullScreen)
    {
        // 토글로 전체화면, 창모드 
        // 경계없는 창모드 설정하려면 드롭 다운으로 구현 추가 예정
        screenMode = isFullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;        
    }

    //public void SetVolume(float volume)
    //{
    //    audioMixer.SetFloat("Volume", volume);
    //    currentVolume = volume;
    //}
    //public void SetFullscreen(bool isFullscreen)
    //{
    //    Screen.fullScreen = isFullscreen;
    //}

    //public void SetQuality(int qualityIndex)
    //{
    //    if (qualityIndex != 6) // if the user is not using 
    //                           //any of the presets
    //        QualitySettings.SetQualityLevel(qualityIndex);
    //    switch (qualityIndex)
    //    {
    //        case 0: // quality level - very low
    //            textureDropdown.value = 3;
    //            aaDropdown.value = 0;
    //            break;
    //        case 1: // quality level - low
    //            textureDropdown.value = 2;
    //            aaDropdown.value = 0;
    //            break;
    //        case 2: // quality level - medium
    //            textureDropdown.value = 1;
    //            aaDropdown.value = 0;
    //            break;
    //        case 3: // quality level - high
    //            textureDropdown.value = 0;
    //            aaDropdown.value = 0;
    //            break;
    //        case 4: // quality level - very high
    //            textureDropdown.value = 0;
    //            aaDropdown.value = 1;
    //            break;
    //        case 5: // quality level - ultra
    //            textureDropdown.value = 0;
    //            aaDropdown.value = 2;
    //            break;
    //    }

    //    qualityDropdown.value = qualityIndex;
    //}


    //public void ExitGame()
    //{
    //    Application.Quit();
    //}
    //public void SaveSettings()
    //{
    //    PlayerPrefs.SetInt("QualitySettingPreference",
    //               qualityDropdown.value);
    //    PlayerPrefs.SetInt("ResolutionPreference",
    //               resolutionDropdown.value);
    //    PlayerPrefs.SetInt("TextureQualityPreference",
    //               textureDropdown.value);
    //    PlayerPrefs.SetInt("AntiAliasingPreference",
    //               aaDropdown.value);
    //    PlayerPrefs.SetInt("FullscreenPreference",
    //               Convert.ToInt32(Screen.fullScreen));
    //    PlayerPrefs.SetFloat("VolumePreference",
    //               currentVolume);
    //}

    //public void SetTextureQuality(int textureIndex)
    //{
    //    QualitySettings.masterTextureLimit = textureIndex;
    //    qualityDropdown.value = 6;
    //}
    //public void SetAntiAliasing(int aaIndex)
    //{
    //    QualitySettings.antiAliasing = aaIndex;
    //    qualityDropdown.value = 6;
    //}
    //public void SetResolution(int resolutionIndex)
    //{
    //    Resolution resolution = resolutions[resolutionIndex];
    //    Screen.SetResolution(resolution.width,
    //              resolution.height, Screen.fullScreen);
    //}
    //public void LoadSettings(int currentResolutionIndex)
    //{
    //    if (PlayerPrefs.HasKey("QualitySettingPreference"))
    //        qualityDropdown.value =
    //                     PlayerPrefs.GetInt("QualitySettingPreference");
    //    else
    //        qualityDropdown.value = 3;
    //    if (PlayerPrefs.HasKey("ResolutionPreference"))
    //        resolutionDropdown.value =
    //                     PlayerPrefs.GetInt("ResolutionPreference");
    //    else
    //        resolutionDropdown.value = currentResolutionIndex;
    //    if (PlayerPrefs.HasKey("TextureQualityPreference"))
    //        textureDropdown.value =
    //                     PlayerPrefs.GetInt("TextureQualityPreference");
    //    else
    //        textureDropdown.value = 0;
    //    if (PlayerPrefs.HasKey("AntiAliasingPreference"))
    //        aaDropdown.value =
    //                     PlayerPrefs.GetInt("AntiAliasingPreference");
    //    else
    //        aaDropdown.value = 1;
    //    if (PlayerPrefs.HasKey("FullscreenPreference"))
    //        Screen.fullScreen =
    //        Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
    //    else
    //        Screen.fullScreen = true;
    //    if (PlayerPrefs.HasKey("VolumePreference"))
    //        volumeSlider.value =
    //                    PlayerPrefs.GetFloat("VolumePreference");
    //    else
    //        volumeSlider.value =
    //                    PlayerPrefs.GetFloat("VolumePreference");
    //}
    
}
