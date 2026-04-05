using System;

using UnityEngine;
using UnityEngine.UI;

/******************
 * 图集对象显示集合的脚本对象
 **/
namespace ALPackage
{
    public class _TALUGUIMonoGridWnd<T> : _AALBasicUIWndMono where T : _TALUGUIMonoGridItem
    {
        [ALInfo("根据当前SC拖动朝向，增加拖拽区域边界纵向拖动 \nX+上边界，Y+下边界横向拖动 \nX+左边界，Y+右边界")]
        [ALHeader("根据当前允许朝向，在不同方向上增加的拖拽区域边界尺寸。x表示上/左边界，y表示下/右")]
        public Vector2 paddingForSide;

        //拖拽的scroll rect对象
        public ScrollRect scrollRect;
        //遮罩对象脚本
        public RectTransform gridAreaMaskObj;
        //区域控制的UI对象
        public RectTransform gridAreaUIObj;

        //单元模板对象
        public T itemTemplate;
        //模板间隔
        public Vector2 spaceSize;

        //显示对象方向枚举
        public ALGUIListLayoutStyle layoutStyle;
        //每行或每列的数量，当横向时表示每列数量，当纵向时表示每行数量
        public int perLineItemCount;

        //滚动条对象
        public Scrollbar scrollbar;

        //是否自动适应对应的宽度或高度
        public bool autoFixLine;
    }
}
