using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;


public class PlayFabManager : MonoBehaviour
{
    public TextMeshProUGUI leaderboardText;
    bool isLoggedIn = false;

    public GameObject panelLeaderboard;
    public GameManager gameManager;

    void Start()
    {
        PlayFabSettings.staticSettings.TitleId = "166A30";
        Login();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login success");
        isLoggedIn = true;
    }

    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score)
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

            foreach (var item in result.Leaderboard)
            {
                string name = item.DisplayName;

                if (string.IsNullOrEmpty(name))
                    name = "Player";

                leaderboardText.text += item.Position + " - " + name + " - " + item.StatValue + "\n";
            }
        }, OnError);
    }

    public void SetPlayerName(string playerName)
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

    public void HideLeaderboard()
    {
        panelLeaderboard.SetActive(false);
    }
}
