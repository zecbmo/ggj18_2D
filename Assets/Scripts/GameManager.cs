using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum GameState {PreGame, InGame, EndGame };
public enum TeamColor { Red, Blue };


public class GameManager : Singleton<GameManager>
{
    public GameState gameState = GameState.PreGame;

    [Header("Pre Game Settings")]
    [SerializeField]
    float gameStartDelay = 3.0f;
    float startDelayDivided = 0.0f;

    [Header("TeamColors")]
    [SerializeField]
    Color[] redColours = new Color[4] { Color.red, Color.red, Color.red, Color.red };
    [SerializeField]
    Color[] blueColors = new Color[4] { Color.blue, Color.blue, Color.blue, Color.blue };
    int redCount = 0;
    int blueCount = 0;




    [Header("In Game Settings")]
    [SerializeField]
    float gameLength = 180.0f;

    [Header("Towers")]
    [SerializeField]
    public Tower redTower = null;

    [SerializeField]
    public Tower blueTower = null;

    [SerializeField]
    public GameObject[] redSpawns;
    [SerializeField]
    public GameObject[] blueSpawns;


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
    [SerializeField]
    float respawnTime = 2.0f;

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

        List<GameObject> newPlayers =  PlayerManager.Instance().SpawnPlayers();
        SetUpTeams(newPlayers);

    }

    void SetUpTeams(List<GameObject> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            //on red team
            if (i % 2 == 0)
            {
                AddPlayerToTeam(players[i], TeamColor.Red);
            }
            else //on blue team
            {
                AddPlayerToTeam(players[i], TeamColor.Blue);

            }
        }
    }

    void AddPlayerToTeam(GameObject player, TeamColor color)
    {
        ColorChanger changer = player.GetComponent<ColorChanger>();

        switch (color)
        {
            case TeamColor.Red:
                {
                    changer.ChangeColor(redColours[redCount]);
                    redCount++;
                    if (redCount > redColours.Length)
                    {
                        redCount = 0;
                    }
                    MoveToRandomSpawn(player, redSpawns);
                }
                break;
            case TeamColor.Blue:
                {
                    changer.ChangeColor(blueColors[blueCount]);
                    blueCount++;
                    if (blueCount > blueColors.Length)
                    {
                        blueCount = 0;
                    }
                    MoveToRandomSpawn(player, blueSpawns);

                }
                break;
            default:
                break;
        }
    }

    void MoveToRandomSpawn(GameObject player, GameObject[] spawns)
    {
        int rand = Random.Range(0, spawns.Length);
        player.transform.position = spawns[rand].transform.position;

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

    public void RespawnPlayer(GameObject player)
    {
        player.transform.position = new Vector3(17, 12,1973);
        //player.SetActive(false);
        StartCoroutine(RespawnPlease(player, respawnTime));

    }

    IEnumerator RespawnPlease(GameObject player, float delay)
    {
        yield return new WaitForSeconds(delay);

        //player.SetActive(true);

        int rand = Random.Range(0, 2);

        if (rand == 0)
            MoveToRandomSpawn(player, redSpawns);
        else
            MoveToRandomSpawn(player, blueSpawns);

    }
}



