using ALPackage;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    // 背包骑士阶段展示
    public class GGUIWndBagHeroStepContainerItem : _ATALBasicUISubWnd<GGUIMonoBagHeroStepContainerItem>
    {
        // 阶段图片
        private NPGGuiWndTexture _m_stepImgWnd;
        //合成列表
        private GGUIWndBagHeroStepSubContainer _m_containerWnd;

        
        public GGUIWndBagHeroStepContainerItem(GGUIMonoBagHeroStepContainerItem _wnd) : base(_wnd)
        {
        }

        protected override void _onDiscard()
        {
            if (null != _m_stepImgWnd)
                _m_stepImgWnd.discard();
            _m_stepImgWnd = null;

            if (null != _m_containerWnd)
                _m_containerWnd.discard();
            _m_containerWnd = null;
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {

        }

        protected override void _onShowWnd()
        {
        }

        // 初始化
        protected override void _onWndInitDone()
        { 
            // 物品
            if(null != wnd.stepImg)
                _m_stepImgWnd = new NPGGuiWndTexture(wnd.stepImg);

            if (null != wnd.containerMono)
                _m_containerWnd = new GGUIWndBagHeroStepSubContainer(wnd.containerMono);
        }

        // 初始化UI
        public void refreshItem(HeroStepRefObj _refObj,bool _isLastIdx)
        {
            ALUGUICommon.setGameObjEnable(wnd.lastHideGoList, !_isLastIdx);
            if (null == _refObj)
                return;

            // if (null != _m_stepImgWnd)
            //     _m_stepImgWnd.setTexture(_refObj.icon);
            //
            // ALUGUICommon.setLabelTxt(wnd.stepNameTxt, TextTranslate.instance.getLanguage(_refObj.title_name));

            if (null != _refObj.cost_item_list)
            {
                List<NPCommonCostItem> list = new List<NPCommonCostItem>();
                foreach (NPCommonCostItem temp in _refObj.cost_item_list)
                {
                    NPCommonCostItem item = new NPCommonCostItem(temp.getItemType(), temp.subId, NPPlayer.instance.bagComp.getItemCount(temp.subId));
                    list.Add(item);
                }

                if (null != _m_containerWnd)
                {
                    _m_containerWnd.showWnd();
                    _m_containerWnd.setData(list);
                }
            }
        }

    }
}
