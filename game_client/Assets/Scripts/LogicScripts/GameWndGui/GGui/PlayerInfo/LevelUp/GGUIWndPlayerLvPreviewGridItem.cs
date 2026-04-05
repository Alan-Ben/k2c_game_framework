using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using NPEnum;
using UnityEngine.UI;


namespace GOE
{
    public class GGUIWndPlayerLvPreviewGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoPlayerLvPreviewGridItem>
    {

        private NPGGUIWndCommonTextItemGrid m_textItemGridWnd;

        #region override
        public GGUIWndPlayerLvPreviewGridItem(GGUIMonoPlayerLvPreviewGridItem _wnd)
           : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            
            if (null != wnd.textItemGridMono)
                m_textItemGridWnd = new NPGGUIWndCommonTextItemGrid(wnd.textItemGridMono);
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

        //重置Grid单个对象
        protected override void _resetGridItem()
        {
        }

        protected override void _onDiscard()
        {
           if(null != m_textItemGridWnd)
                m_textItemGridWnd.discard();
           m_textItemGridWnd = null;
        }

        #endregion
        // 初始化UI
        public void refreshItem(PlayerLvlRefObj _refObj)
        {
            if (null == _refObj)
                return;

            List<CommonTextItemStruct> textItemStructList = new List<CommonTextItemStruct>();

            if (_refObj.special_key_list != null)
            {
                for (int i = 0; i < _refObj.special_key_list.Count; i++)
                {
                    CommonTextItemStruct tempStruct = new CommonTextItemStruct();
                    //给文本添加对应颜色
                    tempStruct.strOne = GCommon.addColorForRichText(TextTranslate.instance.getLanguage(_refObj.special_key_list[i]), wnd.specialColor);

                    if (_refObj.special_value_list != null && _refObj.special_value_list.Count > i)
                        tempStruct.strTwo = GCommon.addColorForRichText(_refObj.special_value_list[i], wnd.specialColor);

                    tempStruct.needShowGo = false;

                    textItemStructList.Add(tempStruct);
                }
            }
            
            //检查每日奖励
            NPCommonCostItem currentDailyReward = _refObj.daily_reward_item.GetFirst();
            if (currentDailyReward != null)
            {
                CommonTextItemStruct dailyPropertyData = new CommonTextItemStruct();
                dailyPropertyData.needShowGo = true;
                dailyPropertyData.strOne = GCommon.addColorForRichText(TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_dailyReward_none), wnd.normalColor);
                dailyPropertyData.texture = GCommon.getItemTexIcon(currentDailyReward.getItemType(), currentDailyReward.subId);
                dailyPropertyData.strTwo = GCommon.addColorForRichText(currentDailyReward.count.ToString(), wnd.normalColor);
                textItemStructList.Add(dailyPropertyData);
            }

            if (_refObj.player_property != null && _refObj.player_property.propertyObjList != null)
            {
                NPPlayerPropertyModifier playerPropertyModifier = _refObj.player_property.duplicate();
                playerPropertyModifier.propertyObjList.Sort(_sortPropertyList);
                for (int i = 0; i < playerPropertyModifier.propertyObjList.Count; i++)
                {
                    CommonTextItemStruct tempStruct = new CommonTextItemStruct();
                    NPPlayerPropertyRefObj playerPropertyRefObj = GRefdataCoreMgr.instance.playerPropertyCore.getRef((long)playerPropertyModifier.propertyObjList[i].type);
                    if (playerPropertyRefObj != null && playerPropertyRefObj.is_show)
                    {
                        tempStruct.needShowGo = false;
                        tempStruct.strOne = GCommon.addColorForRichText(GCommon.getPlayerPropertyName(playerPropertyModifier.propertyObjList[i].type), wnd.normalColor);
                        tempStruct.texture = GCommon.getPlayerPropertyIcon(playerPropertyRefObj.player_property_type);
                        tempStruct.strTwo = GCommon.addColorForRichText(playerPropertyModifier.propertyObjList[i].value.ToString(), wnd.normalColor);
                        textItemStructList.Add(tempStruct);
                    }
                }
            }

            if (_refObj.child_educate_get_hero_exp > 0)
            {
                // 检查子嗣上课获得伙伴经验
                CommonTextItemStruct heroExpPropertyData = new CommonTextItemStruct();
                heroExpPropertyData.needShowGo = false;
                heroExpPropertyData.strOne = GCommon.addColorForRichText(TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_childEducateHeroExp_none), wnd.normalColor);
                heroExpPropertyData.texture = GCommon.getItemTexIcon(ENPItemType.CURRENCY, (long)ECurrency.HERO_EXP);
                heroExpPropertyData.strTwo = GCommon.addColorForRichText(_refObj.child_educate_get_hero_exp.ToString(), wnd.normalColor);
                textItemStructList.Add(heroExpPropertyData);
            }

            if (null != m_textItemGridWnd)
                m_textItemGridWnd.setItemList(textItemStructList);
        }

        //排序属性列表，按排序id从小到大
        private int _sortPropertyList(NPPlayerPropertyInfoObj _a, NPPlayerPropertyInfoObj _b)
        {
            if (_a == null || _b == null)
                return 0;

            NPPlayerPropertyRefObj aRefObj = GRefdataCoreMgr.instance.playerPropertyCore.getRef((long)_a.type);
            NPPlayerPropertyRefObj bRefObj = GRefdataCoreMgr.instance.playerPropertyCore.getRef((long)_b.type);
            if (aRefObj == null || bRefObj == null)
                return 0;

            return aRefObj.sort_id.CompareTo(bRefObj.sort_id);
        }
    }
}
