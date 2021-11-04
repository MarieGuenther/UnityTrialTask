using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public enum GameMode { Edit, Play}
    private GameMode _currentMode = GameMode.Edit;

    public static System.Action<GameMode> OnModeChange;

    public GameMode CurrentMode
    {
        get => _currentMode;
    }

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
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
}
