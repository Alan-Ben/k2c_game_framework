using System;
using ALPackage;
using Common.NpPlayerInfoObj;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 组合称号文本
    /// </summary>
    public class GGUIWndPlayerTitleComboTextGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoPlayerTitleComboTextGridItem>
    {
        //类型
        private ENPItemType _m_eItemType;
        //唯一id
        private long _m_lId;
        //点击选中
        private Action<GGUIWndPlayerTitleComboTextGridItem> _m_onClickSelect;

        /// <summary>
        /// 类型
        /// </summary>
        public ENPItemType itemType { get { return _m_eItemType; } }
        /// <summary>
        /// 唯一id
        /// </summary>
        public long id { get { return _m_lId; } }
        /// <summary>
        /// 点击选中
        /// </summary>
        public Action<GGUIWndPlayerTitleComboTextGridItem> onClickSelect
        {
            get { return _m_onClickSelect; }
            set { _m_onClickSelect = value; }
        }

        public GGUIWndPlayerTitleComboTextGridItem(GGUIMonoPlayerTitleComboTextGridItem _wnd) : base(_wnd)
        {
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _resetGridItem()
        {
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickItem);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(ENPItemType _itemType, long _id)
        {
            if (wnd == null)
                return;

            _m_eItemType = _itemType;
            _m_lId = _id;

            //设置名称
            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(_m_eItemType, _m_lId));

            //设置解锁状态
            bool isUnlock = false;
            bool isCurrent = false;
            PlayerInfo_ComboTitle curComboTitle = NPPlayer.instance.titleComp.getCurWearComboTitleInfo();
            switch (_m_eItemType)
            {
                case ENPItemType.TITLE_PRE:
                    PlayerTitleComboPreInfo preInfo = NPPlayer.instance.titleComp.getTitleComboPreInfo(_id);
                    isUnlock = preInfo != null && preInfo.isUnlock;
                    isCurrent = curComboTitle != null && curComboTitle.getPreId() == _id;
                    if (preInfo != null && preInfo.isNew)
                        ALUGUICommon.setGameObjEnable(wnd.goNewRedTip, true);
                    else
                        ALUGUICommon.setGameObjEnable(wnd.goNewRedTip, false);

                    break;
                case ENPItemType.TITLE_SFX:
                    PlayerTitleComboSfxInfo sfxInfo = NPPlayer.instance.titleComp.getTitleComboSfxInfo(_id);
                    isUnlock = sfxInfo != null && sfxInfo.isUnlock;
                    isCurrent = curComboTitle != null && curComboTitle.getSfxId() == _id;
                    if (sfxInfo != null && sfxInfo.isNew)
                        ALUGUICommon.setGameObjEnable(wnd.goNewRedTip, true);
                    else
                        ALUGUICommon.setGameObjEnable(wnd.goNewRedTip, false);

                    break;
            }
            ALUGUICommon.setGameObjEnable(wnd.goLockShowList, !isUnlock);
            ALUGUICommon.setGameObjEnable(wnd.goLockHideList, isUnlock);
            ALUGUICommon.setGameObjEnable(wnd.goCurrentShowList, isCurrent);
            ALUGUICommon.setGameObjEnable(wnd.goCurrentHideList, !isCurrent);
        }

        /// <summary>
        /// 设置选中
        /// </summary>
        /// <param name="_isSelect"></param>
        public void setSelect(bool _isSelect)
        {
            if (wnd == null)
                return;

            if (_isSelect)
                _setIsView();

            ALUGUICommon.setGameObjEnable(wnd.goSelectShowList, _isSelect);
            ALUGUICommon.setGameObjEnable(wnd.goSelectHideList, !_isSelect);
        }
        
        //设置已查看
        private void _setIsView()
        {
            switch (_m_eItemType)
            {
                case ENPItemType.TITLE_PRE:
                    PlayerTitleComboPreInfo preInfo = NPPlayer.instance.titleComp.getTitleComboPreInfo(_m_lId);
                    if (preInfo != null && preInfo.isNew)
                    {
                        preInfo.setIsViewed(true);
                        NPPlayer.instance.titleComp.reqviewComboTitlePre(preInfo.id);
                    }
                    break;
                case ENPItemType.TITLE_SFX:
                    PlayerTitleComboSfxInfo sfxInfo = NPPlayer.instance.titleComp.getTitleComboSfxInfo(_m_lId);
                    if (sfxInfo != null && sfxInfo.isNew)
                    {
                        sfxInfo.setIsViewed(true);
                        NPPlayer.instance.titleComp.reqViewComboTitleSfx(sfxInfo.id);
                    }
                    break;
            }
        }

        //点击按钮
        private void _onClickItem(GameObject _go)
        {
            _setIsView();
            _m_onClickSelect?.Invoke(this);
        }
    }
}
