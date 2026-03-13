using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public int Tickets = 10;
    public TextMeshProUGUI Text;

    public float tiempoRestante = 30f;
    public TextMeshProUGUI timerText;
    private bool juegoTerminado = false;


    public GameObject panelVictoria;
    public float tiempoEspera = 3f;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {

        if (!juegoTerminado && tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            UpdateTimerUI();

            if (tiempoRestante <= 0)
            {
                tiempoRestante = 0;
                juegoTerminado = true;
                Restart();
            }
        }
    }
    public void AddTicket()
    {
        Tickets--;
        ContadorUI();

        if (Tickets <= 0)
        {
            StartCoroutine(Victoria());
        }
    }

    void ContadorUI()
    {
        if (Text != null)
        {
            Text.text = "Collectibles Left: " + Tickets.ToString();
        }
    }
    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutos = Mathf.FloorToInt(tiempoRestante / 60);
            int segundos = Mathf.FloorToInt(tiempoRestante % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutos, segundos);
        }
    }

    IEnumerator Victoria()
    {
        juegoTerminado = true;

        if (panelVictoria != null)
            panelVictoria.SetActive(true);

        yield return new WaitForSeconds(tiempoEspera);

        Restart();
    }

    void Restart()
    {
        string nombreEscena = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("New Scene");
    }
}
