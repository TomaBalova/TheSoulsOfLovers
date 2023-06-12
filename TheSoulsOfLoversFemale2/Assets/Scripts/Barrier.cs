using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] List<string> enemys;
    [SerializeField] BoxCollider2D barrier;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int count = 0;
        if (collision.gameObject.CompareTag("Player"))
        {
            if (enemys.Count <= DataPersistenceManager.instance.gameData.EnemiesID.Count)
            {
                for (int i = 0; i < enemys.Count; i++)
                {
                    for (int j = 0; j < DataPersistenceManager.instance.gameData.EnemiesID.Count; j++)
                        if (enemys[i] == DataPersistenceManager.instance.gameData.EnemiesID[j])
                        {
                            count++;
                        }
                }
            }
            if (count == enemys.Count)
            {
                barrier.enabled = false;
                this.gameObject.GetComponent<Interactable>().enabled = false;
            }
        }
    }
}
