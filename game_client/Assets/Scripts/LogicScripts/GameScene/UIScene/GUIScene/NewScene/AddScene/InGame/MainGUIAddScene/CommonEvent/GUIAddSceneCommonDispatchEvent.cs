using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 派遣事件Scene
    /// </summary>
    public class GUIAddSceneCommonDispatchEvent : _ANPBasicAddContainerUIScene
    {
        private static GUIAddSceneCommonDispatchEvent _g_instance = new GUIAddSceneCommonDispatchEvent();
        public static GUIAddSceneCommonDispatchEvent instance { get { return _g_instance ??= new GUIAddSceneCommonDispatchEvent(); } }

        private CommonSimpleDispatchEventAgent _m_iDispatchEventShowInfo;//派遣事件信息 
        private Action _m_aOnDealDone;//事件处理完成回调
        
        private bool _m_bShowSelectHeroWnd;//是否显示选择大臣窗口,若为false显示GGUIWndCommonDispatchEvent窗口，若为true显示GGUIWndCommonDispatchEventSelectHero窗口
        [NotNull] private List<_IHeroCardShow> _m_lSelectedHeroShowList = new List<_IHeroCardShow>();//选中的大臣id列表

        //背景遮罩
        private NPPGUIWndInstanceTransparentBk _m_wTransBk;
        private long _m_lSerialize;
        
        public bool showSelectHeroWnd{ get { return _m_bShowSelectHeroWnd; } }
        [NotNull] public List<_IHeroCardShow> selectedHeroShowList{ get { return _m_lSelectedHeroShowList; } }
        
        protected override void _onEnterScene()
        {
            setSceneInited();//设置Scene初始化成功
        }

        protected override void _onSceneInited()
        {
        }

        protected override void _dealQuitScene()
        {
            _hideTransBk();
            
            _m_lSelectedHeroShowList.Clear();
        }

        public override void _dealShowScene(Action _delegate)
        {
            if(!_m_bShowSelectHeroWnd)
                showGGUIWndCommonDispatchEvent();
            else
                showGGUIWndCommonDispatchEventSelectHero();
        }

        public override void _dealHideScene(Action _delegate)
        {
            _hideTransBk();
            
            _m_lSelectedHeroShowList.Clear();
        }

        /// <summary>
        /// 显示派遣事件窗口
        /// </summary>
        public void showGGUIWndCommonDispatchEvent()
        {
            _m_bShowSelectHeroWnd = false;

            showMainWnd(GGUIWndCommonSimpleDispatchEvent.instance, () =>
            {
                _showTransBk(GGUIWndCommonSimpleDispatchEvent.instance, () =>
                {
                    //模糊背景需要跟着窗口后面，先将模糊背景移动到最前
                    if (_m_wTransBk != null)
                        GCommon.moveTransformToLastAndRefreshLayer(_m_wTransBk.getGameObj(), GGUIWndCommonSimpleDispatchEvent.instance.getGameObj());
                    else
                        //将窗口移到最前
                        GCommon.moveTransformToLastAndRefreshLayer(GGUIWndCommonSimpleDispatchEvent.instance.getGameObj());
                    
                    GGUIWndCommonSimpleDispatchEvent.instance.setEventAgent(_m_iDispatchEventShowInfo, _m_aOnDealDone);
                });
            });
        }

        /// <summary>
        /// 显示派遣事件选择大臣窗口
        /// </summary>
        public void showGGUIWndCommonDispatchEventSelectHero()
        {
            _m_bShowSelectHeroWnd = true;
            
            showMainWnd(GGUIWndCommonSimpleDispatchEventSelectHero.instance, () =>
            {
                _showTransBk(GGUIWndCommonSimpleDispatchEventSelectHero.instance, () =>
                {
                    //模糊背景需要跟着窗口后面，先将模糊背景移动到最前
                    if (_m_wTransBk != null)
                        GCommon.moveTransformToLastAndRefreshLayer(_m_wTransBk.getGameObj(), GGUIWndCommonSimpleDispatchEventSelectHero.instance.getGameObj());
                    else
                        //将窗口移到最前
                        GCommon.moveTransformToLastAndRefreshLayer(GGUIWndCommonSimpleDispatchEventSelectHero.instance.getGameObj());
                    
                    GGUIWndCommonSimpleDispatchEventSelectHero.instance.setEventAgent(_m_iDispatchEventShowInfo, _m_aOnDealDone);
                });
            });
        }

        /// <summary>
        /// 设置是否显示选择大臣窗口
        /// </summary>
        /// <param name="_showSelectHeroWnd"></param>
        public void setShowSelectHeroWnd(bool _showSelectHeroWnd)
        {
            _m_bShowSelectHeroWnd = _showSelectHeroWnd;
        }

        public void setShowData(CommonSimpleDispatchEventAgent _dispatchEventShowInfo, Action _onDealDone)
        {
            _m_iDispatchEventShowInfo = _dispatchEventShowInfo;
            _m_aOnDealDone = _onDealDone;

            _initSelectedHeroShowList();
        }

        private void _showTransBk(_AALBasicLoadUIWndBasicClass _uiObj, Action _showDone)
        {
            if (_m_wTransBk != null && _m_wTransBk.uiObj == _uiObj)
            {
                _m_wTransBk.showWnd();
                _showDone?.Invoke();
            }
            else
            {
                _hideTransBk();//先隐藏遮罩
                _m_lSerialize = ALSerializeOpMgr.next();
                long serialize = _m_lSerialize;
                    
                _m_wTransBk = NPPGUIWndInstanceTransparentBk.showTransparentBk(
                    _uiObj
                    , () => { QueueMgr.instance.DoUIRollBackByEscByType(typeof(GNodeCommonDispatchEvent)); }//点击遮罩时使用esc回退
                    , () =>
                    {
                        if(serialize != _m_lSerialize)
                            return;
                       
                        _showDone?.Invoke();
                    });
            }
        }

        private void _hideTransBk()
        {
            _m_lSerialize = ALSerializeOpMgr.next();

            //隐藏背景
            if(_m_wTransBk != null)
                NPUIInstanceTransparentBkController.instance.hideTransparentBk(_m_wTransBk);
            
            _m_wTransBk = null;
        }
        
        /// <summary>
        /// 初始化选中大臣列表数据
        /// </summary>
        private void _initSelectedHeroShowList()
        {
            // 更新选中大臣显示数据
            _m_lSelectedHeroShowList.Clear();
            // if (_m_iDispatchEventInfo != null && _m_iDispatchEventInfo.eventDetailInfo != null &&
            //     _m_iDispatchEventInfo.eventDetailInfo.getHeroList() != null)
            // {
            //     foreach (long heroId in _m_iDispatchEventInfo.eventDetailInfo.getHeroList())
            //     {
            //         _IHeroCardShow heroShowInfo = _findHeroShowInfo(_m_lAllHeroShowInfoList, heroId);//在全部大臣显示数据列表中找是否存在该大臣
            //         if(heroShowInfo != null)
            //             _m_lSelectedHeroShowList.Add(heroShowInfo);
            //     }
            // }
        }
    }
}