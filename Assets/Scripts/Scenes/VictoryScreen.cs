using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{
    public ScoreHandler ScoreHandler;
    public Text UberScoreText;

    private void Start()
    {
        ScoreHandler = GameObject.FindGameObjectWithTag("ScoreHandler").GetComponent<ScoreHandler>();
        UberScoreText.text = string.Format("Uber score:{0}", ScoreHandler.TotalPlayerPoints.ToString("F0"));

        AnalyticLogger.AddData(AnalyticEventType.PlayerWon);
        if (ScoreHandler.BossFightStarted)
        {
            if (ScoreHandler.BossHealth <= 0.0f)
            {
                AnalyticLogger.AddData(AnalyticEventType.BossKilled);
            }
            else
            {
                AnalyticLogger.AddData(AnalyticEventType.BossHealth, ScoreHandler.BossHealth.ToString("F0"));
            }
        }
        AnalyticLogger.AddData(AnalyticEventType.EndScore, ScoreHandler.TotalPlayerPoints.ToString("F0"));
        AnalyticLogger.AddData(AnalyticEventType.ShotsFired, ScoreHandler.ShotsFired.ToString("F0"));
        AnalyticLogger.SaveToFile();
    }

    private void Update()
    {

    }

    public void RestartGame()
    {
        AnalyticLogger.AddData(AnalyticEventType.NewGameStarted);
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OnApplicationQuit()
    {
        AnalyticLogger.AddData(AnalyticEventType.GameClosed);
        AnalyticLogger.SaveToFile();
    }
}
