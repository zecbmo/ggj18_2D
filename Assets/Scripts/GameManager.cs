using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {PreGame, InGame, EndGame };

public class GameManager : Singleton<GameManager>
{
    public GameState gameState = GameState.InGame;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
