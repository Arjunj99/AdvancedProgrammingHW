using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ChessSolver : MonoBehaviour
{
    public const int BOARDLENGTH = 8;
    public readonly Vector2Int[] PAWNTHREAT = { new Vector2Int(1, 1), new Vector2Int(1, -1) };
    public readonly Vector2Int[] KNIGHTTHREAT = { new Vector2Int(2, 1), new Vector2Int(2, -1), new Vector2Int(1, 2), new Vector2Int(1, -2), new Vector2Int(-2, 1), new Vector2Int(-2, -1), new Vector2Int(-1, 2), new Vector2Int(-1, -2) };
    public readonly Vector2Int[] KINGTHREAT = { new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 1), new Vector2Int(-1, -1), new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };


    public ChessPiece[,] board = new ChessPiece[BOARDLENGTH, BOARDLENGTH];
    public ChessThreat[,] threatBoard = new ChessThreat[BOARDLENGTH, BOARDLENGTH];
    public string chessFileName;
    public char[] debug ;

    // Start is called before the first frame update
    void Start()
    {
        ParseFile(chessFileName);
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
                    //Debug.Log(line);
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
        PopulateBoards(text);
    }

    private void PopulateBoards(string boardInfo)
    {
        //Debug.Log(boardInfo);
        char[] boardInfoChar = boardInfo.ToCharArray();

        string s = "";
        foreach (char c in boardInfoChar)
        {
            if (c == 'p' || c == 'P' || c == 'b' || c == 'B' || c == 'n' || c == 'N' || c == 'r' || c == 'R' || c == 'q' || c == 'Q' || c == 'k' || c == 'K' || c == '.')
            {
                s += c;
            }
            
        }

        debug = s.ToCharArray();

        Debug.Log(s.ToCharArray().Length + "LENGTH");
        for (int i = 0; i < BOARDLENGTH; i++)
        {
            for (int j = 0; j < BOARDLENGTH; j++)
            {
                Debug.Log(boardInfoChar[i + j]);
                board[i,j] = CharToChessPiece(s.ToCharArray()[(8*i)+j], new Vector2Int(i, j));
                Debug.Log(board[i, j]);
            }
        }

        for (int i = 0; i < BOARDLENGTH; i++)
        {
            for (int j = 0; j < BOARDLENGTH; j++)
            {
                threatBoard[i, j] = new ChessThreat(new Vector2Int(i, j));
            }
        }



        for (int i = 0; i < BOARDLENGTH; i++)
        {
            for (int j = 0; j < BOARDLENGTH; j++)
            {
                if (board[i, j].chessPieceType != ChessPieceType.none)
                {
                    CalculateThreatSpaces(board[i, j]);
                }
            }
        }

        if (Check(ChessPieceTeam.black)) { Debug.Log("Black is in Check"); }
        else if (Check(ChessPieceTeam.white)) { Debug.Log("White is in Check"); }
        else { Debug.Log("No one is in Check"); }

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

    //public Vector2Int[] GetAllNextMoves(int length, int width)
    //{
    //    ChessPiece chessPiece = board[length, width];
    //    List<Vector2Int> possibleLocations = new List<Vector2Int>();
    //    switch (chessPiece.chessPieceType)
    //    {
    //        case (ChessPieceType.pawn):
    //            if (chessPiece.team == ChessPieceTeam.white)
    //            {
    //                if (length + 1 < 8 && board[length + 1, width].chessPieceType == ChessPieceType.none)
    //                {
    //                    possibleLocations.Add(new Vector2Int(length + 1, width));
    //                }
    //            }
    //            else
    //            {
    //                if (length - 1 > 0 && board[length - 1, width].chessPieceType == ChessPieceType.none)
    //                {
    //                    possibleLocations.Add(new Vector2Int(length - 1, width));
    //                }
    //            }
    //            break;
    //    }
            
    //}

    //public Vector2Int[] GetAllNextMoves(int length, int width)
    //{
    //    ChessPiece chessPiece = board[length, width];
    //    List<Vector2Int> possibleLocations = new List<Vector2Int>();
    //    switch (chessPiece.chessPieceType)
    //    {
    //        case (ChessPieceType.pawn):
    //            if (chessPiece.team == ChessPieceTeam.white)
    //            {
    //                if (length + 1 < 8 && board[length + 1, width].chessPieceType == ChessPieceType.none)
    //                {
    //                    possibleLocations.Add(new Vector2Int(length + 1, width));
    //                }
    //            }
    //            else
    //            {
    //                if (length - 1 > 0 && board[length - 1, width].chessPieceType == ChessPieceType.none)
    //                {
    //                    possibleLocations.Add(new Vector2Int(length - 1, width));
    //                }
    //            }
    //            break;
    //    }

    //}

    public void CalculateThreatSpaces(ChessPiece chessPiece)
    {
        List<Vector2Int> threatLocations = new List<Vector2Int>();
        Vector2Int temp = new Vector2Int(-1, -1);

        switch (chessPiece.chessPieceType)
        {
            case (ChessPieceType.pawn):
                foreach (Vector2Int threat in PAWNTHREAT)
                {
                    if (chessPiece.team == ChessPieceTeam.black) { temp = chessPiece.location + threat; }
                    else if (chessPiece.team == ChessPieceTeam.white) { temp = -chessPiece.location + threat; }
                        
                    if (IsAttackLocationValid(temp, chessPiece)) { threatLocations.Add(temp); }
                }
                break;
            case (ChessPieceType.bishop):
                LineHelper(temp, chessPiece, new Vector2Int(1, 1), threatLocations);
                LineHelper(temp, chessPiece, new Vector2Int(-1, 1), threatLocations);
                LineHelper(temp, chessPiece, new Vector2Int(1, -1), threatLocations);
                LineHelper(temp, chessPiece, new Vector2Int(-1, -1), threatLocations);
                break;
            case (ChessPieceType.knight):
                foreach (Vector2Int threat in KNIGHTTHREAT)
                {
                    temp = chessPiece.location + threat;

                    if (IsAttackLocationValid(temp, chessPiece)) { threatLocations.Add(temp); }
                }
                break;
            case (ChessPieceType.rook):
                LineHelper(temp, chessPiece, new Vector2Int(0, 1), threatLocations);
                LineHelper(temp, chessPiece, new Vector2Int(0, -1), threatLocations);
                LineHelper(temp, chessPiece, new Vector2Int(1, 0), threatLocations);
                LineHelper(temp, chessPiece, new Vector2Int(-1, 0), threatLocations);
                break;
            case (ChessPieceType.queen):
                LineHelper(temp, chessPiece, new Vector2Int(1, 1), threatLocations);
                LineHelper(temp, chessPiece, new Vector2Int(-1, 1), threatLocations);
                LineHelper(temp, chessPiece, new Vector2Int(1, -1), threatLocations);
                LineHelper(temp, chessPiece, new Vector2Int(-1, -1), threatLocations);
                LineHelper(temp, chessPiece, new Vector2Int(0, 1), threatLocations);
                LineHelper(temp, chessPiece, new Vector2Int(0, -1), threatLocations);
                LineHelper(temp, chessPiece, new Vector2Int(1, 0), threatLocations);
                LineHelper(temp, chessPiece, new Vector2Int(-1, 0), threatLocations);
                break;
            case (ChessPieceType.king):
                foreach (Vector2Int threat in KINGTHREAT)
                {
                    temp = chessPiece.location + threat;

                    if (IsAttackLocationValid(temp, chessPiece)) { threatLocations.Add(temp); }
                }
                break;
        }

        foreach (Vector2Int threat in threatLocations)
        {
            threatBoard[threat.x, threat.y].AddPiece(chessPiece);
        }
    }

    public bool Check(ChessPieceTeam team)
    {
        if (team == ChessPieceTeam.black)
        {
            Debug.Log("Black");
            for (int i = 0; i < BOARDLENGTH; i++)
            {
                for (int j = 0; j < BOARDLENGTH; j++)
                {
                    if (board[i, j].chessPieceType == ChessPieceType.king && board[i, j].team == ChessPieceTeam.black)
                    {
                        Debug.Log($"King is at ({i}, {j})");
                        if (threatBoard[i, j].IsLocationThreatened(ChessPieceTeam.black))
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }
        }
        else if (team == ChessPieceTeam.white)
        {
            Debug.Log("White");
            for (int i = 0; i < BOARDLENGTH; i++)
            {
                for (int j = 0; j < BOARDLENGTH; j++)
                {
                    if (board[i, j].chessPieceType == ChessPieceType.king && board[i, j].team == ChessPieceTeam.white)
                    {
                        Debug.Log($"King is at ({i}, {j})");
                        Debug.Log($"Threat spot is ({threatBoard[i, j].chessPieces.Count})");

                        if (threatBoard[i, j].IsLocationThreatened(ChessPieceTeam.white))
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }
        }
        return false;
    }

    //public bool CheckMate()
    //{

    //}

    public void LineHelper(Vector2Int temp, ChessPiece chessPiece, Vector2Int diagonalValue, List<Vector2Int> threatLocations)
    {
        bool stopOnHit = false;
        temp = chessPiece.location;
        while (IsAttackLocationValid(temp + diagonalValue, chessPiece) && !stopOnHit)
        {
            threatLocations.Add(temp);
            if (board[temp.x, temp.y].chessPieceType != ChessPieceType.none) { stopOnHit = true; }
            temp += diagonalValue;
        }
    }

    public bool IsAttackLocationValid(Vector2Int location, ChessPiece chessPiece)
    {
        if (location.x < 8 && location.x >= 0 && location.y < 8 && location.y > 0 && board[location.x, location.y].team != chessPiece.team)
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

    public ChessThreat(Vector2Int location)
    {
        this.chessPieces = new List<ChessPiece>();
        this.location = location;
    }

    public ChessThreat(List<ChessPiece> chessPieces, Vector2Int location)
    {
        this.chessPieces = chessPieces;
        this.location = location;
    }

    public bool AddPiece(ChessPiece chessPiece)
    {
        chessPieces.Add(chessPiece);
        return true;
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
