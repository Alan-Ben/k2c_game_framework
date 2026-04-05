using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChapterAutoForwardReward : _ATALBasicUIWnd<GGUIMonoChapterAutoForwardReward>
    {
        private static GGUIWndChapterAutoForwardReward _g_instance = new GGUIWndChapterAutoForwardReward();

        public static GGUIWndChapterAutoForwardReward instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndChapterAutoForwardReward();
                return _g_instance;
            }
        }

        private ChapterAutoForwardRewardData _m_data;
        //奖励列表
        private GGUIWndCommonRewardContainer _m_itemContainer;
        
        public GGUIWndChapterAutoForwardReward() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath
        {
            get { return GGUIMonoChapterAutoForwardReward.assetPath; }
        }

        protected override string _monoObjName
        {
            get { return GGUIMonoChapterAutoForwardReward.objName; }
        }

        protected override _AALResourceCore _resourceCore
        {
            get { return GameResCore.instance; }
        }


        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            if(null != _m_itemContainer)
                _m_itemContainer.hideWnd();
        }

        protected override void _onReset()
        {
            if (null != _m_itemContainer)
                _m_itemContainer.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (null != _m_itemContainer)
                _m_itemContainer.discard();
            _m_itemContainer = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            if (null != wnd.itemContainer)
                _m_itemContainer = new GGUIWndCommonRewardContainer(wnd.itemContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _ocClickClose);
        }

        private void _ocClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Chapter.C_CHAPTER_AUTO_FORWARD_REWARD);

        }
        
        public void setInfo(ChapterAutoForwardRewardData _data)
        {
            _m_data = _data;

            _refresh();
        }

        private void _refresh()
        {
            if(null == wnd || null == _m_data)
                return;

            ALUGUICommon.setLabelTxt(wnd.textOldChapter, GCommon.getChapterName(_m_data.startChapterId, _m_data.startPoint));
            ALUGUICommon.setLabelTxt(wnd.textNewChapter, GCommon.getChapterName(_m_data.endChapterId, _m_data.endPoint));
            ALUGUICommon.setLabelTxt(wnd.textCostGold, $"-{_m_data.costGoldNum.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)}");
            ALUGUICommon.setLabelTxt(wnd.textHeroExp, _m_data.rewardHeroExp.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            ALUGUICommon.setLabelTxt(wnd.textPlayerExp, _m_data.rewardPlayerExp.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            
            //刷新奖励列表
            if (null != _m_itemContainer)
            {
                _m_itemContainer.showWnd();
                _m_itemContainer.setRewardList(_m_data.getRewardItemList());
            }
        }
    }
}