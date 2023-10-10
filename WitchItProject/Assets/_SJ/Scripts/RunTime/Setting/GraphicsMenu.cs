using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEditor;

public class GraphicsMenu : MonoBehaviour
{

    #region Variables 
    [Header ("Audio")]
    public AudioMixer audioMixer;
    
    [Header("FPS")]
    public TMP_Text fpsText;
    private RectTransform fpsRect = default;    
    private float width;
    private float height;
    private int size = 15;
    private Color color = Color.black;

    [Header("Resolution")]
    public Toggle fullScreenToggle;
    public TMP_Dropdown resoultionsDropdown;
    private List<Resolution> resolutions =  default;
    private int resolutionNum = default;
    private FullScreenMode screenMode;
    
    [Header("Vsync")]
    public Toggle vsyncToggle;
    public TMP_Dropdown antiAliasingDropdown;    
    private int antiAliasingNum = default;

    [Header("Gamma")]
    private PostProcessVolume volume;
    private ColorGrading colorGrading;
    public Slider gammaSlider;
    public TMP_Text gammaText;
    private float gammaNum;
    


    #endregion
    
    private void Start()
    {       
        Init();
    }

    private void Update()
    {
        SetFpsText();       
    }

    #region Initialize

    public void Init()
    {
      
        
        fpsRect = fpsText.GetComponent<RectTransform>();

        #region 해상도 초기화
        // { 해상도 초기화 
        resolutions = new List<Resolution>();

        for (int i = 0; i < Screen.resolutions.Length; i++)
        {

            if (Screen.resolutions[i].width > 1000 && Screen.resolutions[i].refreshRate == 60)
            {
                resolutions.Add(Screen.resolutions[i]);
            }

        }       // Loop : 지원 해상도 List에 추가

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

        }       // Loop : 지원 해상도 드롭다운 추가

        resoultionsDropdown.RefreshShownValue();

        // { 현재 모니터 해상도로 초기 해상도 설정
        Screen.SetResolution(resolutions[resoultionsDropdown.value].width, resolutions[resoultionsDropdown.value].height,
         FullScreenMode.FullScreenWindow, 60);
        // } 현재 모니터 해상도로 초기 해상도 설정

        // } 해상도 초기화 
        #endregion

        #region 안티엘리어싱 초기화
        // { 안티엘리어싱 초기화
        antiAliasingDropdown.options.Clear();
        for (int i = 0; i < 4; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            switch (i)
            {
                case 0:
                    option.text = "Disabled";
                    break;
                case 1:
                    option.text = "2x Multi Sampling";
                    break;
                case 2:
                    option.text = "4x Multi Sampling";
                    break;
                case 3:
                    option.text = "8x Multi Sampling";
                    break;
            }
            antiAliasingDropdown.options.Add(option);
            antiAliasingDropdown.value = 0;

        }       // Loop : 안티엘리어싱 드롭다운 추가
        antiAliasingDropdown.RefreshShownValue();
        // } 안티엘리어싱 초기화
        #endregion

        #region Vsync Anialiasing 초기화        
        vsyncToggle.isOn = false;
        QualitySettings.vSyncCount = 0;
        QualitySettings.antiAliasing = 0;
        #endregion
        
        #region 감마 초기화
        
        volume = Camera.main.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out colorGrading);
        gammaNum = 0;
        #endregion




    }       // Init()

    // FPS Text Info Initialize
    public void SetFpsText()
    {
        // 게임중 해상도가 변경되면 그에 대응해서 위치 이동?? 
        // 다만 그런 경우 앵커로 해결 하면 되지 않나?
        width = Screen.width - (fpsRect.rect.width / 2);
        height = Screen.height - (fpsRect.rect.height / 2);
        Rect rect = new Rect(width, height, Screen.width, Screen.height);

        float fps = 1.0f / Time.deltaTime;
        float ms = Time.deltaTime * 1000.0f;

        fpsText.text = string.Format("{0:N0} FPS {1:N1}ms", fps, ms);
        fpsText.fontSize = size;
        fpsText.color = color;

        fpsText.rectTransform.position = rect.position;
    }       // SetFpsText()

    #endregion

    #region DropDownOptionChange

    public void DropdownOptionChange(int idx)
    {
        resolutionNum = idx;
    }

    public void AnitiAliasingOptionChange(int idx)
    {
        antiAliasingNum = idx;
    }

    #endregion

    #region CheckBoxChange
    public void SetFullScreen(bool isFullScreen)
    {
        // 토글로 전체화면, 창모드 
        // 경계없는 창모드 설정하려면 드롭 다운으로 구현 추가 예정
        screenMode = isFullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }       // SetFullScreen()
    public void OnFpsText(bool isFps)
    {
        fpsText.enabled = isFps;
    }       // OnFpsText()

    public void SetVsnc(bool isVsnc)
    {
        QualitySettings.vSyncCount = isVsnc ? 1 : 0;
        Debug.LogFormat("{0}", QualitySettings.vSyncCount);

    }       // SetVsnc()

    #endregion

    #region SliderOptionChange
    public void SliderOptionChange(float value)
    {
        gammaNum = value;
        gammaText.text = string.Format("{0:F1}",gammaNum/ 50f);
        colorGrading.brightness.value = gammaNum;
    }
    #endregion

    // Click Apply Button
    public void SetResoultion()
    {
        // { Resolution Fullscreen Set
        Screen.SetResolution(resolutions[resolutionNum].width,
        resolutions[resolutionNum].height, screenMode, resolutions[resolutionNum].refreshRate);
        // } Resolution Fullscreen Set

        // { Antialiasing Set
        QualitySettings.antiAliasing = antiAliasingNum;
        // } Antialiasing Set

        //// { Gamma Set
        //colorGrading.brightness.value = gammaNum;
        //// } Gamma Set

    }       // SetResoultion()


    public void OffCanvas()
    {
        this.gameObject.SetActive(false);
    }


    // TODO : 소리 조절 
    //public void SetVolume(float volume)
    //{
    //    audioMixer.SetFloat("Volume", volume);
    //    currentVolume = volume;
    //}


}
