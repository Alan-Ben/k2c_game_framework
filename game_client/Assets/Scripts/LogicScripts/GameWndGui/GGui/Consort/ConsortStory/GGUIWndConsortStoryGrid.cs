using System;
using System.Collections.Generic;
using ALPackage;
using Common.ConsortEnum;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 妃子故事Grid
    /// </summary>
    public class GGUIWndConsortStoryGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoConsortStoryGridItem, GGUIMonoConsortStoryGrid, GGUIWndConsortStoryGridItem>
    {
        private GGottenConsortInfo _m_iGottenConsortInfo;//已解锁的妃子信息
        private Dictionary<EConsortStoryType, List<ConsortStoryRefObj>> _m_consortStoryDic;//妃子故事数据字典
        private Action _m_aDealCloseWnd;//关闭窗口方法

        [NotNull] private List<ConsortStoryRefObj> _m_lTmpStoryRefObjList = new List<ConsortStoryRefObj>();//临时故事列表
        [NotNull] private List<GGUIWndConsortStoryBarController> _m_storyBarControllerList = new List<GGUIWndConsortStoryBarController>();//妃子故事bar控制器列表

        //是否需要刷新bar
        private bool _m_bNeedRefreshBar;

        
        public GGUIWndConsortStoryGrid(GGUIMonoConsortStoryGrid _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            // 初始化容器
            setItemCount(0);

            _m_bNeedRefreshBar = true;
        }
        
        protected override void _onDiscard()
        {
            _removeAllBar();
            
            _m_consortStoryDic?.Clear();
            _m_consortStoryDic = null;
            
            _m_lTmpStoryRefObjList.Clear();
            
            _m_bNeedRefreshBar = true;
            
            _m_aDealCloseWnd = null;
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

        protected override GGUIWndConsortStoryGridItem _createItemWnd(GGUIMonoConsortStoryGridItem _itemMono)
        {
            GGUIWndConsortStoryGridItem itemWnd = new GGUIWndConsortStoryGridItem(_itemMono);

            return itemWnd;
        }

        protected override void _onRefreshItemWnd(GGUIWndConsortStoryGridItem _itemMono, int _itemIdx)
        {
            if(_itemMono == null || _itemIdx < 0 || _itemIdx >= _m_lTmpStoryRefObjList.Count)
                return;
            
            _itemMono.setData(_m_lTmpStoryRefObjList[_itemIdx], _m_iGottenConsortInfo, _m_aDealCloseWnd);
        }

        public void setData(Dictionary<EConsortStoryType, List<ConsortStoryRefObj>> _consortStoryDic, GGottenConsortInfo _consortInfo, Action _dealCloseWnd)
        {
            _m_consortStoryDic = _consortStoryDic;
            _m_aDealCloseWnd = _dealCloseWnd;
            if (_m_consortStoryDic == null || _m_consortStoryDic.Count <= 0)
            {
                _removeAllBar();
                
                if(wnd != null)
                    ALUGUICommon.setGameObjEnable(wnd.noneItemsTips, true);
                
                setItemCount(0);
                return;
            }
            
            _m_iGottenConsortInfo = _consortInfo;
            
            _m_lTmpStoryRefObjList.Clear();
            List<ConsortStoryRefObj> refObjList = null;
            for (int i = 0; i < EConsortStoryTypeComparer.g_iEnumCount; i++)
            {
                EConsortStoryType storyType = ConsortUtil.consortStoryShowOrderToType(i);
                if(_m_consortStoryDic.TryGetValue((EConsortStoryType) storyType, out refObjList) && refObjList != null)
                    _m_lTmpStoryRefObjList.AddRange(refObjList);
            }
            
            _m_bNeedRefreshBar = true;

            if(wnd != null)
                ALUGUICommon.setGameObjEnable(wnd.noneItemsTips, _m_lTmpStoryRefObjList.Count <= 0);
            
            setItemCount(_m_lTmpStoryRefObjList.Count);
            
            if(_m_bNeedRefreshBar)
                _addBar();
        }

        /// <summary>
        /// 添加bar
        /// </summary>
        private void _addBar()
        {
            if(wnd == null)
                return;
            
            _m_bNeedRefreshBar = false;

            if (_m_consortStoryDic == null || _m_consortStoryDic.Count <= 0)
            {
                _removeAllBar();
                forceRefreshBar();
                return;
            }
            
            int barNum = 0;
            int showItemNum = 0;
            GGUIWndConsortStoryBarController barController = null;
            List<ConsortStoryRefObj> refObjList = null;
            for (int i = 0; i < EConsortStoryTypeComparer.g_iEnumCount; i++)
            {
                EConsortStoryType eStoryType = ConsortUtil.consortStoryShowOrderToType(i);
                if(!_m_consortStoryDic.TryGetValue(eStoryType, out refObjList) || refObjList == null)
                    continue;
                
                if (refObjList.Count > 0)
                {
                    if (barNum >= _m_storyBarControllerList.Count)
                    {
                        barController =
                            new GGUIWndConsortStoryBarController(UIResPathAssistant.getAssetInfo(wnd.storyTypeBarUiResId), wnd.gridAreaUIObj);
                        _m_storyBarControllerList.Add(barController);
                        addBar(barController);
                    }
                    else
                    {
                        barController = _m_storyBarControllerList[barNum];
                        if (barController == null)
                        {
                            barController = new GGUIWndConsortStoryBarController(UIResPathAssistant.getAssetInfo(wnd.storyTypeBarUiResId), wnd.gridAreaUIObj);
                            _m_storyBarControllerList[barNum] = barController;
                            addBar(barController);
                        }
                    }
                    
                    barController.regLoadDoneDelegate(() =>
                    {
                        barController.setInsertIndex(showItemNum);
                        barController.setStoryType(eStoryType);
                    });

                    barNum++;
                    showItemNum += refObjList.Count;
                }
            }
            
            // 移除多余bar
            for(int i = _m_storyBarControllerList.Count - 1; i >= barNum; i--)//逆序遍历方便删除
            {
                barController = _m_storyBarControllerList[i];
                if (barController != null)
                {
                    removeBar(barController);
                    barController.discard();
                }
                
                _m_storyBarControllerList.RemoveAt(i);
            }
            
            forceRefreshBar();
        }

        private void _removeAllBar()
        {
            foreach (var barWnd in _m_storyBarControllerList)
            {
                if (barWnd != null)
                {
                    removeBar(barWnd);
                    barWnd.discard();
                }
            }
            _m_storyBarControllerList.Clear();
        }
    }
}