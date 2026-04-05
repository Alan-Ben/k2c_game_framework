using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 热更窗体Grid专用mono
    /// </summary>
    [RequireComponent(typeof(MonoSkin))]
    public class GGUIHotfixGridMono : _AALBasicUIWndMono
    {
        [HideInInspector]
        public MonoSkin monoSkin;

        [ALInfo("根据当前SC拖动朝向，增加拖拽区域边界纵向拖动 \nX+上边界，Y+下边界横向拖动 \nX+左边界，Y+右边界")]
        [ALHeader("根据当前允许朝向，在不同方向上增加的拖拽区域边界尺寸。x表示上/左边界，y表示下/右")]
        public Vector2 paddingForSide;
        [ALHeader("拖拽的scroll rect对象")]
        public ScrollRect scrollRect;
        [ALHeader("遮罩对象脚本")]
        public RectTransform gridAreaMaskObj;
        [ALHeader("区域控制的UI对象")]
        public RectTransform gridAreaUIObj;
        [ALHeader("单元模板对象")]
        public GGUIHotfixCommonMono itemTemplate;
        [ALHeader("单元模板对象-宽")]
        public int itemTemplateWidth;
        [ALHeader("单元模板对象-高")]
        public int itemTemplateHeight;
        [ALHeader("模板间隔")]
        public Vector2 spaceSize;
        [ALHeader("显示对象方向枚举")]
        public ALGUIListLayoutStyle layoutStyle;
        [ALHeader("每行或每列的数量，当横向时表示每列数量，当纵向时表示每行数量")]
        public int perLineItemCount;
        [ALHeader("滚动条对象")]
        public Scrollbar scrollbar;
        [ALHeader("是否自动适应对应的宽度或高度")]
        public bool autoFixLine;
        [ALHeader("是否开启显示窗口逐个播放item的显示动画")]
        public bool openItemShowAnim = false;
        [ALHeader("显示窗口的第一批Item播放刷新动画的播放间隔")]
        public float firstItemShowAnimDelay = 0.2f;

        private void Awake () {
            monoSkin = this.GetComponent<MonoSkin>();
        }
    }
}