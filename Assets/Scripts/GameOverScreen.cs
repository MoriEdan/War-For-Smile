using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public ScoreHandler ScoreHandler;
    public Text UberScoreText;

    private void Start ()
    {
        ScoreHandler = GameObject.FindGameObjectWithTag("ScoreHandler").GetComponent<ScoreHandler>();
        UberScoreText.text = string.Format("Uber score:{0}", ScoreHandler.TotalPlayerPoints.ToString("F0"));
    }

    private void Update ()
    {
	    
	}

    public void RestartGame()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
