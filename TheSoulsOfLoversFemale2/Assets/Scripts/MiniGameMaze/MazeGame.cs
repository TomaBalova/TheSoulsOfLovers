using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeGame : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        saveData();
        SceneManager.LoadScene(1, LoadSceneMode.Single);

        Debug.Log("Победа");
    }

    public void click_OnGiveUpButton()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        Debug.Log("Вы сдались");
    }

    public void saveData()
    {
        GameData gameData = DataPersistenceManager.instance.gameData;
        for (int i = 0; i < gameData.EnemiesID.Count; i++)
        {
            if (gameData.EnemiesID[i].Equals(MiniGameSettings.enemyId))
            {
                gameData.EnemiesID.RemoveAt(i);
                gameData.EnemiesDef.RemoveAt(i);
            }
        }
        gameData.EnemiesID.Add(MiniGameSettings.enemyId);
        gameData.EnemiesDef.Add(true);
        gameData.playerPos = MiniGameSettings.PlayerPosition;
        

        DataPersistenceManager.instance.fileDataHandler.Save(gameData);
    }

    
}
