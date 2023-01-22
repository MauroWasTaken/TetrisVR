using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    private GameManagerScript gameManager;
    private DayAndNightControl dayAndNightControl;
    private float targetTime;
    [SerializeField]
    private float speed = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        dayAndNightControl = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();    
        targetTime = dayAndNightControl.currentTime;

        gameManager = GameManagerScript.instance;
        gameManager.singleLine.AddListener(() => { UpdateTargetTime(1); });
        gameManager.doubleLine.AddListener(() => { UpdateTargetTime(2); });
        gameManager.tripleLine.AddListener(() => { UpdateTargetTime(3); });
        gameManager.tetrisLine.AddListener(() => { UpdateTargetTime(4); });
    }
    void UpdateTargetTime(int nbLines)
    {
        dayAndNightControl.currentTime += nbLines * 0.025f;
        if (dayAndNightControl.currentTime > 1)
        {
            dayAndNightControl.currentTime -= 1;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(gameManager.isEasterEggActive)
        {
            dayAndNightControl.SecondsInAFullDay = 0.5f;
        }
        
    }

}
