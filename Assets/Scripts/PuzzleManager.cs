using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private static PuzzlePiece[] puzzlePieces;

    private void Awake()
    {
        puzzlePieces = transform.GetComponentsInChildren<PuzzlePiece>();
    }

    public static void CheckIfPuzzleSolved()
    {
        bool solved = true;
        foreach (PuzzlePiece piece in puzzlePieces)
        {
            solved &= piece.solved;
        }

        if (solved)
        {
            Debug.Log("Puzzle complete!");
        }
    }
}
