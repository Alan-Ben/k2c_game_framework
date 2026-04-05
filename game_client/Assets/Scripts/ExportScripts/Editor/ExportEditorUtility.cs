using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections.Generic;

/// <summary>
/// Unity相关扩展，包含全局方法和扩展方法
/// </summary>

public static class ExportEditorUtility {


//正则表达式
//      "^//d+$"　　//非负整数（正整数 + 0） 
//      "^[0-9]*[1-9][0-9]*$"　　//正整数 
//      "^((-//d+)|(0+))$"　　//非正整数（负整数 + 0） 
//      "^-[0-9]*[1-9][0-9]*$"　　//负整数 
//      "^-?//d+$"　　　　//整数 
//      "^//d+(//.//d+)?$"　　//非负浮点数（正浮点数 + 0） 
//      "^(([0-9]+//.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*//.[0-9]+)|([0-9]*[1-9][0-9]*))$"　　//正浮点数 
//      "^((-//d+(//.//d+)?)|(0+(//.0+)?))$"　　//非正浮点数（负浮点数 + 0） 
//      "^(-(([0-9]+//.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*//.[0-9]+)|([0-9]*[1-9][0-9]*)))$"　　//负浮点数 
//      "^(-?//d+)(//.//d+)?$"　　//浮点数 
//      "^[A-Za-z]+$"　　//由26个英文字母组成的字符串 
//      "^[A-Z]+$"　　//由26个英文字母的大写组成的字符串 
//      "^[a-z]+$"　　//由26个英文字母的小写组成的字符串 
//      "^[A-Za-z0-9]+$"　　//由数字和26个英文字母组成的字符串 
//      "^//w+$"　　//由数字、26个英文字母或者下划线组成的字符串 
//      "^[//w-]+(//.[//w-]+)*@[//w-]+(//.[//w-]+)+$"　　　　//email地址 
//      "^[a-zA-z]+://(//w+(-//w+)*)(//.(//w+(-//w+)*))*(//?//S*)?$"　　//url
   
    //是否正整数
    public static bool IsNumberic (string _txt, out int _value) {
        //System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(@"^[0-9]*[1-9][0-9]*$");
        System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(@"^[-]*[1-9]\d*|0$");
        _value = -1;
        if (rex.IsMatch(_txt)) {
            try {
                _value = int.Parse(_txt);
            }
            catch (Exception ex) {
                UnityEngine.Debug.LogError("字符串解释为int出错，_txt:  " + _txt + "\n" + ex.ToString());
               _value = 0 ;
            }
            return true;
        }
        else {
            return false;
        }
    }

    public static bool IsNum (string str)
    {
        bool blResult = true;//默认状态下是数字 
        if (str == "")
            blResult = false;
        else {
            foreach (char Char in str) 
            {
                if (!char.IsNumber(Char)) 
                {
                    blResult = false;
                    break;
                }
            }

            if (blResult) 
            {
                if (int.Parse(str) == 0)
                    blResult = false;
            }
        }
        return blResult;
    }

    public static bool IsNum (string _str, out int _value) {
        bool blResult = true;//默认状态下是数字 
        _value = -1;
        if (_str == "")
            blResult = false;
        else {
            foreach (char Char in _str) {
                if (!char.IsNumber(Char)) {
                    blResult = false;
                    break;
                }
            }

            int.TryParse(_str, out _value);
            if (_value < 0)
                blResult = false;

        }
        return blResult;
    }


    public static bool IsFloat (string _txt, out float _value) {
        System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(@"^\d+(\.\d+)?$");
        _value = 0.0f;
        if (rex.IsMatch(_txt)) {
            _value = float.Parse(_txt);
            return true;
        }
        else
            return false;
    }

    public static Vector3 readXlsVector3String (string str, string  _columnName) {
        if (string.IsNullOrEmpty(str)) {
            Debug.LogError(" str IsNullOrEmpty! ");
            return Vector3.zero;
        }

        string[] strArray = str.Split(';', ':');
        if (strArray.Length < 3) {
            Debug.LogError(" str.Split use ';',strArray.Length < 3, 就是配置错了！ 字段名字是:  " + _columnName);
            return Vector3.zero;
        }

        return new Vector3(float.Parse(strArray[0]), float.Parse(strArray[1]), float.Parse(strArray[2]));
    }
    //偏移解析，偏移定义有三个值，第一个值为高度偏移，第二个值为前后偏移，第三个值为左右偏移，不填则为0偏移
    public static Vector3 ParseOffset(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return Vector3.zero;
        }
        string[] strs = str.Split(':');
        Vector3 v = new Vector3();
        if (strs.Length >= 1)
        {
            v.y = float.Parse(strs[0]);
        }
        if (strs.Length >= 2)
        {
            v.z = float.Parse(strs[1]);
        }
        if (strs.Length >= 3)
        {
            v.x = float.Parse(strs[2]);
        }
        return v;
    }


    /// <summary>
    /// string 转换到color, eg:"00FFF4FF"
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static Color stringToColor(string hex)
    {
        byte br = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte bg = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte bb = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        byte cc = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        float r = br / 255f;
        float g = bg / 255f;
        float b = bb / 255f;
        float a = cc / 255f;
        return new Color(r, g, b, a);
    }
}
