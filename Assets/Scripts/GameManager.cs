using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public enum GameMode { Edit, Play}
    private GameMode _currentMode = GameMode.Edit;

    public enum EditMode { Camera, Terraforming, CharacterSet}
    private EditMode _currentEditMode = EditMode.Camera;

    public static System.Action<GameMode> OnModeChange;

    private float _deformationStrength = 1, _deformationRadius = 1;
    private bool _deformationDirection = true;

    public float DeformationStrength
    {
        get => _deformationStrength;
    }

    public float DeformationRadius
    {
        get => _deformationRadius;
    }

    public bool DeformationDirection
    {
        get => _deformationDirection;
    }

    public GameMode CurrentMode
    {
        get => _currentMode;
    }
    public EditMode CurrentEditMode
    {
        get => _currentEditMode;
    }

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        SetFromPlayerPrefValues();
    }

    private void SetFromPlayerPrefValues()
    {
        _deformationStrength = PlayerPrefs.GetFloat("DeformationStrength");
        _deformationRadius = PlayerPrefs.GetFloat("DeformationRadius");
        if (PlayerPrefs.GetInt("DeformationDirection") == 1)
            _deformationDirection = true;
        else
            _deformationDirection = false;

    }

    public void ChangePlayMode()
    {
        if (_currentMode == GameMode.Edit)
        {
            _currentMode = GameMode.Play;
        }
        else
        {
            _currentMode = GameMode.Edit;
        }

        OnModeChange(_currentMode);
    }

    public void ChangeEditMode(EditMode in_mode)
    {
        _currentEditMode = in_mode;
    }

    public void ChangeDeformationStrength(float in_value)
    {
        _deformationStrength = in_value;
    }

    public void ChangeDeformationRadius(float in_value)
    {
        _deformationRadius = in_value;
    }

    public void ChangeDeformationDirection(bool in_value)
    {
        _deformationDirection = in_value;
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("DeformationStrength", _deformationStrength);
        PlayerPrefs.SetFloat("DeformationRadius", _deformationRadius);
        if (_deformationDirection)
            PlayerPrefs.SetInt("DeformationDirection", 1);
        else
            PlayerPrefs.SetInt("DeformationDirection", 0);
    }

}
