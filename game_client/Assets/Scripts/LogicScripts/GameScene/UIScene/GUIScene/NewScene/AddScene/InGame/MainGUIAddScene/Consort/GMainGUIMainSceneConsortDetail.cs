using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 妃子Scene
    /// </summary>
    public partial class GMainGUIMainSceneConsortDetail : _ANPGMainGUIAddSceneResBar
    {
        private static GMainGUIMainSceneConsortDetail _g_instance = new GMainGUIMainSceneConsortDetail();
        public static GMainGUIMainSceneConsortDetail instance { get { return _g_instance ??= new GMainGUIMainSceneConsortDetail(); } }

        //当前展示的妃子列表
        private List<GGottenConsortInfo> _m_lConsortInfoList = new List<GGottenConsortInfo>();
        private EConsortTdShowAniType _m_eShowAniType = EConsortTdShowAniType.ENTER;

        public int showConsortListCount { get { return _m_lConsortInfoList?.Count ?? 0; } }
        
        protected override void _onEnterScene()
        {
            //加载主窗口对象
            //加载妃子显示showcase
            GGUIWndConsortMainShowCaseWnd.instance.load();
            //加载妃子上部基础信息
            GGUIWndUnLockConsortSubMainInfoWnd.instance.load();
            //加载下面页签
            GGUIWndUnLockConsortMainDetailWnd.instance.load();

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(3);
            stepCounter.regAllDoneDelegate(setSceneInited);

            //注册加载步骤
            GGUIWndConsortMainShowCaseWnd.instance.regLoadDoneDelegate(stepCounter.addDoneStepCount);
            GGUIWndUnLockConsortMainDetailWnd.instance.regLoadDoneDelegate(stepCounter.addDoneStepCount);
            GGUIWndUnLockConsortSubMainInfoWnd.instance.regLoadDoneDelegate(stepCounter.addDoneStepCount);
        }

        protected override void _onSceneInited()
        {
            //显示默认窗口,并设置展示的妃子索引
            GGUIWndConsortMainShowCaseWnd.instance.showWnd();
            GGUIWndUnLockConsortSubMainInfoWnd.instance.showWnd();
            GGUIWndUnLockConsortMainDetailWnd.instance.showWnd();

            //显示资源栏
            showResBar(GGUIWndUnLockConsortSubMainInfoWnd.instance.wnd.barResId, GGUIWndUnLockConsortSubMainInfoWnd.instance.wnd.playerIconResId);
        }

        public override void _dealShowScene(Action _delegate)
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(4);
            stepCounter.regAllDoneDelegate(_delegate);
            
            //逐个显示
            GGUIWndConsortMainShowCaseWnd.instance.showWnd(stepCounter.addDoneStepCount);
            GGUIWndUnLockConsortSubMainInfoWnd.instance.showWnd(stepCounter.addDoneStepCount);
            GGUIWndUnLockConsortMainDetailWnd.instance.showWnd(stepCounter.addDoneStepCount);

            //显示资源栏
            showResBar(GGUIWndUnLockConsortSubMainInfoWnd.instance.wnd.barResId, GGUIWndUnLockConsortSubMainInfoWnd.instance.wnd.playerIconResId, stepCounter.addDoneStepCount);
            
            setShowIdex(consortDetailWndShowConsortIndex, _m_eShowAniType);

            WinMsg.RegisterMsg(WinMsgType.RET_CONSORT_SEND_GIFT, _onRetSendGift);

        }

        protected override void _dealQuitSceneSub()
        {
            //释放妃子显示showcase
            GGUIWndConsortMainShowCaseWnd.instance.discard();
            //释放妃子上部基础信息
            GGUIWndUnLockConsortSubMainInfoWnd.instance.discard();
            //释放下面页签
            GGUIWndUnLockConsortMainDetailWnd.instance.discard();
        }

        protected override void _dealHideSceneSub(Action _delegate)
        {
            WinMsg.UnregisterMsg(WinMsgType.RET_CONSORT_SEND_GIFT, _onRetSendGift);

            //逐个窗口调用隐藏，隐藏完成调用回调
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(3);
            stepCounter.regAllDoneDelegate(_delegate);

            //逐个隐藏
            GGUIWndConsortMainShowCaseWnd.instance.hideWnd(stepCounter.addDoneStepCount);
            GGUIWndUnLockConsortSubMainInfoWnd.instance.hideWnd(stepCounter.addDoneStepCount);
            GGUIWndUnLockConsortMainDetailWnd.instance.hideWnd(stepCounter.addDoneStepCount);
            
            _m_bIsPlaySendGiftVoice = false;
            _m_bIsShowSendGiftTdAni = false;
        }

        /// <summary>
        /// 初始化显示数据
        /// </summary>
        /// <param name="_lConsortInfoList"></param>
        /// <param name="_idx"></param>
        public void initShowData(List<GGottenConsortInfo> _lConsortInfoList, int _idx, EConsortTdShowAniType _showAniType = EConsortTdShowAniType.ENTER)
        {
            _m_lConsortInfoList = _lConsortInfoList;
            consortDetailWndShowConsortIndex = _idx;
            _m_eShowAniType = _showAniType;

            GGUIWndUnLockConsortMainDetailWnd.instance.initWndParam(this);
        }
        
        //显示上一个或下一个
        public void showPre()
        {
            if(_m_lConsortInfoList == null || _m_lConsortInfoList.Count <= 1)
                return;

            consortDetailBusinessPageSelectSkillId = 0;//切换妃子时, 选中经营技能重置
            setShowIdex(consortDetailWndShowConsortIndex - 1, EConsortTdShowAniType.IDLE);
        }
        public void showNext()
        {
            if (_m_lConsortInfoList == null || _m_lConsortInfoList.Count <= 1)
                return;

            consortDetailBusinessPageSelectSkillId = 0;//切换妃子时, 选中经营技能重置
            setShowIdex(consortDetailWndShowConsortIndex + 1, EConsortTdShowAniType.IDLE);
        }

        /// <summary>
        /// 设置刷新索引位置
        /// </summary>
        /// <param name="_idx"></param>
        public void setShowIdex(int _idx, EConsortTdShowAniType _aniType)
        {
            //如果无数据则不处理
            if (_m_lConsortInfoList == null || _m_lConsortInfoList.Count <= 0)
                return;
            
            //设置索引
            consortDetailWndShowConsortIndex = (_idx + _m_lConsortInfoList.Count) % _m_lConsortInfoList.Count;

            GGottenConsortInfo curConsortInfo = _m_lConsortInfoList[consortDetailWndShowConsortIndex];
            // 设置已读获取妃子红点
            if(curConsortInfo != null)
                NPPlayer.instance.consortComp.setReadGainConsortRedTip(curConsortInfo.consortId);
            
            //设置数据
            GGUIWndConsortMainShowCaseWnd.instance.setData(curConsortInfo, _aniType == EConsortTdShowAniType.ENTER);
            GGUIWndUnLockConsortSubMainInfoWnd.instance.setData(curConsortInfo);
            GGUIWndUnLockConsortMainDetailWnd.instance.setConsortInfo(curConsortInfo);

            //播放动画
            GGUIWndConsortMainShowCaseWnd.instance.setTdShowAni(_aniType, false, null);
        }

        /// <summary>
        /// 仅显示形象
        /// </summary>
        /// <param name="_show"></param>
        public void onlyShowActor(bool _show)
        {
            GGUIWndUnLockConsortSubMainInfoWnd.instance.onlyShowActor(_show);
            GGUIWndUnLockConsortMainDetailWnd.instance.onlyShowActor(_show);
        }

        private bool _m_bIsPlaySendGiftVoice;//是否正在播放送礼配音
        private bool _m_bIsShowSendGiftTdAni;//是否正在展示送礼人物动作
        
        /// <summary>
        /// 收到送礼回包
        /// </summary>
        private void _onRetSendGift(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is BagItemConsortRefObj _bagItemConsortRefObj))
                return;
            
            // 播放送礼特效
            GGUIWndConsortMainShowCaseWnd.instance.playSendGiftSfx(_bagItemConsortRefObj.show_type);

            if (!_m_bIsPlaySendGiftVoice)
            {
                _m_bIsPlaySendGiftVoice = true;
                GGUIWndUnLockConsortMainDetailWnd.instance.refreshBubble(EConsortVoiceType.Gift, () =>
                {
                    _m_bIsPlaySendGiftVoice = false;
                });//播放赠送礼物配音
            }

            if (!_m_bIsShowSendGiftTdAni)
            {
                _m_bIsShowSendGiftTdAni = true;
                GGUIWndConsortMainShowCaseWnd.instance.setTdShowAni(EConsortTdShowAniType.GIVE_GIFT, true, () =>
                {
                    _m_bIsShowSendGiftTdAni = false;
                });//播放赠送礼物动画
            }
        }
    }
}