using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{

    [SerializeField]
    private int noControllers = 4;

    [SerializeField]
    GameObject playerPrefab;

    private Dictionary<int, int> controllerMap = new Dictionary<int, int>();
    private int availablePlayerId = 0;

    public bool waitingNewPlayers = false;

    public int GetNoControllers()
    {
        return noControllers;
    }

    public int EnterNewPlayer(int controllerId)
    {
        if (!controllerMap.ContainsValue(controllerId))
        {
            controllerMap[availablePlayerId] = controllerId;
            Debug.Log("New Player entered the game! Controller: " + controllerId);
            return availablePlayerId++;
        }
        return -1;
    }

    public int GetControllerByPlayerId(int playerId)
    {
        return controllerMap.ContainsKey(playerId) ? controllerMap[playerId] : -1;
    }


    private void Update()
    {
        if (waitingNewPlayers)
        {
            for (int i = 0; i < noControllers; ++i)
            {
                if (!controllerMap.ContainsValue(i) && InputManager.GetButtonDown(GameControls.Sprint, i))
                {
                    
                    Instantiate(playerPrefab);
                    playerPrefab.GetComponent<MarioMovement>().SetPlayerId(EnterNewPlayer(i));
                }
            }
        }
    }
}
