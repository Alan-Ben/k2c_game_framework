using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerEnum;
using Common.DinnerObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 宴会item
    /// </summary>
    public class GGUIWndDinnerItem : _ANPGGUIBasicGridItemWnd<GGUIMonoDinnerItem>
    {
        private DinnerIndex _m_dinnerInfo;
        private NPGGuiWndTexture _m_dinnerBanner;
        private NPGGUIWndPlayerIcon _m_playerIcon;
        private GGUIWndConsortIconItem _m_consortCardItem;
        private GGUISubWndChildInfo _m_childCardItem;
        private long _m_showSerializeOp;

        public GGUIWndDinnerItem(GGUIMonoDinnerItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            
        }

        protected override void _onHideWnd()
        {
            _m_showSerializeOp = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            _m_showSerializeOp = ALSerializeOpMgr.next();
            _m_dinnerBanner?.discardTexture();
            _m_playerIcon?.hideWnd();
            _m_playerIcon?.resetWnd();
            _m_consortCardItem?.resetWnd();
            _m_childCardItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_dinnerBanner?.discard();
            _m_dinnerBanner = null;
            _m_playerIcon?.discard();
            _m_playerIcon = null;
            _m_consortCardItem?.discard();
            _m_consortCardItem = null;
            _m_childCardItem?.discard();
            _m_childCardItem = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _clickEnter);
            ALUGUICommon.combineBtnClick(wnd.btnEntered, _clickEntered);
            if (null != wnd.texBanner)
                _m_dinnerBanner = new NPGGuiWndTexture(wnd.texBanner);

            if (null != wnd.playerIcon)
                _m_playerIcon = new NPGGUIWndPlayerIcon(wnd.playerIcon);
            
            if (wnd.consortIconItem != null)
                _m_consortCardItem = new GGUIWndConsortIconItem(wnd.consortIconItem);
            if (wnd.childCardItem != null)
                _m_childCardItem = new GGUISubWndChildInfo(wnd.childCardItem);
        }

        protected override void _resetGridItem()
        {
            
            
        }

        /// <summary>
        /// 点击按钮，进入宴会
        /// </summary>
        /// <param name="obj"></param>
        private void _clickEnter(GameObject obj)
        {
            //点击进入宴会
            GCommon.enterDinner(new GDinnerInfo(_m_dinnerInfo));
        }
        /// <summary>
        /// 点击已参加按钮，进入宴会
        /// </summary>
        /// <param name="obj"></param>
        private void _clickEntered(GameObject obj)
        {
            //点击进入宴会
            GCommon.enterDinner(new GDinnerInfo(_m_dinnerInfo));
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        /// <param name="_dinnerInfo"></param>
        public void setInfo(DinnerIndex _dinnerInfo)
        {
            _m_dinnerInfo = _dinnerInfo;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            if (null == _m_dinnerInfo)
                return;
            
            GDinnerTypeRefObj dinnerTypeRefObj = GRefdataCoreMgr.instance.dinnerTypeRefCore.getRef(_m_dinnerInfo.dinnerId);
            if (null != dinnerTypeRefObj)
            {
                _m_dinnerBanner?.showWnd();
                _m_dinnerBanner?.setTexture(dinnerTypeRefObj.banner);

                ALUGUICommon.setLabelTxt(wnd.txtDinnerName, TextTranslate.instance.getLanguage(dinnerTypeRefObj.name));
                ALUGUICommon.setLabelTxt(wnd.txtCount,TextTranslate.instance.getLanguage(TransKeyConst.dinner_common_joiner_count, 
                    _m_dinnerInfo.joinerCount,
                    dinnerTypeRefObj.default_seat_num
                ));
            }
            ALUGUICommon.setLabelTxt(wnd.txtScore, _m_dinnerInfo.score);

            _m_playerIcon?.hideWnd();
            _m_showSerializeOp = ALSerializeOpMgr.next();
            long serializeOp = _m_showSerializeOp;
            GCommon.reqPlayerInfo(_m_dinnerInfo.ownerCid, (_playerInfo) =>
            {
                if (serializeOp != _m_showSerializeOp)
                    return;
                _m_playerIcon?.showWnd();
                _m_playerIcon?.setPlayerInfo(_playerInfo);
            });
            if (_m_dinnerInfo.permitType == EDinnerPermitType.FAMILY)
            {
                _m_consortCardItem?.showWnd();
                _m_consortCardItem?.setInfo(new ConsortInfo(_m_dinnerInfo.permitTypeId), 0);
                _m_childCardItem?.hideWnd();
            }
            else if (_m_dinnerInfo.permitType == EDinnerPermitType.GIFTDE_CHILD_CELE)
            {
                _m_consortCardItem?.hideWnd();
                _m_childCardItem?.hideWnd();
                NPPlayer.instance.childComp.getAdultChildInfoById(_m_dinnerInfo.permitTypeId, (_childInfo) =>
                {
                    if (serializeOp != _m_showSerializeOp)
                        return;
                    if (_childInfo == null)
                    {
                        _m_childCardItem?.hideWnd();
                        return;
                    }

                    _m_childCardItem?.showWnd();
                    _m_childCardItem?.refreshWnd(_childInfo);
                });
            }
            else
            {
                _m_consortCardItem?.hideWnd();
                _m_childCardItem?.hideWnd();
            }

            EDinnerJoinStat stat = EDinnerJoinStat.NO_JOINED;
            if(_m_dinnerInfo.isJoined)
                stat= EDinnerJoinStat.JOINED;
            
            NPCommonEnumStatInfo<EDinnerJoinStat>.setStat(wnd.statInfos,stat);
            if (dinnerTypeRefObj != null)
                DinnerTypeShow.SetDinnerType(wnd.dinnerTypeShowList, dinnerTypeRefObj.dinner_show_type);
            _refreshLifeTime(serializeOp);
        }

        /// <summary>
        /// 刷新凭证使用倒计时
        /// </summary>
        private void _refreshLifeTime(long _serializeOp)
        {
            if (null == wnd || !isShow)
                return;
            if (_serializeOp != _m_showSerializeOp)
                return;
            long elapsedTs = _m_dinnerInfo.getRemainTimeMs();
            if (elapsedTs > 0)
            {
                ALUGUICommon.setLabelTxt(wnd.txtCD,  TimeUtil.millisecondsToTime_hms(elapsedTs));
            }
            else
            {
                GGUIWndDinnerListMain.instance.refreshWnd();
            }

            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                _refreshLifeTime(_serializeOp);
            }, 1f);
        }
    }
}
