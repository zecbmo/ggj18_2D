using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{



    enum Team { Red = -1, None = 0, Blue }
    private Team hoveredTeam = 0;

    [SerializeField]
    float controllerPosition = 4.75f;

    [SerializeField]
    float controllerSpeed = 10f;

    void Update()
    {
        for (int i = 0; i < PlayerManager.Instance().GetNoControllers(); ++i)
        {
            /*float horizontalAxis = InputManager.GetAxis(AxisControls.Horizontal, i);
            if (horizontalAxis != 0)
            {
                UpdateController(horizontalAxis > 0 ? Team.Blue : Team.Red);
                break;
            }*/

            if (InputManager.GetButtonDown(GameControls.Jump, i)) // A button (?)
            {
                // Go to scene and assign player 1 to controller i
                // Debug.Log("GO Team " + hoveredTeam.ToString());

                //PlayerManager.Instance().EnterNewPlayer(i);
                PlayerManager.Instance().waitingNewPlayers = true;
                //SceneManager.LoadScene("GarysScene2");
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("TestScene");
        }

    }

    void UpdateController(Team team)
    {
        hoveredTeam = team;
        StartCoroutine(MathUtil.MoveObjectTowardsLocation(gameObject, new Vector2(controllerPosition * (int)team, transform.position.y), controllerSpeed));
        //transform.position = new Vector2(controllerPosition * (int)team, transform.position.y);
    }

}
