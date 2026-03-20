using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.Rendering;

public class WeatherManager : MonoBehaviour
{
    [SerializeField] private WeatherData[] weatherDatas;
    [SerializeField] private UnitType unitType;

    [SerializeField] Light light;

    public Volume volume;

    public string apiURL;
    private string appID = "9b3105c6ecbafaef6511d9d51bd0557b";

    private string rawJson;

    void Start()
    {
        SetURL(weatherDatas[0]); // Aqui configuramos el url con la info del primer lugar del a
        StartCoroutine(RetrieveWeatherData());
    }

    public void SetURL(WeatherData data)
    {
        apiURL = "https://api.openweathermap.org/data/3.0/onecall" +
                 $"lat={data.latitude}&" +
                 $"lon={data.longitude}&" +
                 $"appid={appID}&" +
                 $"units={(unitType == 0 ? "standar" : (int)unitType == 1 ? "imperial" : "metric")}&" +
                 $"exclude=minutely,hourly,daily,alerts";
    }

    IEnumerator RetrieveWeatherData()
    {
        // Peticion o solicitud
        UnityWebRequest request = new UnityWebRequest(apiURL); // Creamos el request con el url
        // Aqui defino que tipo de informacion espero obtener de mi request
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest(); // Esta linea es la que hace que la corrutina
        // se detenga hasta que se realice la solicitud

        // result es la informacion que te regresa si sale bien
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("Informacion obtenida");
            rawJson = request.downloadHandler.text;

            DecodeJson();
        }
    }

    private void DecodeJson()
    {
        var currentWeather = JSON.Parse(rawJson); // Nos guarda en una variable el json ya de forma que se puede

        weatherDatas[0].timezone = currentWeather["timezone"].Value;
        weatherDatas[0].temp = float.Parse(currentWeather["current"]["temp"].Value);
        weatherDatas[0].description = currentWeather["current"]["weather"][0]["description"].Value;

        ChangeWeather();
    }

    private void ChangeWeather()
    {
        float temperatura = weatherDatas[0].temp;


        switch (temperatura)
        {
            case (0):
                {
                    
                    break;
                }

            case (>20):
                {
                    break;
                }
        }
    }

    [System.Serializable]
    public struct WeatherData
    {
        public string location;

        public float latitude;
        public float longitude;

        public string timezone;
        public float temp;
        public string description;
    }

    public enum UnitType
    {
        Standar,
        Imperial,
        Metric
    }
}