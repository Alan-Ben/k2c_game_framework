using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 藏品详情Scene
    /// </summary>
    public class GMainGUIMainSceneEquipDetail : _ANPGMainGUIAddSceneResBar
    {
        private static GMainGUIMainSceneEquipDetail _g_instance = new GMainGUIMainSceneEquipDetail();
        public static GMainGUIMainSceneEquipDetail instance { get { return _g_instance ??= new GMainGUIMainSceneEquipDetail(); } }

        //当前展示的藏品列表
        private List<EquipInfo> _m_lEquipInfoList = new List<EquipInfo>();
        //当前展示的下标
        private int _m_iCurShowIndex;

        protected override void _onEnterScene()
        {
            //加载主窗口对象
            //先设置主窗口的资源id
            GGUIWndEquipDetail.instance.load();
            GGUIWndEquipDetailPageControl.instance.load();

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(setSceneInited);

            //注册加载步骤
            GGUIWndEquipDetail.instance.regLoadDoneDelegate(stepCounter.addDoneStepCount);
            GGUIWndEquipDetailPageControl.instance.regLoadDoneDelegate(stepCounter.addDoneStepCount);
        }

        protected override void _onSceneInited()
        {
            //显示默认窗口
            GGUIWndEquipDetail.instance.showWnd();
            GGUIWndEquipDetailPageControl.instance.showWnd();

            //显示资源栏
            showResBar(GGUIWndEquipDetail.instance.wnd.barResId, GGUIWndEquipDetail.instance.wnd.playerIconResId);
        }

        public override void _dealShowScene(Action _delegate)
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(_delegate);

            //逐个显示
            GGUIWndEquipDetail.instance.showWnd(stepCounter.addDoneStepCount);
            GGUIWndEquipDetailPageControl.instance.showWnd(stepCounter.addDoneStepCount);

            //显示资源栏
            showResBar(GGUIWndEquipDetail.instance.wnd.barResId, GGUIWndEquipDetail.instance.wnd.playerIconResId, stepCounter.addDoneStepCount);
            
            setShowIdex(_m_iCurShowIndex);
        }

        protected override void _dealQuitSceneSub()
        {
            //释放妃子显示showcase
            GGUIWndEquipDetail.instance.discard();
            GGUIWndEquipDetailPageControl.instance.discard();
        }

        protected override void _dealHideSceneSub(Action _delegate)
        {
            //逐个窗口调用隐藏，隐藏完成调用回调
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(_delegate);

            //逐个隐藏
            GGUIWndEquipDetail.instance.hideWnd(stepCounter.addDoneStepCount);
            GGUIWndEquipDetailPageControl.instance.hideWnd(stepCounter.addDoneStepCount);
        }

        /// <summary>
        /// 初始化显示数据
        /// </summary>
        /// <param name="_equipInfoList"></param>
        /// <param name="_idx"></param>
        public void initShowData(List<EquipInfo> _equipInfoList, int _idx)
        {
            _m_lEquipInfoList = _equipInfoList;
            _m_iCurShowIndex = _idx;
        }
        
        //显示上一个或下一个
        public void showPre()
        {
            if(_m_lEquipInfoList == null || _m_lEquipInfoList.Count <= 0)
                return;

            setShowIdex(_m_iCurShowIndex - 1);
        }
        public void showNext()
        {
            if (_m_lEquipInfoList == null || _m_lEquipInfoList.Count <= 0)
                return;

            setShowIdex(_m_iCurShowIndex + 1);
        }

        /// <summary>
        /// 设置刷新索引位置
        /// </summary>
        /// <param name="_idx"></param>
        public void setShowIdex(int _idx)
        {
            //如果无数据则不处理
            if (_m_lEquipInfoList == null || _m_lEquipInfoList.Count <= 0)
                return;

            //设置索引
            _m_iCurShowIndex = (_idx + _m_lEquipInfoList.Count) % _m_lEquipInfoList.Count;

            EquipInfo equipInfo = _m_lEquipInfoList[_m_iCurShowIndex];
            if (equipInfo == null)
                return;

            //设置数据
            GGUIWndEquipDetail.instance.setInfo(
                equipInfo,
                equipInfo.equipRef,
                _m_iCurShowIndex == 0,
                _m_iCurShowIndex == _m_lEquipInfoList.Count - 1, 
                showPre, 
                showNext);
            GGUIWndEquipDetailPageControl.instance.setInfo(equipInfo);
        }
    }
}