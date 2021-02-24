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
        Debug.Log(text);

        //foreach

        //char[] separators = { '\n' };
        //string[] strValues = text.Split(separators);

        //List<int> intValues = new List<int>();

        //foreach (string str in strValues)
        //{
        //    strings.Add(str);
        //    int val = 0;
        //    if (int.TryParse(str, out val))
        //        intValues.Add(val);
        //}
    }

    private bool PopulateBoard(string boardInfo)
    {
        char[] boardInfoChar = boardInfo.ToCharArray();
        for (int i = 0; i < BOARDLENGTH; i++)
        {
            for (int j = 0; j < BOARDLENGTH; j++)
            {
                board[i,j] = boardInfoChar[i];
            }
        }
    }

    private ChessPiece CharToChessPiece(char chessChar)
    {
        switch (chessChar)
        {
            case ('p'):
                return ChessPiece.pawn;
            case ('')
        }
    }

    public enum ChessPiece {
        pawn,
        bishop,
        knight,
        rook,
        queen,
        king,
        none
    }
}
