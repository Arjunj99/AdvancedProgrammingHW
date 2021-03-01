using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/// <summary>
/// Solves the Chess Problem (Problem 4)
/// </summary>
public class ChessSolver : MonoBehaviour
{
    #region variables
    public const int BOARDLENGTH = 8;

    public readonly Vector2Int[] PAWNTHREAT = { new Vector2Int(1, 1), new Vector2Int(1, -1) };
    public readonly Vector2Int[] KNIGHTTHREAT = { new Vector2Int(2, 1), new Vector2Int(2, -1), new Vector2Int(1, 2), new Vector2Int(1, -2),
        new Vector2Int(-2, 1), new Vector2Int(-2, -1), new Vector2Int(-1, 2), new Vector2Int(-1, -2) };
    public readonly Vector2Int[] KINGTHREAT = { new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 1), new Vector2Int(-1, -1),
        new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };

    public ChessPiece[,] board = new ChessPiece[BOARDLENGTH, BOARDLENGTH];
    public ChessThreat[,] threatBoard = new ChessThreat[BOARDLENGTH, BOARDLENGTH];
    public string chessFileName;
    #endregion

    #region functions
    // Start is called before the first frame update
    void Start()
    {
        PopulateBoards(ParseFile(chessFileName));
    }

    #region ParsingShowcase
    /// <summary>
    /// Parses the Text File and returns a formatted string containing all chess data.
    /// </summary>
    /// <param name="filePath">The File Name of the Chess Data.</param>
    /// <returns>Formatted Chess Data string.</returns>
    string ParseFile(string filePath)
    {
        // Reads file data and formats as string
        string trueFilePath = Application.streamingAssetsPath + "\\" + filePath;
        string boardInfo = File.ReadAllText(trueFilePath);

        char[] boardInfoChar = boardInfo.ToCharArray();

        // Discards all non-essential characters
        string s = "";
        foreach (char c in boardInfoChar)
        {
            if (c == 'p' || c == 'P' || c == 'b' || c == 'B' || c == 'n' || c == 'N' || c == 'r' || c == 'R' || c == 'q' || c == 'Q' || c == 'k' || c == 'K' || c == '.')
            {
                s += c;
            }
        }

        // Throws Argument Exception if data does not fit board. Returns String if valid.
        if (s.Length == 64) { return s; }
        else { throw new ArgumentException(); }
    }
    #endregion

    #region BoardPopulation
    /// <summary>
    /// Populates the board and threatBoard and then Prints Check Statements.
    /// </summary>
    /// <param name="boardInfo">Chess data in a string format.</param>
    private void PopulateBoards(string boardInfo)
    {
        // Note to self: How to optimize for Locality
        for (int i = 0; i < BOARDLENGTH; i++)
        {
            for (int j = 0; j < BOARDLENGTH; j++)
            {
                // Initializes a new Chess Piece for each location in the board
                board[i,j] = CharToChessPiece(boardInfo.ToCharArray()[(8*i)+j], new Vector2Int(i, j));
            }
        }

        for (int i = 0; i < BOARDLENGTH; i++)
        {
            for (int j = 0; j < BOARDLENGTH; j++)
            {
                // Initializes an empty Chess Threat for each location in the threatboard
                threatBoard[i, j] = new ChessThreat(new Vector2Int(i, j));
            }
        }

        for (int i = 0; i < BOARDLENGTH; i++)
        {
            for (int j = 0; j < BOARDLENGTH; j++)
            {
                if (board[i, j].chessPieceType != ChessPieceType.none)
                {
                    // Reformats data for every non-empty Chess Threat
                    CalculateThreatSpaces(board[i, j]);
                }
            }
        }

        // Prints Check Statement
        if (Check(ChessPieceTeam.black)) { Debug.Log("Black is in Check"); }
        else if (Check(ChessPieceTeam.white)) { Debug.Log("White is in Check"); }
        else { Debug.Log("No one is in Check"); }
    }
    #endregion

    #region ThreatCalculation
    /// <summary>
    /// Calculates Threat Space of each Chess Piece and updates the ThreatBoard.
    /// </summary>
    /// <param name="chessPiece">A chess piece instance.</param>
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

        // Assigns Threat spaces to the threatboard
        foreach (Vector2Int threat in threatLocations)
        {
            threatBoard[threat.x, threat.y].AddPiece(chessPiece);
        }
    }
    #endregion

    #region HelperFunction
    /// <summary>
    /// Returns a ChessPiece instance from a Character and Location.
    /// </summary>
    /// <param name="chessChar">Character denoting chess team and type.</param>
    /// <param name="location">Location denoting chess location on board.</param>
    /// <returns>Chess piece with all data.</returns>
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

    /// <summary>
    /// Returns true if check is possible for corresponding team.
    /// </summary>
    /// <param name="team">The team that check will be checked for.</param>
    /// <returns>True for check, false for not check.</returns>
    public bool Check(ChessPieceTeam team)
    {
        if (team == ChessPieceTeam.black)
        {
            for (int i = 0; i < BOARDLENGTH; i++)
            {
                for (int j = 0; j < BOARDLENGTH; j++)
                {
                    if (board[i, j].chessPieceType == ChessPieceType.king && board[i, j].team == ChessPieceTeam.black)
                    {
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
            for (int i = 0; i < BOARDLENGTH; i++)
            {
                for (int j = 0; j < BOARDLENGTH; j++)
                {
                    if (board[i, j].chessPieceType == ChessPieceType.king && board[i, j].team == ChessPieceTeam.white)
                    {
                        if (threatBoard[i, j].IsLocationThreatened(ChessPieceTeam.white))
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }
        }
        throw new ArgumentException();
    }

    /// <summary>
    /// Line Helper used to calculate threats for pieces with line-base movement, like rooks, bishops and queens.
    /// </summary>
    /// <param name="temp">Temp value used to store location data.</param>
    /// <param name="chessPiece">Corresponding chess pieces.</param>
    /// <param name="lineValue">What increment should each line be.</param>
    /// <param name="threatLocations">Stores all threat locations.</param>
    public void LineHelper(Vector2Int temp, ChessPiece chessPiece, Vector2Int lineValue, List<Vector2Int> threatLocations)
    {
        bool stopOnHit = false;
        temp = chessPiece.location;
        while (IsAttackLocationValid(temp + lineValue, chessPiece) && !stopOnHit) // Stops when you reach border, space before friendly unit or enemy unit
        {
            threatLocations.Add(temp);
            if (board[temp.x, temp.y].chessPieceType != ChessPieceType.none) { stopOnHit = true; }
            temp += lineValue;
        }
    }

    /// <summary>
    /// Returns true if Location is valid (In board and does not contain an enemy item).
    /// </summary>
    /// <param name="location">The Location to Test.</param>
    /// <param name="chessPiece">The Piece to Test.</param>
    /// <returns></returns>
    public bool IsAttackLocationValid(Vector2Int location, ChessPiece chessPiece)
    {
        if (location.x < 8 && location.x >= 0 && location.y < 8 && location.y > 0 && board[location.x, location.y].team != chessPiece.team) return true;
        else return false;
    }
    #endregion

    #region FutureFunctions
    public bool CheckMate(ChessPieceTeam chessPieceTeam)
    {
        throw new System.NotImplementedException();
    }

    public void TakeNextStep()
    {
        throw new System.NotImplementedException();
    }

    public bool PreventCheckMate(ChessPieceTeam chessPieceTeam)
    {
        throw new System.NotImplementedException();
    }
    #endregion
    #endregion

    #region Enums
    /// <summary>
    /// Types for all Chess Pieces (Use none for empty space).
    /// </summary>
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

    /// <summary>
    /// Team for all Chess Pieces (Use none for empty space).
    /// </summary>
    public enum ChessPieceTeam
    {
        black,
        white,
        none
    }
    #endregion
}

/// <summary>
/// ChessPiece representation.
/// </summary>
public struct ChessPiece
{
    #region variables
    public ChessSolver.ChessPieceType chessPieceType; // Type of Chess Piece (Pawn, Rook, Eg)
    public ChessSolver.ChessPieceTeam team; // Type of Chess Team (White, Black)
    public Vector2Int location; // Location of the ChessPiece
    #endregion

    #region constructors
    /// <summary>
    /// Constructor of a ChessPiece
    /// </summary>
    /// <param name="chessPieceType">Type of Chess Piece (Pawn, Rook, Eg)</param>
    /// <param name="team">Type of Chess Team (White, Black)</param>
    /// <param name="location">Location of the ChessPiece</param>
    public ChessPiece(ChessSolver.ChessPieceType chessPieceType, ChessSolver.ChessPieceTeam team, Vector2Int location)
    {
        this.chessPieceType = chessPieceType;
        this.team = team;
        this.location = location;
    }
    #endregion

    #region functions
    /// <summary>
    /// Displays a ChessPiece as a String.
    /// </summary>
    /// <returns>Returns a string representation of a ChessPiece.</returns>
    public override string ToString()
    {
        if (chessPieceType == ChessSolver.ChessPieceType.none || team == ChessSolver.ChessPieceTeam.none) { return "Empty space"; }
        return $"{team} {chessPieceType} ({location.x}, {location.y})";
    }
    #endregion
}

/// <summary>
/// ChessThreat Representation.
/// </summary>
public struct ChessThreat
{
    #region variables
    public List<ChessPiece> chessPieces; // All ChessPieces threathening a specific square.
    public Vector2Int location; // The Location of the ChessThreat.
    #endregion

    #region constructors
    /// <summary>
    /// First Constructor (Initalizes new ChessPiece List)
    /// </summary>
    /// <param name="location">Location of the Threathened Square.</param>
    public ChessThreat(Vector2Int location)
    {
        this.chessPieces = new List<ChessPiece>();
        this.location = location;
    }
    

    /// <summary>
    /// Second Constructor (Requires all values)
    /// </summary>
    /// <param name="chessPieces">List of All threathening ChessPieces.</param>
    /// <param name="location">Location of the Threathened Square.</param>
    public ChessThreat(List<ChessPiece> chessPieces, Vector2Int location)
    {
        this.chessPieces = chessPieces;
        this.location = location;
    }
    #endregion

    #region functions
    // Adds the ChessPiece to the List
    public bool AddPiece(ChessPiece chessPiece)
    {
        chessPieces.Add(chessPiece);
        return true;
    }

    // Returns true if the location is threathened by a corresponding chess team.
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
        throw new ArgumentException();
    }
    #endregion
}
