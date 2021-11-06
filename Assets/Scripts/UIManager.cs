using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI _modeButtonText = default;
    [SerializeField]
    private CanvasGroup _editUI = default, _terraformingUI = default, _playUI = default, _joystick = default;
    [SerializeField]
    private Button _terraformingButton = default, _characterButton = default;

    [SerializeField]
    private Slider _strengthSlider = default, _radiusSlider = default;
    [SerializeField]
    private Toggle _directionToggle = default;

    private void Start()
    {
        GameManager.OnModeChange += OnModeChangeHandler;
        GameManager.OnCharacterPlaced += OnCharacterPlacedHandler;
        _terraformingUI.alpha = 0;
        _terraformingUI.blocksRaycasts = false;
        _playUI.alpha = 0;
        _playUI.blocksRaycasts = false;
        _joystick.alpha = 0;
        _joystick.blocksRaycasts = false;

        SetFromPlayerPrefValues();
    }

    private void OnDestroy()
    {
        GameManager.OnModeChange -= OnModeChangeHandler;
    }

    private void SetFromPlayerPrefValues()
    {
        _strengthSlider.value = PlayerPrefs.GetFloat("DeformationStrength");
        _radiusSlider.value = PlayerPrefs.GetFloat("DeformationRadius");
        _directionToggle.isOn = PlayerPrefs.GetInt("DeformationDirection") == 1;

    }


    private void OnModeChangeHandler(GameManager.GameMode in_mode)
    {
        _editUI.DOKill(true);
        if(in_mode== GameManager.GameMode.Edit)
        {
            _modeButtonText.text = "Play";
            _editUI.DOFade(1, 0.2f);
            _editUI.blocksRaycasts = true;
            _playUI.DOFade(0, 0.2f);
            _playUI.blocksRaycasts = false;
        }
        else
        {
            _modeButtonText.text = "Edit";
            _editUI.DOFade(0, 0.2f);
            _editUI.blocksRaycasts = false;
            _playUI.DOFade(1, 0.2f);
            _playUI.blocksRaycasts = true;
        }
    }

    private void OnCharacterPlacedHandler()
    {
        _joystick.alpha = 1;
        _joystick.blocksRaycasts = true;
    }

    public void PressedCharacterButton()
    {
        if (GameManager.Instance.CurrentEditMode != GameManager.EditMode.CharacterSet)
        {
            TerraformUIHandler(false);
            GameManager.Instance.ChangeEditMode(GameManager.EditMode.CharacterSet);
        }
        else
        {
            GameManager.Instance.ChangeEditMode(GameManager.EditMode.Camera);
        }
    }

    public void ShowTerraformingUI()
    {
        
        TerraformUIHandler(GameManager.Instance.CurrentEditMode != GameManager.EditMode.Terraforming);
    }

    private void TerraformUIHandler(bool in_value)
    {
        _terraformingUI.DOKill(true);
        if (in_value)
        {
            _terraformingUI.DOFade(1, 0.2f);
            _terraformingUI.blocksRaycasts = true;
            GameManager.Instance.ChangeEditMode(GameManager.EditMode.Terraforming);

        }
        else
        {
            _terraformingUI.DOFade(0, 0.2f);
            _terraformingUI.blocksRaycasts = false;
            GameManager.Instance.ChangeEditMode(GameManager.EditMode.Camera);

        }

    }
}
