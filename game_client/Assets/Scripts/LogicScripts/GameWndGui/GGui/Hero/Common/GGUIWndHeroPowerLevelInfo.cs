using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用伙伴属性信息附加窗口
    /// </summary>
    public class GGUIWndHeroPowerLevelInfo : _ATALBasicUISubWnd<GGUIMonoHeroPowerLevelInfo>
    {
        //伙伴信息
        private HeroInfo _m_heroInfo;
        //觉醒星级
        private GGUIWndHeroCommonStar _m_wStar;
        //上个实力
        private long _m_lLastPower;
        //操作序列号
        private long _m_lOpSerialize;

        public GGUIWndHeroPowerLevelInfo(GGUIMonoHeroPowerLevelInfo _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_POWER_CHG, _onHeroPowerChg);
            _m_lOpSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_POWER_CHG, _onHeroPowerChg);
            _m_wStar?.hideWnd();
            _m_lOpSerialize = ALSerializeOpMgr.next();
            _m_heroInfo = null;
            _m_lLastPower = 0;
        }

        protected override void _onReset()
        {
            _m_wStar?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wStar?.discard();
            _m_wStar = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnPowerInfo, _onClickPowerInfo);
            ALUGUICommon.uncombineBtnClick(wnd.btnSuitDetail, _onClickSuitDetail);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoStar != null)
                _m_wStar = new GGUIWndHeroCommonStar(wnd.monoStar);

            ALUGUICommon.combineBtnClick(wnd.btnPowerInfo, _onClickPowerInfo);
            ALUGUICommon.combineBtnClick(wnd.btnSuitDetail, _onClickSuitDetail);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroInfo"></param>
        public void setInfo(HeroInfo _heroInfo)
        {
            if (_heroInfo == null)
                return;

            //如果伙伴发生了变化，重置记录的实力
            if (_m_heroInfo == null || _m_heroInfo.id != _heroInfo.id)
                _m_lLastPower = 0;

            _m_heroInfo = _heroInfo;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_heroInfo == null)
                return;

            //总实力
            if (_m_lLastPower == 0)
            {
                //如果未记录上个实力，则直接设置显示
                ALUGUICommon.setLabelTxt(wnd.txtTotalPower, wnd.powerNoShowLargeNum ? _m_heroInfo.power.ToString() : _m_heroInfo.power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            }
            else
            {
                //如果有记录实力，实力值从开始到结束变化
                _showTotalPowerValueLerp(_m_lLastPower, _m_heroInfo.power);
            }
            _m_lLastPower = _m_heroInfo.power;

            //等级
            ALUGUICommon.setLabelTxt(wnd.txtLevel, _m_heroInfo.level);
            //觉醒星级
            if (_m_wStar != null)
            {
                if (_m_heroInfo.haveStarSkill)
                {
                    _m_wStar.showWnd();
                    _m_wStar.setInfo(_m_heroInfo.star);
                }
                else
                {
                    _m_wStar.hideWnd();
                }
            }
            //资质
            ALUGUICommon.setLabelTxt(wnd.txtTalent, _m_heroInfo.getTotalTalent());
            //村庄收益 TODO
            ALUGUICommon.setLabelTxt(wnd.txtVillageIncome, 0);
            //套系数量
            long suitId = _m_heroInfo.belongSuitId;
            HeroSuitInfo suitInfo = NPPlayer.instance.heroComponent.getHeroSuitInfo(suitId);
            if (suitInfo != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.goNoSuitHideList, true);
                long curCount = suitInfo.suitOwnHeroCount;
                long totalCount = suitInfo.suitTotalHeroCount;
                ALUGUICommon.setLabelTxt(wnd.txtSuitCount, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, curCount, totalCount));
            }
            else
                ALUGUICommon.setGameObjEnable(wnd.goNoSuitHideList, false);
        }

        //一段时间内，实力值从开始到结束的变化过程
        private void _showTotalPowerValueLerp(long _start, long _end)
        {
            if (null == wnd || _start == _end)
                return;

            _m_lOpSerialize = ALSerializeOpMgr.next();
            NPMonoTaskLerpStartEndValueByTime.startLerpTask(() => { return wnd == null || !isShow; }
                , _m_lOpSerialize
                , _start
                , _end
                , Math.Abs(wnd.upgradeTotalPowerChgTime) < 0.01 ? 1.0f : wnd.upgradeTotalPowerChgTime
                , value =>
                {
                    ALUGUICommon.setLabelTxt(wnd.txtTotalPower, wnd.powerNoShowLargeNum ? value.ToString() : value.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
                }
                , () => { return _m_lOpSerialize; }
                , null);
        }

        #region 点击事件

        //点击实力详情按钮
        private void _onClickPowerInfo(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndHeroSuitAndPower.instance, () =>
            {
                GGUIWndHeroSuitAndPower.instance.showWnd();
                GGUIWndHeroSuitAndPower.instance.setInfo(_m_heroInfo,EHeroSuitAndPowerTabType.POWER);
            }, UINodeTagConst.C_HERO_SUIT_AND_POWER);
        }

        //点击套系详情按钮
        private void _onClickSuitDetail(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndHeroSuitAndPower.instance, () =>
            {
                GGUIWndHeroSuitAndPower.instance.showWnd();
                GGUIWndHeroSuitAndPower.instance.setInfo(_m_heroInfo, EHeroSuitAndPowerTabType.SUIT);
            }, UINodeTagConst.C_HERO_SUIT_AND_POWER);
        }

        #endregion

        #region 消息事件

        //伙伴实力变更
        private void _onHeroPowerChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            HeroInfo heroInfo =_objects[0] as HeroInfo;

            if(heroInfo != null && _m_heroInfo != null && heroInfo.id == _m_heroInfo.id)
                _refreshWnd();
        }

        #endregion
    }
}
