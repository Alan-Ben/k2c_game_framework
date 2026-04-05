using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public static class PrimitiveExtension
    {
        public static void Example()
        {
            // long
            Debug.Log("-------------------------long ");
            Debug.Log(1L.ToLargeString(ELargeStringType.DEFAULT));
            Debug.Log(12L.ToLargeString(ELargeStringType.DEFAULT));
            Debug.Log(123L.ToLargeString(ELargeStringType.DEFAULT));
            Debug.Log(1234L.ToLargeString(ELargeStringType.DEFAULT));
            Debug.Log(12345L.ToLargeString(ELargeStringType.DEFAULT));
            Debug.Log(123456L.ToLargeString(ELargeStringType.DEFAULT));
            Debug.Log(1234567L.ToLargeString(ELargeStringType.DEFAULT));
            Debug.Log(12345678L.ToLargeString(ELargeStringType.DEFAULT));
            Debug.Log(123456789L.ToLargeString(ELargeStringType.DEFAULT));
            Debug.Log(1234567890L.ToLargeString(ELargeStringType.DEFAULT));
            Debug.Log(12345678901L.ToLargeString(ELargeStringType.DEFAULT));
            Debug.Log(123456789012L.ToLargeString(ELargeStringType.DEFAULT));
            Debug.Log(1234567890123L.ToLargeString(ELargeStringType.DEFAULT));
            Debug.Log(12345678901234L.ToLargeString(ELargeStringType.DEFAULT));
            Debug.Log(123456789012345L.ToLargeString(ELargeStringType.DEFAULT));
            Debug.Log(1234567890123456L.ToLargeString(ELargeStringType.DEFAULT));
            Debug.Log(12345678901234567L.ToLargeString(ELargeStringType.DEFAULT));
            Debug.Log(123456789012345678L.ToLargeString(ELargeStringType.DEFAULT));
            Debug.Log(1234567890123456789L.ToLargeString(ELargeStringType.DEFAULT));
            
            Debug.Log("------------------------- float ");
            
            Debug.Log(1f.TODecimalString(10,2));
            Debug.Log(12f.TODecimalString(10,2));
            Debug.Log(123f.TODecimalString(10,2));
            Debug.Log(1234f.TODecimalString(10,2));
            Debug.Log(12345f.TODecimalString(10,2));
            Debug.Log(123456f.TODecimalString(10,2));
            
            Debug.Log(1f.TODecimalString(100,2));
            Debug.Log(12f.TODecimalString(100,2));
            Debug.Log(123f.TODecimalString(100,2));
            Debug.Log(1234f.TODecimalString(100,2));
            Debug.Log(12345f.TODecimalString(100,2));
            Debug.Log(123456f.TODecimalString(100,2));
            
            Debug.Log(1f.TODecimalString(1000,2));
            Debug.Log(12f.TODecimalString(1000,2));
            Debug.Log(123f.TODecimalString(1000,2));
            Debug.Log(1234f.TODecimalString(1000,2));
            Debug.Log(12345f.TODecimalString(1000,2));
            Debug.Log(123456f.TODecimalString(1000,2));
        }
        
        #region float Extension
        /// <summary>
        /// 转成小数点字符串
        /// </summary>
        /// <param name="_num"></param>
        /// <param name="_divisor">除数，默认除以1</param>
        /// <param name="_decimalCount">保留的小数点后位数,默认保留2位</param>
        /// <returns></returns>
        public static string TODecimalString(this float _num,float _divisor = 1,int _decimalCount = 2)
        {
            float pow = Mathf.Pow(10, _decimalCount);
            _num = (int) ((_num / _divisor) * pow) / pow;
            return _num.ToString();
        }
        #endregion
        
        #region long Extension

        public static string ToFileSizeString(this long _byteSize)
        {
            if (_byteSize < 1024)
            {
                return _byteSize + "B";
            }
            else if (_byteSize < 1024 * 1024)
            {
                return (_byteSize / 1024f).ToString("F2") + "KB";
            }
            else if (_byteSize < 1024 * 1024 * 1024)
            {
                return (_byteSize / 1024f / 1024f).ToString("F2") + "MB";
            }
            else
            {
                return (_byteSize / 1024f / 1024f / 1024f).ToString("F2") + "GB";
            }
        }
        
        public enum ELargeStringType
        {
            DEFAULT,
            GOLD,
            BATTLE,
        }
        
        public static string ToLargeString(this long _num, ELargeStringType _type)
        {
            switch (_type)
            {
                case ELargeStringType.DEFAULT:
                    return _toDefaultLargeString(_num);
                case ELargeStringType.GOLD:
                    // 【GOB-0】通用-赚速&美金表达放大1000倍去掉 https://www.teambition.com/task/69a7c866110329cd8d67ec8a
                    return _toDefaultLargeString(_num);
                    // return _toGoldLargeString(_num);
                case ELargeStringType.BATTLE:
                    return _toBattleLargeString(_num);
                default:
                    return _toDefaultLargeString(_num);
            }
        }
        
        /// <summary>
        /// 默认的大数字展示
        /// </summary>
        /// <param name="_num"></param>
        /// <returns></returns>
        private static string _toDefaultLargeString(this long _num)
        {
            string numString;
            if(_num >= 1000000000000L)
            {
                if(_num % 1000000000000 == 0)
                {
                    numString = _num / 1000000000L / 1000d + "T";
                }
                else
                {
                    numString = (_num / 1000000000L).SetValidFigure()  + "T";
                }
            }
            else if(_num >= 1000000000L)
            {
                if(_num % 1000000000 == 0)
                {
                    numString = _num / 1000000L / 1000 + "B";
                }
                else
                {
                    numString = (_num / 1000000L).SetValidFigure()  + "B";
                }
            }
            else if(_num >= 1000000L)
            {
                if(_num % 1000000 == 0)
                {
                    numString = (_num / 1000L) / 1000 + "M";
                }
                else
                {
                    numString = (_num / 1000L).SetValidFigure()  + "M";
                }
            }
            else if(_num >= 1000L)
            {
                if(_num % 1000 == 0)
                {
                    numString = _num / 1000 + "K";
                }
                else
                {
                    numString = _num.SetValidFigure() + "K";
                }
            }
            else
            {
                numString = _num.ToString();
            }
            return numString;
        }
        
        /// <summary>
        /// 金币的特殊大数字展示
        /// </summary>
        /// <param name="_num"></param>
        /// <returns></returns>
        private static string _toGoldLargeString(this long _num)
        {
            string numString;
            if(_num >= 1000000000000L)
            {
                if(_num % 1000000000000 == 0)
                {
                    numString = _num / 1000000000L / 1000d + "Q";
                }
                else
                {
                    numString = (_num / 1000000000L).SetValidFigure()  + "Q";
                }
            }
            else if(_num >= 1000000000L)
            {
                if(_num % 1000000000 == 0)
                {
                    numString = _num / 1000000L / 1000 + "T";
                }
                else
                {
                    numString = (_num / 1000000L).SetValidFigure()  + "T";
                }
            }
            else if(_num >= 1000000L)
            {
                if(_num % 1000000 == 0)
                {
                    numString = (_num / 1000L) / 1000 + "B";
                }
                else
                {
                    numString = (_num / 1000L).SetValidFigure()  + "B";
                }
            }
            else if(_num >= 1000L)
            {
                if(_num % 1000 == 0)
                {
                    numString = _num / 1000 + "M";
                }
                else
                {
                    numString = _num.SetValidFigure() + "M";
                }
            }
            else
            {
                numString = _num + "K";
            }
            return numString;
        }
        
        /// <summary>
        /// 战斗中的大数值
        /// </summary>
        /// <param name="_num"></param>
        /// <returns></returns>
        private static string _toBattleLargeString(this long _num)
        {
            string numString;
            if(_num >= 100000000000L)
            {
                numString = _num / 1000000L / 1000 + "B";
            }
            else if(_num >= 100000000L)
            {
                numString = (_num / 1000L) / 1000 + "M";
            }
            else if(_num >= 100000L)
            {
                numString = _num / 1000 + "K";
            }
            else
            {
                numString = _num.ToString();
            }
            return numString;
        }
        
        private static string SetValidFigure(this long _num)
        {
#if NP_GAME
            //获取配置的有效位数
            int significantDigit = 5;
            //获取配置的小数位数
            int decimalDigit = 2;
       
            int Digit = 1; //位数总计
            long weight = 10;
            while ( _num >= weight )
            {
                weight *= 10;
                Digit++;
            }
            
            //不四舍五入 保留小数后 validDigit 位
            int validDigit = 3 - (Digit - significantDigit);//需要保留的小数位数
            long divisor = 1;
            if(validDigit > 0)
            {
                if(validDigit < decimalDigit)
                    decimalDigit = validDigit;
                for (int j = 0; j < decimalDigit; j++)
                {
                    divisor *= 10;
                }
            }

            long i =(long)(_num * divisor /1000.0d);
            
            return ((i*1.0)/divisor).ToString();
#else
            return _num.ToString();
#endif
        }

        public static long ToAbs(this long _num)
        {
            if (_num < 0)
                return -_num;
            return _num;
        }
        
        // 普通阿拉伯数字转罗马数字：
        public static long[] aArray = {1000,900,500,400,100,90,50,40,10,9,5,4,1};
        public static string[] rArray = {"M","CM","D","CD","C","XC","L","XL","X","IX","V","IV","I"};
        public static string ToRome(this long _aNumber)
        {
            if(_aNumber < 1)
            {
                return "0";
            }
            string rNumber = "";
            for(int i=0; i<aArray.Length; i++){
                while(_aNumber >= aArray[i]){
                    rNumber += rArray[i];
                    _aNumber -= aArray[i];
                }
            }
            return rNumber;
        }
        public static string ToRome(this int _aNumber)
        {
            if (_aNumber < 1)
            {
                return "0";
            }
            string rNumber = "";
            for (int i = 0; i < aArray.Length; i++)
            {
                while (_aNumber >= aArray[i])
                {
                    rNumber += rArray[i];
                    _aNumber -= (int)aArray[i];
                }
            }
            return rNumber;
        }

        #endregion

        #region byte Extension

        public static bool Contains(this byte _selfByteValue, int _enum)
        {

            return (_selfByteValue & (byte)1 << _enum) != 0;
        }

        public static bool ContainsByte(this byte _selfByteValue, byte _byte)
        {

            return (_selfByteValue & _byte) != 0;
        }


        #endregion

        #region float Extension

        public static string ToLargeString(this float _num)
        {
            if (_num < 1000L)
            {
                return _num.ToString();
            }
            else
            {
                return ((long) _num).ToLargeString(ELargeStringType.DEFAULT);
            }
        }
        
        private static string SetValidFigure(this float _num)
        {
#if NP_GAME
            //获取配置的有效位数
            int significantDigit = 5;
            //获取配置的小数位数
            int decimalDigit = 2;
       
            int Digit = 1; //位数总计
            long weight = 10;
            while ( _num >= weight )
            {
                weight *= 10;
                Digit++;
            }
            
            //不四舍五入 保留小数后 validDigit 位
            int validDigit = 3 - (Digit - significantDigit);//需要保留的小数位数
            int divisor = 1;
            if(validDigit > 0)
            {
                if(validDigit < decimalDigit)
                    decimalDigit = validDigit;
                for (int j = 0; j < decimalDigit; j++)
                {
                    divisor *= 10;
                }
            }

            int i =(int)(_num * divisor /1000.0d);
            
            return ((i*1.0)/divisor).ToString();
#else
            return _num.ToString();
#endif
        }
        
        public static float Remap(this float _value, float _from1, float _from2, float _to1, float _to2)
        {
            if(_from1 == _from2)
            {
                return _to1;
            }
            else
            {
                return _to1 + (_to2 - _to1) * (_value - _from1) / (_from2 - _from1);
            }
        }

        public static float RemapClamp(this float _value, float _from1, float _from2, float _to1, float _to2)
        {
            if(_from1 == _from2)
            {
                return _to1;
            }
            else
            {
                _value = _from2 >= _from1 ? Mathf.Clamp(_value, _from1, _from2) : Mathf.Clamp(_value, _from2, _from1);
                return _to1 + (_to2 - _to1) * (_value - _from1) / (_from2 - _from1);
            }
        }

        public static float RemapPow4(this float _value, float _from1, float _from2, float _to1, float _to2)
        {
            if(_from1 == _from2)
            {
                return _to1;
            }
            else
            {
                float x = (_value - _from1) / (_from2 - _from1);
                float y = -Mathf.Pow(x - 1, 4) + 1;
                return _to1 + (_to2 - _to1) * y;
            }
        }

        public static float RemapPow5(this float _value, float _from1, float _from2, float _to1, float _to2)
        {
            if(_from1 == _from2)
            {
                return _to1;
            }
            else
            {
                float x = (_value - _from1) / (_from2 - _from1);
                float y = 0.7f * Mathf.Pow(x, 5) - 1.8f * Mathf.Pow(x, 3) + 2.1f * x;
                return _to1 + (_to2 - _to1) * y;
            }
        }

        public static float RemapPow5Offset(this float _value, float _from1, float _from2, float _to1, float _to2)
        {
            if(_from1 == _from2)
            {
                return _to1;
            }
            else
            {
                float x = (_value - _from1) / (_from2 - _from1);
                x = x * 2f - 1;
                float y = 0.7f * Mathf.Pow(x, 5) - 1.8f * Mathf.Pow(x, 3) + 2.1f * x;
                y = y * 0.5f + 0.5f;
                return _to1 + (_to2 - _to1) * y;
            }
        }

        //public static float RemapBezier(this float _value, float _from1, float _from2, float _to1, float _to2, Vector2 _p1, Vector2 _p2)
        //{
        //    if(_from1 == _from2)
        //    {
        //        return _to1;
        //    }
        //    else
        //    {
        //        float x = (_value - _from1) / (_from2 - _from1);
        //        float y = BezierUtils.CalculateCubicBezierPoint(x, Vector2.zero, _p1, _p2, Vector2.one).y;
        //        return _to1 + (_to2 - _to1) * y;
        //    }
        //}

        //public static float RemapBezierClamp(this float _value, float _from1, float _from2, float _to1, float _to2, Vector2 _p1,
        //    Vector2 _p2)
        //{
        //    if(_from1 == _from2)
        //    {
        //        return _to1;
        //    }
        //    else
        //    {
        //        _value = _from2 >= _from1 ? Mathf.Clamp(_value, _from1, _from2) : Mathf.Clamp(_value, _from2, _from1);
        //        float x = (_value - _from1) / (_from2 - _from1);
        //        float y = BezierUtils.CalculateCubicBezierPoint(x, Vector2.zero, _p1, _p2, Vector2.one).y;
        //        return _to1 + (_to2 - _to1) * y;
        //    }
        //}

        public static float RemapCurve(this float _value, float _from1, float _from2, float _to1, float _to2, AnimationCurve _curve)
        {
            if(_from1 == _from2)
            {
                return _to1;
            }
            else
            {
                float x = (_value - _from1) / (_from2 - _from1);
                float y = _curve.Evaluate(x);
                return _to1 + (_to2 - _to1) * y;
            }
        }

        public static string ToPercentage(this float _value)
        {
            return _value.ToString("P");
        }

        public static int ToCeilingIntValue(this float _value)
        {
            return (int) Math.Ceiling(_value);
        }
        public static long ToCeilingLongValue(this float _value)
        {
            return (long) Math.Ceiling(_value);
        }
        #endregion
        
        #region string Extension

        //替换中文前的空格为不可换行空格
        public static string ReplaceSpaceInChinese(this string _value)
        {
            if(_value == null)
                return null;
            bool isHaveChinese = false;//是否有中文
            char[] charArray = _value.ToCharArray();
            for (var i = charArray.Length -1; i >= 0; i--)
            {
                if(charArray[i] != ' ')
                {
                    //是否有中文
                    if(charArray[i] >= 0x4e00 && charArray[i] <= 0x9fbb)
                        isHaveChinese = true;
                }
                else
                {
                    if(isHaveChinese)
                    {
                        //改为不间断空格
                        charArray[i] = '\u00A0';
                    }
                    if( i > 0 && charArray[i-1] != ' ') 
                        isHaveChinese = false;
                }
            }
            string newValue = new string(charArray);
            return newValue;
        }
        
        /// <summary>
        /// 替换第一个
        /// </summary>
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
        
        //特殊字符
        public static string specialCharacter="[^\u001E-\uffff]|[`~!@#$%^&*()+=|{}':;',\\[\\].<>/?~！@#￥%……&*（）——+|{}【】‘；：”“’。，、？]";
        public static bool checkSpecialCharacter(this string _value)
        {
            if(_value == null || specialCharacter == null)
                return false;
            
            if(Regex.IsMatch(_value, specialCharacter))
            {
                return true;
            }
            
            return false;
        }

        public static T ToEnum<T>(this string _str)
        {
            //这里这样写是因为，枚举解析错误，这种错误比较严重，所以在Editor下就直接不捕获抛异常
#if !UNITY_EDITOR
            try
            {
#endif
                return (T) Enum.Parse(typeof(T), _str);
#if !UNITY_EDITOR
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("Enum [" + typeof(T).ToString() + "] Parse err value: " + _str);
                return default;
            }
#endif
        }
        
        [NotNull] private static readonly Regex ColorRegex = new Regex(@"<color=(.*?)>(.*?)</color>", RegexOptions.IgnoreCase);
        /// <summary>
        /// 移除富文本的颜色操作字符串
        /// 只考虑只有颜色码的富文本
        /// </summary>
        public static string removeColor(this string str)
        {
            //while (str.Contains("</color>") || str.Contains("</Color>"))
            //{
            //    int startIndex = str.IndexOf("<", StringComparison.OrdinalIgnoreCase);
            //    int endIndex = str.IndexOf(">", StringComparison.OrdinalIgnoreCase);
            //    str = str.Remove(startIndex, endIndex - startIndex + 1);
            //}
           
            if (string.IsNullOrEmpty(str))
                return str;

            //解析后的文本
            StringBuilder outputBuilder = new StringBuilder();
            //获得不带颜色标签的文本
            int tempIndex = 0;
            Match tmpMatch = null;
            for (int i = 0; i < ColorRegex.Matches(str).Count; i++)
            {
                tmpMatch = ColorRegex.Matches(str)[i];
                //取出非颜色的内容
                outputBuilder.Append(str.Substring(tempIndex, tmpMatch.Index - tempIndex));
                //两个尖括号之间的值，  eg: <color=#00E734FF>57</color>   =>       value = 57
                outputBuilder.Append(tmpMatch.Groups[2].Value);
                //设置当前下标位置
                tempIndex = tmpMatch.Index + tmpMatch.Length;
            }

            return outputBuilder.ToString();
        }

        /// <summary>
        /// 移除富文本的标签
        /// </summary>
        public static string removeRichText(this string str, string _tag)
        {
            //开头的标签
            string targetStartRemove = $"<{_tag}=";
            //结尾的标签
            string targetEndRemove = $"</{_tag}>";
            StringBuilder sb = new StringBuilder(str);

            //获取开头标签的起始下标
            int startTagIndex = str.IndexOf(targetStartRemove,StringComparison.InvariantCultureIgnoreCase);
            //获取开头标签的结束下标
            int startTagIndexEnd = startTagIndex + targetStartRemove.Length;
            while (true)
            {
                //如果未找到开头标签的结束标记，直接停止
                if (str.Length <= startTagIndexEnd || (startTagIndexEnd - startTagIndex > 100))
                {
                    Debug.LogError($"removeRichText 未找到匹配的富文本开头标签，str:{str}");
                    return str;
                }

                //查找结束的标记
                startTagIndexEnd++;
                if (str[startTagIndexEnd].Equals('>'))
                    break;
            }
            //移除开头标签
            int startTagLength = startTagIndexEnd - startTagIndex + 1;
            sb.Remove(startTagIndex, startTagLength);

            //获取结尾标签的开始下标
            int endTagIndex = str.IndexOf(targetEndRemove, StringComparison.InvariantCultureIgnoreCase) - startTagLength;
            //移除结尾标签
            sb.Remove(endTagIndex, targetEndRemove.Length);

            return sb.ToString();
        }
        
        #endregion

        #region char

        public static ENPLanguage getCharLanguage(this char _char)
        {
            // 英文字母
            if ((_char >= 0x0041 && _char <= 0x005A) || (_char >= 0x0061 && _char <= 0x007A))
            {
                return ENPLanguage.EN_US;
            }

            // 中文简体/中文繁体/日语/朝语
            if (_char >= 0x2E80 && _char <= 0x9FFF)
            {
                return ENPLanguage.ZH_CN;
            }

            // 韩语
            if (_char >= 0xAC00 && _char <= 0xD7AF)
            {
                return ENPLanguage.KO_KR;
            }

            // 俄语
            if (_char >= 0x0400 && _char <= 0x04FF)
            {
                return ENPLanguage.RU_RU;
            }
            
            // 阿拉伯语
            if(_char >= 0x0600 && _char <= 0x06FF)
            {
                return ENPLanguage.AR_AR;
            }
            
            // 土耳其语 或 法语
            if(_char >= 0x0100 && _char <= 0x017F)
            {
                return ENPLanguage.TR_TR;
            }
            
            // 德语 或 西班牙语 或 葡萄牙
            if(_char >= 0x007E && _char <= 0x00FF)
            {
                return ENPLanguage.DE_DE;
            }

            return ENPLanguage.NONE;
        }
        

        #endregion

        #region int Extension

        // /// <summary>
        // /// 每隔n位数带逗号的字符串
        // /// </summary>
        // /// <param name="_num"></param>
        // /// <returns></returns>
        // public static string ToCommaString(this int _num, int count = 3)
        // {
        //     string numString = _num.ToString();
        //     int length = numString.Length;
        //     int curIndex = 0;
        //     int curCharIndex = 0;
        //     List<string> numStrings = new List<string>();
        //     while (curIndex < length)
        //     {
        //         curIndex ++;
        //         if ((length - curIndex) % count == 0)
        //         {
        //             numStrings.Add(numString.Substring(curCharIndex,curIndex - curCharIndex));
        //             curCharIndex = curIndex;
        //         }
        //
        //         if (length - curIndex == count)
        //         {
        //             numStrings.Add(numString.Substring(curIndex,count));
        //             break;
        //         }
        //     }
        //
        //     numString = string.Join(',', numStrings);
        //     return numString;
        // }

        public static string ToLargeString(this int _num, ELargeStringType _largeStringType)
        {
            return ((long) _num).ToLargeString(_largeStringType);
        }
        
        private static string SetValidFigure(this int _num)
        {
#if NP_GAME
            //获取配置的有效位数
            int significantDigit = 5;
            //获取配置的小数位数
            int decimalDigit = 2;
       
            int Digit = 1; //位数总计
            long weight = 10;
            while ( _num >= weight )
            {
                weight *= 10;
                Digit++;
            }
            
            //不四舍五入 保留小数后 validDigit 位
            int validDigit = 3 - (Digit - significantDigit);//需要保留的小数位数
            int divisor = 1;
            if(validDigit > 0)
            {
                if(validDigit < decimalDigit)
                    decimalDigit = validDigit;
                for (int j = 0; j < decimalDigit; j++)
                {
                    divisor *= 10;
                }
            }

            int i =(int)(_num * divisor /1000.0d);
            
            return ((i*1.0)/divisor).ToString();
#else
            return _num.ToString();
#endif
        }

        #endregion

        #region double

        public static int ToCeilingIntValue(this double _value)
        {
            return (int) Math.Ceiling(_value);
        }
        public static long ToCeilingLongValue(this double _value)
        {
            return (long) Math.Ceiling(_value);
        }

        #endregion
    }
}