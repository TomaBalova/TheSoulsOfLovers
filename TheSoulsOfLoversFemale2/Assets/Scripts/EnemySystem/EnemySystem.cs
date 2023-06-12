using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySystem : MonoBehaviour, IDataPersistence
{
    [SerializeField] string miniGame;
    [SerializeField] int difficultyLevel;
    [SerializeField] private string enemyId;

    private bool isDefeated = false;
    private SpriteRenderer visual;

    private void Awake()
    {
        visual = this.gameObject.GetComponent<SpriteRenderer>();
    }

    public void LoadData(GameData gameData)
    {
        for (int i = 0; i < gameData.EnemiesID.Count; i++)
        {
            if (gameData.EnemiesID[i].Equals(enemyId))
            {
                this.gameObject.SetActive(false);
                Debug.Log("Объект скрыт");
            }
        }
    }

    public void SaveData(ref GameData gameData)
    {
    }

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        enemyId = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (miniGame.Equals("Maze"))
            {
                Debug.Log("Лабиринт");
                MiniGameSettings.enemyId = enemyId;
                MiniGameSettings.DifficultyLevel = difficultyLevel;
                MiniGameSettings.PlayerPosition = collision.transform.position;
                SceneManager.LoadScene(2, LoadSceneMode.Single);
            }
            if (miniGame.Equals("Sudoku"))
            {
                Debug.Log("Судоку");
                MiniGameSettings.enemyId = enemyId;
                MiniGameSettings.DifficultyLevel = difficultyLevel;
                MiniGameSettings.PlayerPosition = collision.transform.position;
                SceneManager.LoadScene(3, LoadSceneMode.Single);
            }
        }
    }

}
