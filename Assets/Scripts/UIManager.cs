using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI _modeButtonText = default;
    [SerializeField]
    private CanvasGroup _editUI = default, _terraformingUI = default;

    private bool _terraformingState = false;

    private void Start()
    {
        GameManager.OnModeChange += OnModeChangeHandler;
        _terraformingUI.alpha = 0;
        _terraformingUI.blocksRaycasts = false;
    }

    private void OnDestroy()
    {
        GameManager.OnModeChange -= OnModeChangeHandler;
    }

    private void OnModeChangeHandler(GameManager.GameMode in_mode)
    {
        _editUI.DOKill(true);
        if(in_mode== GameManager.GameMode.Edit)
        {
            _modeButtonText.text = "Play";
            _editUI.DOFade(1, 0.2f);
            _editUI.blocksRaycasts = true;
        }
        else
        {
            _modeButtonText.text = "Edit";
            _editUI.DOFade(0, 0.2f);
            _editUI.blocksRaycasts = false;
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

        }
        else
        {
            _terraformingUI.DOFade(0, 0.2f);
            _terraformingUI.blocksRaycasts = false;

        }
    }
}
