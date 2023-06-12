using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState
    {
        Available,
        Current,
        Completed,
        Start,
        Final
    }

public class MazeNode : MonoBehaviour
{
    [SerializeField] GameObject[] walls;
    [SerializeField] GameObject floor;
    [SerializeField] GameObject finalTrigger;

    public void RemoveWall(int wallToRemove)
    {
        walls[wallToRemove].gameObject.SetActive(false);
    }

    public void SetState(NodeState state)
    {
        switch (state)
        {
            case NodeState.Available:
                floor.GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case NodeState.Current:
                floor.GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case NodeState.Completed:
                floor.GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case NodeState.Start:
                floor.SetActive(true);
                floor.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case NodeState.Final:
                finalTrigger.SetActive(true);
                floor.SetActive(true);
                floor.GetComponent<SpriteRenderer>().color = Color.red;
                break;
        }
    }

    public void removeFloor()
    {
        floor.SetActive(false);
    }


}
