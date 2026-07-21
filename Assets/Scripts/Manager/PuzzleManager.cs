using System;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;
    public Dictionary<PuzzleID, PuzzleState> puzzleData { get; private set; }

    public Action<PuzzleID, PuzzleState> OnPuzzleStateChanged;

    private void Awake()
    {
        Instance = this;

        puzzleData = new Dictionary<PuzzleID, PuzzleState>();

        foreach (PuzzleID puzzle in Enum.GetValues(typeof(PuzzleID)))
        {
            puzzleData.Add(puzzle, PuzzleState.NotBegin);
        }
    }

    public void SetPuzzleState(PuzzleID puzzleID, PuzzleState newState)
    {

        if (!puzzleData.TryGetValue(puzzleID, out PuzzleState curState)) return;

        puzzleData[puzzleID] = newState;

        OnPuzzleStateChanged?.Invoke(puzzleID, newState);

    }

    public PuzzleState GetPuzzleState(PuzzleID puzzleID)
    {
        puzzleData.TryGetValue(puzzleID, out PuzzleState curState);
        return curState;

    }
}


public enum PuzzleID
{
    Cauldron,
    BabyJar
}

public enum PuzzleState
{
    NotBegin,
    HalfWayCompleted,
    Completed
}