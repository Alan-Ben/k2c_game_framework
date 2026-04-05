using System;

using UnityEngine.UI;

/******************
 * 图集对象显示集合的脚本对象
 **/
namespace ALPackage
{
    public class _TALUGUIMonoContainerWnd<T> : _AALBasicUIWndMono where T : _AALBasicUIWndMono
    {
        //拖拽的scroll rect对象,请注意，当这个scroll rect有设置的时候，将会由代码调整 container 的大小以达到可被scroll rect拖拽的目的
        public ScrollRect scrollRect;
        //单元对象存放的图集容器
        public LayoutGroup itemContainer;
        //单元模板对象
        public T itemTemplate;
    }
}
