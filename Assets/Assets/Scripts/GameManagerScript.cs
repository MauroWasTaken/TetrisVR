using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance { get; private set; }

    public int level  { get; private set; } = 1;

    [SerializeField]
    private GameObject levelText;

    private int score = 0;

    [SerializeField]
    private GameObject scoreText;

    private int lines = 0;

    [SerializeField]
    private GameObject linesText;

    private int linesPerLevel = 10;

    public bool isEasterEggActive { get; private set; } = false;

    //Events
    public UnityEvent OnLevelChanged;
    public UnityEvent singleLine;
    public UnityEvent doubleLine;
    public UnityEvent tripleLine;
    public UnityEvent tetrisLine;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy (gameObject);
        }
    }

    public void UpdateHUD()
    {
        scoreText.GetComponent<TextMeshPro>().SetText("" + score);
        levelText.GetComponent<TextMeshPro>().SetText("" + level);
        linesText.GetComponent<TextMeshPro>().SetText("" + lines);
    }

    public void AddScore(int nbLines)
    {
        switch (nbLines)
        {
            case 1:
                score += 40 * (level + 1);
                singleLine.Invoke();
                break;
            case 2:
                score += 100 * (level + 1);
                doubleLine.Invoke();
                break;
            case 3:
                score += 300 * (level + 1);
                tripleLine.Invoke();
                break;
            case 4:
                score += 1200 * (level + 1);
                tetrisLine.Invoke();
                break;
        }
    }

    public void AddLines(int nbLines)
    {
        AddScore(nbLines);
        lines += nbLines;
        if (lines >= linesPerLevel)
        {
            level++;
            lines -= linesPerLevel;
            //todo : add an event to notify the level has changed
            OnLevelChanged.Invoke();
        }
        UpdateHUD();
    }

    public void ToggleEasterEgg()
    {
        if (isEasterEggActive)return;
        isEasterEggActive = true;
        GameObject
            .Find("LevelLabel")
            .GetComponent<TextMeshPro>()
            .SetText("MADE");
        GameObject
            .Find("LinesLabel")
            .GetComponent<TextMeshPro>()
            .SetText("IN");
        GameObject
            .Find("ScoreLabel")
            .GetComponent<TextMeshPro>()
            .SetText("HEAVEN");
        GameObject.Find("Main Camera").GetComponent<AudioSource>().Play();
        GameObject
            .Find("Day and Night Controller")
            .GetComponent<DayAndNightControl>()
            .SecondsInAFullDay = 5f;
        
    }
    public void GameOver(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
