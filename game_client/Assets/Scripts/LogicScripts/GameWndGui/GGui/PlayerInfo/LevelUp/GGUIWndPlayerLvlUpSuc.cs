using ALPackage;
using CommonEnum;
using NPEnum;
using Puerts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 玩家升级成功弹窗
    /// </summary>
    public class GGUIWndPlayerLvlUpSuc : _ANPGGUIBasicWnd<GGUIMonoPlayerLvlUpSuc>
    {
        private static GGUIWndPlayerLvlUpSuc _g_instance = new GGUIWndPlayerLvlUpSuc();

        public static GGUIWndPlayerLvlUpSuc instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndPlayerLvlUpSuc();

                return _g_instance;
            }
        }

        public GGUIWndPlayerLvlUpSuc() : base(EALUIWndLayer.ADDITION) { }

        //等级数据
        private PlayerLvlRefObj _m_lvlRefObj;
        //等级信息子窗口
        private GGUIWndPlayerLv _m_lvWnd;
        //等级属性列表
        private GGUIWndPlayerLvlUpSucContainer _m_wLevelPropertyItemWnd;
        //等级特殊条目列表
        private GGUIWndPlayerLvlUpSucContainer _m_wSpecialItemWnd;
        //玩家形象
        private NPGGUIWndCommonShowCase _m_playerShowcase;
        //玩家半身像
        private NPGGuiWndTexture _m_wPlayerTex;
        //每日奖励图标
        private NPGGuiWndTexture _m_wDailyRewardIcon;
        //操作序列号
        private long _m_lOpSerialize;

        private Action _m_dealDone;

        protected override string _monoAssetPath { get { return GGUIMonoPlayerLvlUpSuc.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoPlayerLvlUpSuc.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        public override bool needDiscardOnSwitch { get { return true; } }


        #region override
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.lvMono)
                _m_lvWnd = new GGUIWndPlayerLv(wnd.lvMono);

            if (null != wnd.itemMono)
                _m_wLevelPropertyItemWnd = new GGUIWndPlayerLvlUpSucContainer(wnd.itemMono);

            if (null != wnd.specialItemMono)
                _m_wSpecialItemWnd = new GGUIWndPlayerLvlUpSucContainer(wnd.specialItemMono);

            if (null != wnd.playerShowcase)
                _m_playerShowcase = new NPGGUIWndCommonShowCase(wnd.playerShowcase);

            if (null != wnd.imgPlayer)
                _m_wPlayerTex = new NPGGuiWndTexture(wnd.imgPlayer);

            if (null != wnd.imgDailyRewardIcon)
                _m_wDailyRewardIcon = new NPGGuiWndTexture(wnd.imgDailyRewardIcon);

            ALUGUICommon.combineBtnClick(wnd.closeBtn, _closeBtnDidClick);
        }
        protected override void _onShowWnd()
        {
            _m_lOpSerialize = ALSerializeOpMgr.next();
        }
        protected override void _onHideWnd()
        {
            _m_lOpSerialize = ALSerializeOpMgr.next();
            if (null != _m_playerShowcase)
                _m_playerShowcase.hideWnd();

            if (null != _m_wPlayerTex)
                _m_wPlayerTex.hideWnd();

            if(null != _m_wLevelPropertyItemWnd)
                _m_wLevelPropertyItemWnd.hideWnd();

            if(null != _m_wSpecialItemWnd)
                _m_wSpecialItemWnd.hideWnd();

            if(null != _m_wDailyRewardIcon)
                _m_wDailyRewardIcon.hideWnd();
        }

        protected override void _onReset()
        {
            if (null != _m_wLevelPropertyItemWnd)
                _m_wLevelPropertyItemWnd.resetWnd();

            if (null != _m_wSpecialItemWnd)
                _m_wSpecialItemWnd.resetWnd();

            if (null != _m_wPlayerTex)
                _m_wPlayerTex.discardTexture();

            if (null != _m_wDailyRewardIcon)
                _m_wDailyRewardIcon.discardTexture();
        }

        protected override void _onDiscard()
        {
            if (null != _m_lvWnd)
                _m_lvWnd.discard();
            _m_lvWnd = null;

            if (null != _m_wLevelPropertyItemWnd)
                _m_wLevelPropertyItemWnd.discard();
            _m_wLevelPropertyItemWnd = null;

            if (null != _m_wSpecialItemWnd)
                _m_wSpecialItemWnd.discard();
            _m_wSpecialItemWnd = null;

            if (null != _m_playerShowcase)
                _m_playerShowcase.discard();
            _m_playerShowcase = null;

            if (null != _m_wPlayerTex)
                _m_wPlayerTex.discard();
            _m_wPlayerTex = null;

            if (null != _m_wDailyRewardIcon)
                _m_wDailyRewardIcon.discard();
            _m_wDailyRewardIcon = null;

            _m_dealDone?.Invoke();
            _m_dealDone = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.closeBtn, _closeBtnDidClick);
        }

        #endregion

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_refObj"></param>
        /// <param name="_dealDone"></param>
        public void setData(PlayerLvlRefObj _refObj, Action _dealDone)
        {
            if (null == _refObj)
            {
                _dealDone?.Invoke();
                return;
            }

            _m_dealDone = _dealDone;
            _m_lvlRefObj = _refObj;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshPlayerInfo();
            _refreshPropertyList();
            _refreshDailyReward();
        }

        /// <summary>
        /// 刷新玩家信息
        /// </summary>
        private void _refreshPlayerInfo()
        {
            if (null == wnd)
                return;

            if (null != _m_playerShowcase)
            {
                _m_playerShowcase.showWnd(new ShowCaseCommonResUnitInfoObj(NPPlayer.instance.playerInfo.curSkinRef?.td_show));
                _m_playerShowcase.regInitDoneDelegate(() =>
                {
                    if (!string.IsNullOrEmpty(wnd.lvUpActStr))
                        _m_playerShowcase.playAnim(0, wnd.lvUpActStr);
                });
            }

            if (null != _m_wPlayerTex)
            {
                _m_wPlayerTex.showWnd();
                _m_wPlayerTex.setTexture(NPPlayer.instance.playerInfo.curSkinRef?.card_image);
            }

            //设置日期
            ALUGUICommon.setLabelTxt(wnd.txtDate,TimeUtil.Milliseconds2StringYMD(FpsAndPingMgr.instance.serverTimeTag));
        }

        //刷新属性列表
        private void _refreshPropertyList()
        {
            if (_m_lvlRefObj == null)
                return;

            //等级信息子窗口
            if (null != _m_lvWnd)
                _m_lvWnd.setLvRefObj(_m_lvlRefObj);

            //===================设置特殊条目列表===================
            List<PlayerLvlUpSucItemData> specialItemDataList = new List<PlayerLvlUpSucItemData>();
            //添加特殊条目列表
            if (_m_lvlRefObj.special_key_list != null)
            {
                for (int i = 0; i < _m_lvlRefObj.special_key_list.Count; i++)
                {
                    PlayerLvlUpSucItemData specialData = new PlayerLvlUpSucItemData();
                    specialData.isSpecial = true;
                    specialData.name = TextTranslate.instance.getLanguage(_m_lvlRefObj.special_key_list[i]);
                    if (_m_lvlRefObj.special_value_list != null && _m_lvlRefObj.special_value_list.Count > i)
                        specialData.specialValueStr = TextTranslate.instance.getLanguage(_m_lvlRefObj.special_value_list[i]);
                    specialItemDataList.Add(specialData);
                }
            }

            //设置特殊条目列表
            if (null != _m_wSpecialItemWnd)
            {
                if (specialItemDataList.Count > 0)
                {
                    _m_wSpecialItemWnd.showWnd();
                    _m_wSpecialItemWnd.showItemList(specialItemDataList);
                }
                else
                {
                    _m_wSpecialItemWnd.hideWnd();
                }
            }


            //===================设置属性列表===================
            List<PlayerLvlUpSucItemData> itemDataList = new List<PlayerLvlUpSucItemData>();
            //添加属性变化和未变化条目列表
            PlayerLvlRefObj lastLvlRefObj = GRefdataCoreMgr.instance.playerLvlCore.getRef(_m_lvlRefObj.lvl - 1);
            if (lastLvlRefObj != null && lastLvlRefObj.player_property != null)
            {
                //当前等级的属性列表
                List<NPPlayerPropertyInfoObj> curLevelPropertyInfoList = new List<NPPlayerPropertyInfoObj>();
                if (_m_lvlRefObj.player_property != null && _m_lvlRefObj.player_property.propertyObjList != null)
                {
                    //遍历需要展示的属性列表
                    for (int i = 0; i < _m_lvlRefObj.player_property.propertyObjList.Count; i++)
                    {
                        NPPlayerPropertyInfoObj propertyInfoObj = _m_lvlRefObj.player_property.propertyObjList[i];
                        if (propertyInfoObj == null)
                            continue;

                        NPPlayerPropertyRefObj playerPropertyRefObj = GRefdataCoreMgr.instance.playerPropertyCore.getRef((long)propertyInfoObj.type);
                        if (playerPropertyRefObj != null && playerPropertyRefObj.is_show)
                            curLevelPropertyInfoList.Add(propertyInfoObj);
                    }
                    //按照排序id排序
                    curLevelPropertyInfoList.Sort(_sortPropertyList);
                }

                //未变化的属性列表
                List<PlayerLvlUpSucItemData> unChangeDataList = new List<PlayerLvlUpSucItemData>();
                
                //构造item数据
                for (int i = 0; i < curLevelPropertyInfoList.Count; i++)
                {
                    PlayerLvlUpSucItemData propertyData = new PlayerLvlUpSucItemData();
                    propertyData.isSpecial = false;
                    propertyData.name = GCommon.getPlayerPropertyName(curLevelPropertyInfoList[i].type);
                    // propertyData.icon = GCommon.getPlayerPropertyIcon(curLevelPropertyInfoList[i].type);
                    propertyData.curValue = curLevelPropertyInfoList[i].value;
                    propertyData.lastValue = lastLvlRefObj.player_property.getPropertyValue(curLevelPropertyInfoList[i].type);

                    if (propertyData.curValue != propertyData.lastValue)
                        itemDataList.Add(propertyData);//如果数据有变更，添加到列表里
                    else if (_m_lvlRefObj.need_show_unchange)
                        unChangeDataList.Add(propertyData);//数据没有变更并且需要展示，先保存到未变化列表里
                }
                
                // 检查子嗣上课获得伙伴经验
                PlayerLvlUpSucItemData heroExpPropertyData = new PlayerLvlUpSucItemData();
                heroExpPropertyData.isSpecial = false;
                heroExpPropertyData.name = TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_childEducateHeroExp_none);
                // heroExpPropertyData.icon = GCommon.getItemTexIcon(ENPItemType.CURRENCY, (long) ECurrency.HERO_EXP);
                heroExpPropertyData.curValue = _m_lvlRefObj.child_educate_get_hero_exp;
                heroExpPropertyData.lastValue = lastLvlRefObj.child_educate_get_hero_exp;
                if (heroExpPropertyData.curValue != heroExpPropertyData.lastValue)
                    itemDataList.Add(heroExpPropertyData);//如果数据有变更，添加到列表里
                else if (_m_lvlRefObj.need_show_unchange)
                    unChangeDataList.Add(heroExpPropertyData);//数据没有变更并且需要展示，先保存到未变化列表里

                if (unChangeDataList.Count > 0)
                    itemDataList.AddRange(unChangeDataList);
            }

            //设置列表
            if (null != _m_wLevelPropertyItemWnd)
            {
                _m_wLevelPropertyItemWnd.showWnd();
                _m_wLevelPropertyItemWnd.showItemList(itemDataList);
            }
        }

        //刷新每日奖励
        private void _refreshDailyReward()
        {
            if (wnd == null || _m_lvlRefObj == null)
                return;

            PlayerLvlRefObj lastLvlRefObj = GRefdataCoreMgr.instance.playerLvlCore.getRef(_m_lvlRefObj.lvl - 1);
            NPCommonCostItem currentDailyReward = _m_lvlRefObj.daily_reward_item.GetFirst();
            NPCommonCostItem lastDailyReward = lastLvlRefObj?.daily_reward_item.GetFirst();

            if (currentDailyReward == null)
                return;

            long curValue = currentDailyReward.getCount();
            long lastValue = lastDailyReward != null ? lastDailyReward.getCount() : 0;
            long addValue = curValue - lastValue;

            //设置图标
            _m_wDailyRewardIcon?.showWnd();
            _m_wDailyRewardIcon?.setTexture(GCommon.getItemTexIcon(currentDailyReward.getItemType(), currentDailyReward.subId));
            //设置增加数量
            ALUGUICommon.setLabelTxt(wnd.txtDailyRewardAddCount, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, addValue));
            //设置数量变化
            _showValueLerp(lastValue, curValue);
            //设置上个等级数量
            ALUGUICommon.setLabelTxt(wnd.txtLastDailyRewardCount, lastValue);

            //首次升级或每日奖励没有变化时显隐
            bool isFirstUpgrade = lastLvlRefObj == null || lastLvlRefObj.lvl == 1 || addValue == 0;
            ALUGUICommon.setGameObjEnable(wnd.goFirstUpgradeHideList, !isFirstUpgrade);
            ALUGUICommon.setGameObjEnable(wnd.goFirstUpgradeShowList, isFirstUpgrade);
        }

        //一段时间内，值从开始到结束的变化过程
        private void _showValueLerp(long _start, long _end)
        {
            if (null == wnd || _start == _end)
                return;

            //播放动画
            wnd?.aniShowTextChg?.forcePlay();

            if (wnd.countChgDurationSec <= 0)
            {
                ALUGUICommon.setLabelTxt(wnd.txtDailyRewardCount, _end);
                return;
            }

            NPMonoTaskLerpStartEndValueByTime.startLerpTask(() => { return wnd == null || !isShow; }
                , _m_lOpSerialize
                , _start
                , _end
                , Math.Abs(wnd.countChgDurationSec) < 0.01 ? 1.0f : wnd.countChgDurationSec
                , value =>
                {
                    ALUGUICommon.setLabelTxt(wnd.txtDailyRewardCount, value);
                }
                , () => { return _m_lOpSerialize; }
                , null);
        }

        #region 点击事件

        //点击关闭按钮
        private void _closeBtnDidClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_PLAYER_LVL_UP_SUC);
        }

        #endregion

        //排序属性列表，按排序id从小到大
        private int _sortPropertyList(NPPlayerPropertyInfoObj _a, NPPlayerPropertyInfoObj _b)
        {
            if (_a == null || _b == null)
                return 0;

            NPPlayerPropertyRefObj aRefObj = GRefdataCoreMgr.instance.playerPropertyCore.getRef((long) _a.type);
            NPPlayerPropertyRefObj bRefObj = GRefdataCoreMgr.instance.playerPropertyCore.getRef((long) _b.type);
            if (aRefObj == null || bRefObj == null)
                return 0;

            return aRefObj.sort_id.CompareTo(bRefObj.sort_id);
        }
    }
}
