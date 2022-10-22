using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[ExecuteInEditMode]
public class HomeDebugger : MonoBehaviour
{
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

    [Button("Show Level Section")]
    public void ShowLevelSection()
    {
        DOShowLevelSection();
        DOHideMainHomeUI();
    }  

    [Button("Show Main Home UI")]
    public void ShowMainHomeUI()
    {
        DOShowMainHomeUI();
        DOHideLevelSection();
    }
    void DOShowLevelSection()
    {
        ManualLevel.GetComponent<RectTransform>().anchoredPosition = new Vector2(500, ManualLevel.GetComponent<RectTransform>().anchoredPosition.y);
        CustomLevel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-500, CustomLevel.GetComponent<RectTransform>().anchoredPosition.y);
        BackButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(120, -120);
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
    }

    [Button("Reset to play mode")]
    public void ResetToPlayMode()
    {
        DOHideMainHomeUI();
        DOHideLevelSection();
    }
}
