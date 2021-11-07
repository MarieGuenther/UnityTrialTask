using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkyManager : MonoBehaviour
{
    public enum DaytimeEnum { morning, day, evening, night}
    private DaytimeEnum _currentDaytime = DaytimeEnum.day;
    public enum City { London, Berlin, Moscow, NewYork, Hongkong, Sidney}
    private City _currentCity = City.Berlin;

    [SerializeField]
    private Transform _directionalLight = default;
    [SerializeField]
    private DaytimeDetails _morning = default, _day = default, _evening = default, _night = default;

    private void Start()
    {
        ChangeTime(DaytimeEnum.morning);
        DOVirtual.DelayedCall(5, () => ChangeTime(DaytimeEnum.day));
        DOVirtual.DelayedCall(10, () => ChangeTime(DaytimeEnum.evening));
        DOVirtual.DelayedCall(15, () => ChangeTime(DaytimeEnum.night));
    }


    public void ChangeTime(DaytimeEnum in_daytime)
    {
        DaytimeDetails details = _day;
        switch (in_daytime)
        {
            case DaytimeEnum.morning:
                details = _morning;
                break;
            case DaytimeEnum.day:
                details = _day;

                break;
            case DaytimeEnum.evening:
                details = _evening;

                break;
            case DaytimeEnum.night:
                details = _night;

                break;
        }

        RenderSettings.skybox = details._skyboxMaterial;
        _directionalLight.rotation = Quaternion.Euler(details._sunRotation);
    }

    public void ChangeCity(int in_cityIndex)
    {
        if (in_cityIndex < 0 || in_cityIndex >= 6)
            return;
        _currentCity = (City)in_cityIndex;
    }

}

[System.Serializable]
public class DaytimeDetails
{
    public Material _skyboxMaterial = default;
    public Vector3 _sunRotation = default;
}
