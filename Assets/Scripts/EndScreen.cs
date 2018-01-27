using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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



    // Use this for initialization
    void Start ()
    {
        blueScale = blueIndicator.transform.localScale;
        redScale = redIndicator.transform.localScale;

        blueIndicator.transform.localScale = new Vector3(blueScale.x, 0, blueScale.z);
        redIndicator.transform.localScale = new Vector3(redScale.x, 0, redScale.z);


        UpdateIndicator(blueIndicator, blueScale, 90.0f);
        UpdateIndicator(redIndicator, redScale, 100.0f);
        StartCoroutine(UpdateText(blueText, 90.0f, textIncreaseSpeed));
        StartCoroutine(UpdateText(redText, 100.0f, textIncreaseSpeed));

    }

    // Update is called once per frame
    void UpdateIndicator(GameObject indicator, Vector3 finalScale, float percentageFilled)
    {
        //get the percentage value of the currentScale
        float newY = (finalScale.y / 100.0f) * percentageFilled;
        Vector3 newScale = new Vector3(finalScale.x, newY, finalScale.z);

        StartCoroutine(MathUtil.ScaleLerp(indicator, newScale, textIncreaseSpeed));
    }

    public IEnumerator UpdateText(Text objectToLerp, float percent, float speed)
    {
        float elapsedTime = 0;

        while (elapsedTime < 1)
        {
            objectToLerp.text = (Mathf.Lerp(0, percent, (elapsedTime / 1))).ToString("F2") + "%";
            elapsedTime += (Time.deltaTime * speed) / percent * 40;
            yield return new WaitForEndOfFrame();
        }

        //float startTime = 0;
        //float endTime = Time.time + textIncreaseSpeed;

        //while (Time.time < endTime)
        //{
        //    if (startTime < percent)
        //    {
        //        startTime += Time.deltaTime;
        //    }
        //    else
        //    {
        //        startTime = percent;
        //    }


        //    objectToLerp.text = startTime.ToString("F2") + "%";

        //    yield return new WaitForEndOfFrame();
        //}

        objectToLerp.text = percent.ToString("F2") + "%";

        StartCoroutine(ShowGameObject(winnerUI, winnerUIDelay));
        StartCoroutine(ShowGameObject(finalUI, finalUIDelay));

    }

    protected IEnumerator ShowGameObject(GameObject go, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        go.SetActive(true);
    }
}
