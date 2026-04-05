
using System;
using System.Collections.Generic;
using ALPackage;
using NPCommon;
using NPEnum;

namespace GOE
{
    /// <summary>
    ///获得妃子弹窗
    /// </summary>
    public class NPNoticeDealer_GetConsort : NPUINoticeMgr._ANPUINoticeDealer
    {
        private GGottenConsortInfo _m_consortInfo;
        private Action _m_aOnDealerDone;

        public NPNoticeDealer_GetConsort(GGottenConsortInfo _consortInfo, Action _onDealDone = null)
        {
            _m_consortInfo = _consortInfo;
            _m_aOnDealerDone = _onDealDone;
        }
        
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return false; } }
        public override bool isNoticeFullScreen { get { return true; } }
        public override bool isOnlyUINode { get { return true; } }
        public override string nodeTag { get { return UINodeTagConst.C_CONSORT_GET; } }


        public override void dealShowNotice()
        {
            if (_m_consortInfo == null)
            {
                Debug.LogError($"获得妃子notice数据异常，_m_consortInfo:{_m_consortInfo}");
                setDealerDone();
                return;
            }

            GGUIWndConsortGet.instance.setInfo(_m_consortInfo, setDealerDone);
            GUISceneMain.instance.showMainWnd(GGUIWndConsortGet.instance);
        }

        public override void dealHideNotice()
        {
        }

        protected override void _onDealerDone()
        {
            _m_aOnDealerDone?.Invoke();
            _m_aOnDealerDone = null;
        }
    }
}
