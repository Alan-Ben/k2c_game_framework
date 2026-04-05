using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟宝箱详情
    /// </summary>
    public class GGUIWndGuildBoxDetail : _ATALBasicUIWnd<GGUIMonoGuildBoxDetail>
    {
        private static GGUIWndGuildBoxDetail _g_instance = new GGUIWndGuildBoxDetail();
    
        public static GGUIWndGuildBoxDetail instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildBoxDetail();
                return _g_instance;
            }
        }
        
        private GGUIWndGuildBoxDetailGrid _m_itemGridWnd;
        private GuildBoxRefObj _m_guildBoxRefObj;

        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>

        public GGUIWndGuildBoxDetail() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoGuildBoxDetail.assetPath; }
        protected override string _monoObjName { get => GGUIMonoGuildBoxDetail.objName; }
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
            _m_itemGridWnd?.discard();
            _m_itemGridWnd = null;
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickbtnClose);
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
                
            if (wnd.itemGrid != null)
                _m_itemGridWnd = new GGUIWndGuildBoxDetailGrid(wnd.itemGrid);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickbtnClose);
        }

        public void setInfo(GuildBoxRefObj _boxRefObj)
        {
            _m_guildBoxRefObj = _boxRefObj;
            _refreshWnd();
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd || null == _m_guildBoxRefObj)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtBoxName, GCommon.getItemName(ENPItemType.GUILD_BOX, _m_guildBoxRefObj.id));

            NPSORewardRefObj rewardRefObj = GRefdataCoreMgr.instance.rewardMap.getRef(_m_guildBoxRefObj.reward_id);
            _m_itemGridWnd?.showWnd();
            _m_itemGridWnd?.showItemList(rewardRefObj.show_item_list);
        }
        
        // <AutoGen:Method>
        
        // 关闭按钮点击事件
        private void _onClickbtnClose(GameObject go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_BOX_ACTIVE_BOX_DETAIL);
        }
        // </AutoGen:Method>
    }
}