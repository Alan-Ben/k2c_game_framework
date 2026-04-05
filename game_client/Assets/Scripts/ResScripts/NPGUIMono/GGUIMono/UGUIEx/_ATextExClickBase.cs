using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public abstract class _ATextExClickBase : TextEx, IPointerClickHandler
{
    
    [ALHeader("文本内链接字体颜色")]
    public Color urlColor = Color.blue;
    [ALHeader("是否显示链接下划线")]
    public bool isShowUnderLine = true;
    [ALHeader("链接下划线颜色")]
    public Color underLineColor = Color.blue;
    // private static readonly Regex ClickRegex = new Regex(@"<u>(.*?)</u>", RegexOptions.Singleline);
    private static readonly Regex ClickRegex = new Regex(@"<a\shref=([^>\n\s]+)>(.*?)(</a>)", RegexOptions.Singleline);

    private static readonly string[] _g_uguiSymbols1 = {"b", "i"};
    private static readonly string[] _g_uguiSymbols2 = {"color", "size"};
    
    /// <summary>
    /// 下划线距字符距离 可考虑是否开放设置
    /// </summary>
    protected float _m_underLineDistance = 0.0f;
    
    /// <summary>
    /// 超链接信息列表
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
        if (!isShowUnderLine)
        {
            return;
        }
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
        for (int i = 0; i < _m_herfInfos.Count; i++)
        {
            var info = _m_herfInfos[i];

            for (int j = 0; j < info.boxes.Count; j++)
            {
                if (info.boxes[j].width <= 0 || info.boxes[j].height <= 0)
                    continue;
                temVecs[0] = info.boxes[j].min + new Vector2(0, -_m_underLineDistance); //下划线左上顶点
                temVecs[1] = temVecs[0] + new Vector3(info.boxes[j].width, 0); //右上顶点
                temVecs[2] = temVecs[0] + new Vector3(info.boxes[j].width, -(height)); //右下顶点
                temVecs[3] = temVecs[0] + new Vector3(0, -(height)); //左下顶点

                temVecs[4] = info.boxes[j].min + new Vector2(0, -_m_underLineDistance) + new Vector2(halfSizeX, 0); //中 左上顶点
                temVecs[5] = temVecs[0] + new Vector3(info.boxes[j].width, 0) + new Vector3(-halfSizeX, 0); //中 右上顶点
                temVecs[6] = temVecs[0] + new Vector3(info.boxes[j].width, -(height)) +
                             new Vector3(-halfSizeX, 0); //中 右下顶点
                temVecs[7] = temVecs[0] + new Vector3(0, -(height)) + new Vector3(halfSizeX, 0); //中 左下顶点

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
                    toFill.AddVert(temVecs[k], underLineColor, temUvs[k]);
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
        int tempDelLength = 0;//因为空格和回车而去掉的字符数量
        foreach (Match match in ClickRegex.Matches(_outputText))
        {
            string appendStr = _outputText.Substring(indexText, match.Index - indexText);
            builder.Append(appendStr);
            tempLength += appendStr.Length - appendStr.Replace(" ", "").Replace("\n", "").Length;
            for (int i = 0; i < _g_uguiSymbols1.Length; i++)
            {
                tempLength += appendStr.Length - appendStr.Replace($"<{_g_uguiSymbols1[i]}>", "").Replace($"</{_g_uguiSymbols1[i]}>", "").Length;
            }
            for (int i = 0; i < _g_uguiSymbols2.Length; i++)
            {
                string pattern = $"<{_g_uguiSymbols2[i]}=(.*?)>";
                tempLength += appendStr.Length - Regex.Replace(appendStr, pattern, "").Length;
                tempLength += appendStr.Length - appendStr.Replace($"</{_g_uguiSymbols2[i]}>", "").Length;
            }
            //多出来的标签长度要去掉
            builder.Append("<color=#");
            builder.Append(ColorUtility.ToHtmlStringRGB(urlColor));
            builder.Append(">");
            tempLength += 15;
            
            Group group = match.Groups[1];
            Group group2 = match.Groups[2];
            //采集超链接信息
            HrefInfo info = new HrefInfo();
            //空格和回车没有顶点渲染，需要去掉
            info.startIndex = (builder.Length - tempLength - tempDelLength) * 4;
            // info.endIndex = info.startIndex + group2.Length * 4;
            //空格和回车没有顶点渲染，需要去掉
            int valueLength = group2.Value.Replace(" ", "").Replace("\n", "").Length;
            tempDelLength += group2.Value.Length - valueLength;
            info.endIndex = info.startIndex + valueLength * 4;
            
            
            info.content = group.Value;
            _m_herfInfos.Add(info);

            //点击文本内容
            builder.Append(group2.Value);
            
            //多出来的标签长度要去掉
            builder.Append("</color>");
            tempLength += 8;
            
            //设置当前下标位置
            indexText = match.Index + match.Length;
        }

        builder.Append(_outputText.Substring(indexText, _outputText.Length - indexText));
        return builder.ToString();
    }
    
    /// <summary>
    /// 具体的点击处理，由继承类实现
    /// </summary>
    /// <param name="_name"></param>
    protected abstract void _onHrefClick(string _name);


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
    
    /// <summary>
    /// 文本点击的时候，判断是否点击了超链接所在区域
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_m_herfInfos == null)
            return;

        Vector2 clickLocalPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, eventData.position, eventData.pressEventCamera, out clickLocalPos);

        for (int i = 0; i < _m_herfInfos.Count; i++)
        {
            HrefInfo info = _m_herfInfos[i];
            if (info != null && info.boxes != null)
            {
                for (int j = 0; j < info.boxes.Count; ++j)
                {
                    //在当前点击效果范围内
                    if (info.boxes[j].Contains(clickLocalPos))
                    {
                        _onHrefClick(info.content);
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 添加科室包围圈（测试用）
    /// </summary>
    private void _addVisibleBound()
    {
        #if UNITY_EDITOR
        int index = 0;
        foreach (Transform item in this.gameObject.transform)
        {
            Destroy(item.gameObject);
        }

        foreach (HrefInfo herfInfo in _m_herfInfos)
        {
            Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f),0.3f);
            index++;
            foreach (Rect rect in herfInfo.boxes)
            {
                GameObject go = new GameObject();
                go.name = string.Format("go_bound_box[{0}]", herfInfo.content);
                go.transform.SetParent(this.gameObject.transform);
                go.transform.localScale = Vector3.one;
                go.layer = this.gameObject.layer;

                RectTransform rectTransform = go.AddComponent<RectTransform>();
                rectTransform.sizeDelta = rect.size;
                rectTransform.localPosition = new Vector3(rect.position.x + rect.size.x / 2, rect.position.y + rect.size.y / 2, 0);
                Image image = go.AddComponent<Image>();
                image.color = color;
                image.raycastTarget = false;
            }
        }
        #endif
    }
}