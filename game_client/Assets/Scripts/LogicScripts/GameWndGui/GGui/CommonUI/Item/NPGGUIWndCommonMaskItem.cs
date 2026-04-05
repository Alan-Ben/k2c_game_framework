using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 新的物品UI样式，根据物品数据动态加载UI内容，支持显示{已有数量}/{物品数量}(item带遮罩)
    /// </summary>
    public class NPGGUIWndCommonMaskItem : _ANPGGUIWndCommonItem<NPGGUIMonoCommonMaskItem>
    {
    
        public NPGGUIWndCommonMaskItem(NPGGUIMonoCommonMaskItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        /// <summary>
        /// 设置遮罩和置灰的显示
        /// </summary>
        /// <param name="_isShowMask">是否显示遮罩</param>
        /// <param name="_needShowGray">是否置灰</param>
        public void setIsShowMask(bool _isShowMask, bool _needShowGray)
        {
            if (null == wnd)
                return;
            
            ALUGUICommon.setGameObjEnable(wnd.maskList, _isShowMask);

            if(_needShowGray)
                GGameCommonInfo.grayImage(wnd.grayList);
            else
                GGameCommonInfo.disgrayImage(wnd.grayList);
        }
    }

}
