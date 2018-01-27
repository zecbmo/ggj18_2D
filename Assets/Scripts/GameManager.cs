using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum GameState {PreGame, InGame, EndGame };

public class GameManager : Singleton<GameManager>
{
    public GameState gameState = GameState.PreGame;

    [Header("Pre Game Settings")]
    [SerializeField]
    float gameStartDelay = 3.0f;
    float startDelayDivided = 0.0f;

    [Header("In Game Settings")]
    [SerializeField]
    float gameLength = 180.0f;

    [Header("Towers")]
    [SerializeField]
    public Tower redTower = null;

    [SerializeField]
    public Tower blueTower = null;


    [Header("Pre Game UI")]
    [SerializeField]
    GameObject introUI = null;
    [SerializeField]
    Text introText = null;
    [SerializeField]
    string[] startingText = new string[] { "Ready", "Set", "Go" };

    [Header("In Game UI")]
    [SerializeField]
    GameObject timerUIObject = null;
    [SerializeField]
    Text timerText = null;
    [SerializeField]
    float timerObjectOffset = 5;
    [SerializeField]
    float timerMoveInSpeed = 5;

    [Header("Post Game UI")]
    [SerializeField]
    GameObject endGameUI = null;
    

    Vector3 timerStartPosition;
    Vector3 timerOffPosition;


    // Use this for initialization
    void Start ()
    {
        timerStartPosition = timerUIObject.transform.localPosition;
        timerOffPosition = new Vector3(timerStartPosition.x, timerStartPosition.y + timerObjectOffset, timerStartPosition.z);
        timerUIObject.transform.localPosition = timerOffPosition;


        startDelayDivided = gameStartDelay / 3.0f;
        StartCoroutine(ChangeIntroTextAndRepeat(startingText, startDelayDivided, 0));

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    IEnumerator ChangeIntroTextAndRepeat(string[] text,  float delayTime, int count)
    {
        introText.text = text[count];
        yield return new WaitForSeconds(delayTime);

        if (count < text.Length-1)
        {
            count++;
            StartCoroutine(ChangeIntroTextAndRepeat(text, delayTime, count));
        }
        else
        {
            //Move on to next scene
            gameState = GameState.InGame;
            introUI.SetActive(false);
            MoveTimerObjectToPoint(timerStartPosition);
            StartCoroutine(StartGameTimer());
        }
        
    }

    IEnumerator StartGameTimer()
    {
        while (gameLength > 0)
        {

            string minutes = Mathf.Floor(gameLength / 60).ToString("00");
            string seconds = Mathf.Floor(gameLength % 60).ToString("00");

            timerText.text = minutes + ":" + seconds;

            gameLength -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        gameState = GameState.EndGame;
        EndGame();
    }

    void EndGame()
    {
        MoveTimerObjectToPoint(timerOffPosition);
        TowerManager.Instance().EndGame();
        endGameUI.SetActive(true);
    }

    void MoveTimerObjectToPoint(Vector3 newlocation)
    {
        StartCoroutine(MathUtil.MoveObjectTowardsLocation(timerText.gameObject.transform.parent.gameObject, newlocation, timerMoveInSpeed, 0, true));
    }
}



