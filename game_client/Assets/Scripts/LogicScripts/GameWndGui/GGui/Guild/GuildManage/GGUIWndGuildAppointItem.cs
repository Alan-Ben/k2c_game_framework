using System;
using ALPackage;
using Common.GuildEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟成员任命界面Item
    /// </summary>
    public class GGUIWndGuildAppointItem : _ATALBasicUISubWnd<GGUIMonoGuildAppointItem>
    {
        //点击item事件
        private Action<GGUIWndGuildAppointItem> _m_aOnClickItem;
        //成员职位id
        private long _m_lMemberPositionId;

        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<GGUIWndGuildAppointItem> onClickItem { get { return _m_aOnClickItem; } set { _m_aOnClickItem = value; } }
        /// <summary>
        /// 当前item职位类型
        /// </summary>
        public EGuildPositionType positionType { get { return wnd != null ? wnd.positionType : EGuildPositionType.NONE; } }

        public GGUIWndGuildAppointItem(GGUIMonoGuildAppointItem _wnd) : base(_wnd)
        {
            initWnd();
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

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_aOnClickItem = null;
            _m_lMemberPositionId = 0;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickItem);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(long _memberPositionId)
        {
            _m_lMemberPositionId = _memberPositionId;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            GuildPositionRefObj memberPositionRef = GRefdataCoreMgr.instance.guildPositionRefCore.getRef(_m_lMemberPositionId);
            if (memberPositionRef == null)
                return;

            //是否是现职位
            if (wnd.positionType == memberPositionRef.type)
            {
                //现职位：{0}
                ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(TransKeyConst.guild_currentPosition_name, TextTranslate.instance.getLanguage(memberPositionRef.name)));
                ALUGUICommon.setGameObjEnable(wnd.goCurPositionShowList, true);
            }
            else
            {
                GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
                if (guildInfo != null)
                {
                    //联盟等级
                    GuildLevelRefObj levelRef = GRefdataCoreMgr.instance.guildLevelRefCore.getRef(guildInfo.level);
                    //当前item职位
                    GuildPositionRefObj positionRef = GRefdataCoreMgr.instance.guildPositionRefCore.getRef((long)wnd.positionType);
                    //当前等级限制的职位人数
                    long limitCount = 0;
                    if (wnd.positionType == EGuildPositionType.MEMBER)
                    {
                        limitCount = levelRef.member_limit - 1;
                    }
                    else
                    {
                        for (int j = 0; j < levelRef.position_limit_list.Count; j++)
                        {
                            if (levelRef.position_limit_list[j] != null &&
                                levelRef.position_limit_list[j].enumValue == wnd.positionType)
                            {
                                limitCount = levelRef.position_limit_list[j].longValue;
                                break;
                            }
                        }
                    }

                    //{0}({1}/{2})
                    ALUGUICommon.setLabelTxt(wnd.txtDesc,
                        TextTranslate.instance.getLanguage(
                            TransKeyConst.guild_positionNameWithCount_name_curCount_totalCount,
                            TextTranslate.instance.getLanguage(positionRef?.name),
                            NPPlayer.instance.guildComp.getTargetPositionMemberCount(wnd.positionType),
                            limitCount));
                }
            }
        }

        //点击事件
        private void _onClickItem(GameObject _go)
        {
            if (wnd == null)
                return;

            _m_aOnClickItem?.Invoke(this);
        }
    }
}
