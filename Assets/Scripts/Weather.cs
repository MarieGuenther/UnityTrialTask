[System.Serializable]
public class Weather
{
    public DataJson data;
}

[System.Serializable]
public class DataJson
{
    public GetCityByName getCityByName = default;
}

[System.Serializable]
public class GetCityByName
{
    public string name = "";
    public WeatherData weather = default;
}

[System.Serializable]
public class WeatherData
{
    public double timestamp;
    public TemperatureData temperature;
    public SummaryData summary;
}

[System.Serializable]
public class TemperatureData
{
    public float actual;
}

[System.Serializable]
public class SummaryData
{
    public string title;
    public string description;
}
