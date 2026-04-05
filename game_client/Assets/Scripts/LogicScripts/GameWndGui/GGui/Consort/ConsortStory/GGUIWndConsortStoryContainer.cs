using System;
using System.Collections.Generic;
using ALPackage;
using Common.ConsortEnum;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndConsortStoryContainer : _ATALUGUISubWndBasicContainer<GGUIMonoConsortStoryGridItem, GGUIMonoConsortStoryContainer, GGUIWndConsortStoryGridItem>
    {
        [NotNull] private List<GGUISubWndConsortStoryBar> _m_lStoryBarList = new List<GGUISubWndConsortStoryBar>();//妃子故事bar列表
        
        public GGUIWndConsortStoryContainer(GGUIMonoConsortStoryContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            // 无特殊初始化
        }

        protected override void _onDiscard()
        {
            _discardAllBar();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _dealAllBar((_bar) =>
            {
                _bar?.hideWnd();
            });
        }

        protected override void _onReset()
        {
            _dealAllBar((_bar) =>
            {
                _bar?.resetWnd();
            });
        }

        protected override GGUIWndConsortStoryGridItem _createItemWnd(GGUIMonoConsortStoryGridItem _itemMono)
        {
            return new GGUIWndConsortStoryGridItem(_itemMono);
        }

        protected override void _onAddItemWnd(GGUIWndConsortStoryGridItem _itemWnd)
        {
            // 无需额外处理
        }

        /// <summary>
        /// 按数据展示故事条目；根据是否配置 CG 选择不同的模板
        /// </summary>
        /// <param name="_consortStoryDic">故事数据</param>
        /// <param name="_consortInfo">妃子信息（用于判定触发/解锁）</param>
        /// <param name="_dealCloseWnd">需要跳转时的关闭回调</param>
        public void setData(Dictionary<EConsortStoryType, List<ConsortStoryRefObj>> _consortStoryDic, GGottenConsortInfo _consortInfo, Action _dealCloseWnd)
        {
            // 清空旧项
            clearAll();

            if (_consortStoryDic == null || _consortStoryDic.Count <= 0 || wnd == null)
                return;

            int barCount = 0;
            GGUISubWndConsortStoryBar barWnd = null;
            List<ConsortStoryRefObj> storyRefObjList = null;
            for (int i = 0; i < EConsortStoryTypeComparer.g_iEnumCount; i++)
            {
                EConsortStoryType storyType = ConsortUtil.consortStoryShowOrderToType(i);
                if (_consortStoryDic.TryGetValue((EConsortStoryType) storyType, out storyRefObjList) &&
                    storyRefObjList != null && storyRefObjList.Count > 0)
                {
                    // 显示bar
                    if (barCount >= _m_lStoryBarList.Count)
                    {
                        // 添加bar
                        barWnd = _addBar();
                        if (barWnd != null)
                            _m_lStoryBarList.Add(barWnd);
                    }
                    else
                    {
                        barWnd = _m_lStoryBarList[barCount];
                        if (barWnd == null)
                        {
                            barWnd = _addBar();
                            _m_lStoryBarList[barCount] = barWnd;
                        }
                    }

                    if (barWnd != null)
                    {
                        // bar移动到当前最后
                        if(barWnd.wnd != null && barWnd.wnd.transform != null)
                            barWnd.wnd.transform.SetAsLastSibling();
                        
                        barWnd.showWnd();
                        barWnd.setData(storyType);
                        barCount++;
                    }
                
                    // 显示故事
                    foreach (var storyRefObj in storyRefObjList)
                    {
                        if (storyRefObj == null)
                            continue;

                        // 选模板：有 CG 优先用 monoHasCGItemTemplate，否则用 monoNoCGItemTemplate；都缺失时回退到默认 itemTemplate
                        GGUIMonoConsortStoryGridItem template = null;
                        if (storyRefObj.unlock_cg > 0)
                            template = wnd.monoHasCGItemTemplate;
                        else
                            template = wnd.monoNoCGItemTemplate;

                        GGUIWndConsortStoryGridItem itemWnd;
                        if (template != null)
                            itemWnd = addItemWnd(template);
                        else
                            itemWnd = addItemWnd(); // 若都没有模板，使用容器默认模板

                        if (itemWnd != null)
                            itemWnd.setData(storyRefObj, _consortInfo, _dealCloseWnd);
                    }
                }
            }

            // 没有使用的bar隐藏
            for (int i = barCount; i < _m_lStoryBarList.Count; i++)
            {
                barWnd = _m_lStoryBarList[i];
                barWnd?.hideWnd();
            }
        }

        #region bar

        private void _dealAllBar(Action<GGUISubWndConsortStoryBar> _action)
        {
            if(_action == null)
                return;

            _m_lStoryBarList.ForEach(_action);
        }

        /// <summary>
        /// 添加bar方法, 直接模仿_ATALUGUISubWndBasicContainer基类中addItemWnd方法
        /// </summary>
        /// <returns></returns>
        private GGUISubWndConsortStoryBar _addBar()
        {
            //判断对应数据是否有效
            if(null == wnd || null == wnd.itemContainer || null == wnd.monoBar)
                return null;

            //实例化一个子窗口对象
            GGUIMonoConsortStoryBar barMono = GameObject.Instantiate(wnd.monoBar) as GGUIMonoConsortStoryBar;
            //创建一个子窗口管理对象
            GGUISubWndConsortStoryBar barWnd = new GGUISubWndConsortStoryBar(barMono);
            if(null == barWnd.wnd)
            {
                //创建对象无效，删除创建对象资源并退出
                ALUnityCommon.releaseGameObj(barMono);
                return null;
            }

            //将子窗口添加到容器中
            barWnd.wnd.transform.SetParent(wnd.itemContainer.transform);
            barWnd.wnd.transform.localPosition = Vector3.zero;
            barWnd.wnd.transform.localScale = Vector3.one;
            barWnd.wnd.transform.localRotation = Quaternion.identity;

            //调用子窗口的初始化函数
            barWnd.initWnd();
            //调用子窗口的显示函数
            barWnd.showWnd();

            //刷新拖拽区域大小
            _refreshGrid();

            return barWnd;
        }
        
        private void _discardBar(GGUISubWndConsortStoryBar _barWnd)
        {
            if(_barWnd == null)
                return;
            
            GGUIMonoConsortStoryBar barMono = _barWnd.wnd;
            _barWnd.discard();
            ALUnityCommon.releaseGameObj(barMono);
            
            // 移除
            _m_lStoryBarList.Remove(_barWnd);
        }
        
        private void _discardAllBar()
        {
            _dealAllBar((_bar) =>
            {
                if(_bar == null)
                    return;

                GGUIMonoConsortStoryBar barMono = _bar.wnd;
                _bar.discard();
                ALUnityCommon.releaseGameObj(barMono);
            });
            
            _m_lStoryBarList.Clear();
        }
        
        #endregion
    }
}