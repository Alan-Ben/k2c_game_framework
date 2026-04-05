using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟派遣主窗口
    /// </summary>
    public class GGUIWndGuildDispatchMain : _ANPGGUIBasicResBarWnd<GGUIMonoGuildDispatchMain>
    {
        private static GGUIWndGuildDispatchMain _g_instance;
        public static GGUIWndGuildDispatchMain instance { get { return _g_instance ??= new GGUIWndGuildDispatchMain(); } }
        
        [NotNull] private List<GuildDispatchInfo> _m_lShowDispatchInfoList = new List<GuildDispatchInfo>();//显示的派遣信息列表
        
        private GGUIWndGuildDispatchItemContainer _m_wItemContainer;//派遣item列表
        public override bool needDiscardOnSwitch { get { return true; } }

        public GGUIWndGuildDispatchMain() : base(EALUIWndLayer.NORMAL)
        {
        }
        
        protected override string _monoAssetPath { get { return GGUIMonoGuildDispatchMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoGuildDispatchMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoDispatchItemContainer != null)
                _m_wItemContainer = new GGUIWndGuildDispatchItemContainer(wnd.monoDispatchItemContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnDispatch, _onDispatchBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnDispatch, _onDispatchBtnClick);
            }
            
            _m_wItemContainer?.discard();
            _m_wItemContainer = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
            
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_DISPATCH_INFO_CHG, _onDispatchInfoChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_DISPATCH_INFO_CHG, _onDispatchInfoChg);

            _m_lShowDispatchInfoList.Clear();
            _m_wItemContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wItemContainer?.resetWnd();
        }
        
        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            _m_lShowDispatchInfoList.Clear();
            if (NPPlayer.instance.guildComp.guildInfo != null)
            {
                foreach (var dispatchInfo in NPPlayer.instance.guildComp.guildInfo.dispatchInfoList)
                {
                    // 相性为NONE的不显示
                    if(dispatchInfo != null && dispatchInfo.specAttrType != ESpecAttrType.NONE)
                        _m_lShowDispatchInfoList.Add(dispatchInfo);
                }
            }
            
            if (_m_wItemContainer != null)
            {
                _m_wItemContainer.showWnd();
                _m_wItemContainer.setData(_m_lShowDispatchInfoList);
            }

            _refreshGuildEarnings();
            _refreshDispatchHeroCount();
        }

        private void _refreshGuildEarnings()
        {
            if(wnd == null)
                return;

            long totalEarnings = NPPlayer.instance.guildComp.guildInfo?.calTotalDispatchEarnings() ?? 0;
            
            string txtGuildEarningsKey = string.IsNullOrEmpty(wnd.txtGuildEarningsKey) ? TransKeyConst.common_add_num : wnd.txtGuildEarningsKey;
            ALUGUICommon.setLabelTxt(wnd.txtGuildEarnings,
                TextTranslate.instance.getLanguage(txtGuildEarningsKey, totalEarnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
        }

        /// <summary>
        /// 刷新派遣大臣数量
        /// </summary>
        private void _refreshDispatchHeroCount()
        {
            if(wnd == null)
                return;
            
            int dispatchHeroCount = 0;
            foreach (var guildDispatchInfo in _m_lShowDispatchInfoList)
            {
                if(guildDispatchInfo != null && guildDispatchInfo.dispatchHeroInfoList != null)
                    dispatchHeroCount += guildDispatchInfo.dispatchHeroInfoList.Count;
            }
            
            // 因为显示相性过滤了NONE, 所以总共可以派遣的大臣数量为(相性种类数-1) * 每种相性可以派遣的大臣数量
            int totalCanDispatchHeroCount = (ESpecAttrTypeComparer.g_iEnumCount - 1) * GRefdataCoreMgr.instance.npGeneral.each_attr_can_dispatch_hero_num;
            string txtDispatchNumKey = string.IsNullOrEmpty(wnd.txtDispatchNumKey) ? TransKeyConst.common_currentTotalNum_num_num : wnd.txtDispatchNumKey;
            ALUGUICommon.setLabelTxt(wnd.txtDispatchNum, TextTranslate.instance.getLanguage(txtDispatchNumKey, dispatchHeroCount, totalCanDispatchHeroCount));
        }
        
        /// <summary>
        /// 派遣信息变更
        /// </summary>
        private void _onDispatchInfoChg(params object[] _objs)
        {
            // 派遣信息变更时, 不刷新派遣item列表, 因为派遣item列表item自己会监听消息并更新

            _refreshGuildEarnings();
            _refreshDispatchHeroCount();
        }
        
        /// <summary>
        /// 委任按钮点击
        /// </summary>
        private void _onDispatchBtnClick(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildDispatch.instance, ()=>{
                GGUIWndGuildDispatch.instance.showWnd();
            }, UINodeTagConst_Guild.C_GUILD_DISPATCH_HERO);
        }
    }
}