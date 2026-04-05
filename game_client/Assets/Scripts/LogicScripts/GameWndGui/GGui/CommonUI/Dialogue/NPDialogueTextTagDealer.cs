using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 文本标签处理器
    /// </summary>
    public class NPDialogueTextTagDealer
    {
        [NotNull] private List<NPTextTag> _m_sTagList;//标签列表

        public NPDialogueTextTagDealer()
        {
            _m_sTagList = new List<NPTextTag>();
        }

        /// <summary>
        /// 初始化标签信息
        /// </summary>
        /// <param name="_originText"></param>
        /// <returns>去除标签后的文本</returns>
        public string initTagInfo(string _originText)
        {
            //先清除标签列表
            clear();

            //开始解析文本中的标签
            int textLength = 0;//文本部分长度
            int tagLength = 0;//标签部分长度
            bool isCutting = false;//是否正在截取标签
            while (textLength + tagLength < _originText.Length)
            {
                if (!isCutting)
                {
                    if (_originText[textLength] == '<')
                    {
                        //开始截取标签
                        isCutting = true;
                        tagLength = 1;
                    }
                    else
                    {
                        textLength++;
                    }
                }
                else
                {
                    if (_originText[textLength + tagLength] == '>')
                    {
                        //截取标签文本
                        string tagStr = _originText.Substring(textLength + 1, tagLength - 1);
                        //解析并插入到列表中
                        _analyseTag(tagStr, textLength);
                        //从文本中移除
                        _originText = _originText.Remove(textLength, tagLength + 1);
                        //重置参数
                        isCutting = false;
                        tagLength = 0;
                    }
                    else
                    {
                        tagLength++;
                    }
                }
            }

            return _originText;
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void clear()
        {
            _m_sTagList.Clear();
        }

        /// <summary>
        /// 对文本应用当前标签，入参为不带标签的文本
        /// </summary>
        /// <param name="_text"></param>
        /// <returns></returns>
        public string applyTag(string _text)
        {
            int curLength = _text.Length;
            for (int i = _m_sTagList.Count - 1; i >= 0; i--)
            {
                NPTextTag temp = _m_sTagList[i];
                if (temp == null)
                    continue;

                _text = temp.applyTagLabel(_text, curLength);
            }
            return _text;
        }

        /// <summary>
        /// 解析标签信息并插入到列表中
        /// </summary>
        /// <param name="_tagStr"></param>
        /// <param name="_insertIndex"></param>
        private void _analyseTag(string _tagStr, int _insertIndex)
        {
            if (string.IsNullOrEmpty(_tagStr))
                return;

            //加粗
            if (string.Equals(_tagStr,"b"))
            {
                _m_sTagList.Add(new NPTextTag(ENPTextTagType.BOLD, _insertIndex, _insertIndex, "<b>"));
            }
            else if (string.Equals(_tagStr, "/b"))
            {
                tryInsertRightTag(ENPTextTagType.BOLD, "</b>");
            }
            //字体大小
            else if (_tagStr.StartsWith("size"))
            {
                _m_sTagList.Add(new NPTextTag(ENPTextTagType.SIZE, _insertIndex, _insertIndex, $"<{_tagStr}>"));
            }
            else if (string.Equals(_tagStr, "/size"))
            {
                tryInsertRightTag(ENPTextTagType.SIZE, "</size>");
            }
            //斜体
            else if (string.Equals(_tagStr, "i"))
            {
                _m_sTagList.Add(new NPTextTag(ENPTextTagType.ITALIC, _insertIndex, _insertIndex, "<i>"));
            }
            else if (string.Equals(_tagStr, "/i"))
            {
                tryInsertRightTag(ENPTextTagType.ITALIC, "</i>");
            }
            //颜色
            else if (_tagStr.Length > 6 && string.Equals(_tagStr.Substring(0, 6), "color="))//<color=#FFFFFF>
            {
                string richTextColor = _tagStr.Remove(0, 7);
                _m_sTagList.Add(new NPTextTag(ENPTextTagType.COLOR, _insertIndex, _insertIndex, string.Format("<color=#{0}>", richTextColor)));
            }
            else if (_tagStr.Length > 5 && string.Equals(_tagStr.Substring(0, 5), "color"))
            {
                _tagStr = _tagStr.Remove(0, 6);
                string[] strs = _tagStr.Split(new char[] { ';', ':' });
                if (strs.Length < 3 || strs[0] == null || strs[1] == null || strs[2] == null)
                {
                    Debug.LogError("【NPDialogueTextTagMgr._analyseTag Error】对话文字效果<color:255:0:0> 内，rgb颜色配置错误");
                    return;
                }
                int r = int.Parse(strs[0]);
                int g = int.Parse(strs[1]);
                int b = int.Parse(strs[2]);
                int a = 255;
                string richTextColor = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, a);
                _m_sTagList.Add(new NPTextTag(ENPTextTagType.COLOR, _insertIndex, _insertIndex, string.Format("<color=#{0}>", richTextColor)));
            }
            else if (string.Equals(_tagStr, "/color"))
            {
                tryInsertRightTag(ENPTextTagType.COLOR, "</color>");
            }

            //尝试插入[右标签]到列表中
            void tryInsertRightTag(ENPTextTagType _tagType, string _tagLabel)
            {
                for (int i = _m_sTagList.Count - 1; i >= 0; i--)
                {
                    NPTextTag temp = _m_sTagList[i];
                    if (temp == null)
                        continue;

                    if (temp.tagType == _tagType && !temp.isEnd)
                    {
                        //设置[左标签]已经终止，即已找到对应的[右标签]
                        temp.setEnd();
                        _m_sTagList.Add(new NPTextTag(_tagType, temp.startIndex, _insertIndex, _tagLabel, true));
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 文本标签类型
        /// </summary>
        private enum ENPTextTagType
        {
            BOLD,            //加粗
            ITALIC,          //斜体
            COLOR,           //颜色
            SIZE,           //字体大小
        }

        /// <summary>
        /// 文本标签信息
        /// </summary>
        private class NPTextTag
        {
            private ENPTextTagType _m_eTagType;//标签类型
            private bool _m_bIsEnd;//标记这个标签是否已经终止，即找到另一半标签
            private int _m_iStartIndex;//原始文本中，开始作用的下标
            private int _m_iInsertIndex;//原始文本中，实际插入的下标
            private string _m_sTagLabel;//标签文本

            public NPTextTag(ENPTextTagType _tagType, int _startIndex, int _insertIndex, string _tagLabel,bool _isEnd = false)
            {
                _m_eTagType = _tagType;
                _m_iStartIndex = _startIndex;
                _m_iInsertIndex = _insertIndex;
                _m_sTagLabel = _tagLabel;
                _m_bIsEnd = _isEnd;
            }

            public ENPTextTagType tagType { get { return _m_eTagType; }}
            public bool isEnd { get { return _m_bIsEnd; } }
            public int startIndex { get { return _m_iStartIndex; } }

            /// <summary>
            /// 应用标签，这个方法需要按标签插入顺序从后往前调用
            /// </summary>
            /// <param name="_text">当前文本</param>
            /// <param name="_curLength">当前文本原始长度</param>
            /// <returns></returns>
            public string applyTagLabel(string _text, int _curLength)
            {
                if (string.IsNullOrEmpty(_text) || string.IsNullOrEmpty(_m_sTagLabel))
                    return _text;

                if (_curLength >= _m_iStartIndex)
                {
                    _text = _text.Insert(_curLength > _m_iInsertIndex ? _m_iInsertIndex : _curLength, _m_sTagLabel);
                }
                return _text;
            }

            /// <summary>
            /// 设置这个标签已经终止了，即找到另一半标签
            /// </summary>
            public void setEnd()
            {
                _m_bIsEnd = true;
            }
        }
    }
}
