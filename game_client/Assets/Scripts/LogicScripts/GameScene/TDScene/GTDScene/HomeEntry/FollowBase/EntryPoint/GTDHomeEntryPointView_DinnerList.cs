using ALPackage;

namespace GOE
{
    /// <summary>
    /// 我的宴会入口点
    /// </summary>
    public class GTDHomeEntryPointView_DinnerList : _AGTDHomeEntryPointView_Base<GTDHomeEntryPointMono_DinnerList, GGUIHomeEntryPointItemController_DinnerList, GGUIMonoHomeEntryPointItem_DinnerList, GGUIWndHomeEntryPointItem_DinnerList>
    {

        private string _m_lasPlayAniName;
        public GTDHomeEntryPointView_DinnerList(GTDHomeEntryPointMono_DinnerList _entryPointMono) : base(_entryPointMono)
        {
        }

        protected override void _onInit()
        {
            _m_lasPlayAniName = _m_entryPointMono.defaultCheckName;
            WinMsg.RegisterMsgAct(WinMsgType.ON_DINNER_LIST_DISCARD, _refreshAni);
        }

        protected override void _onDiscard()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_DINNER_LIST_DISCARD, _refreshAni);
            _m_lasPlayAniName = null;
        }

        protected override void _onRefreshShow()
        {
            if(null == _m_entryPointMono)
                return;
            // _playAni(_m_entryPointMono.normalAniName);
            _refreshAni();
        }

        private void _refreshAni()
        {
            // NPPlayer.instance.dinnerComp.reqGetDinnerList(1,1, (_info) =>
            // {
            //     if(null == _info)
            //         return;
            //     if(_info.getSimpleInfoList().Count > 0)
            //         _playAni(_m_entryPointMono.hasOtherDinnerAniName);
            //     else
            //         _playAni(_m_entryPointMono.normalAniName);
            // });
        }

        private void _playAni(string _aniName)
        {
            if (!string.IsNullOrEmpty(_m_lasPlayAniName) && _m_lasPlayAniName.Equals(_aniName))
                return;
            if (null == _m_entryPointMono || null == _m_entryPointMono.aniShowInterface)
                return;
            _m_lasPlayAniName = _aniName;
            _m_entryPointMono.aniShowInterface.playAni(_aniName);
        }

        protected override void _onHide()
        {

        }

        protected override void _onRefreshFollowControl(GGUIHomeEntryPointItemController_DinnerList _followControl)
        {
            if (null == _followControl)
                return;

            _followControl.setShowData(_m_entryPointRefObj);
        }
    }
}