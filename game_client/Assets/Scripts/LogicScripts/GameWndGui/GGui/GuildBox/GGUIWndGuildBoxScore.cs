using System.Collections.Generic;
using ALPackage;
using Common.GuildEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟宝箱
    /// </summary>
    public class GGUIWndGuildBoxScore : _ATALBasicUIWnd<GGUIMonoGuildBoxScore>
    {
        private static GGUIWndGuildBoxScore _g_instance = new GGUIWndGuildBoxScore();
    
        public static GGUIWndGuildBoxScore instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildBoxScore();
                return _g_instance;
            }
        }
        
        private GGUIWndGuildBoxScoreContainer _m_itemContainerWnd;

        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>

        public GGUIWndGuildBoxScore() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoGuildBoxScore.assetPath; }
        protected override string _monoObjName { get => GGUIMonoGuildBoxScore.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
        }
    
        protected override void _onReset()
        {
        }
    
        protected override void _onDiscard()
        {
            _m_itemContainerWnd?.discard();
            _m_itemContainerWnd = null;
            if (wnd == null) return;
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickbtnClose);
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
                
            if (wnd.itemContainer != null)
                _m_itemContainerWnd = new GGUIWndGuildBoxScoreContainer(wnd.itemContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickbtnClose);
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;

            bool addFreeBox = false;
            List<GuildBoxRefObj> dataList = new List<GuildBoxRefObj>();
            foreach (GuildBoxRefObj boxRefObj in GRefdataCoreMgr.instance.guildBoxRefCore.refList)
            {
                if(boxRefObj == null || boxRefObj.type == EGuildBoxType.GUILD_ACTIVE_BOX || (boxRefObj.type == EGuildBoxType.GUILD_FREE_BOX && addFreeBox))
                    continue;

                //免费宝箱都一样只添加一个
                if (boxRefObj.type == EGuildBoxType.GUILD_FREE_BOX)
                    addFreeBox = true;

                dataList.Add(boxRefObj);
            }
            //排序
            dataList.Sort(_sortList);

            _m_itemContainerWnd?.showWnd();
            _m_itemContainerWnd?.showItemList(dataList);

        }

        //排序 付费>免费 ,一样按照id排序
        private int _sortList(GuildBoxRefObj _a, GuildBoxRefObj _b)
        {
            if(_a == null || _b == null)
                return 0;

            if (_a.type != _b.type)
                return -(_a.type.CompareTo(_b.type));

            return _a.id.CompareTo(_b.id);
        }
        
        // <AutoGen:Method>
        // 关闭按钮点击事件
        private void _onClickbtnClose(GameObject go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_BOX_SCORE);
        }
        // </AutoGen:Method>
    }
}
