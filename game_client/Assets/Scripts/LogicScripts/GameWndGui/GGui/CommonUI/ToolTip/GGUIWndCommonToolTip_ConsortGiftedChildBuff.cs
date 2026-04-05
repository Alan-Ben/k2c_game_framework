using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子卷王子嗣buff通用ToolTip
    /// </summary>
    public class GGUIWndCommonToolTip_ConsortGiftedChildBuff : _ATNPGGUIWndCommonItemToolTip<GGUIMonoCommonToolTip_ConsortGiftedChildBuff>
    {
        public GGUIWndCommonToolTip_ConsortGiftedChildBuff(string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            
            if(wnd == null)
                return;
        }
        
        protected override void _onDiscard()
        {
            base._onDiscard();
        }
        
        protected override void _onShowWnd()
        {
            base._onShowWnd();
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
        }

        protected override void _onReset()
        {
            base._onReset();
        }

        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(GNodeCommonToolTip_ConsortGiftedChildBuff));
        }

        public void setData(RectTransform _targetTransRoot, float _intervalX,float _intervalY)
        {
            _refreshWnd();
            
            setPos(_targetTransRoot, _intervalX, _intervalY);
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            long giftedBuffCount = NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.BIRTH_GIFTDE_COUM);
            string buffCountKey = string.IsNullOrEmpty(wnd.txtBuffCountKey) ? TransKeyConst.common_value : wnd.txtBuffCountKey;
            ALUGUICommon.setLabelTxt(wnd.txtBuffCount, TextTranslate.instance.getLanguage(buffCountKey, giftedBuffCount));
            
            ALUGUICommon.setGameObjEnable(wnd.hasBuffCountShow, giftedBuffCount > 0);
            ALUGUICommon.setGameObjEnable(wnd.noBuffCountShow, giftedBuffCount <= 0);
        }
    }
}