using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GraphQlClient.Core;
using UnityEngine.Networking;

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
    public GraphApi weatherAPIReference;

    private void Start()
    {
        //ChangeTime(DaytimeEnum.morning);
        ChangeTime(DaytimeEnum.day);
        //DOVirtual.DelayedCall(5, () => ChangeTime(DaytimeEnum.day));
        //DOVirtual.DelayedCall(10, () => ChangeTime(DaytimeEnum.evening));
        //DOVirtual.DelayedCall(15, () => ChangeTime(DaytimeEnum.night));
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
        GetWeather(_currentCity.ToString());
    }

    private void GetWeather(string in_cityName)
    {
        GetCityWeather();
    }

    public async void GetCityWeather()
    {
        //Gets the needed query from the Api Reference
        GraphApi.Query createUser = weatherAPIReference.GetQueryByName("getCityByName", GraphApi.Query.Type.Query);

        //Converts the JSON object to an argument string and sets the queries argument
        createUser.SetArgs(new {  name = "Berlin"  });

        //Performs Post request to server
        UnityWebRequest request = await weatherAPIReference.Post(createUser);

        //UnityWebRequest request = await weatherAPIReference.Post("GetCityWeather", GraphApi.Query.Type.Query);
        string data = request.downloadHandler.text;

        Weather weather = JsonUtility.FromJson<Weather>(data);
        Debug.Log(JsonUtility.ToJson(weather));
        print(weather.data.getCityByName.name);
        print("Temperature " + weather.data.getCityByName.weather.temperature.actual);

    }

}

[System.Serializable]
public class DaytimeDetails
{
    public Material _skyboxMaterial = default;
    public Vector3 _sunRotation = default;
}

