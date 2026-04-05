using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 新的物品UI样式，根据物品数据动态加载UI内容，支持显示{已有数量}/{物品数量}
    /// </summary>
    public class NPGGUIWndCommonItem : _ANPGGUIWndCommonItem<NPGGUIMonoCommonItem>
    {
    
        public NPGGUIWndCommonItem(NPGGUIMonoCommonItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }
    }



    //不是动态创建的
    public class NPGGUIWndPrefabItemWnd : NPGGUIWndCommonItem
    {
        public NPGGUIWndPrefabItemWnd(NPGGUIMonoCommonItem _wnd)
           : base(_wnd)
        {
        }

        /***************
         * 释放窗口资源相关对象
         **/
        public override void discard()
        {
            //判断是否加载完成，是则隐藏本窗口
            hideWnd();

            //调用事件函数
            _onDiscard();

            _m_monoWnd = null;
            _m_rtRectTransform = null;
        }
    }

}
