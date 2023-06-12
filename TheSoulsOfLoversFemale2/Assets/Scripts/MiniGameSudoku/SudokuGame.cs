using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SudokuGame : MonoBehaviour
{

    [SerializeField] GameObject MainPanel;

    [SerializeField] GameObject SudokuPanel;

    [SerializeField] GameObject ControlPanel;

    [SerializeField] GameObject ControlPrefab;

    [SerializeField] GameObject FieldPrefab;

    [SerializeField] Button TinyNumBtn;

    [SerializeField] Button GiveUpBtn;

    [SerializeField] Button FinishBtn;

    [SerializeField] TMP_Text HealthCount;

    int TryCount = 3;


    void Start()
    {
        CreateSudokuFields();
        CteareSudokuControlFields();
        CreateSudokuGame();
    }

    public void ClickOn_CiveUpButton()
    {
        if(TryCount > 1)
        {
            TryCount--;
            HealthCount.text = TryCount + "/3";
            RestartGame();
        }
        else
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
            Debug.Log("Попытки кончились");
        }
    }

    private void RestartGame()
    {
        Sudoku newSudoku = new Sudoku();
        newSudoku.Values = (int[,])_gameSudoku.Values.Clone();
        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                newSudoku.Values[row, column] = 0;
                SudokuPrefab sudokuField = _sudokuPrefabDict[new Tuple<int, int>(row, column)];
                sudokuField.RemoveNumber();
                sudokuField.SetState(PrefabState.Default);
                sudokuField.Number = 0;
                sudokuField.isChangable = true;
            }
        }
        _gameSudoku = null;
        _finalSudoku = null;

        CreateSudokuGame();
    }

    bool isVictory = false;

    public void ClickOn_FinishButton()
    {
        int errorAmount = 0;
        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                SudokuPrefab fieldSudoku = _sudokuPrefabDict[new Tuple<int, int>(row, column)];

                if (fieldSudoku.isChangable)
                {
                    fieldSudoku.isChangable = false;
                    if (_finalSudoku.Values[row, column] == fieldSudoku.Number)
                    {
                        fieldSudoku.SetState(PrefabState.Right);
                        
                    }
                    else
                    {
                        fieldSudoku.SetState(PrefabState.False);
                        errorAmount++;
                    }
                }
            }
        }
        if (errorAmount == 0)
        {
            isVictory = true;
            saveData();
            SceneManager.LoadScene(1, LoadSceneMode.Single);
            Debug.Log("Победа!");
        }
        else
        {
            ClickOn_CiveUpButton();
            Debug.Log("Проигрыш!");
        }
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
        gameData.EnemiesDef.Add(isVictory);

        gameData.playerPos = MiniGameSettings.PlayerPosition;

        DataPersistenceManager.instance.fileDataHandler.Save(gameData);
    }

    private Sudoku _gameSudoku;
    private Sudoku _finalSudoku;

    private void CreateSudokuGame()
    {
        SudokuGenerator.CreateSudokuGame(out Sudoku finalSudoku, out Sudoku gameSudoku);
        _gameSudoku = gameSudoku;
        _finalSudoku = finalSudoku;
        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                var currentValue = _gameSudoku.Values[row, column];
                if (currentValue != 0)
                {
                    SudokuPrefab sudokuField = _sudokuPrefabDict[new Tuple<int, int>(row, column)];
                    sudokuField.SetNumber(currentValue);
                    sudokuField.isChangable = false;
                }
            }
        }
    }

    private bool isTBtnActive = false;
    public void Click_TinyButton()
    {
        if (isTBtnActive)
        {
            isTBtnActive = false;
            TinyNumBtn.GetComponent<Image>().color = new Color(0.84f, 0.80f, 0.73f);
        }
        else
        {
            isTBtnActive = true;
            TinyNumBtn.GetComponent<Image>().color = new Color(0.70f, 0.67f, 0.61f);
        }
    }

    private Dictionary<Tuple<int, int>, SudokuPrefab> _sudokuPrefabDict = new Dictionary<Tuple<int, int>, SudokuPrefab>();

    

    private void CreateSudokuFields()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                GameObject instance = GameObject.Instantiate(FieldPrefab, SudokuPanel.transform);
                SudokuPrefab sudokuPrefab = new SudokuPrefab(instance, row, column);
                
                if (!_sudokuPrefabDict.ContainsKey(new Tuple<int, int>(row, column)))
                {
                    _sudokuPrefabDict.Add(new Tuple<int, int>(row, column), sudokuPrefab);
                }
                instance.GetComponent<Button>().onClick.AddListener(() => onClick_SudokuPrefab(sudokuPrefab));
            }
        }
    }

    private void CteareSudokuControlFields()
    {
        for (int row = 1; row < 10; row++)
        {
            GameObject instance = GameObject.Instantiate(ControlPrefab, ControlPanel.transform);
            instance.GetComponentInChildren<TMP_Text>().text = row.ToString();
            ControlPrefab controlPrefab = new ControlPrefab();
            controlPrefab.Number = row;
            instance.GetComponent<Button>().onClick.AddListener(() => onClick_ControlPrefab(controlPrefab));
        }
    }

    private SudokuPrefab _currentSudokuPrefab;

    private void onClick_SudokuPrefab(SudokuPrefab sudokuPrefab)
    {
        if (sudokuPrefab.isChangable)
        {
            if (_currentSudokuPrefab != null)
            {
                _currentSudokuPrefab.unsetHoverMode();
            }
            sudokuPrefab.setHoverMode();
            _currentSudokuPrefab = sudokuPrefab;
        }
    }

    private void onClick_ControlPrefab(ControlPrefab controlPrefab)
    {
        if (_currentSudokuPrefab != null)
        {
            if (isTBtnActive)
            {
                _currentSudokuPrefab.SetSmallNumber(controlPrefab.Number);
            }
            else
            {
                _currentSudokuPrefab.SetNumber(controlPrefab.Number);
                _currentSudokuPrefab.SetState(PrefabState.Change);
            }
            
        }
    }
}
