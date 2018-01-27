using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EndScreen : MonoBehaviour {

    [SerializeField]
    GameObject redIndicator = null;
    [SerializeField]
    GameObject blueIndicator = null;

    [SerializeField]
    Text blueText;
    [SerializeField]
    Text redText;

    [SerializeField]
    GameObject winnerUI = null;
    [SerializeField]
    Text winnerText = null;
    [SerializeField]
    GameObject finalUI = null;

    [SerializeField]
    float winnerUIDelay = 2.0f;

    [SerializeField]
    float finalUIDelay = 4.0f;


    [SerializeField]
    float textIncreaseSpeed = 5.0f;

    Vector3 blueScale;
    Vector3 redScale;

    [Header("Text Options")]
    [SerializeField]
    string BlueWinsText = "Blue Wins";
    [SerializeField]
    string RedWinsText = "Red Wins";
    [SerializeField]
    string DrawText = "Draw Wins";

    float bluePer = 0;
    float redPer = 0;

    // Use this for initialization
    void Start ()
    {
        blueScale = blueIndicator.transform.localScale;
        redScale = redIndicator.transform.localScale;

        blueIndicator.transform.localScale = new Vector3(blueScale.x, 0, blueScale.z);
        redIndicator.transform.localScale = new Vector3(redScale.x, 0, redScale.z);


        bluePer = GameManager.Instance().blueTower.GetPercentageFilled();
        redPer  = GameManager.Instance().redTower.GetPercentageFilled();

        if (bluePer > redPer)
        {
            winnerText.text = BlueWinsText;
        }
        else if (bluePer == redPer)
        {
            winnerText.text = DrawText;

        }
        else
        {
            winnerText.text = RedWinsText;

        }


        UpdateIndicator(blueIndicator, blueScale, bluePer);
        UpdateIndicator(redIndicator, redScale, redPer);
        //StartCoroutine(UpdateText(blueText, bluePer, textIncreaseSpeed));
       // StartCoroutine(UpdateText(redText, redPer, textIncreaseSpeed));

    }

    float lastRedVal = 0;
    float lastBlueVal = 0;
    float curRedVal = 0;
    float curBlueVal = 0;

    bool doOnce = true;

    private void Update()
    {
        curRedVal = UpdateText(redIndicator, redText, redScale.y);
        curBlueVal =  UpdateText(blueIndicator, blueText, blueScale.y);

        if (curRedVal == lastRedVal && lastBlueVal == curBlueVal)
        {
            if (doOnce)
            {
                StartCoroutine(ShowGameObject(winnerUI, winnerUIDelay));
                StartCoroutine(ShowGameObject(finalUI, finalUIDelay));

                doOnce = false;
            }
        }

        lastBlueVal = curBlueVal;
        lastRedVal = curRedVal;


        if (!doOnce)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {

            }
        }


    }



    float UpdateText(GameObject indicator, Text text, float finalScale)
    {
        float curY = indicator.transform.localScale.y/finalScale * 100;
        text.text = curY.ToString("F2");
        

        return curY;
    }

    // Update is called once per frame
    float UpdateIndicator(GameObject indicator, Vector3 finalScale, float percentageFilled)
    {
        //get the percentage value of the currentScale
        float newY = (finalScale.y / 100.0f) * percentageFilled;
        Vector3 newScale = new Vector3(finalScale.x, newY, finalScale.z);

        StartCoroutine(MathUtil.ScaleLerp(indicator, newScale, textIncreaseSpeed, true));
        return newY;
    }

    //public IEnumerator UpdateText2(Text objectToLerp,  float percent, float newY)
    //{
    //    float elapsedTime = 0;

    //    while (elapsedTime < 1)
    //    {
    //        objectToLerp.text = (Mathf.Lerp(0, percent, (elapsedTime / 1))).ToString("F2") + "%";
    //        elapsedTime += (Time.deltaTime * speed) / percent * 40;
    //        yield return new WaitForEndOfFrame();
    //    }

    //    //float startTime = 0;
    //    //float endTime = Time.time + textIncreaseSpeed;

    //    //while (Time.time < endTime)
    //    //{
    //    //    if (startTime < percent)
    //    //    {
    //    //        startTime += Time.deltaTime;
    //    //    }
    //    //    else
    //    //    {
    //    //        startTime = percent;
    //    //    }


    //    //    objectToLerp.text = startTime.ToString("F2") + "%";

    //    //    yield return new WaitForEndOfFrame();
    //    //}

    //    objectToLerp.text = percent.ToString("F2") + "%";

    //    StartCoroutine(ShowGameObject(winnerUI, winnerUIDelay));
    //    StartCoroutine(ShowGameObject(finalUI, finalUIDelay));

    //}

    protected IEnumerator ShowGameObject(GameObject go, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        go.SetActive(true);
    }
}
