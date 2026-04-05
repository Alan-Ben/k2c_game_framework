using System.Collections.Generic;
using ALPackage;
using Common.GuildDungeonObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟副本排行
    /// </summary>
    public class GGUIWndGuildDungeonRank : _ATALBasicUIWnd<GGUIMonoGuildDungeonRank>
    {
        private static GGUIWndGuildDungeonRank _g_instance = new GGUIWndGuildDungeonRank();
    
        public static GGUIWndGuildDungeonRank instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildDungeonRank();
                return _g_instance;
            }
        }
        
        private GGUIWndGuildDungeonRankGrid _m_itemGridWnd;
        private List<Common.GuildDungeonObj.GuildDungeon_DamageRankItem> _m_itemDataList = new List<Common.GuildDungeonObj.GuildDungeon_DamageRankItem>();

        private int _m_page = 1;
        private int _m_nextPage = 1;
    
        private bool _m_isReqPageListIng = false;
        private int _m_refreshSerialize = -1;

        // <AutoGen:WndDeclaration>
        private NPGGUIWndPlayerIcon _m_selfPlayerInfoWnd;
        // </AutoGen:WndDeclaration>

        public GGUIWndGuildDungeonRank() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoGuildDungeonRank.assetPath; }
        protected override string _monoObjName { get => GGUIMonoGuildDungeonRank.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
            _m_page = 1;
            _m_isReqPageListIng = false;
            _m_refreshSerialize = ALSerializeOpMgr.next();
            _m_itemDataList?.Clear();

        }
    
        protected override void _onReset()
        {
        }
    
        protected override void _onDiscard()
        {   
            _m_page = 1;
            _m_isReqPageListIng = false;
            _m_refreshSerialize = ALSerializeOpMgr.next();
            _m_itemDataList?.Clear();
            
            _m_itemGridWnd?.discard();
            _m_itemGridWnd = null;
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickbtnClose);
            _m_selfPlayerInfoWnd?.discard();
            _m_selfPlayerInfoWnd = null;
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if (wnd.itemGrid != null)
            {
                _m_itemGridWnd = new GGUIWndGuildDungeonRankGrid(wnd.itemGrid);
                wnd.itemGrid.scrollRect.onValueChanged.AddListener(_onScrollRectValueChg);
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickbtnClose);
            if (wnd.selfPlayerInfo != null)
                _m_selfPlayerInfoWnd = new NPGGUIWndPlayerIcon(wnd.selfPlayerInfo);
        }
        
        
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            
            if(_m_selfPlayerInfoWnd != null)
            {
                _m_selfPlayerInfoWnd.showWnd();
                _m_selfPlayerInfoWnd.setSelfInfo();
            }
            _m_page = 1;
            _m_itemDataList?.Clear();
            _m_refreshSerialize = ALSerializeOpMgr.next();

            //先设置默认显示状态
            ALUGUICommon.setGameObjEnable(wnd.goSelfNotInRankHideList, false);
            ALUGUICommon.setGameObjEnable(wnd.goSelfNotInRankShowList, true);

            //刷新列表
            _refreshList(_m_page);
        }
        
        private void _onScrollRectValueChg(Vector2 arg0)
        {
            if (_m_page == _m_nextPage)//没有下一页了
                return;
            if (arg0.y <= 0f)
            {
                _refreshList(_m_nextPage);
            }
        }
        /// <summary>
        /// 刷列表
        /// </summary>
        /// <param name="_pageStartSerial"></param>
        private void _refreshList(int page)
        {
            if (_m_isReqPageListIng)
                return;
            int inputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            _m_refreshSerialize = ALSerializeOpMgr.next();
            int refreshSerialize = _m_refreshSerialize;
            _m_isReqPageListIng = true;
            NPPlayer.instance.guildDungeonComp.reqDamageRank( page, wnd.perPageItemNum, (_msg) =>
            {
                if (refreshSerialize != _m_refreshSerialize || _msg == null)
                {
                    _m_isReqPageListIng = false;
                    MainCameraMono.selfInstance.closeAllInputMask(inputMaskSerialize);
                    return;
                }

                ALUGUICommon.setLabelTxt(wnd.txtRank, _msg.getMyRank());
                ALUGUICommon.setLabelTxt(wnd.txtScore, _msg.getMyValue().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));

                //设置自己排名显隐状态
                ALUGUICommon.setGameObjEnable(wnd.goSelfNotInRankHideList, _msg.getMyRank() > 0);
                ALUGUICommon.setGameObjEnable(wnd.goSelfNotInRankShowList, _msg.getMyRank() <= 0);

                _m_page = page;
                List<GuildDungeon_DamageRankItem> records = _msg.getItemList();
                
                if (_m_itemDataList != null && records != null) 
                    _m_itemDataList.AddRange(records);
                if (records == null || records.Count < wnd.perPageItemNum)// 如果小于每页数量，则没有下一页
                    _m_nextPage = _m_page;
                else
                    _m_nextPage = page + 1;
                _refreshGrid();
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    _m_isReqPageListIng = false;
                    MainCameraMono.selfInstance.closeAllInputMask(inputMaskSerialize);
                }, 0.2f);
            });
        }
        private void _refreshGrid()
        {
            _m_itemGridWnd?.showWnd();
            _m_itemGridWnd?.showItemList(_m_itemDataList);
            if (wnd != null) 
                ALUGUICommon.setGameObjEnable(wnd.emptyShowGos, _m_itemDataList?.Count == 0);
        }
        
        // <AutoGen:Method>
        
        // 关闭按钮点击事件
        private void _onClickbtnClose(GameObject go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_DUNGEON_RANK);
        }
        // </AutoGen:Method>
    }
}