using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ChessSolver : MonoBehaviour
{
    public const int BOARDLENGTH = 8;
    public readonly Vector2Int[] PAWNTHREAT = { new Vector2Int(1, 1), new Vector2Int(1, -1) };
    //public readonly Vector2Int[] PAWNTHREAT = { new Vector2Int(1, 1), new Vector2Int(1, -1) };

    //public readonly Vector2Int[] PAWNTHREAT = { new Vector2Int(1, 1), new Vector2Int(1, -1) };

    //public readonly Vector2Int[] PAWNTHREAT = { new Vector2Int(1, 1), new Vector2Int(1, -1) };

    //public readonly Vector2Int[] PAWNTHREAT = { new Vector2Int(1, 1), new Vector2Int(1, -1) };


    public ChessPiece[,] board = new ChessPiece[BOARDLENGTH, BOARDLENGTH];
    public ChessThreat[,] threatBoard = new ChessThreat[BOARDLENGTH, BOARDLENGTH];

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
                board[i,j] = CharToChessPiece(boardInfoChar[i + j], new Vector2Int(i, j));
                Debug.Log(board[i, j]);
            }
        }
    }

    private ChessPiece CharToChessPiece(char chessChar, Vector2Int location)
    {
        switch (chessChar)
        {
            case ('p'):
                return new ChessPiece(ChessPieceType.pawn, ChessPieceTeam.white, location);
            case ('P'):
                return new ChessPiece(ChessPieceType.pawn, ChessPieceTeam.black, location);
            case ('b'):
                return new ChessPiece(ChessPieceType.bishop, ChessPieceTeam.white, location);
            case ('B'):
                return new ChessPiece(ChessPieceType.bishop, ChessPieceTeam.black, location);
            case ('n'):
                return new ChessPiece(ChessPieceType.knight, ChessPieceTeam.white, location);
            case ('N'):
                return new ChessPiece(ChessPieceType.knight, ChessPieceTeam.black, location);
            case ('r'):
                return new ChessPiece(ChessPieceType.rook, ChessPieceTeam.white, location);
            case ('R'):
                return new ChessPiece(ChessPieceType.rook, ChessPieceTeam.black, location);
            case ('q'):
                return new ChessPiece(ChessPieceType.queen, ChessPieceTeam.white, location);
            case ('Q'):
                return new ChessPiece(ChessPieceType.queen, ChessPieceTeam.black, location);
            case ('k'):
                return new ChessPiece(ChessPieceType.king, ChessPieceTeam.white, location);
            case ('K'):
                return new ChessPiece(ChessPieceType.king, ChessPieceTeam.black, location);
            default:
                return new ChessPiece(ChessPieceType.none, ChessPieceTeam.none, new Vector2Int(-1, -1));
        }
    }

    public Vector2Int[] GetAllNextMoves(int length, int width)
    {
        ChessPiece chessPiece = board[length, width];
        List<Vector2Int> possibleLocations = new List<Vector2Int>();
        switch (chessPiece.chessPieceType)
        {
            case (ChessPieceType.pawn):
                if (chessPiece.team == ChessPieceTeam.white)
                {
                    if (length + 1 < 8 && board[length + 1, width].chessPieceType == ChessPieceType.none)
                    {
                        possibleLocations.Add(new Vector2Int(length + 1, width));
                    }
                }
                else
                {
                    if (length - 1 > 0 && board[length - 1, width].chessPieceType == ChessPieceType.none)
                    {
                        possibleLocations.Add(new Vector2Int(length - 1, width));
                    }
                }
                break;
        }
            
    }

    public Vector2Int[] GetAllNextMoves(int length, int width)
    {
        ChessPiece chessPiece = board[length, width];
        List<Vector2Int> possibleLocations = new List<Vector2Int>();
        switch (chessPiece.chessPieceType)
        {
            case (ChessPieceType.pawn):
                if (chessPiece.team == ChessPieceTeam.white)
                {
                    if (length + 1 < 8 && board[length + 1, width].chessPieceType == ChessPieceType.none)
                    {
                        possibleLocations.Add(new Vector2Int(length + 1, width));
                    }
                }
                else
                {
                    if (length - 1 > 0 && board[length - 1, width].chessPieceType == ChessPieceType.none)
                    {
                        possibleLocations.Add(new Vector2Int(length - 1, width));
                    }
                }
                break;
        }

    }

    public List<ChessThreat> CalculateThreatSpaces(ChessPiece chessPiece)
    {
        switch (chessPiece.chessPieceType)
        {
            case (ChessPieceType.pawn):
                if (chessPiece.team == ChessPieceTeam.black)
                {
                    List<Vector2Int> threatLocations = new List<Vector2Int>();
                    Vector2Int temp;
                    temp = chessPiece.location + new Vector2Int(1, 1);
                    if (IsLocationValid(temp)) { threatLocations.Add(temp); }


                }
                break;
        }
    }

    public bool Check()
    {

    }

    public bool CheckMate()
    {

    }

    public bool IsLocationValid(Vector2Int location)
    {
        if (location.x < 8 && location.x >= 0 && location.y < 8 && location.y > 0 && board[location.x, location.y].chessPieceType == ChessPieceType.none)
        {
            return true;
        }
        else
        {
            return false;
        }
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
    public Vector2Int location;

    public ChessPiece(ChessSolver.ChessPieceType chessPieceType, ChessSolver.ChessPieceTeam team, Vector2Int location)
    {
        this.chessPieceType = chessPieceType;
        this.team = team;
        this.location = location;
    }

    public override string ToString()
    {
        if (chessPieceType == ChessSolver.ChessPieceType.none || team == ChessSolver.ChessPieceTeam.none) { return "Empty space"; }
        return $"{team} {chessPieceType} ({location.x}, {location.y})";
    }
}

public struct ChessThreat
{
    public List<ChessPiece> chessPieces;
    public Vector2Int location;

    public ChessThreat(List<ChessPiece> chessPieces, Vector2Int location)
    {
        this.chessPieces = chessPieces;
        this.location = location;
    }

    public bool IsLocationThreatened(ChessSolver.ChessPieceTeam chessPieceTeam)
    {
        if (chessPieceTeam == ChessSolver.ChessPieceTeam.white)
        {
            foreach (ChessPiece chessPiece in chessPieces)
            {
                if (chessPiece.team == ChessSolver.ChessPieceTeam.black)
                {
                    return true;
                }
            }
            return false;
        }
        else if (chessPieceTeam == ChessSolver.ChessPieceTeam.black)
        {
            foreach (ChessPiece chessPiece in chessPieces)
            {
                if (chessPiece.team == ChessSolver.ChessPieceTeam.white)
                {
                    return true;
                }
            }
            return false;
        }
        return false;
    }
}
