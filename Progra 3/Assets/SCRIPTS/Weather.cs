using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WeatherManager : MonoBehaviour
{
    [SerializeField] private WeatherData[] weatherDatas;
    [SerializeField] private UnitType unitType;

    [SerializeField] Light light;
    [SerializeField] private float transitionSpeed = 2f;

    public Volume volume;

    public string apiURL;
    private string appID = "9b3105c6ecbafaef6511d9d51bd0557b";

    private string rawJson;

    private Bloom bloom;
    private ColorAdjustments colorAdjustments; //Estos son los valores que se van a modificar


    void Start()
    {
               volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out colorAdjustments);

        StartCoroutine(WeatherLoop());
    }

    public void SetURL(WeatherData data)
    {
        apiURL = "https://api.openweathermap.org/data/3.0/onecall?" +
          $"lat={data.latitude}&" +
          $"lon={data.longitude}&" +
          $"appid={appID}&" +
          $"units=metric&" +
          $"exclude=minutely,hourly,daily,alerts";
    }

    IEnumerator WeatherLoop()  //Aqui literal estamos haciendo que cambie de lugar cada 15 segundos
    {
        while (true)
        {
            int randomIndex = Random.Range(0, weatherDatas.Length);
            SetURL(weatherDatas[randomIndex]);

            yield return StartCoroutine(RetrieveWeatherData());

            yield return new WaitForSeconds(15f);
        }
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
            //Debug.Log("Informacion obtenida");
            rawJson = request.downloadHandler.text;

            DecodeJson();
        }
    }

    private void DecodeJson() //cosas del api
    {
        var currentWeather = JSON.Parse(rawJson); // Nos guarda en una variable el json ya de forma que se puede

        weatherDatas[0].timezone = currentWeather["timezone"].Value;
        weatherDatas[0].temp = float.Parse(currentWeather["current"]["temp"].Value);
        weatherDatas[0].description = currentWeather["current"]["weather"][0]["description"].Value;

        ChangeWeather();
    }

    private void ChangeWeather()
    {
        float temperatura = weatherDatas[0].temp;  //Okay, aqui basicamente lo que esta haciendo es sacando la temperatura REAL. weatherDatas[0] es el clima actual y .temp es en grados

        float targetBloom = 0f;
        float targetExposure = 0f;   //Y estas son las variables que quiero cambiar, el bloom del nivel y la saturacion. (o color exposure en el volumen)

        if (temperatura < 10) //SI LA TEMPERATURA ESTA FRIA
        {
            Debug.Log("Hace frio");
            targetBloom = 0.02f;
            targetExposure = -2.5f; //el nivel se pone mas apagado
        }
        else if (temperatura < 20) //SI LA TEMPERATURA ESTA OK
        {
            Debug.Log("Its alright");
            targetBloom = 0.6f;
            targetExposure = 0f; //el nivel se ve normal
        }
        else 
        {
            Debug.Log("Soleado");
            targetBloom = 2.0f;
            targetExposure = 2f; //el nivel se ve mas saturado
        }

        StartCoroutine(SmoothVolumeChange(targetBloom, targetExposure));
    }

    IEnumerator SmoothVolumeChange(float targetBloom, float targetExposure) //Aqui literal esta corrutina es para que el cambio de los colores sea despacito, que no lo haga de golpe y se vea feo
    {
        float startBloom = bloom.intensity.value;
        float startExposure = colorAdjustments.postExposure.value;  //Aqui esta guardando los valores iniciales de la temperatura, que tenia antes del cambio

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;

            bloom.intensity.value = Mathf.Lerp(startBloom, targetBloom, t);
            colorAdjustments.postExposure.value = Mathf.Lerp(startExposure, targetExposure, t);

            //Aqui usamos el LERP para que se muevan las variables gradualmente

            yield return null;
        }
    }
    [System.Serializable]
    public struct WeatherData  //Aqui este struct es en donde vamos a poner los datos de los lugares
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
        Standard,
        Imperial,
        Metric
    }
}