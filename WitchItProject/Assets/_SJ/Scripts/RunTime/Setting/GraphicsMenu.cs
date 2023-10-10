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

        #region �ػ� �ʱ�ȭ
        // { �ػ� �ʱ�ȭ 
        resolutions = new List<Resolution>();

        for (int i = 0; i < Screen.resolutions.Length; i++)
        {

            if (Screen.resolutions[i].width > 1000 && Screen.resolutions[i].refreshRate == 60)
            {
                resolutions.Add(Screen.resolutions[i]);
            }

        }       // Loop : ���� �ػ� List�� �߰�

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

        }       // Loop : ���� �ػ� ��Ӵٿ� �߰�

        resoultionsDropdown.RefreshShownValue();

        // { ���� ����� �ػ󵵷� �ʱ� �ػ� ����
        Screen.SetResolution(resolutions[resoultionsDropdown.value].width, resolutions[resoultionsDropdown.value].height,
         FullScreenMode.FullScreenWindow, 60);
        // } ���� ����� �ػ󵵷� �ʱ� �ػ� ����

        // } �ػ� �ʱ�ȭ 
        #endregion

        #region ��Ƽ������� �ʱ�ȭ
        // { ��Ƽ������� �ʱ�ȭ
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

        }       // Loop : ��Ƽ������� ��Ӵٿ� �߰�
        antiAliasingDropdown.RefreshShownValue();
        // } ��Ƽ������� �ʱ�ȭ
        #endregion

        #region Vsync Anialiasing �ʱ�ȭ        
        vsyncToggle.isOn = false;
        QualitySettings.vSyncCount = 0;
        QualitySettings.antiAliasing = 0;
        #endregion
        
        #region ���� �ʱ�ȭ
        
        volume = Camera.main.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out colorGrading);
        gammaNum = 0;
        #endregion




    }       // Init()

    // FPS Text Info Initialize
    public void SetFpsText()
    {
        // ������ �ػ󵵰� ����Ǹ� �׿� �����ؼ� ��ġ �̵�?? 
        // �ٸ� �׷� ��� ��Ŀ�� �ذ� �ϸ� ���� �ʳ�?
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
        // ��۷� ��üȭ��, â��� 
        // ������ â��� �����Ϸ��� ��� �ٿ����� ���� �߰� ����
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


    // TODO : �Ҹ� ���� 
    //public void SetVolume(float volume)
    //{
    //    audioMixer.SetFloat("Volume", volume);
    //    currentVolume = volume;
    //}


}
