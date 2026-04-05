using System;
using System.Collections.Generic;
using System.Text;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using GOE;
using UnityEngine.Pool;

/// <summary>
/// TextEx 文本超框显示为省略号的Mono
/// </summary>
public class TextExEllipsis : TextEx
{
    [NotNull] private static readonly Regex ClickRegex = new Regex(@"<u>(.*?)</u>", RegexOptions.Singleline);
    private static readonly string EllipsisText = "…";

    [ALHeader("是否长按显示全部文本提示框")]
    public bool ShowFullTextTip = true;      //是否显示提示框
    [ALHeader("提示框与物品间距")]
    public float interval = 0;             //提示框间距

    private bool _m_bHasRegAction = false; // 用于确保每次注册都会被注销掉

    private string _m_outPutText = String.Empty;//去除标签的文本
    private bool _m_bIsEllipsisTextDirty = false; //省略号功能是否需要重新生成
    private string _m_textWithEllipsis = String.Empty;//去除标签的文本
    
    public override string text
    {
        get
        {
            if (!_m_bIsEllipsisTextDirty) 
                return _m_textWithEllipsis;
            // 如果是Vertices Dirty 则需要重新生成省略号文本
            _m_outPutText = getOutputText(base.text);
            _m_textWithEllipsis = getTextWithEllipsis(_m_outPutText);
            _m_bIsEllipsisTextDirty = false;
            return _m_textWithEllipsis;
        }
        set
        {
            base.text = value;
        }
    }
    
    public override void SetVerticesDirty()
    {
        // 设置Vertices Dirty 的时候把省略号文本信息也要重新生成
        _m_bIsEllipsisTextDirty = true;
        base.SetVerticesDirty();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        tryCloseClickShow();
    }
    // 超框显示...
    protected string getTextWithEllipsis(string _value)
    {
#if NP_GAME
        // 替换不换行空格
        if (GCommon.getIsNoBreakSpace(g_Lang))
            _value = _value?.Replace(" ", "\u00A0");
#endif

        if (cachedTextGenerator == null || _value == null)
            return _value;
        var set = GetGenerationSettings(rectTransform.rect.size);
        set.horizontalOverflow = HorizontalWrapMode.Wrap;
        set.verticalOverflow = VerticalWrapMode.Truncate;

        cachedTextGenerator.Populate(EllipsisText, set);

        float ellipsisWidth = 0;
        foreach (UICharInfo character in cachedTextGenerator.characters)
            ellipsisWidth += character.charWidth;

        //获取没有富文本的字符串
        string withoutRichValue = getOutputTextWithoutRich(_value);
        cachedTextGenerator.Populate(withoutRichValue, set);

        int characterCountVisible = cachedTextGenerator.characterCountVisible;
        if (withoutRichValue.Length <= characterCountVisible)
        {
            tryCloseClickShow();
            return _value;
        }

        //如果未超框则不需要有点击功能
        tryOpenClickShow();

        //从通过字符数量的判断改为 字符宽度的计算，确保字符宽度不一的情况下，导致字符省略号超出可显示的区域导致省略号没显示的问题
        int visibleReplaceEllipsisNum = 0;
        float sumWidth = 0;
        for (int i = cachedTextGenerator.characters.Count - 1; i >= 0; i--)
        {
            UICharInfo character = cachedTextGenerator.characters[i];
            sumWidth += character.charWidth;
            visibleReplaceEllipsisNum++;
            if (sumWidth > ellipsisWidth)
                break;
        }

        if (characterCountVisible <= visibleReplaceEllipsisNum)
        {
            return EllipsisText;
        }

#if NP_GAME
        TxtMatchInfos info = GenericPool<TxtMatchInfos>.Get();
        info.setInfo(_value);

        //可显示文本数量
        int showVisibleCount = characterCountVisible - visibleReplaceEllipsisNum;
        // 实际文本数量  = 可显示文本数量 + 富文本数量
        int realCount = 0;
        for (int i = 0; i < showVisibleCount;)
        {
            if (info.isValid(realCount))
            {
                if (!info.isValidRichChar(realCount))
                {
                    i++;
                }
            }
            else
            {
                break;
            }

            realCount++;
        }


        string updatedText = _value.Substring(0, realCount);
        updatedText += EllipsisText;

        //补全可能被截断的富文本
        StringBuilder sb = GenericPool<StringBuilder>.Get();
        for (int i = realCount - 1; i < info.getCount(); i++)
        {
            if (info.isValidRichChar(i))
                sb.Append(info.getChar(i));
        }
        updatedText += sb.ToString();
        sb.Clear();
        GenericPool<StringBuilder>.Release(sb);

        info.clear();
        GenericPool<TxtMatchInfos>.Release(info);

        return updatedText;
#else
        return null;
#endif

    }
#if NP_GAME
    /// <summary>
    /// 用于判断相应char是富文本字符或空字符
    /// </summary>
    private class TxtMatchInfos
    {
        private struct TxtMatch
        {
            public bool isRich;
            public bool isEmpty;
            public char c;
        }

        private List<TxtMatch> matches;
        // 统计字符是否是有效字符，而不是富文本之类的
        public void setInfo(string txt)
        {
            if (string.IsNullOrEmpty(txt))
                return;

            matches = ListPool<TxtMatch>.Get();// new List<TxtMatch>(txt.Length);

            for (int i = 0; i < txt.Length; i++)
            {
                matches.Add(new TxtMatch { isEmpty = false, isRich = false, c = txt[i] });
            }

            MatchCollection richCollect = RichTextRegex.Matches(txt);
            for (int k = 0; k < richCollect.Count; k++)
            {
                Match match = richCollect[k];

                for (int i = 0; i < match.Length; i++)
                {

                    TxtMatch t = matches[match.Index + i];
                    t.isRich = true;
                    t.isEmpty = true;
                    matches[match.Index + i] = t;
                }
            }
        }


        public bool isValidRichChar(int _index)
        {
            return matches != null && _index < matches.Count && matches[_index].isRich;
        }

        public bool isValid(int _index)
        {
            return matches != null && _index < matches.Count;
        }

        public char getChar(int _index)
        {
            if (isValid(_index))
                return matches[_index].c;

            return '\0';
        }

        public int getCount()
        {
            return matches != null ? matches.Count : 0;
        }

        public void clear()
        {
            ListPool<TxtMatch>.Release(matches);
        }
    }

#endif

    protected void tryOpenClickShow()
    {
        if (!ShowFullTextTip || _m_bHasRegAction)
            return;
        _m_bHasRegAction = true;
        ALUGUICommon.combineBtnClick(gameObject, _showToolTip);
    }

    protected void tryCloseClickShow()
    {
        if (!ShowFullTextTip && !_m_bHasRegAction)
            return;
        _m_bHasRegAction = false;
        ALUGUICommon.uncombineBtnClick(gameObject, _showToolTip);
    }

    private void _showToolTip(GameObject _go)
    {
#if NP_GAME

        ALCommonTaskController.CommonActionAddLaterMonoTask(() =>
        {
            bool isActiveInHierarchy = false;
            if (gameObject != null)
                isActiveInHierarchy = gameObject.activeInHierarchy;
            if (isActiveInHierarchy)
            {
                QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_Text(
                    UIResPathAssistant.getAssetPath(UIResPathConst.WIN_TOOL_TIP_TEXT), 
                    UIResPathAssistant.getObjName(UIResPathConst.WIN_TOOL_TIP_TEXT), 
                    _m_outPutText, 
                    rectTransform, 
                    0, 
                    interval));
            }
        }, NPConst.TOOL_TIP_DELAY_TIME);

#endif
    }

    
    /// <summary>
    /// 获取解析后的输出文本
    /// </summary>
    /// <returns>_outputText</returns>
    private string getOutputText(string _outputText)
    {
#if NP_GAME
        if (_outputText == null)
            return String.Empty;

        StringBuilder builder = new StringBuilder();

        //上一个截取字符下标的位置
        var indexText = 0;
        int vertexSum = 0;//顶点数量和
        foreach (Match match in ClickRegex.Matches(_outputText))
        {
            var subStr = _outputText.Substring(indexText, match.Index - indexText);
            builder.Append(subStr);

            var content = match.Groups[1].Value;
            builder.Append(content);

            cachedTextGenerator.Populate(subStr, GetGenerationSettings(rectTransform.rect.size));
            int startIndex = vertexSum + cachedTextGenerator.vertexCount;
            vertexSum = startIndex;

            cachedTextGenerator.Populate(content, GetGenerationSettings(rectTransform.rect.size));
            int endIndex = vertexSum + cachedTextGenerator.vertexCount;
            vertexSum = endIndex;

            //设置当前下标位置
            indexText = match.Index + match.Length;
        }
        builder.Append(_outputText.Substring(indexText, _outputText.Length - indexText));
        return builder.ToString();
#else
        return null;
#endif
    }

    /// <summary>
    /// 获取解析后的输出文本
    /// </summary>
    /// <returns>_outputText</returns>
    private string getOutputTextWithoutRich(string _outputText)
    {
#if NP_GAME
        if (_outputText == null)
            return String.Empty;

        StringBuilder builder = new StringBuilder();

        //上一个截取字符下标的位置
        var indexText = 0;
        int vertexSum = 0;//顶点数量和
        foreach (Match match in RichTextRegex.Matches(_outputText))
        {
            var subStr = _outputText.Substring(indexText, match.Index - indexText);
            builder.Append(subStr);

            var content = match.Groups[1].Value;
            builder.Append(content);

            cachedTextGenerator.Populate(subStr, GetGenerationSettings(rectTransform.rect.size));
            int startIndex = vertexSum + cachedTextGenerator.vertexCount;
            vertexSum = startIndex;

            cachedTextGenerator.Populate(content, GetGenerationSettings(rectTransform.rect.size));
            int endIndex = vertexSum + cachedTextGenerator.vertexCount;
            vertexSum = endIndex;

            //设置当前下标位置
            indexText = match.Index + match.Length;
        }
        builder.Append(_outputText.Substring(indexText, _outputText.Length - indexText));
        return builder.ToString();
#else
        return null;
#endif
    }


}
