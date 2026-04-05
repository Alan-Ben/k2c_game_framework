using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using CommonEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 伙伴信息资质页签
    /// </summary>
    public class GGUIWndHeroInfoTalentPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoHeroInfoTalentPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        //伙伴信息
        private HeroInfo _m_heroInfo;
        //当前选中的资质技能
        private HeroTalentSkillRefObj _m_curSelectTalentSkillRef;
        //资质技能列表
        private GGUIWndHeroTalentSkillContainer _m_wSkillContainer;
        //连升十级勾选
        private NPGGUIWndCommonToggleEx _m_wTenLevelUpToggle;
        //升级消耗
        private NPGGUIWndCommonItem _m_wLevelUpCostItem;
        //点击关闭按钮
        private Action _m_aOnClickClose;
        //升级完成回调
        private Action _m_aOnReqUpgradeDone;
        //特效列表
        private List<CommonUISfxObj> _m_lSfxObjList;
        //显示序列号
        private long _m_lShowSerialize;

        public GGUIWndHeroInfoTalentPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent)
            : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName = _assetPathInfo.obj_name;
        }

        /**************
         * 窗口相关加载配置
         **/
        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_TALENT_SKILL_CHG, _onTalentSkillChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagChg);//背包变更
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagChg);//背包变更
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagChg);//背包变更
            _m_wTenLevelUpToggle?.showWnd();
            _m_wTenLevelUpToggle?.setSelected(AccountSettingMgr.instance.accountSetting.isHeroTalentTenUpgrade, true, false);
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }
        
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_TALENT_SKILL_CHG, _onTalentSkillChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagChg);//背包变更
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagChg);//背包变更
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagChg);//背包变更
            _m_wLevelUpCostItem?.hideWnd();
            _m_wSkillContainer?.hideWnd();

            _m_curSelectTalentSkillRef = null;
            _m_lShowSerialize = ALSerializeOpMgr.next();

            if (_m_lSfxObjList != null)
            {
                for (int i = 0; i < _m_lSfxObjList.Count; i++)
                {
                    _m_lSfxObjList[i]?.forceDiscard();
                }
                _m_lSfxObjList.Clear();
                _m_lSfxObjList = null;
            }
        }
        
        protected override void _onReset()
        {
            _m_wLevelUpCostItem?.resetWnd();
            _m_wSkillContainer?.resetWnd();
        }
        
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wTenLevelUpToggle?.discard();
            _m_wTenLevelUpToggle = null;

            _m_wLevelUpCostItem?.discard();
            _m_wLevelUpCostItem = null;

            _m_wSkillContainer?.discard();
            _m_wSkillContainer = null;

            if (_m_lSfxObjList != null)
            {
                for (int i = 0; i < _m_lSfxObjList.Count; i++)
                {
                    _m_lSfxObjList[i]?.forceDiscard();
                }
                _m_lSfxObjList.Clear();
                _m_lSfxObjList = null;
            }

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnTalentInfo, _onClickTalentInfo);
            ALUGUICommon.uncombineBtnClick(wnd.btnUpgrade, _onClickUpgrade);
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.toggleTenLevelUp != null)
            {
                _m_wTenLevelUpToggle = new NPGGUIWndCommonToggleEx(wnd.toggleTenLevelUp);
                _m_wTenLevelUpToggle.clickDelegate += _onClickToggle;
            }

            if (wnd.monoTalentSkillContainer != null)
            {
                _m_wSkillContainer = new GGUIWndHeroTalentSkillContainer(wnd.monoTalentSkillContainer);
                _m_wSkillContainer.onSelectItem += _onClickTalentSkillItem;
            }

            if (wnd.monoUpgradeCostItem != null)
                _m_wLevelUpCostItem = new NPGGUIWndCommonItem(wnd.monoUpgradeCostItem);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnTalentInfo, _onClickTalentInfo);
            ALUGUICommon.combineBtnClick(wnd.btnUpgrade, _onClickUpgrade);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(HeroInfo _heroInfo, Action _onClickClose, Action _onReqUpgradeDone)
        {
            if(null == _heroInfo)
                return;

            _m_heroInfo = _heroInfo;
            _m_aOnClickClose = _onClickClose;
            _m_aOnReqUpgradeDone = _onReqUpgradeDone;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshPowerInfo();
            _refreshTalentSKillList();
            _refreshButtonState();
        }

        //刷新实力属性信息
        private void _refreshPowerInfo()
        {
            if (wnd == null || _m_heroInfo == null)
                return;

            //总资质
            ALUGUICommon.setLabelTxt(wnd.txtTotalTalent, _m_heroInfo.getTotalTalent());
        }

        //刷新资质技能列表
        private void _refreshTalentSKillList()
        {
            if (_m_heroInfo == null || _m_heroInfo.heroRefObj == null)
                return;

            List<long> talentSkillIdList = new List<long>();
            //添加默认资质技能列表
            if(_m_heroInfo.heroRefObj.default_talent_skill_id_list != null)
                talentSkillIdList.AddRange(_m_heroInfo.heroRefObj.default_talent_skill_id_list);
            //添加觉醒额外解锁的资质技能
            if (_m_heroInfo.heroRefObj.extra_talent_skill_id_list != null)
            {
                for (int i = 0; i < _m_heroInfo.heroRefObj.extra_talent_skill_id_list.Count; i++)
                {
                    if (_m_heroInfo.heroRefObj.extra_talent_skill_id_list[i] != null)
                    {
                        talentSkillIdList.Add(_m_heroInfo.heroRefObj.extra_talent_skill_id_list[i].second());
                    }
                }
            }

            if (talentSkillIdList.Count <= 0)
                return;

            //排序
            talentSkillIdList.Sort(_sortSkillList);

            if (_m_wSkillContainer != null)
            {
                _m_wSkillContainer.showWnd();
                _m_wSkillContainer.showItemList(_m_heroInfo.id, talentSkillIdList);
                _m_wSkillContainer.setSelectItem(talentSkillIdList[0]);//默认选中第一个
            }
        }

        //刷新资质技能详情
        private void _refreshTalentSkillDetail()
        {
            if (_m_heroInfo == null || _m_curSelectTalentSkillRef == null || wnd == null)
                return;

            HeroTalentSkillInfo talentSkillInfo = _m_heroInfo.heroTalentSkillInfoMgr.getTalentSkillInfo(_m_curSelectTalentSkillRef.id);
            //是否满级
            bool isMaxLevel = talentSkillInfo != null && talentSkillInfo.isMaxLevel;
            //是否可升级（未解锁或条件不通过则不可升级）
            bool canUpgrade = talentSkillInfo != null && 
                              talentSkillInfo.baseTalentSkillLevelRefObj != null && 
                              talentSkillInfo.baseTalentSkillLevelRefObj.cost != null &&
                              talentSkillInfo.baseTalentSkillLevelRefObj.cost.getItemType() != ENPItemType.NONE &&
                              ((talentSkillInfo.baseTalentSkillLevelRefObj.upgrade_condition == null ||
                                talentSkillInfo.baseTalentSkillLevelRefObj.upgrade_condition.isEmpty ||
                                talentSkillInfo.baseTalentSkillLevelRefObj.upgrade_condition.IsEnable(null)) &&
                               (talentSkillInfo.baseTalentSkillLevelRefObj.upgrade_hero_condition == null ||
                                talentSkillInfo.baseTalentSkillLevelRefObj.upgrade_hero_condition.isEmpty ||
                                talentSkillInfo.baseTalentSkillLevelRefObj.upgrade_hero_condition.IsEnable(_m_heroInfo.heroRefObj, null)));

            if (talentSkillInfo == null)
            {
                //未解锁
                HeroTalentSkillLevelRefObj talentSkillLevelRef = _m_curSelectTalentSkillRef.getBaseTalentSkillLevelRefByLevel(1);
                long talentValue = 0;
                if(talentSkillLevelRef != null && talentSkillLevelRef.self_attr_prop_modifier != null)
                    talentValue = talentSkillLevelRef.self_attr_prop_modifier.getPropValue(EBasicAttrType.TALENT);
                ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_curSelectTalentSkillRef.name));
                ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(TransKeyConst.hero_talentAddValue_num, talentValue));
            }
            else
            {
                //已解锁
                ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(TransKeyConst.hero_talentSkillNameLevel_name_curLevel_maxLevel, 
                    TextTranslate.instance.getLanguage(_m_curSelectTalentSkillRef.name),talentSkillInfo.level, _m_curSelectTalentSkillRef.level_limit));

                //当前资质
                HeroTalentSkillLevelRefObj curTalentSkillLevelRef = talentSkillInfo.baseTalentSkillLevelRefObj;
                PlayerAttrPropertyContainer curModifier = new PlayerAttrPropertyContainer();
                if (curTalentSkillLevelRef != null)
                {
                    curModifier.addModifier(curTalentSkillLevelRef.self_attr_prop_modifier);
                    curModifier.addModifier(curTalentSkillLevelRef.self_attr_prop_modifier_per_level, talentSkillInfo.levelBonusStack);
                }
                long curTalent = curModifier.getValue(EBasicAttrType.TALENT);
                //下一级资质
                long nextTalent = 0;
                HeroTalentSkillLevelRefObj nextTalentSkillLevelRef = talentSkillInfo.talentSkillRefObj?.getBaseTalentSkillLevelRefByLevel(talentSkillInfo.level + 1);
                if (curTalentSkillLevelRef != null && nextTalentSkillLevelRef != null)
                {
                    if (curTalentSkillLevelRef.id != nextTalentSkillLevelRef.id)
                    {
                        //如果不同的基础资质技能等级数据，重新计算资质
                        curModifier.clear();
                        curModifier.addModifier(nextTalentSkillLevelRef.self_attr_prop_modifier);
                        nextTalent = curModifier.getValue(EBasicAttrType.TALENT);
                    }
                    else
                    {
                        //如果是当前基础资质技能等级数据，直接加每级加成
                        curModifier.addModifier(curTalentSkillLevelRef.self_attr_prop_modifier_per_level);
                        nextTalent = curModifier.getValue(EBasicAttrType.TALENT);
                    }
                }

                //是否满级，满级不展示下一级资质
                if (isMaxLevel)
                    ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(TransKeyConst.hero_talentAddValue_num, curTalent));
                else
                    ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(TransKeyConst.hero_talentValueAndNextValue_num_num, curTalent, nextTalent));
            }

            
            if (isMaxLevel || !canUpgrade)
            {
                //不可升级情况
                //已满级
                if (isMaxLevel)
                    ALUGUICommon.setLabelTxt(wnd.txtLockOrCanNotUpgradeDesc, TextTranslate.instance.getLanguage(TransKeyConst.hero_talentSkillIsMaxLevel_none));
                else
                {
                    //未解锁
                    if (talentSkillInfo == null) 
                        ALUGUICommon.setLabelTxt(wnd.txtLockOrCanNotUpgradeDesc, TextTranslate.instance.getLanguage(_m_curSelectTalentSkillRef.unlock_desc, _m_curSelectTalentSkillRef.unlock_desc_args));
                    else//升级条件不通过
                        ALUGUICommon.setLabelTxt(wnd.txtLockOrCanNotUpgradeDesc, TextTranslate.instance.getLanguage(talentSkillInfo.baseTalentSkillLevelRefObj?.upgrade_condition_desc));

                }

                ALUGUICommon.setGameObjEnable(wnd.goCanNotUpgradeHideList, false);
                ALUGUICommon.setGameObjEnable(wnd.goCanNotUpgradeShowList, true);
            }
            else
            {
                //可升级情况
                ALUGUICommon.setGameObjEnable(wnd.goCanNotUpgradeHideList, true);
                ALUGUICommon.setGameObjEnable(wnd.goCanNotUpgradeShowList, false);
            }
        }

        //刷新按钮状态
        private void _refreshButtonState()
        {
            if (wnd == null || _m_heroInfo == null || _m_curSelectTalentSkillRef == null)
                return;

            //设置消耗的道具
            bool isTen = _m_wTenLevelUpToggle != null && _m_wTenLevelUpToggle.isOn;
            NPCommonCostItem levelUpCostItem = _m_heroInfo.getTalentSkillLevelUpCostItem(_m_curSelectTalentSkillRef.id, isTen);
            //是否满级
            HeroTalentSkillInfo talentSkillInfo = _m_heroInfo.heroTalentSkillInfoMgr.getTalentSkillInfo(_m_curSelectTalentSkillRef.id);
            bool isMaxLevel = talentSkillInfo != null && talentSkillInfo.isMaxLevel;
            if (_m_wLevelUpCostItem != null)
            {
                if(!isMaxLevel)
                {
                    _m_wLevelUpCostItem.showWnd();
                    _m_wLevelUpCostItem.setItem(levelUpCostItem);
                }
                else
                    _m_wLevelUpCostItem.hideWnd();
            }

            if(levelUpCostItem != null && levelUpCostItem.IsValid)
                ALUGUICommon.setLabelTxt(wnd.txtUseItemName, TextTranslate.instance.getLanguage(TransKeyConst.hero_talentUpgradeCostItemName_name, levelUpCostItem.getItemName()));

            bool canTenUpgrade = GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.hero_talent_upgrade_ten_times_simple_unlock_id);
            ALUGUICommon.setGameObjEnable(wnd.goTenLevelUpHideList, !canTenUpgrade);
            ALUGUICommon.setGameObjEnable(wnd.goTenLevelUpShowList, canTenUpgrade);

            //刷新红点
            _refreshUpgradeRedTip();
        }

        //刷新升级按钮红点
        private void _refreshUpgradeRedTip()
        {
            if (wnd == null || _m_heroInfo == null || _m_curSelectTalentSkillRef == null)
                return;

            HeroTalentSkillInfo talentSkillInfo = _m_heroInfo.heroTalentSkillInfoMgr.getTalentSkillInfo(_m_curSelectTalentSkillRef.id);
            ALUGUICommon.setGameObjEnable(wnd.goUpgradeRedTip, talentSkillInfo != null && talentSkillInfo.canUpgrade());
        }

        //排序
        private int _sortSkillList(long _idA, long _idB)
        {
            if (_m_heroInfo == null)
                return 0;

            //已解锁>未解锁
            bool isUnlockA = _m_heroInfo.heroTalentSkillInfoMgr.isTalentSkillUnlock(_idA);
            bool isUnlockB = _m_heroInfo.heroTalentSkillInfoMgr.isTalentSkillUnlock(_idB);
            if (isUnlockA.CompareTo(isUnlockB) != 0)
                return -isUnlockA.CompareTo(isUnlockB);

            //未满级>满级
            HeroTalentSkillInfo infoA = _m_heroInfo.heroTalentSkillInfoMgr.getTalentSkillInfo(_idA);
            HeroTalentSkillInfo infoB = _m_heroInfo.heroTalentSkillInfoMgr.getTalentSkillInfo(_idB);
            bool isMaxLevelA = infoA != null && infoA.isMaxLevel;
            bool isMaxLevelB = infoB != null && infoB.isMaxLevel;
            if (isMaxLevelA.CompareTo(isMaxLevelB) != 0)
                return isMaxLevelA.CompareTo(isMaxLevelB);

            //可升级>不可升级
            bool canUpgradeA = isUnlockA && infoA != null && infoA.canUpgrade();
            bool canUpgradeB = isUnlockB && infoB != null && infoB.canUpgrade();
            if (canUpgradeA.CompareTo(canUpgradeB) != 0)
                return -canUpgradeA.CompareTo(canUpgradeB);

            //id从小到大
            return _idA.CompareTo(_idB);
        }

        #region 点击事件

        //点击连升十级开关
        private void _onClickToggle(NPGGUIWndCommonToggleEx _commonToggleEx)
        {
            if (_m_wTenLevelUpToggle == null)
                return;

            if (!GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.hero_talent_upgrade_ten_times_simple_unlock_id,true))
                return;

            bool isOn = !_m_wTenLevelUpToggle.isOn;
            _m_wTenLevelUpToggle.setSelected(isOn, true);
            AccountSettingMgr.instance.accountSetting.setIsHeroTalentTenUpgrade(isOn);
            _refreshButtonState();
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            _m_aOnClickClose?.Invoke();
        }

        //点击资质信息
        private void _onClickTalentInfo(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndHeroTalentValueDetail.instance, () =>
            {
                GGUIWndHeroTalentValueDetail.instance.showWnd();
                GGUIWndHeroTalentValueDetail.instance.setInfo(_m_heroInfo);
            },UINodeTagConst.C_HERO_TALENT_VALUE_DETAIL);
        }

        //点击升级
        private void _onClickUpgrade(GameObject _go)
        {
            if (wnd == null || _m_heroInfo == null || _m_curSelectTalentSkillRef == null)
                return;

            if (!_m_heroInfo.heroTalentSkillInfoMgr.isTalentSkillUnlock(_m_curSelectTalentSkillRef.id))
                return;

            bool isTen = _m_wTenLevelUpToggle != null && _m_wTenLevelUpToggle.isOn;
            NPCommonCostItem levelUpCostItem = _m_heroInfo.getTalentSkillLevelUpCostItem(_m_curSelectTalentSkillRef.id, false);
            if (levelUpCostItem == null || !GCommon.isItemEnough(levelUpCostItem, true))
                return;

            long serialize = _m_lShowSerialize;
            long talentSkillId = _m_curSelectTalentSkillRef.id;
            NPPlayer.instance.heroComponent.reqHeroTalentSkillUpgrade(_m_heroInfo.id, talentSkillId, isTen, (_isSuc) =>
            {
                if (wnd == null || !isShow || serialize != _m_lShowSerialize)
                    return;

                if (!_isSuc)
                    return;

                //播放特效
                if (_m_lSfxObjList == null)
                    _m_lSfxObjList = new List<CommonUISfxObj>();

                if (wnd.upgradeSfxId > 0 && wnd.upgradeSfxParent != null)
                {
                    CommonUISfxObj sfxObj = PlaySfxMgr.instance.playUISfx(wnd.upgradeSfxId, wnd.upgradeSfxParent);
                    _m_lSfxObjList.Add(sfxObj);
                }

                //当前选中的item播放特效
                _m_wSkillContainer?.playSelectItemSfx(talentSkillId);

                //刷新界面
                _refreshButtonState();
                _refreshTalentSkillDetail();

                //执行回调
                _m_aOnReqUpgradeDone?.Invoke();
            });
        }

        //点击资质技能列表item
        private void _onClickTalentSkillItem(GGUIWndHeroTalentSkillContainerItem _item)
        {
            if (_item == null)
                return;

            _m_curSelectTalentSkillRef = _item.talentSkillRef;
            _refreshButtonState();
            _refreshTalentSkillDetail();
        }

        #endregion


        #region 消息事件

        //资质技能变更
        private void _onTalentSkillChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 2 || _m_heroInfo == null)
                return;

            long heroId = (long)_objects[0];
            long talentSkillId = (long) _objects[1];
            if (heroId == _m_heroInfo.id)
            {
                _refreshPowerInfo();
                _refreshButtonState();
                _refreshTalentSkillDetail();
            }
        }

        /// <summary>
        /// 背包变更
        /// </summary>
        /// <param name="_objects"></param>
        private void _onBagChg(params object[] _objects)
        {
            _refreshButtonState();
            _m_wSkillContainer?.refreshRedTip();
        }

        #endregion
    }

}