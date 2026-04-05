
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// item+文本+文本tip模板
    /// </summary>
    public class NPGGUIWndItemTextTextTip : _ATNPGGUIWndTip<NPGGUIMonoItemTextTextTip>
    {
        public NPGGUIWndItemTextTextTip(NPGGUIMonoItemTextTextTip _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        private NPGGUIWndCommonItem _m_wItemWnd;//图片
        private GGuiWndSprite _m_spcQualityWnd;//特殊显示品质框
        protected override void _onShowWndEx()
        {
            if (_m_wItemWnd != null)
                _m_wItemWnd.showWnd();
            if (null != _m_spcQualityWnd)
            {
                _m_spcQualityWnd.showWnd();
            }
        }

        protected override void _onResetEx()
        {
            if (_m_wItemWnd != null)
                _m_wItemWnd.resetWnd();
            if (null != _m_spcQualityWnd)
            {
                _m_spcQualityWnd.discardTexture();
            }
        }

        protected override void _onHideWndEx()
        {
            if (_m_wItemWnd != null)
                _m_wItemWnd.hideWnd();
            
            if (null != _m_spcQualityWnd)
            {
                _m_spcQualityWnd.discardTexture();
            }
        }

        protected override void _onDiscardEx()
        {
            if (_m_wItemWnd != null)
                _m_wItemWnd.discard();
            _m_wItemWnd = null;
            
            if (null != _m_spcQualityWnd)
            {
                _m_spcQualityWnd.discard();
                _m_spcQualityWnd = null;
            }
        }

        protected override void _onWndInitDoneEx()
        {
            if (wnd == null)
                return;

            if (wnd.item != null)
            {
                _m_wItemWnd = new NPGGUIWndCommonItem(wnd.item);
            }
            if (wnd.itemQualityImg != null)
            {
                _m_spcQualityWnd = new GGuiWndSprite(wnd.itemQualityImg);
            }
        }

        /// <summary>
        /// 设置tip数据
        /// </summary>
        /// <param name="_icon"></param>
        /// <param name="_str"></param>
        public void setTipData(_IItem _item, string _strOne, string _strTow)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtStrOne, _strOne);
            ALUGUICommon.setLabelTxt(wnd.txtStrTow, _strTow);

            if (_m_wItemWnd != null)
            {
                _m_wItemWnd.setItem(_item);
                _m_wItemWnd.showWnd();
            }

            NPQualityRefObj qualityRefObj = GRefdataCoreMgr.instance.getQuality(_item.getItemType(), _item.getQuality());
            
            if (_m_spcQualityWnd != null && qualityRefObj != null && qualityRefObj.sp_icon.enable())
            {
                _m_spcQualityWnd.setTexture(qualityRefObj.sp_icon);
                _m_spcQualityWnd.showWnd();
            }
        }
    }
}