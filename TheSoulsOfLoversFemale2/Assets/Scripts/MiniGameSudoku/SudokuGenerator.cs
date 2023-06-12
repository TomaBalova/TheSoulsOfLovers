using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuGenerator
{
    public static void CreateSudokuGame(out Sudoku finalSudoku, out Sudoku gameSudoku)
    {
        _finalSudoku = null;
        Sudoku sudoku = new Sudoku();
        CreateRandomGroups(sudoku);
        if (TryToSolve(sudoku))
        {
            sudoku = _finalSudoku;
        }
        else
        {
            throw new System.Exception("Что-то пошло не так");
        }
        finalSudoku = sudoku;
        gameSudoku = RemoveSomeRandomNumbers(sudoku);
    }

    private static Sudoku RemoveSomeRandomNumbers(Sudoku sudoku)
    {
        Sudoku newSudoku = new Sudoku();
        newSudoku.Values = (int[,])sudoku.Values.Clone();
        List<Tuple<int,int>> values = GetValues();
        int EndValueIndex = 10;
        if (MiniGameSettings.DifficultyLevel == 1) EndValueIndex = 71;
        if (MiniGameSettings.DifficultyLevel == 2) EndValueIndex = 61;
        if (MiniGameSettings.DifficultyLevel == 3) EndValueIndex = 51;
        bool isFinish = false;
        while (!isFinish)
        {
            int index = UnityEngine.Random.Range(0, values.Count);
            var searchedIndex = values[index];
            Sudoku nextSudoku = new Sudoku();
            nextSudoku.Values = (int[,])newSudoku.Values.Clone();
            nextSudoku.Values[searchedIndex.Item1, searchedIndex.Item2] = 0;

            if(TryToSolve(nextSudoku, true))
            {
                newSudoku = nextSudoku;
            }
            values.RemoveAt(index);

            if (values.Count < EndValueIndex)
            {
                isFinish = true;
            }
        }
        return newSudoku;
    }

    private static List<Tuple<int, int>> GetValues()
    {
        List<Tuple<int, int>> values = new List<Tuple<int, int>>();
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                values.Add(new Tuple<int, int>(i, j));
            }
        }
        return values;
    }

    private static Sudoku _finalSudoku;

    public static void CreateRandomGroups(Sudoku sudoku)
    {
        List<int> values = new List<int>() { 0, 1, 2};
        int index = UnityEngine.Random.Range(0, values.Count);
        InsertRandomGroups(sudoku, 1 + values[index]);
        values.RemoveAt(index);

        index = UnityEngine.Random.Range(0, values.Count);
        InsertRandomGroups(sudoku, 4 + values[index]);
        values.RemoveAt(index);

        index = UnityEngine.Random.Range(0, values.Count);
        InsertRandomGroups(sudoku, 7 + values[index]);
    }

    public static void InsertRandomGroups(Sudoku sudoku, int group)
    {
        sudoku.GetGroupIndex(group, out int startRow, out int startColumn);
        List<int> values = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        for (int row = startRow; row < startRow + 3; row++)
        {
            for (int column = startColumn; column < startColumn + 3; column++)
            {
                int index = UnityEngine.Random.Range(0, values.Count);
                sudoku.Values[row, column] = values[index];
                values.RemoveAt(index);
            }
        }
    }

    private static bool TryToSolve(Sudoku sudoku, bool OnlyOne = false)
    {
        // Поиск пустых ячеек, в которые можно вставить число
        if (HasEmptyFieldsToFill(sudoku, out int row, out int column, OnlyOne))
        {
            List<int> possibleValues = GetPossibleValues(sudoku, row, column);
            foreach (var possibleValue in possibleValues)
            {
                Sudoku nextSudoku = new Sudoku();
                nextSudoku.Values = (int[,])sudoku.Values.Clone();
                nextSudoku.Values[row, column] = possibleValue;
                if (TryToSolve(nextSudoku, OnlyOne))
                {
                    return true;
                }
            }
        }

        // Проверка на наличие пустых ячеек
        if (HasEmptyField(sudoku))
        {
            return false;
        }
        _finalSudoku = sudoku;
        return true;
    }

    private static bool HasEmptyField(Sudoku sudoku)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (sudoku.Values[i, j] == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static List<int> GetPossibleValues(Sudoku sudoku, int row, int column)
    {
        List<int> possibleValues = new List<int>();
        for (int value = 1; value < 10; value++)
        {
            if (sudoku.IsPossibleNumberInPosition(value, row, column))
            {
                possibleValues.Add(value);
            }
        }
        return possibleValues;
    }

    private static bool HasEmptyFieldsToFill(Sudoku sudoku, out int row, out int column, bool OnlyOne = false)
    {
        row = 0;
        column = 0;
        int ammountOfPossibleValues = 10;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if(sudoku.Values[i,j] == 0)
                {
                    int currentAmount = GetPossibleAmountOfValues(sudoku, i, j);
                    if (currentAmount != 0)
                    {
                        if (currentAmount < ammountOfPossibleValues)
                        {
                            ammountOfPossibleValues = currentAmount;
                            row = i;
                            column = j;
                        }
                    }
                }
            }
        }
        if (OnlyOne)
        {
            if (ammountOfPossibleValues == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        if (ammountOfPossibleValues == 10)
        {
            return false;
        }
        return true;
    }

    public static int GetPossibleAmountOfValues(Sudoku sudoku, int row, int column)
    {
        int amount = 0;
        for (int k = 1; k < 10; k++)
        {
            if (sudoku.IsPossibleNumberInPosition(k, row, column))
            {
                amount++;
            }
        }
        return amount;
    }
}
