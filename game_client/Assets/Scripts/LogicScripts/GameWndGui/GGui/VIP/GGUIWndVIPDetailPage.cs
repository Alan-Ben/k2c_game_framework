using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using CommonEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// VIP详情加载页面
    /// </summary>
    public class GGUIWndVIPDetailPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoVIPDetailPage>
    {
        //资源id
        private long _m_lUIResId;
        //形象附加界面
        private List<GGUIWndSubVIPDetailActor> _m_lActorList;
        //vip配置
        private VipRefObj _m_vipRef;

        public GGUIWndVIPDetailPage(long _uiResId, Transform _parent) : base(_parent)
        {
            _m_lUIResId = _uiResId;
        }

        /**************
         * 窗口相关加载配置
         **/
        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lUIResId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lUIResId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        /// <summary>
        /// 资源id
        /// </summary>
        public long uiResId { get { return _m_lUIResId; } }

        protected override void _onShowWnd()
        {
        }
        
        protected override void _onHideWnd()
        {
            if (_m_lActorList != null)
            {
                for (int i = 0; i < _m_lActorList.Count; i++)
                {
                    _m_lActorList[i]?.hideWnd();
                }
            }
        }
        
        protected override void _onReset()
        {
            if (_m_lActorList != null)
            {
                for (int i = 0; i < _m_lActorList.Count; i++)
                {
                    _m_lActorList[i]?.resetWnd();
                }
            }
        }
        
        protected override void _onDiscard()
        {
            if (_m_lActorList != null)
            {
                for (int i = 0; i < _m_lActorList.Count; i++)
                {
                    _m_lActorList[i]?.discard();
                }
            }
            _m_lActorList?.Clear();
            _m_lActorList = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lActorList = new List<GGUIWndSubVIPDetailActor>();
            if (wnd.monoActorList != null)
            {
                for (int i = 0; i < wnd.monoActorList.Count; i++)
                {
                    GGUIWndSubVIPDetailActor actorSubWnd = new GGUIWndSubVIPDetailActor(wnd.monoActorList[i]);
                    _m_lActorList.Add(actorSubWnd);
                }
            }
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(VipRefObj _vipRef)
        {
            if (_vipRef == null)
                return;

            _m_vipRef = _vipRef;
            _refreshWnd();
        }

        //刷新界面
        private void _refreshWnd()
        {
            if (wnd == null || _m_vipRef == null)
                return;

            //设置形象
            if (_m_vipRef.show_special_reward_list != null && _m_lActorList != null)
            {
                for (int i = 0; i < _m_vipRef.show_special_reward_list.Count; i++)
                {
                    if (_m_lActorList.Count > i)
                    {
                        _m_lActorList[i]?.showWnd();
                        _m_lActorList[i]?.setInfo(_m_vipRef.show_special_reward_list[i]);
                    }
                }
            }

            //领取奖励条件提示
            long curVIPLevel = NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.VIP_LVL);
            if (curVIPLevel + 1 != _m_vipRef.vip_lvl)
                ALUGUICommon.setLabelTxt(wnd.txtGetRewardTip, "");
            else
            {
                long curExp = GCommon.getItemCount(ENPItemType.CURRENCY, (long) ECurrency.VIP_EXP);
                long needExp = _m_vipRef.vip_exp - curExp;
                ALUGUICommon.setLabelTxt(wnd.txtGetRewardTip, TextTranslate.instance.getLanguage(TransKeyConst.vip_unlockNewRewardTip_num, needExp));
            }
        }
    }
}