using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Helpers;

[ExecuteInEditMode]
public class HomeDebugger : MonoBehaviour
{
    [Header("Camera")]
    public GameObject Camera;
    [Space(10)]
    [Header("Main Button")]
    public GameObject StartButton;
    public GameObject ShopButton;
    public GameObject SettingButton;
    public GameObject CreditButton;
    public GameObject ExitButton;
    [Space(10)]
    [Header("Avatar")]
    public GameObject AvatarField;
    [Space(10)]
    [Header("Coin")]
    public GameObject CoinField;
    [Space(10)]
    [Header("Level Type")]
    public GameObject ManualLevel;
    public GameObject CustomLevel;
    public GameObject BackButton;
    [Space(10)]
    [Header("Scene Transition")]
    public GameObject SceneTransition;
    [Space(10)]
    [Header("Chapter Selection")]
    public GameObject[] Chapters;
    [Button("Show Chapter Selection")]
    public void ShowChapterSelection()
    {
        DOHideLevelSection();
        DOHideMainHomeUI();
        DOHideSceneTransition();
        DOShowChapterSelection();
    }

    [Button("Show Level Section")]
    public void ShowLevelSection()
    {
        DOHideMainHomeUI();
        DOHideSceneTransition();
        DOHideChapterSelection();
        DOShowLevelSection();
    }  

    [Button("Show Main Home UI")]
    public void ShowMainHomeUI()
    {
        DOShowMainHomeUI();
        DOHideLevelSection();
        DOHideSceneTransition();
        DOHideChapterSelection();
    }
    [Button("Show Scene Transition")]
    public void ShowSceneTransition()
    {
        DOShowSceneTransition();
    }
    void DOShowChapterSelection()
    {
        Vector2 basePosition = new Vector2(400, -50);
        float factor = 560;
        foreach (var (g, index) in Chapters.WithIndex())
        {
            Vector2 position = new Vector2(basePosition.x + index * factor, basePosition.y);
            g.GetComponent<RectTransform>().anchoredPosition = position;
        }
        BackButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(120, -120);
        
        Vector3 cameraPos = Camera.transform.position;

        cameraPos.x = -14;
        
        Camera.transform.position = cameraPos;
    }
    void DOHideChapterSelection()
    {
        foreach (var (g, index) in Chapters.WithIndex())
        {
            Vector2 position = new Vector2(2420, -50);
            g.GetComponent<RectTransform>().anchoredPosition = position;
        }
        BackButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100, -100);
    }
    void DOShowSceneTransition()
    {
        SceneTransition.SetActive(true);
    }
    void DOHideSceneTransition()
    {
        SceneTransition.SetActive(false);
    }
    void DOShowLevelSection()
    {
        ManualLevel.GetComponent<RectTransform>().anchoredPosition = new Vector2(500, ManualLevel.GetComponent<RectTransform>().anchoredPosition.y);
        CustomLevel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-500, CustomLevel.GetComponent<RectTransform>().anchoredPosition.y);
        BackButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(120, -120);
        
        Vector3 cameraPos = Camera.transform.position;

        cameraPos.x = -14;
        
        Camera.transform.position = cameraPos;
    }
    void DOHideLevelSection()
    {
        ManualLevel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-500, ManualLevel.GetComponent<RectTransform>().anchoredPosition.y);
        CustomLevel.GetComponent<RectTransform>().anchoredPosition = new Vector2(500, CustomLevel.GetComponent<RectTransform>().anchoredPosition.y);
        BackButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100, 100);
    }
    void DOHideMainHomeUI()
    {
        StartButton.transform.localScale = Vector3.zero;
        ShopButton.transform.localScale = Vector3.zero;
        SettingButton.transform.localScale = Vector3.zero;
        CreditButton.transform.localScale = Vector3.zero;
        ExitButton.transform.localScale = Vector3.zero;

        AvatarField.GetComponent<RectTransform>().anchoredPosition = new Vector2(AvatarField.GetComponent<RectTransform>().anchoredPosition.x, 800);
        CoinField.GetComponent<RectTransform>().anchoredPosition = new Vector2(CoinField.GetComponent<RectTransform>().anchoredPosition.x, 800);
    }
    void DOShowMainHomeUI()
    {
        StartButton.transform.localScale = Vector3.one;
        ShopButton.transform.localScale = Vector3.one;
        SettingButton.transform.localScale = Vector3.one;
        CreditButton.transform.localScale = Vector3.one;
        ExitButton.transform.localScale = Vector3.one;

        AvatarField.GetComponent<RectTransform>().anchoredPosition = new Vector2(AvatarField.GetComponent<RectTransform>().anchoredPosition.x, 540);
        CoinField.GetComponent<RectTransform>().anchoredPosition = new Vector2(CoinField.GetComponent<RectTransform>().anchoredPosition.x, 540);
        
        Vector3 cameraPos = Camera.transform.position;

        cameraPos.x = -4;
        
        Camera.transform.position = cameraPos;
    }

    [Button("Reset to play mode")]
    public void ResetToPlayMode()
    {
        DOHideMainHomeUI();
        DOHideLevelSection();
        DOShowSceneTransition();
        DOHideChapterSelection();

        Vector3 cameraPos = Camera.transform.position;

        cameraPos.x = -4;
        
        Camera.transform.position = cameraPos;
    }
    [Button("Hide all UI (not player)")]
    public void HideAllUI()
    {
        DOHideMainHomeUI();
        DOHideLevelSection();
        DOHideSceneTransition();
        DOHideChapterSelection();
        
        Vector3 cameraPos = Camera.transform.position;

        cameraPos.x = -4;
        
        Camera.transform.position = cameraPos;
    }
    [Button("Hide all UI (including player)")]
    public void HideAllUIAndPlayer()
    {
        DOHideMainHomeUI();
        DOHideLevelSection();
        DOHideSceneTransition();
        DOHideChapterSelection();

        Vector3 cameraPos = Camera.transform.position;

        cameraPos.x = -14;
        
        Camera.transform.position = cameraPos;
    }
}
