using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using UnityEngine;


public class Utility : MonoBehaviour
{
    public static Utility s_instance;
    public static StringBuilder m_sb = new StringBuilder();
    public static Utility Instance
    {
        get
        {
            Init();
            return s_instance;
        }
    }


    static readonly string[] m_currencyUnits = new string[] { "", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", "ca", "cb", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj", "ck", "cl", "cm", "cn", "co", "cp", "cq", "cr", "cs", "ct", "cu", "cv", "cw", "cx", };

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Utility>();
        }
    }

    public static StringBuilder ToCurrencyString(double number)
    {
        m_sb.Clear();
        if (number is > -1d and < 1d)
        {
            m_sb.Append("0");
            return m_sb;
        }

        if (double.IsInfinity(number))
        {
            m_sb.Append("Infinity");
            return m_sb;
        }

        if (number < 0)
        {
            m_sb.Append("-");
        }

        string[] partsSplit = number.ToString("E").Split('+');
        int.TryParse(partsSplit[1], out int exponent);
        int quotient = exponent / 3;
        int remainder = exponent % 3;

        if (exponent < 3)
        {
            m_sb.Append(System.Math.Truncate(number));
        }
        else
        {
            var temp = double.Parse(partsSplit[0].Replace("E", "")) * System.Math.Pow(10, remainder);

            m_sb.Append(temp.ToString("F").Replace(".00", ""));
        }

        m_sb.Append(m_currencyUnits[quotient]);
        return m_sb;
    }

    public static StringBuilder ToCurrencyString2(double number)
    {
        m_sb.Clear();

        if (number is > -1d and < 1d)
        {
            m_sb.Append("0");
            return m_sb;
        }

        if (double.IsInfinity(number))
        {
            m_sb.Append("Infinity");
            return m_sb;
        }

        if (number < 0)
        {
            number = -number;
            m_sb.Append("-");
        }

        int exponent = 0;

        while (number >= 10)
        {
            number /= 10d;
            exponent++;
        }

        int quotient = exponent / 3;
        int remainder = exponent % 3;

        number *= Math.Pow(10, remainder);
        m_sb.Append(number.ToString("0.00"));
        m_sb.Append(m_currencyUnits[quotient]);

        return m_sb;
    }
}
