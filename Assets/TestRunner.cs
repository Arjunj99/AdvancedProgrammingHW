using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestRunner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(ChangingFunction(9999));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int BytesToInt(byte a, byte b, byte c, byte d)
    {
        int sum = d;
        char[] currentByte = new char[8];

        sum = BytesToIntHelper(currentByte, c, sum, 1);
        sum = BytesToIntHelper(currentByte, b, sum, 2);
        sum = BytesToIntHelper(currentByte, a, sum, 3);

        return sum;
    }

    private int BytesToIntHelper(char[] currentByte, byte byteVal, int currentSum, byte byteMargin)
    {
        currentByte = Convert.ToString(byteVal, 2).ToCharArray();
        Array.Reverse(currentByte);


        for (int i = 0; i < currentByte.Length; i++)
        {
            if (currentByte[i] == '1')
            {
                currentSum += PowerOfTwo((byteMargin * 8) + i);
            }
        }
        return currentSum;
    }

    private int PowerOfTwo(int power)
    {
        return (int)Mathf.Pow(2, power);
    }

    public int SmallestPrimeFactor(int input)
    {
        if (input % 2 == 0)
            return 2;

        for (int i = 3; i * i <= input; i += 2)
        {
            if (input % i == 0)
                return i;
        }
        return input;
    }

    public int NumberOfDigits(int input)
    {
        return (int)Math.Floor(Math.Log10(input) + 1); ;
    }

    // Imagine this is your "Start()" function
    public void Initialize()
    {

    }

    public int ChangingFunction(int input)
    {
        if (NumberOfDigits(input) == 3) return SmallestPrimeFactor(input);
        else return NumberOfDigits(input);
    }
}
