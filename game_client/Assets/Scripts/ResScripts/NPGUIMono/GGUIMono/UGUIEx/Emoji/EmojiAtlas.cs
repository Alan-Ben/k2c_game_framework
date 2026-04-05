using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.U2D;

namespace CEmoji
{
	[Serializable]
	public class EmojiCodeMap : SerializableDictionary<string, int>
	{
		public void Sort()
		{
			if (list != null)
				list.Sort((x, y) =>
				{
					if (x.Value == y.Value)
						return 0;
					else if (x.Value > y.Value)
						return 1;
					else
						return -1;
				});
		}
	}
	[CreateAssetMenu(fileName = "EmojiAtlas", menuName = "ScriptableObject/EmojiAtlas", order = 0)]
	public class EmojiAtlas : EmojiParser
	{
		public const int EMOJI_SEQ_START = 0xE030; //0xE000-0xE02F 用作自定义 Emoji
		private static char EMJSPACE = '\u2001';
		private static string DEFAULT_EMOJI = "\u2753";
		private static int s_emojiSeq = EMOJI_SEQ_START;  //使用E000-F8FF之间的编码表示emoji
		private static readonly int _g_emojiTex = Shader.PropertyToID("_EmojiTex");
		
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
		
		public string spritesPath;               // Sprite路径
		public SpriteAtlas spriteAtlas;
		[ALHeader("默认Emoji,Sprite名字，用于获取主贴图")]
		public string defaultSpriteName = "1f600";
		public EmojiCodeMap m_EmojiSpriteName2Code = new EmojiCodeMap(); // Emoji表情列表

		private Texture _m_texture;
		private Dictionary<int, Sprite> _m_dCode2Sprite;
		private Dictionary<string, int> _m_dEmoji2Code;
		private Dictionary<int,string> _m_dCode2Emoji;

		private bool _m_bIsInit = false;
		
		public Texture texture
		{
			get
			{
				if (_m_texture == null)
				{
					var spt = spriteAtlas?.GetSprite(defaultSpriteName);
					if (spt != null)
						_m_texture = spt.texture;
				}
				return _m_texture;
			}
		}
		
		/// <summary>
		/// 试图获取表情的UV坐标
		/// </summary>
		/// <param name="index"></param>
		/// <param name="_strTransText"></param>
		/// <param name="uvs"></param>
		/// <returns></returns>
		public bool TryGetEmojiUV(string _strTransText, int index, out Vector2[] uvs)
		{
			init();
			if (string.IsNullOrEmpty(_strTransText))
			{
				uvs = null;
				return false;
			}
			if (index < 0 || index >= _strTransText.Length)
			{
				uvs = null;
				return false;
			}
			int code = (int)_strTransText[index];
			if (!_m_dCode2Sprite.TryGetValue(code, out Sprite sprite))
			{
				uvs = null;
				return false;
			}
			if ( sprite != null)
			{
				var uv = UnityEngine.Sprites.DataUtility.GetOuterUV(sprite);
				
				// 0---1
				// 3---2
				// 顶点顺序 ：0-1-2 2-3-0
				uvs = new Vector2[4];
				uvs[0] = new Vector2(uv.x, uv.w);
				uvs[1] = new Vector2(uv.z, uv.w);
				uvs[2] = new Vector2(uv.z, uv.y);
				uvs[3] = new Vector2(uv.x, uv.y);
				return true;
			}

			uvs = null;
			return false;
		}

		public void init()
        {
            if (!_m_bIsInit || _m_dCode2Emoji == null || _m_dEmoji2Code == null)
            {
                _init();
                _m_bIsInit = true;
            }
        }

        protected void _init()
        {
	        if (m_EmojiSpriteName2Code == null || spriteAtlas == null) 
		        return;
	     
	        Sprite spt = spriteAtlas.GetSprite(defaultSpriteName);

	        if (spt != null)
	        {
		        _m_texture = spt.texture;
	        }
	        else
		        Debug.LogError("EmojiMap _init, sprites[0] is null, 材质贴图会是错的，请检查" );
	        
	        _m_dCode2Emoji = new Dictionary<int, string>();
	        _m_dEmoji2Code = new Dictionary<string, int>();
	        _m_dCode2Sprite = new Dictionary<int, Sprite>();
	        foreach (var sp2Code in m_EmojiSpriteName2Code)
	        {
		        int emojiCode = sp2Code.Value;
		        string strEmojiKey = GetConvertedString(sp2Code.Key);
		        var sprite = spriteAtlas.GetSprite(sp2Code.Key);
		        _m_dCode2Sprite[emojiCode] = sprite;
		        _m_dEmoji2Code[strEmojiKey] = emojiCode;
		        _m_dCode2Emoji[emojiCode] = strEmojiKey;
	        }
        }

        private string GetEmojiKeyOfCode(int code, out bool hit)
        {
            hit = false;
            string emoji;
            if (_m_dCode2Emoji.TryGetValue(code, out emoji))
            {
                hit = true;
                return emoji;
            }
            else
            {
                return DEFAULT_EMOJI;
            }
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
            if (string.IsNullOrEmpty(input))
	            return input;
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
                        if(!_m_dEmoji2Code.ContainsKey(strEmoji))
                        {
                            strEmoji = DEFAULT_EMOJI;
                        }
                        int emojiSeq = _m_dEmoji2Code[strEmoji];
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
                Debug.LogError($"Translate :{input}: Emoji Exception,msg:{ex.Message},stack trace:{ex.StackTrace}");
            }
            return input;
        }//TransEmojiCodeOfStr
	}


}
