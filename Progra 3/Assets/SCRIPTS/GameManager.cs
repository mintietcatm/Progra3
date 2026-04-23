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
    public PlayFabManager playFabManager;


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
        Debug.Log("Tickets restantes: " + Tickets);

        if (Tickets <= 0)
        {
            StartCoroutine(Victoria());
        }
    }

   

    void UpdateScoreUI()
    {
        if (Text != null)
        {
            Text.text = "Score: " + score.ToString();
        }
    }

    void ContadorUI()
    {
        if (Text != null)
        {
            Text.text =  score.ToString();
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
        playFabManager.SendLeaderboard(score);

        Restart();
    }

    void Restart()
    {
        string nombreEscena = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("New Scene");
    }

    public int score;

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);

        UpdateScoreUI();
    }
}
