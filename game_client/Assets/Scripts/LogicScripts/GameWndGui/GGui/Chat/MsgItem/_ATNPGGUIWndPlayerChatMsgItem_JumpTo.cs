using ALPackage;
using ChatPackage;
using JetBrains.Annotations;
using GS2GC.p004_PlayerOp;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 带玩家头像，还有带功能解锁判断的跳转功能的聊天信息窗体基类
    /// </summary>
    public abstract class _ATNPGGUIWndPlayerChatMsgItem_JumpTo<T_MONO, T_DATA> : _ATNPGGUIWndPlayerChatMsgItem<T_MONO, T_DATA>
        where T_MONO : _ANPGGUIMonoPlayerChatMsgItem_JumpTo
        where T_DATA : _IMsgItemData
    {

        protected _ATNPGGUIWndPlayerChatMsgItem_JumpTo(T_DATA _detailInfo, Transform _parent, [NotNull] GUICacheMgrChatMsgItem _cacheMgr) : base(_detailInfo, _parent, _cacheMgr)
        {

        }
        
        /// <summary>
        /// 用于判断是或否已经解锁的功能类型，NONE表示跳过功能解锁判断
        /// </summary>
        protected abstract ENPFunctionType functionType { get; }

        protected override void _discard()
        {
            if(null == wnd)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnJump, _onClickBtnJump);
            base._discard();
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnJump, _onClickBtnJump);
            base._onWndInitDone();
        }

        private void _onClickBtnJump(GameObject obj)
        {
            if (functionType != ENPFunctionType.NONE && !GCommon.isFuncUnlock(functionType, true))
                return;
                
            _onClickJump();
        }

        /// <summary>
        /// 点击跳转
        /// </summary>
        protected abstract void _onClickJump();
    }
}
