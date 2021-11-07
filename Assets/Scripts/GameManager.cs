using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public enum GameMode { Edit, Play}
    private GameMode _currentMode = GameMode.Edit;

    public enum EditMode { Camera, Terraforming, CharacterSet, CityChange}
    private EditMode _currentEditMode = EditMode.Camera;

    public static System.Action<GameMode> OnModeChange;
    public static System.Action OnCharacterPlaced;

    private float _deformationStrength = 1, _deformationRadius = 1;
    private bool _deformationDirection = true;

    private bool _characterPlaced = false;

    [SerializeField]
    private Transform _pointer = default;
    public Transform Pointer { get => _pointer; }

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

    public bool CharacterPlaced { get => _characterPlaced; }

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

    public void ResetPlane()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void SetFromPlayerPrefValues()
    {
        if (!PlayerPrefs.HasKey("DeformationStrength"))
        {
            PlayerPrefs.SetFloat("DeformationStrength", 0.535f);
        }
        if (!PlayerPrefs.HasKey("DeformationRadius"))
        {
            PlayerPrefs.SetFloat("DeformationRadius", 1.5f);
        }
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

    public void ChangeCharacterPlaced(bool in_value)
    {
        _characterPlaced = in_value;
        if (_characterPlaced)
            OnCharacterPlaced();
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
