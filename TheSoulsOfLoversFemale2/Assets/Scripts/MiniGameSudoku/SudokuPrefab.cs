using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PrefabState
{
    Default,
    Change,
    Right,
    False
}

public class SudokuPrefab
{

    private int _row;
    private int _column;
    private GameObject _instance;

    public SudokuPrefab(GameObject instance, int row, int column)
    {
        Instance = instance;
        Row = row;
        Column = column;
    }

    public bool isChangable = true;

    public PrefabState _state;

    public void SetState(PrefabState state)
    {
        _state = state;
        switch (state)
        {
            case PrefabState.Default:
                _instance.GetComponent<Image>().color = new Color(0.79f, 0.79f, 0.75f);
                break;
            case PrefabState.Change:
                _instance.GetComponent<Image>().color = new Color(0.65f, 0.53f, 0.64f);
                break;
            case PrefabState.Right:
                _instance.GetComponent<Image>().color = new Color(0.58f, 0.70f, 0.66f);
                break;
            case PrefabState.False:
                _instance.GetComponent<Image>().color = new Color(0.70f, 0.61f, 0.58f);
                break;
        }
    }

    public bool TryGetTextByNumber(string name, out TMP_Text text)
    {
        text = null;
        TMP_Text[] texts = _instance.GetComponentsInChildren<TMP_Text>();
        foreach(var currentText in texts)
        {
            if (currentText.name.Equals(name))
            {
                text = currentText;
                return true;
            }
        }
        return false;
    }

    public int Row { get => _row; set => _row = value; }
    public int Column { get => _column; set => _column = value; }
    public GameObject Instance { get => _instance; set => _instance = value; }

    public void setHoverMode()
    {
        _instance.GetComponent<Image>().color = new Color(0.70f, 0.67f, 0.61f);
    }

    public void unsetHoverMode()
    {
        _instance.GetComponent<Image>().color = new Color(0.79f, 0.79f, 0.75f);
        this.SetState(_state);
    }

    public int Number;
    public void SetNumber(int num)
    {
        if (TryGetTextByNumber("Value", out TMP_Text text))
        {
            Number = num;
            text.text = num.ToString();
            for (int i = 1; i < 10; i++)
            {
                if (TryGetTextByNumber($"Num_{i}", out TMP_Text textSN))
                {
                    textSN.text = "";
                }
            }
        }
    }

    public void RemoveNumber()
    {
        TMP_Text[] texts = _instance.GetComponentsInChildren<TMP_Text>();
        foreach (var currentText in texts)
        {
            currentText.text = "";
        }
    }

    public void SetSmallNumber(int num)
    {
        if (TryGetTextByNumber($"Num_{num}", out TMP_Text text))
        {
            text.text = num.ToString();
            if (TryGetTextByNumber("Value", out TMP_Text textN))
            {
                textN.text = "";
            }
        }
    }
}
