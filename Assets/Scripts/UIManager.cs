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
    private CanvasGroup _editUI = default, _terraformingUI = default, _playUI = default;

    [SerializeField]
    private Slider _strengthSlider = default, _radiusSlider = default;
    [SerializeField]
    private Toggle _directionToggle = default;

    private bool _terraformingState = false;

    private void Start()
    {
        GameManager.OnModeChange += OnModeChangeHandler;
        _terraformingUI.alpha = 0;
        _terraformingUI.blocksRaycasts = false;
        _playUI.alpha = 0;
        _playUI.blocksRaycasts = false;

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

    public void ShowTerraformingUI()
    {
        _terraformingState = !_terraformingState;
        _terraformingUI.DOKill(true);
        if (_terraformingState)
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
