using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;
using System.Collections.Generic;


public class PlayFabManager : MonoBehaviour
{
    public TextMeshProUGUI leaderboardText;
    bool isLoggedIn = false;

    public GameObject panelLeaderboard;
    public GameManager gameManager;
    int playerHighScore = 0;
    public GameObject panelVictoria;
    public TMP_InputField inputName;


    void Start()
    {
        PlayFabSettings.staticSettings.TitleId = "166A30";  //Aqui pongo mi usuario de PlayFab para iniciar sesion
        Login();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest //En lugar de usar una cuenta, "crea" una con el id de la computadora
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);  //Aqui lo manda a PlayFab
    }

    void OnLoginSuccess(LoginResult result)
    {
                isLoggedIn = true;
    }

    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score)  //Aqui envia el puntaje a PlayFab
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new System.Collections.Generic.List<StatisticUpdate>()
            {
                new StatisticUpdate
                {
                    StatisticName = "HighScore",
                    Value = score
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, success => {
            Debug.Log("Score enviado");
        }, OnError);
    }

    public void SendLeaderboardIfHighScore(int score) //Aqui revisa que sea un highscore. No lo guarda si es menor al numero
    {
        GetPlayerHighScore(() =>
        {
            if (score > playerHighScore)
            {
                SendLeaderboard(score);
            }
            else
            {
                Debug.Log("No superó el high score");
            }
        });
    }

    public void GetLeaderboard()
    {
        if (!isLoggedIn)
        {
            Debug.Log("Inicia sesion");
            return;
        }

        var request = new GetLeaderboardRequest
        {
            StatisticName = "HighScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };

        PlayFabClientAPI.GetLeaderboard(request, result =>
        {
            leaderboardText.text = "";

            int maxSlots = 10;

            for (int i = 0; i < maxSlots; i++)
            {
                if (i < result.Leaderboard.Count)
                {
                    var item = result.Leaderboard[i];

                    string name = item.DisplayName;
                    if (string.IsNullOrEmpty(name))
                        name = "Player";

                    leaderboardText.text += (i + 1) + " - " + name + " - " + item.StatValue + "\n";
                }
                else
                {
                   //Para que aparezcan los espacios vacios
                    leaderboardText.text += (i + 1) + " - --- - 0\n";
                }
            }
        }, OnError);
    }

    public void SetPlayerName(string playerName)  //Aqui es donde ponermos el nombre del jugador que escribimos 
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = playerName
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request,
            result => Debug.Log("Nombre de jugador guardado"),
            OnError);
    }
    public void ShowLeaderbord()
    {
        panelLeaderboard.SetActive(true);

        if (gameManager.panelVictoria != null)
            gameManager.panelVictoria.SetActive(false);

        GetLeaderboard();
    }

   

    public void BackToVictory()
    {
        panelLeaderboard.SetActive(false);

        if (panelVictoria != null)
            panelVictoria.SetActive(true);
    }

    public void GetPlayerHighScore(System.Action callback)
    {
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(),
        result =>
        {
            foreach (var stat in result.Statistics)
            {
                if (stat.StatisticName == "HighScore")
                {
                    playerHighScore = stat.Value;
                }
            }

            callback?.Invoke();
        },
        OnError);
    }

    

    public void SaveName()
    {
        SetPlayerName(inputName.text);
        Debug.Log("Nombre de jugador: " + inputName.text);
    }

   

}
