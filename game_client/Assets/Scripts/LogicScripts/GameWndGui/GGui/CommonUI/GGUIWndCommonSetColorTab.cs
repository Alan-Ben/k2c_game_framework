using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 可设置选中颜色的页签
    /// </summary>
    public class GGUIWndCommonSetColorTab : _ATNPGGUIWndTabItem<GGUIMonoCommonSetColorTab>
    {
        public GGUIWndCommonSetColorTab(GGUIMonoCommonSetColorTab _wnd) : base(_wnd)
        {
        }

        public override void setSelected(bool _isSelect)
        {
            base.setSelected(_isSelect);
            if (wnd == null || wnd.setColorList == null)
                return;

            Color targetColor = _isSelect ? wnd.selectColor : wnd.notSelectColor;
            ALUGUICommon.setUIObjColor(wnd.setColorList, targetColor);
        }
    }
}
