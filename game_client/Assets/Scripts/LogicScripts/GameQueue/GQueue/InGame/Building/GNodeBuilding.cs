
using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public partial class GNodeBuilding : _AGNodeMain
    {
        [NotNull] private readonly BuildingViewMgr _m_viewMgr;
        [NotNull] private readonly AnecdoteViewMgr _m_anecdoteViewMgr;
        [NotNull] private readonly RankGiftPackPointView _m_rankGiftPackPointView;
        [NotNull] private readonly RushExchangeView _m_rushExchangeView;
        

        private int _m_enterSerialize;
        private Action _m_enterComplete;


        public GNodeBuilding(Action _complete = null) 
            : base(EMainFunctionTabType.BUILDING, UINodeTagConst.C_BUILDING)
        {
            _m_viewMgr = new BuildingViewMgr();
            _m_anecdoteViewMgr = new AnecdoteViewMgr();
            _m_rankGiftPackPointView = new RankGiftPackPointView();
            _m_rushExchangeView = new RushExchangeView();
            _m_enterComplete = _complete;
        }


        /// <summary>
        /// 允许弹出的提示窗口类型，默认都不弹
        /// </summary>
        public override ENoticeType enableNoticeType { get { return ENoticeType.BUILDING; } }
        [NotNull]
        public BuildingViewMgr buildingViewMgr { get { return _m_viewMgr; } }


        public override void onEnterQueue()
        {
        }
        public override void onClose()
        {
            _m_bIsShowingPushPopWnd = false;
            _m_aOnAllPushPopWndDone?.Invoke();
            _m_aOnAllPushPopWndDone = null;

            _m_bHasTryGetNationalPowerTargetPopWndEntry = false;
            _m_nationalPowerTargetPopWndEntry = null;
        }
        protected override void _doEnterNode(Action _triggerEnterDone)
        {
            WinMsg.SendMsg(WinMsgType.ENTER_CITY);
            
            int serialize = _m_enterSerialize;
            GTDSceneMain.instance.showMainScene(MainAdditionBuildingTDScene.instance, () =>
            {
                if (serialize != _m_enterSerialize)
                    return;
                
                GUISceneMain.instance.showMainScene(GMainGUIAddSceneBuilding.instance, () =>
                {
                    if (serialize != _m_enterSerialize)
                        return;

                    ALStepCounter stepCounter = new ALStepCounter();
                    stepCounter.chgTotalStepCount(5);
                    stepCounter.regAllDoneDelegate(() =>
                    {
                        
                        _triggerEnterDone?.Invoke();
                        Action complete = _m_enterComplete;
                        _m_enterComplete = null;
                        complete?.Invoke();
                        
                        WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.CITY_SCENE_ENTER_DONE);
                        WinMsg.SendMsg(WinMsgType.ON_BUILDING_NODE_ENTER_DONE);
                    });
                    _m_viewMgr.init(stepCounter.addDoneStepCount);
                    _m_anecdoteViewMgr.init(stepCounter.addDoneStepCount);
                    _m_rankGiftPackPointView.init(stepCounter.addDoneStepCount);
                    _m_rushExchangeView.init(stepCounter.addDoneStepCount);
                    MainCityPushNoticeMgr.instance.showMainCityPushNotice(stepCounter.addDoneStepCount);
                });
            });
        }
        protected override void _doQuitNode()
        {
            // 注册 Node 变化监听消息, 让 ViewMgr 在下一个 node 加入之后再释放，这个 _onNodeChg 只执行一次
            WinMsg.RegisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            _m_enterSerialize = ALSerializeOpMgr.next();
        }
        
        
        private void _onNodeChg()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            _m_viewMgr.discard();
            _m_anecdoteViewMgr.discard();
            _m_rankGiftPackPointView.discard();
            _m_rushExchangeView.discard();
        }
    }
}