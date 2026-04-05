using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

namespace CEmoji
{
    public class EmojiMap : EmojiParser
    {
        private const int EMOJI_SEQ_START = 0xE030; //0xE000-0xE02F 用作自定义 Emoji
        public static char EMJSPACE = '\u2001';
        private static string DEFAULT_EMOJI = "\u2753";
        private static int s_emojiSeq = EMOJI_SEQ_START;  //使用E000-F8FF之间的编码表示emoji
        
        protected static string GetConvertedString(string inputString)
        {
            string[] converted = inputString.Split('-');
            for (int j = 0; j < converted.Length; j++)
            {
                // fromBase value 中数字的基数，它必须是 2、8、10 或 16(进制)
                converted[j] = char.ConvertFromUtf32(Convert.ToInt32(converted[j], 16));
            }
            return string.Join(string.Empty, converted);
        }

        public static string ReplaceEmojiToSpace(string strText)
        {
            if(string.IsNullOrEmpty(strText))
            {
                return strText;
            }
            return Regex.Replace(strText, @"[\uE000-\uF8FF]", EMJSPACE.ToString());
        }
        
        private Dictionary<string, int> s_emoji2code;
        private Dictionary<int,string> s_code2emoji;

        private bool _m_bIsInit = false;

        protected virtual void _init()
        {
        }

        public void init()
        {
            if (!_m_bIsInit || s_emoji2code == null || s_code2emoji == null)
            {
                s_emoji2code = new Dictionary<string, int>();
                s_code2emoji = new Dictionary<int, string>();
                _init();
                _m_bIsInit = true;
            }
        }

        protected void AddEmojiKey(string emojiKey)
        {
            if(s_emoji2code.ContainsKey(emojiKey))
            {
                Debug.LogErrorFormat("EmojiMap AddEmojiCode,重复，key:{0}", emojiKey);
            }
            s_emoji2code[emojiKey] = s_emojiSeq;
            s_code2emoji[s_emojiSeq] = emojiKey;
            ++s_emojiSeq;
        }

        protected void ClearEmojiKey()
        {
            s_emojiSeq = EMOJI_SEQ_START;
            s_emoji2code.Clear();
            s_code2emoji.Clear();
        }

        private string GetEmojiKeyOfCode(int code, out bool hit)
        {
            hit = false;
            string emoji;
            if (s_code2emoji.TryGetValue(code, out emoji))
            {
                hit = true;
                return emoji;
            }
            else
            {
                return DEFAULT_EMOJI;
            }
        }

        public string GetEmojiKeyOfIndex(int index, string _strTransText)
        {
            init();
            int code = (int)_strTransText[index];
            string strKey;
            s_code2emoji.TryGetValue(code, out strKey);
            return strKey;
        }

        public string InvTranslate(string str)
        {
            init();
            
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            try
            {
                bool hit;
                var sb = StringBuilderPool.Get(str.Length * 2);
                string tmp = string.Empty;
                foreach (char c in str)
                {
                    tmp = GetEmojiKeyOfCode(c, out hit);
                    if (hit)
                    {
                        sb.Append(tmp);
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }

                str = sb.ToString();
                StringBuilderPool.Release(sb);
            }
            catch (System.Exception ex)
            {
                Debug.LogErrorFormat("InvTranslate Emoji Exception,msg:{0},stack trace:{1}", ex.Message, ex.StackTrace);
            }

            return str;
        }

        public string Translate(string input)
        {
            init();
            
            try
            {
                // Debugger.LogWarning("Emoji  Translate '{0}'", input);
                Parse(input);
                List<CharacterNode> chars = GetCharacterList();
                for(int i=chars.Count-1; i>=0; i--)
                {
                    CharacterNode charNode = chars[i];
                    if(charNode.isEmoji)
                    {
                        string strEmoji = input.Substring(charNode.startIndex, charNode.charLength);
                        input = input.Remove(charNode.startIndex, charNode.charLength);
                        if(!s_emoji2code.ContainsKey(strEmoji))
                        {
                            strEmoji = DEFAULT_EMOJI;
                        }
                        int emojiSeq = s_emoji2code[strEmoji];
                        input = input.Insert(charNode.startIndex, char.ConvertFromUtf32(emojiSeq));
                        
                        // Debug.LogFormat("input len:{0}, emojiSeq:{1:X}, emoji len:{2}", input.Length, emojiSeq, strEmoji.Length);
                    }
                    else
                    {
                        // 占2个char的字符替换成空格 不做支持
                        if(charNode.charLength>1)
                        {
                            input = input.Remove(charNode.startIndex, charNode.charLength);
                            input = input.Insert(charNode.startIndex, " ");
                        }
                    }
                }

                // Debugger.LogWarning("Emoji  Translate end '{0}'", input);

            }
            catch(System.Exception ex)
            {
                Debug.LogErrorFormat("Translate Emoji Exception,msg:{0},stack trace:{1}", ex.Message, ex.StackTrace);
            }
            return input;
        }//TransEmojiCodeOfStr

    }
}