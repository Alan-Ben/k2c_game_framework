
using ALPackage;
using ChatPackage;
using UnityEngine;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{

    /// <summary>
    /// 一条分享信息的msgItem,包含文本和点击
    /// </summary>
    public abstract class _AGGUIWndChatMsgItemShareCommon<T_MONO ,T_DATA> : _ATNPGGUIWndPlayerChatMsgItem<T_MONO, T_DATA>
        where T_MONO : GGUIMonoChatMsgItemShareCommon
        where T_DATA : _IMsgItemData
    {
        protected _AGGUIWndChatMsgItemShareCommon(T_DATA _detailInfo, Transform _parent, [NotNull] GUICacheMgrChatMsgItem _cacheMgr) : base(_detailInfo, _parent, _cacheMgr)
        {
        }
        protected override void _onShowWndEx()
        {
            _refreshWnd();
        }

        protected override void _onHideWndEx()
        {
        }

        protected override void _onResetEx()
        {
        }

        protected override void _onDiscardEx()
        {
            ALUGUICommon.uncombineBtnClick(wnd.btnInfo, _clickInfoBtn);
        }

        protected override void _onWndInitDoneEx()
        {
            if (wnd != null)
            {
                ALUGUICommon.combineBtnClick(wnd.btnInfo, _clickInfoBtn);
            }
        }

        /// <summary>
        /// 分享的详情信息
        /// </summary>
        /// <param name="_"></param>
        protected abstract void _clickInfoBtn(GameObject _);

        private void _refreshWnd()
        {
            ALUGUICommon.setLabelTxt(wnd.textName, _getShareContent());
            _refreshWndEx();
        }

        protected abstract void _refreshWndEx();

        protected abstract string _getShareContent();

        protected override void _loadAdditionTemplate(ALStepCounter _stepCounter)
        {

        }

        protected override void _discardAdditionTemplate()
        {
            
        }

        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="_type"></param>
        public void setColor(EChatShareType _type)
        {
            if (wnd == null || wnd.monoColorTypeList == null || wnd.monoColorTypeList.Count == 0 || wnd.needChangeColorList == null)
                return;

            for (int i = 0; i < wnd.monoColorTypeList.Count; i++)
            {
                if (wnd.monoColorTypeList[i].shareType == _type)
                {
                    for (int j = 0; j < wnd.needChangeColorList.Count; j++)
                    {
                        ALUGUICommon.setUIObjColor(wnd.needChangeColorList[j], wnd.monoColorTypeList[i].color);
                    }
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 一条分享信息的msgItem,包含文本和点击
    /// </summary>
    public abstract class _AGGUIWndChatMsgItemShareCommon<T_DATA> : _AGGUIWndChatMsgItemShareCommon<GGUIMonoChatMsgItemShareCommon, T_DATA>
        where T_DATA : _IMsgItemData
    {
        protected _AGGUIWndChatMsgItemShareCommon(T_DATA _detailInfo, Transform _parent, [NotNull] GUICacheMgrChatMsgItem _cacheMgr) : base(_detailInfo, _parent, _cacheMgr)
        {
        }
        
    }
}