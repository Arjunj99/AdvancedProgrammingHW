using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ChessSolver : MonoBehaviour
{
    public const int BOARDLENGTH = 8;
    public ChessPiece[,] board = new ChessPiece[BOARDLENGTH, BOARDLENGTH];

    // Start is called before the first frame update
    void Start()
    {
        ParseFile("chess_mateInOne.txt");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool ReadChessFile(string fileName)
    {
        try
        {
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader sr = new StreamReader($"{Application.dataPath}/{fileName}"))
            {
                string line;
                // Read and display lines from the file until the end of
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    Debug.Log(line);
                }
            }
            return true;
        }
        catch (Exception e)
        {
            // Let the user know what went wrong.
            Debug.Log("The file could not be read:");
            Debug.Log(e.Message);
            return false;
        }
    }



    void ParseFile(string filePath)
    {
        string trueFilePath = Application.streamingAssetsPath + "\\" + filePath;

        string text = File.ReadAllText(trueFilePath);
        PopulateBoard(text);
    }

    private void PopulateBoard(string boardInfo)
    {
        char[] boardInfoChar = boardInfo.ToCharArray();
        for (int i = 0; i < BOARDLENGTH; i++)
        {
            for (int j = 0; j < BOARDLENGTH; j++)
            {
                board[i,j] = CharToChessPiece(boardInfoChar[i + j]);
                Debug.Log(board[i, j]);
            }
        }
    }

    private ChessPiece CharToChessPiece(char chessChar)
    {
        switch (chessChar)
        {
            case ('p'):
                return new ChessPiece(ChessPieceType.pawn, ChessPieceTeam.white);
            case ('P'):
                return new ChessPiece(ChessPieceType.pawn, ChessPieceTeam.black);
            case ('b'):
                return new ChessPiece(ChessPieceType.bishop, ChessPieceTeam.white);
            case ('B'):
                return new ChessPiece(ChessPieceType.bishop, ChessPieceTeam.black);
            case ('n'):
                return new ChessPiece(ChessPieceType.knight, ChessPieceTeam.white);
            case ('N'):
                return new ChessPiece(ChessPieceType.knight, ChessPieceTeam.black);
            case ('r'):
                return new ChessPiece(ChessPieceType.rook, ChessPieceTeam.white);
            case ('R'):
                return new ChessPiece(ChessPieceType.rook, ChessPieceTeam.black);
            case ('q'):
                return new ChessPiece(ChessPieceType.queen, ChessPieceTeam.white);
            case ('Q'):
                return new ChessPiece(ChessPieceType.queen, ChessPieceTeam.black);
            case ('k'):
                return new ChessPiece(ChessPieceType.king, ChessPieceTeam.white);
            case ('K'):
                return new ChessPiece(ChessPieceType.king, ChessPieceTeam.black);
            default:
                return new ChessPiece(ChessPieceType.none, ChessPieceTeam.none);
        }
    }

    public Vector2Int[] GetAllNextMoves(int length, int width)
    {
        ChessPiece chessPiece = board[length, width];
        List<Vector2Int> nextMoveArray = new List<Vector2Int>();
        if (chessPiece.chessPieceType == ChessPieceType.pawn && chespi)
    }

    public enum ChessPieceType
    {
        pawn,
        bishop,
        knight,
        rook,
        queen,
        king,
        none
    }

    public enum ChessPieceTeam
    {
        black,
        white,
        none
    }
}

public struct ChessPiece
{
    public ChessSolver.ChessPieceType chessPieceType;
    public ChessSolver.ChessPieceTeam team;

    public ChessPiece(ChessSolver.ChessPieceType chessPieceType, ChessSolver.ChessPieceTeam team)
    {
        this.chessPieceType = chessPieceType;
        this.team = team;
    }

    public override string ToString()
    {
        if (chessPieceType == ChessSolver.ChessPieceType.none || team == ChessSolver.ChessPieceTeam.none) { return "Empty space"; }
        return $"{team} {chessPieceType}";
    }
}
