using LitJson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ALPackage;
using UnityEngine;

namespace GOE
{
    // 屏蔽字符类型
    public enum EIllegalType
    {
        SENTENCE,//句子
        WORD,//单词
    }
    
    //通用的字符判定管理器
    public class CharacterDetermineMgr : WCGSingleton<CharacterDetermineMgr>
    {
        //非法字符列表
        private List<string> _m_lDetectorCharacterList;

        //玩家取名屏蔽字库
        private List<string> _m_lPlayerNameDetectorLib;
        //字符串
        private byte[] _m_bBytes;
        //JsonData
        private JsonData jsondata;

        //临时数据
        private string _m_sString;


        public CharacterDetermineMgr()
        {
            _m_lDetectorCharacterList = new List<string>();
            _m_lPlayerNameDetectorLib = new List<string>();
        }

        public void init()
        {
            for(int i = 0; i < GRefdataCoreMgr.instance.detectorCharacterMap.refList.Count; i++)
            {
                if(GRefdataCoreMgr.instance.detectorCharacterMap.refList[i] == null)
                    continue;

                _m_lDetectorCharacterList.Add(GRefdataCoreMgr.instance.detectorCharacterMap.refList[i].illegal_character.ToLower());
            }

            NPDetectorPlayerNameRefObj tempRefObj = null;
            for(int i = 0; i < GRefdataCoreMgr.instance.detectorPlayerNameMap.refList.Count; i++)
            {
                tempRefObj = GRefdataCoreMgr.instance.detectorPlayerNameMap.refList[i];
                if(tempRefObj == null)
                    continue;

                _m_lPlayerNameDetectorLib.Add(tempRefObj.illegal_character.ToLower());
            }
        }


        /// <summary>
        /// 是否是非法字符
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public bool isIllegal(string _str)
        {
            if(_m_lDetectorCharacterList != null)
            {
                return checkHasIllegal(_str, _m_lDetectorCharacterList);
            }

            return false;
        }

        /// <summary>
        /// 是否是非法字符
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public bool isPlayerNameIllegal(string _str)
        {
            //玩家名字的屏蔽字库是单独的，和通用屏蔽字库不一样
            if(_m_lPlayerNameDetectorLib != null)
            {
                return checkHasIllegal(_str, _m_lPlayerNameDetectorLib);
            }

            return false;
        }
        
        /// <summary>
        /// 检测是否存在非法字符
        /// </summary>
        /// <param name="_str">要检测的字符串</param>
        /// <param name="_IllegalCharacterList">非法字符表</param>
        /// <returns></returns>
        public bool checkHasIllegal(string _str, List<string> _IllegalCharacterList)
        {
            if(string.IsNullOrEmpty(_str) || _IllegalCharacterList == null || _IllegalCharacterList.Count == 0)
                return false;

            StringBuilder sb = new StringBuilder();
            EIllegalType preIllegalType = _getCharIllegal(_str[0]);//上一个字符的屏蔽类型

            for(int i = 0; i < _str.Length; i++)
            {
                char c = _str[i];
                EIllegalType nowIllegalType = _getCharIllegal(c);//获取当前字符的屏蔽类型

                if(nowIllegalType != preIllegalType)// 若当前屏蔽类型不等于上一屏蔽类型,开始新单词
                {
                    string newString = sb.ToString();

                    for(int j = 0; j < _IllegalCharacterList.Count; j++)
                    {
                        string detectorCharacter = _IllegalCharacterList[j];

                        // 若是单词屏蔽匹配到屏蔽字
                        if(preIllegalType == EIllegalType.WORD && newString.Equals(detectorCharacter, StringComparison.InvariantCultureIgnoreCase))
                        {
#if UNITY_EDITOR
                            if(_AALMonoMain.instance.showDebugOutput)
                                Debug.Log($"[被屏蔽的字符串]{newString}, [屏蔽字]{detectorCharacter}, len:{newString.Length}");
#endif
                            return true;
                        }

                        // 若是句子屏蔽匹配到屏蔽字
                        if(preIllegalType == EIllegalType.SENTENCE && newString.Contains(detectorCharacter))
                        {
#if UNITY_EDITOR
                            if(_AALMonoMain.instance.showDebugOutput)
                                Debug.Log($"[被屏蔽的字符串]{newString}, [屏蔽字]{detectorCharacter}, len:{newString.Length}");
#endif
                            return true;
                        }
                    }
                    sb.Clear();//StringBuilder清空
                    i--;//单词回滚
                }
                else//若当前屏蔽类型等于上一屏蔽类型，不开始新单词
                {
                    sb.Append(c);//字母加入StringBuilder
                }

                preIllegalType = nowIllegalType;// 记录上一次的屏蔽类型
            }

            if(sb.Length > 0)//最后一句话放入列表
            {
                string newString = sb.ToString();

                for(int j = 0; j < _IllegalCharacterList.Count; j++)
                {
                    string detectorCharacter = _IllegalCharacterList[j];

                    // 若是单词屏蔽匹配到屏蔽字
                    if(preIllegalType == EIllegalType.WORD && newString.Equals(detectorCharacter, StringComparison.InvariantCultureIgnoreCase))
                    {
#if UNITY_EDITOR
                        if(_AALMonoMain.instance.showDebugOutput)
                            Debug.Log($"[被屏蔽的字符串]{newString}, [屏蔽字]{detectorCharacter}, len:{newString.Length}");
#endif
                        return true;
                    }

                    // 若是句子屏蔽匹配到屏蔽字
                    if(preIllegalType == EIllegalType.SENTENCE && newString.Contains(detectorCharacter))
                    {
#if UNITY_EDITOR
                        if(_AALMonoMain.instance.showDebugOutput)
                            Debug.Log($"[被屏蔽的字符串]{newString}, [屏蔽字]{detectorCharacter}, len:{newString.Length}");
#endif
                        return true;
                    }
                }
                sb.Clear();//StringBuilder清空
            }

            return false;
        }

        /// <summary>
        /// 获取字母的屏蔽类型
        /// </summary>
        /// <returns></returns>
        private EIllegalType _getCharIllegal(char _ch)
        {
            switch(_ch.getCharLanguage())
            {
                case ENPLanguage.ZH_CN:
                case ENPLanguage.ZH_TW:
                case ENPLanguage.KO_KR:
                case ENPLanguage.JA_JP:
                    // 若是中文简体/中文繁体/日语/韩语
                    return EIllegalType.SENTENCE;//屏蔽类型为句屏蔽

                case ENPLanguage.EN_US:
                case ENPLanguage.RU_RU:
                case ENPLanguage.AR_AR:
                case ENPLanguage.TR_TR:
                case ENPLanguage.DE_DE:
                case ENPLanguage.FR_FR:
                case ENPLanguage.ES_ES:
                case ENPLanguage.PT_PT:
                case ENPLanguage.IT_IT:
                    return EIllegalType.WORD;//屏蔽类型为词屏蔽

                default://若是除上述语言外的任何字符(标点，表情等)
                    return EIllegalType.SENTENCE;//屏蔽类型为句屏蔽
            }
        }
        
        /// <summary>
        /// 非法字符替换
        /// </summary>
        /// <param name="isChat">是否是聊天的替换</param>
        /// <param name="_str">要替换的字符串</param>
        /// <returns></returns>
        public bool replaceIllegalCharacter(bool isChat, ref string _str)
        {
            if(string.IsNullOrEmpty(_str))
                return false;

            List<string> illegalCharacterList;//屏蔽字符字库
            if(isChat)//若是聊天
            {
                illegalCharacterList = _m_lDetectorCharacterList;
            }
            else//若是改名
            {
                illegalCharacterList = _m_lPlayerNameDetectorLib;
            }
            
            return replaceIllegalCharacter(illegalCharacterList,ref _str);

        }

        /// <summary>
        /// 非法字符替换
        /// </summary>
        /// <param name="_illegalCharacterList">屏蔽字字库</param>
        /// <param name="_str">要替换的字符串</param>
        /// <returns></returns>
        public bool replaceIllegalCharacter(List<string> _illegalCharacterList, ref string _str)
        {
            if(string.IsNullOrEmpty(_str) || _illegalCharacterList == null || _illegalCharacterList.Count == 0)
                return false;

            StringBuilder replaceString = new StringBuilder();//记录替换后的字符串
            StringBuilder sb = new StringBuilder();
            EIllegalType preIllegalType = _getCharIllegal(_str[0]);//上一个字符的屏蔽类型
            bool hasIllegal = false;// 是否有屏蔽字标志

            for(int i = 0; i < _str.Length; i++)
            {
                char c = _str[i];
                EIllegalType nowIllegalType = _getCharIllegal(c);//获取当前字符的屏蔽类型

                if(nowIllegalType != preIllegalType)// 若当前屏蔽类型不等于上一屏蔽类型,开始新单词
                {
                    string newString = sb.ToString();

                    for(int j = 0; j < _illegalCharacterList.Count; j++)
                    {
                        string detectorCharacter = _illegalCharacterList[j];

                        // 若是单词屏蔽匹配到屏蔽字
                        if(preIllegalType == EIllegalType.WORD && newString.Equals(detectorCharacter, StringComparison.InvariantCultureIgnoreCase))
                        {
#if UNITY_EDITOR
                            if(_AALMonoMain.instance.showDebugOutput)
                                Debug.Log($"[被屏蔽的字符串]{newString}, [屏蔽字]{detectorCharacter}, len:{newString.Length}");
#endif
                            hasIllegal = true;
                            // 新的字符串使用***替换屏蔽字
                            newString = "***";
                            break;
                        }

                        // 若是句子屏蔽匹配到屏蔽字
                        if(preIllegalType == EIllegalType.SENTENCE && newString.Contains(detectorCharacter))
                        {
#if UNITY_EDITOR
                            if(_AALMonoMain.instance.showDebugOutput)
                                Debug.Log($"[被屏蔽的字符串]{newString}, [屏蔽字]{detectorCharacter}, len:{newString.Length}");
#endif
                            hasIllegal = true;
                            int index = newString.IndexOf(detectorCharacter);// 查找屏蔽字在新串中下标
                            // 新的字符串使用***替换屏蔽字
                            newString = newString.Replace(newString.Substring(index, detectorCharacter.Length), "***");
                            break;
                        }
                    }
                    replaceString.Append(newString);//放入替换字符串

                    sb.Clear();//StringBuilder清空
                    i--;//单词回滚
                }
                else//若当前屏蔽类型等于上一屏蔽类型，不开始新单词
                {
                    sb.Append(c);//字母加入StringBuilder
                }

                preIllegalType = nowIllegalType;// 记录上一次的屏蔽类型
            }

            if(sb.Length > 0)//最后一句话放入列表
            {
                string newString = sb.ToString();

                for(int j = 0; j < _illegalCharacterList.Count; j++)
                {
                    string detectorCharacter = _illegalCharacterList[j];

                    // 若是单词屏蔽匹配到屏蔽字
                    if(preIllegalType == EIllegalType.WORD && newString.Equals(detectorCharacter, StringComparison.InvariantCultureIgnoreCase))
                    {
#if UNITY_EDITOR
                        if(_AALMonoMain.instance.showDebugOutput)
                            Debug.Log($"[被屏蔽的字符串]{newString}, [屏蔽字]{detectorCharacter}, len:{newString.Length}");
#endif
                        hasIllegal = true;
                        newString = "***";
                        break;
                    }

                    // 若是句子屏蔽匹配到屏蔽字
                    if(preIllegalType == EIllegalType.SENTENCE && newString.Contains(detectorCharacter))
                    {
#if UNITY_EDITOR
                        if(_AALMonoMain.instance.showDebugOutput)
                            Debug.Log($"[被屏蔽的字符串]{newString}, [屏蔽字]{detectorCharacter}, len:{newString.Length}");
#endif
                        hasIllegal = true;
                        int index = newString.IndexOf(detectorCharacter);
                        newString = newString.Replace(newString.Substring(index, detectorCharacter.Length), "***");
                        break;
                    }
                }
                replaceString.Append(newString);

                sb.Clear();//StringBuilder清空
            }

            _str = replaceString.ToString();
            return hasIllegal;
        }

        //是否在指定长度区间内
        public bool isSuitableLength(string _str, int _min, int _max)
        {
            int strLength = getUnicodeStringLength(_str);

            if(strLength > _max || strLength < _min)
                return false;

            return true;
        }

        //是否在指定长度区间内,
        public bool isSuitableLength(string _str, int _min, int _max, bool _popTip)
        {
            int strLength = getUnicodeStringLength(_str);

            if(_min > 0 && strLength < _min)
            {
                if(_popTip)
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.string_length_less_min));
                return false;
            }

            if(_max > 0 && strLength > _max)
            {
                if(_popTip)
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.string_length_more_max));
                return false;
            }
            return true;
        }
        
        public int getUnicodeStringLength(string _str)
        {
            if (string.IsNullOrEmpty(_str))
                return 0;

            byte[] stringBytes = Encoding.Unicode.GetBytes(_str);
            return _getUnicodeCharacterLength(stringBytes);
        }
        
        private int _getUnicodeCharacterLength(byte[] _unicodeBytes)
        {
            int result = 0;
            // 一次条两个字节，unicode是定长，每个字符固定两字节，刚好一个ushort的长度
            for(int i = 0; i < _unicodeBytes.Length; i += 2)
            {
                // 一个unicode字符的两个字节的脚标
                int firstByteIndex = i;
                int secondByteIndex = i + 1;

                // 计算这个unicode字符的值
                ushort charValue = Convert.ToUInt16((_unicodeBytes[secondByteIndex] << 8) + _unicodeBytes[firstByteIndex]);
                NPUnicodeLengthCheckRefObj checkRef = null;
                for(int j = 0; j < GRefdataCoreMgr.instance.unicodeLengthCheckRefCore.refList.Count; j++)
                {
                    NPUnicodeLengthCheckRefObj forUnit = GRefdataCoreMgr.instance.unicodeLengthCheckRefCore.refList[j];
                    if(forUnit != null && charValue >= forUnit.min_range_include && charValue <= forUnit.max_range_include)
                    {
                        // 找到对应的配置了，退出子循环
                        checkRef = forUnit;
                        break;
                    }
                }

                // 找到了配置就使用配置配的长度
                if(checkRef != null)
                    result += checkRef.length;
                // 找不到就使用1
                else
                    result += 1;
            }
            return result;
        }

        private bool _m_bNeedTranslateKey = true;//是否需要翻译key
        public bool needTranslateKeyKey { get { return _m_bNeedTranslateKey; } }
        public void setNeedTranslateKeyKey(bool _needTranslateKey)
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            _m_bNeedTranslateKey = _needTranslateKey;

            GCommon.refreshAllText(true, true);
#else
            _m_bNeedTranslateKey = true;
#endif
        }
        
        
        //是否需要翻译, 翻译key格式： '#1_'    '#2_'  '#3_'  '#4_' 
        public bool needTranslation(string _str)
        {
            //空字符就不用翻译了
            if(_str == null)
                return false;

#if UNITY_EDITOR || UNITY_STANDALONE // 只在Editor 或 pc包判断
            // 不需要翻译key, 直接返回
            if (!needTranslateKeyKey)
                return false;
#endif
            
            //小于两个字符，没法翻译
            if(_str.Length < 2)
                return false;

            if(!_str[0].Equals('#'))
                return false;

            return true;
        }

        //尝试解析Json,取Json字符串中的指定语言字符，仅限用于无数据结构的一次性数据读取(PHP后台邮件内容、游戏公告内容.....)
        public string chgJson(string _str)
        {
            if(_str == null)
                return null;

            //尝试解析Json
            try
            {
                jsondata = JsonMapper.ToObject(_str);
            }
            catch
            {
                return _str;
            }

            //尝试获取指定Key值对应字符串
            try
            {
                //取对应语言Key值的字符
                return jsondata[((int)GameSetting.instance.getCurrentLanguage()).ToString()].ToString();
            }
            catch
            {
                //获取失败就取英文的，服务端会验证保证一定有英文,算了还是再判断一次
                _m_sString = ((int)ENPLanguage.EN_US).ToString();

                if(jsondata.Keys.Contains(_m_sString))
                    return jsondata[_m_sString].ToString();
                else
                {
                    Debug.Log("Json字符转不包含英文数据  Json str ：" + _str);
                    return null;
                }
            }
        }

        //邮箱地址格式验证
        public bool checkEmailAdress(string _str)
        {
            if(_str == null)
                return false;

            Regex r = new Regex(@"\w[-\w.+]*@([A-Za-z0-9][-A-Za-z0-9]+\.)+[A-Za-z]{2,14}");

            return r.IsMatch(_str);
        }

    }
}
