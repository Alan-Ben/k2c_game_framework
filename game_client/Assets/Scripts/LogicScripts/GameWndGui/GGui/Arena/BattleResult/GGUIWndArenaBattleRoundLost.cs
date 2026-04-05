using ALPackage;
using System;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场回合失败弹窗
    /// </summary>
    public class GGUIWndArenaBattleRoundLost : _ANPGGUIBasicWnd<GGUIMonoArenaBattleRoundLost>
    {
        private static GGUIWndArenaBattleRoundLost _g_instance;
        public static GGUIWndArenaBattleRoundLost instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndArenaBattleRoundLost();
                return _g_instance;
            }
        }

        //关闭回调
        private Action _m_aOnClose;

        public GGUIWndArenaBattleRoundLost() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoArenaBattleRoundLost.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoArenaBattleRoundLost.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_aOnClose?.Invoke();
            _m_aOnClose = null;
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroId"></param>
        /// <param name="_onClose"></param>
        public void setInfo(long _heroId, Action _onClose)
        {
            if (wnd == null)
                return;

            _m_aOnClose = _onClose;
            //很遗憾，您的伙伴【{0}】血量为0，无法继续谈判！
            ALUGUICommon.setLabelTxt(wnd.txtDesc,
                TextTranslate.instance.getLanguage(TransKeyConst.arena_battleRoundLostDesc_str,
                    GCommon.getItemName(ENPItemType.HERO, _heroId)));
        }

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_BATTLE_ROUND_LOST);
        }
    }
}