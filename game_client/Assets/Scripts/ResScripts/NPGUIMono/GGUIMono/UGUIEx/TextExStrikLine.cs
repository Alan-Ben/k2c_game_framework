using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class TextExStrikLine : TextEx
{

    [ALHeader("删除线左边高度比例")]
    public float strikLeftScale = 0.5f;
    [ALHeader("删除线右边高度比例")]
    public float strikRightScale = 0.5f;
    
    [ALHeader("删除线颜色")]
    public Color strikColor = Color.blue;
    // private static readonly Regex ClickRegex = new Regex(@"<u>(.*?)</u>", RegexOptions.Singleline);
    private static readonly Regex strikRegex = new Regex(@"<s>(.*?)(</s>)", RegexOptions.Singleline);
    protected static readonly Regex strikRichTextRegex = new Regex("(<s>(.*?)(</s>))|(<b>)|(</b>)|(<i>)|(</i>)|(<size.+?>)|(</size>)|(<color.+?>)|(</color>)|(<u.*?>)|(</u>)|(<a.+?>)|(</a>)");

    
    /// <summary>
    /// 删除线内容信息列表
    /// </summary>
    private List<HrefInfo> _m_herfInfos = new List<HrefInfo>();
    

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (_m_herfInfos != null)
            _m_herfInfos.Clear();
        _m_herfInfos = null;
    }
    
    /// <summary>
    /// 刷新顶点的时候 获取点击box范围
    /// </summary>
    /// <param name="_vertexHelper"></param>
    protected override void OnPopulateMesh(VertexHelper _vertexHelper)
    {
        string orignText = m_Text;
        m_Text = getOutputText(orignText);
        base.OnPopulateMesh(_vertexHelper);
        m_Text = orignText;

        if (_m_herfInfos == null || _vertexHelper == null)
            return;
        UIVertex vert = new UIVertex();
        HrefInfo info = null;
        
        //生成用于判断点击范围的列表
        for (int i = 0; i < _m_herfInfos.Count; i++)
        {
            info = _m_herfInfos[i];
            if (info == null || info.boxes == null)
                continue;

            info.boxes.Clear();
            if (info.startIndex >= _vertexHelper.currentVertCount)
            {
                continue;
            }

            // 将文本顶点索引坐标加入到包围框
            _vertexHelper.PopulateUIVertex(ref vert, info.startIndex);
            var pos = vert.position;
            var bounds = new Bounds(pos, Vector3.zero);
            //每四个顶点一个字符，防止出现同一行字符顶点x值小于前一字符顶点x值的情况
            // for (int j = info.startIndex + 2, m = info.endIndex; j < m; j += 4 )
            for (int j = info.startIndex ; j < info.endIndex; j ++ )
            {
                if (j >= _vertexHelper.currentVertCount)
                {
                    break;
                }

                _vertexHelper.PopulateUIVertex(ref vert, j);
                pos = vert.position;
                bool needEncapsulate = true;
                // 换行重新添加包围框
                if ((j - info.startIndex) % 4 == 0)
                {
                    if(j < 4)
                        continue;
                    UIVertex lastV = new UIVertex();
                    _vertexHelper.PopulateUIVertex(ref lastV, j - 4);
                    
                    var lastPos = lastV.position;
                    if (pos.x < lastPos.x && pos.y < lastPos.y)//换行
                    {
                        info.boxes.Add(new Rect(bounds.min, bounds.size));
                        //以新一行行首字符左上顶点添加包围框
                        bounds = new Bounds(vert.position, Vector3.zero);
                        needEncapsulate = false;
                    }
                }
                
                if(needEncapsulate)
                {
                    // 扩展包围框
                    bounds.Encapsulate(pos);
                }
            }

            info.boxes.Add(new Rect(bounds.min, bounds.size));
        }
        //画下划线
        drawUnderLine(_vertexHelper);
        // _addVisibleBound();
    }

    /// <summary>
    /// 画下划线
    /// </summary>
    /// <param name="toFill"></param>
    private void drawUnderLine(VertexHelper toFill)
    {
        // if (!isShowUnderLine)
        // {
        //     return;
        // }
        if (_m_herfInfos == null || _m_herfInfos.Count <= 0)
            return;

        cachedTextGenerator.Populate("—", GetGenerationSettings(rectTransform.rect.size));
        IList<UIVertex> uList = cachedTextGenerator.verts;
        if (uList == null || uList.Count <= 0)
            return;

        Vector3[] temVecs = new Vector3[8];
        Vector2[] temUvs = new Vector2[8];

        float sizeX = uList[1].position.x - uList[0].position.x;
        float sizeY = uList[0].position.y - uList[3].position.y;
        float halfSizeX = sizeX / 2;
        Vector2 uvMiddleUp = (uList[1].uv0 + uList[0].uv0) / 2;
        Vector2 uvMiddleDown = (uList[3].uv0 + uList[2].uv0) / 2;

        float height = sizeY;
        float bocHeight = 0;
        
        for (int i = 0; i < _m_herfInfos.Count; i++)
        {
            var info = _m_herfInfos[i];

            for (int j = 0; j < info.boxes.Count; j++)
            {
                if (info.boxes[j].width <= 0 || info.boxes[j].height <= 0)
                    continue;
                bocHeight = info.boxes[j].height;
                
                temVecs[0] = info.boxes[j].min + new Vector2(0, strikLeftScale * bocHeight + height / 2); //下划线左上顶点
                temVecs[1] = info.boxes[j].min + new Vector2(info.boxes[j].width, strikRightScale * bocHeight + height / 2); //右上顶点
                temVecs[2] = info.boxes[j].min + new Vector2(info.boxes[j].width, strikRightScale * bocHeight - height / 2); //右下顶点
                temVecs[3] = info.boxes[j].min + new Vector2(0, strikLeftScale * bocHeight - height / 2); //左下顶点

                temVecs[4] = info.boxes[j].min + new Vector2(0, strikLeftScale * bocHeight + height / 2); //中 左上顶点
                temVecs[5] = info.boxes[j].min + new Vector2(info.boxes[j].width, strikRightScale * bocHeight + height / 2); //中 右上顶点
                temVecs[6] = info.boxes[j].min + new Vector2(info.boxes[j].width, strikRightScale * bocHeight - height / 2); //中 右下顶点
                temVecs[7] = info.boxes[j].min + new Vector2(0, strikLeftScale * bocHeight - height / 2); //中 左下顶点
                
                temUvs[0] = uList[0].uv0;
                temUvs[1] = uList[1].uv0;
                temUvs[2] = uList[2].uv0;
                temUvs[3] = uList[3].uv0;
                temUvs[4] = uvMiddleUp;
                temUvs[5] = uvMiddleUp;
                temUvs[6] = uvMiddleDown;
                temUvs[7] = uvMiddleDown;

                int startCount = toFill.currentVertCount;

                for (int k = 0; k < 8; k++)
                {
                    toFill.AddVert(temVecs[k], strikColor, temUvs[k]);
                }

                toFill.AddTriangle(startCount + 0, startCount + 4, startCount + 7);
                toFill.AddTriangle(startCount + 7, startCount + 3, startCount + 0);
                toFill.AddTriangle(startCount + 4, startCount + 5, startCount + 6);
                toFill.AddTriangle(startCount + 6, startCount + 7, startCount + 4);
                toFill.AddTriangle(startCount + 5, startCount + 1, startCount + 2);
                toFill.AddTriangle(startCount + 2, startCount + 6, startCount + 5);
            }
        }
    }

    /// <summary>
    /// 获取解析后的输出文本
    /// </summary>
    /// <returns>_outputText</returns>
    private string getOutputText(string _outputText)
    {
        if (_outputText == null)
            return String.Empty;

        StringBuilder builder = new StringBuilder();

        if (null == _m_herfInfos)
            _m_herfInfos = new List<HrefInfo>();
        _m_herfInfos.Clear();
        //上一个截取字符下标的位置
        int indexText = 0;
        int tempLength = 0;
        foreach (Match match in strikRichTextRegex.Matches(_outputText))
        {
            builder.Append(_outputText.Substring(indexText, match.Index - indexText));
            
            if (match.Value.EndsWith("</s>"))
            {
                Group group = match.Groups[2];
                //采集超链接信息
                HrefInfo info = new HrefInfo();
                //空格和回车没有顶点渲染，需要去掉
                info.startIndex = (builder.ToString().Replace(" ","").Replace("\n","").Length - tempLength) * 4;
                info.endIndex = info.startIndex + group.Length * 4;
                info.content = group.Value;
                _m_herfInfos.Add(info);

                //点击文本内容
                builder.Append(group.Value);
            }
            else
            {
                var content = match.Groups[1].Value;
                builder.Append(content);
            }
            
            //设置当前下标位置
            indexText = match.Index + match.Length;
        }
        
        indexText = 0;
        builder.Clear();
        foreach (Match match in strikRegex.Matches(_outputText))
        {
            builder.Append(_outputText.Substring(indexText, match.Index - indexText));
            //点击文本内容
            builder.Append(match.Groups[1].Value);
            //设置当前下标位置
            indexText = match.Index + match.Length;
        }

        builder.Append(_outputText.Substring(indexText, _outputText.Length - indexText));
        return builder.ToString();
    }


    /// <summary>
    /// 超链接信息类
    /// </summary>
    private class HrefInfo
    {
        public int startIndex;
        public int endIndex;
        public string content;//超链接的链接内容
        public readonly List<Rect> boxes = new List<Rect>();
    }
}