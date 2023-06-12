using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLoc1 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameData gameData = DataPersistenceManager.instance.gameData;
            gameData.playerPos = collision.transform.position;
            gameData.playerPos.y -= 1;
            DataPersistenceManager.instance.fileDataHandler.Save(gameData);
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }
}
