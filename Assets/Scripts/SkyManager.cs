using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GraphQlClient.Core;
using UnityEngine.Networking;
using TMPro;
using System;

public class SkyManager : MonoBehaviour
{
    public enum DaytimeEnum { morning, day, evening, night}
    public enum City { London, Berlin, Moscow, NewYork, Hongkong, Sidney}
    private City _currentCity = City.Berlin;

    [SerializeField]
    private Transform _directionalLight = default;
    [SerializeField]
    private TextMeshProUGUI _cityName = default, _temperature = default, _title = default, _time = default;

    [SerializeField]
    private DaytimeDetails _morning = default, _day = default, _evening = default, _night = default;
    public GraphApi weatherAPIReference;

    private int _timezoneDelay = 1;
    private double _currentTimeStamp = 0;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("SavedCity"))
        {
            PlayerPrefs.SetString("SavedCity", "London");
        }
        GetWeather(PlayerPrefs.GetString("SavedCity"));

        if (!PlayerPrefs.HasKey("Timezone"))
        {
            if (DateTime.UtcNow.IsDaylightSavingTime())
            {
                PlayerPrefs.SetInt("SavedCity", 1);                
            }
            else
            {
                PlayerPrefs.SetInt("SavedCity", 0);
            }
        }
        UpdateTimeUI();

        InvokeRepeating(nameof(RepeatedWeatherRequest), 60, 60);
    }

    private void RepeatedWeatherRequest()
    {
        GetWeather(PlayerPrefs.GetString("SavedCity"));
    }

    private void UpdateTimeUI()
    {
        DateTime utc = UnixTimeStampToDateTime(_currentTimeStamp);
        DateTime time = utc.AddHours(PlayerPrefs.GetInt("Timezone"));
        string currentCaseTimeText = String.Format("{0:00}:{1:00}", time.Hour, time.Minute);
        _time.text = "Last Check: " + currentCaseTimeText;
        ChangeTime(GetDaytimeEnum(time.Hour));
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
        _directionalLight.GetComponent<Light>().intensity = details._sunIntensity;
    }

    public void ChangeCity(int in_cityIndex)
    {
        if (in_cityIndex < 0 || in_cityIndex >= 6)
            return;
        _currentCity = (City)in_cityIndex;
        string cityName = "";
        int timeZone = 1;
        switch (_currentCity)
        {
            case City.Berlin:
                cityName = "Berlin";
                if (DateTime.UtcNow.IsDaylightSavingTime())
                    timeZone = 2;
                else
                    timeZone = 1;
                break;
            case City.Hongkong:
                cityName = "Hongkong";
                timeZone = 8;
                break;
            case City.London:
                cityName = "London";
                if (DateTime.UtcNow.IsDaylightSavingTime())
                    timeZone = 1;
                else
                    timeZone = 0;
                break;
            case City.Moscow:
                cityName = "Moscow";
                timeZone = 3;
                break;
            case City.NewYork:
                cityName = "New York";
                if (DateTime.UtcNow.IsDaylightSavingTime())
                    timeZone = -4;
                else
                    timeZone = -5;
                break;
            case City.Sidney:
                cityName = "Sidney";
                if (DateTime.UtcNow.IsDaylightSavingTime())
                    timeZone = 10;
                else
                    timeZone = 11;
                break;
        }
        PlayerPrefs.SetInt("Timezone", timeZone);
        PlayerPrefs.SetString("SavedCity", cityName);

        GetWeather(cityName);
    }


    private DaytimeEnum GetDaytimeEnum(int in_hour)
    {
        if (in_hour > 0 && in_hour <= 5)
        {
            return DaytimeEnum.night;
        }
        else if (in_hour > 5 && in_hour <= 8)
        {
            return DaytimeEnum.morning;
        }
        else if (in_hour > 8 && in_hour <= 16)
        {
            return DaytimeEnum.day;
        }
        else if (in_hour > 16 && in_hour <= 19)
        {
            return DaytimeEnum.evening;
        }
        else
        {
            return DaytimeEnum.night;
        }
    }

    private void GetWeather(string in_cityName)
    {
        GetCityWeather(in_cityName);
    }

    public async void GetCityWeather(string in_cityName)
    {
        //Gets the needed query from the Api Reference
        GraphApi.Query createUser = weatherAPIReference.GetQueryByName("getCityByName", GraphApi.Query.Type.Query);

        //Converts the JSON object to an argument string and sets the queries argument
        createUser.SetArgs(new {  name = in_cityName });

        //Performs Post request to server
        UnityWebRequest request = await weatherAPIReference.Post(createUser);

        //UnityWebRequest request = await weatherAPIReference.Post("GetCityWeather", GraphApi.Query.Type.Query);
        string data = request.downloadHandler.text;

        Weather weather = JsonUtility.FromJson<Weather>(data);
        _currentTimeStamp = weather.data.getCityByName.weather.timestamp;
        UpdateWeatherUI(weather);
        DateTime currentTime;

        currentTime = UnixTimeStampToDateTime(_currentTimeStamp);
        currentTime = currentTime.AddHours(PlayerPrefs.GetInt("Timezone"));
        UpdateTimeUI();
        ChangeTime(GetDaytimeEnum(currentTime.Hour));
    }

    private void UpdateWeatherUI(Weather in_weather)
    {
        if (in_weather.data.getCityByName == null)
            return;
        _cityName.text = in_weather.data.getCityByName.name;
        _temperature.text = (in_weather.data.getCityByName.weather.temperature.actual - 273.15f).ToString("n1") + "°C";
        _title.text = "Summary: " + in_weather.data.getCityByName.weather.summary.title;
    }

    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
        return dtDateTime;
    }
}

[System.Serializable]
public class DaytimeDetails
{
    public Material _skyboxMaterial = default;
    public Vector3 _sunRotation = default;
    public float _sunIntensity = 1;
}

