using System;
using System.Collections.Generic;
using System.Text;
using ALPackage;
using Common.GuildEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 其他联盟信息界面
    /// </summary>
    public class GGUIWndGuildOtherInfo : _ANPGGUIBasicWnd<GGUIMonoGuildOtherInfo>
    {
        private static GGUIWndGuildOtherInfo _g_instance = new GGUIWndGuildOtherInfo();

        public static GGUIWndGuildOtherInfo instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildOtherInfo();
                return _g_instance;
            }
        }

        private GuildOtherInfo _m_guildOtherInfo;
        private bool _m_bJoinGuildFuncOn;
        
        private long _m_lShowSerializeId;
        
        private GGUIWndGuildSubBaseInfo _m_guildBaseInfoSubWnd;//联盟基础信息
        private GGUIWndGuildOtherInfoMemberGrid _m_memberGridWnd;//联盟成员列表
        private GGUIWndJoinGuildBtn _m_joinGuildBtnWnd;//加入联盟按钮
        
        public event Action<bool> onRetJoinGuild;//收到请求加入联盟回调
        public event Action<bool> onRetApplyJoinGuild;//收到请求申请加入联盟回调
        public event Action<bool> onRetCancelJoinGuild;//收到请求取消加入联盟回调
        
        public GGUIWndGuildOtherInfo() : base(EALUIWndLayer.ADDITION)
        {

        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildOtherInfo.assetPath; } }

        protected override string _monoObjName { get { return GGUIMonoGuildOtherInfo.objName; } }

        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            _m_lShowSerializeId = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerializeId = ALSerializeOpMgr.next();
            
            _m_guildBaseInfoSubWnd?.hideWnd();
            _m_memberGridWnd?.hideWnd();
            _m_joinGuildBtnWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_guildBaseInfoSubWnd?.resetWnd();
            _m_memberGridWnd?.resetWnd();
            _m_joinGuildBtnWnd?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_guildBaseInfoSubWnd?.discard();
            _m_guildBaseInfoSubWnd = null;
            
            _m_memberGridWnd?.discard();
            _m_memberGridWnd = null;

            onRetJoinGuild = null;
            onRetApplyJoinGuild = null;
            onRetCancelJoinGuild = null;
            
            if (_m_joinGuildBtnWnd != null)
            {
                _m_joinGuildBtnWnd.onRetJoinGuild -= _onRetJoinGuild;
                _m_joinGuildBtnWnd.onRetApplyJoinGuild -= _onRetApplyJoinGuild;
                _m_joinGuildBtnWnd.onRetCancelJoinGuild -= _onRetCancelJoinGuild;
                
                _m_joinGuildBtnWnd.discard();
                _m_joinGuildBtnWnd = null;
            }

            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _clickCloseBtn);
            }
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoGuildBaseInfo != null)
                _m_guildBaseInfoSubWnd = new GGUIWndGuildSubBaseInfo(wnd.monoGuildBaseInfo);

            if (wnd.monoMemberGrid != null)
                _m_memberGridWnd = new GGUIWndGuildOtherInfoMemberGrid(wnd.monoMemberGrid);

            if (wnd.monoJoinBtn != null)
            {
                _m_joinGuildBtnWnd = new GGUIWndJoinGuildBtn(wnd.monoJoinBtn);
                _m_joinGuildBtnWnd.onRetJoinGuild += _onRetJoinGuild;
                _m_joinGuildBtnWnd.onRetApplyJoinGuild += _onRetApplyJoinGuild;
                _m_joinGuildBtnWnd.onRetCancelJoinGuild += _onRetCancelJoinGuild;
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _clickCloseBtn);
        }

        public void setData(GuildOtherInfo _guildOtherInfo, bool _joinGuildFuncOn)
        {
            _m_guildOtherInfo = _guildOtherInfo;
            _m_bJoinGuildFuncOn = _joinGuildFuncOn;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (_m_guildOtherInfo == null || wnd == null)
                return;

            if (_m_guildBaseInfoSubWnd != null)
            {
                _m_guildBaseInfoSubWnd.showWnd();
                _m_guildBaseInfoSubWnd.setBaseInfo(_m_guildOtherInfo);
            }

            if (_m_memberGridWnd != null)
            {
                long serializeId = _m_lShowSerializeId;
                _m_guildOtherInfo.getSortMemberList((_memberList) =>
                {
                    if(_m_lShowSerializeId != serializeId || _memberList == null || _m_memberGridWnd == null)
                        return;
                    
                    _m_memberGridWnd.showWnd();
                    _m_memberGridWnd.setShowData(_memberList);
                });
            }

            bool isFuncUnlock = GCommon.isFuncUnlock(ENPFunctionType.GUILD);
            if (_m_joinGuildBtnWnd != null)
            {
                // 加入联盟功能开启 且 自身未加入联盟时
                if (_m_bJoinGuildFuncOn && !NPPlayer.instance.guildComp.isJoinGuild() && isFuncUnlock)
                {
                    _m_joinGuildBtnWnd.showWnd();
                    _m_joinGuildBtnWnd.setData(_m_guildOtherInfo);       
                }
                else//否则隐藏
                {
                    _m_joinGuildBtnWnd.hideWnd();
                }
            }

            // 设置解锁条件
            FuncUnlockInfo unlockInfo = NPPlayer.instance.funcUnlockComp.getFuncUnlockInfo(ENPFunctionType.GUILD);
            ALUGUICommon.setLabelTxt(wnd.txtUnlockDesc, unlockInfo?.getUnlockTip());
            ALUGUICommon.setGameObjEnable(wnd.txtUnlockDesc, !isFuncUnlock);
        }
        
        /// <summary>
        /// 收到加入联盟回包
        /// </summary>
        /// <param name="_isSucc"></param>
        private void _onRetJoinGuild(bool _isSucc)
        {
            long serializeId = _m_lShowSerializeId;
            _m_guildOtherInfo?.reReqGuildInfo(() =>
            {
                if(_m_lShowSerializeId != serializeId || wnd == null || !isShow)
                    return;
                
                // 刷新item
                _refreshWnd();       
            });
            
            onRetJoinGuild?.Invoke(_isSucc);
        }
        
        /// <summary>
        /// 收到申请加入联盟回包
        /// </summary>
        /// <param name="_isSucc"></param>
        private void _onRetApplyJoinGuild(bool _isSucc)
        {
            long serializeId = _m_lShowSerializeId;
            _m_guildOtherInfo?.reReqGuildInfo(() =>
            {
                if(_m_lShowSerializeId != serializeId || wnd == null || !isShow)
                    return;
                
                // 刷新item
                _refreshWnd();       
            });
            
            onRetApplyJoinGuild?.Invoke(_isSucc);
        }
        
        /// <summary>
        /// 收到取消加入联盟回包
        /// </summary>
        /// <param name="_isSucc"></param>
        private void _onRetCancelJoinGuild(bool _isSucc)
        {
            // 刷新item
            _refreshWnd();
            
            onRetCancelJoinGuild?.Invoke(_isSucc);
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _clickCloseBtn(GameObject _go)
        {
            closeWnd();
        }
        
        public void closeWnd()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_OTHER_GUILD_INFO);
        }
    }
}