using System;
using System.Collections.Generic;
using ALPackage;
using DG.Tweening;

namespace GOE
{
    /// <summary>
    /// 展示滚动文本列表
    /// </summary>
    public class GGUIWndCommonShowScrollTextGrid : _ANPGGUIBasicGridSubWnd<GGUIMonoCommonShowScrollTextGridItem, GGUIMonoCommonShowScrollTextGrid, GGUIWndCommonShowScrollTextGridItem>
    {
        //文本列表
        private List<string> _m_lStringList;
        //操作序列号
        private long _m_lOpSerialize;
        //移动控制
        private Tweener _m_tweener;
        //移动控制
        private Tweener _m_indexTweener;
        //是否正在滚动中
        private bool _m_bIsScrolling;

        private float _m_fCurShowIndex;

        /// <summary>
        /// 文本数据数量
        /// </summary>
        public long stringCount
        {
            get { return _m_lStringList != null ? _m_lStringList.Count : 0; }
        }

        /// <summary>
        /// 是否正在滚动
        /// </summary>
        public bool isScrolling
        {
            get { return _m_bIsScrolling; }
        }

        public GGUIWndCommonShowScrollTextGrid(GGUIMonoCommonShowScrollTextGrid _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override GGUIWndCommonShowScrollTextGridItem _createItemWnd(GGUIMonoCommonShowScrollTextGridItem _itemMono)
        {
            GGUIWndCommonShowScrollTextGridItem item = new GGUIWndCommonShowScrollTextGridItem(_itemMono);
            return item;
        }

        protected override void _onShowWnd()
        {
            _m_lOpSerialize = ALSerializeOpMgr.next();
            _m_bIsScrolling = false;
        }

        protected override void _onHideWnd()
        {
            _m_lOpSerialize = ALSerializeOpMgr.next();
            _m_bIsScrolling = false;

            _m_lStringList?.Clear();

            _m_tweener?.Kill();
            _m_tweener = null;

            _m_indexTweener?.Kill();
            _m_indexTweener = null;
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_lOpSerialize = ALSerializeOpMgr.next();

            _m_tweener?.Kill();
            _m_tweener = null;

            _m_indexTweener?.Kill();
            _m_indexTweener = null;

            _m_lStringList?.Clear();
            _m_lStringList = null;
        }

        protected override void _onWndInitDone()
        {
            _m_lStringList = new List<string>();
        }

        protected override void _refreshItemwnd(GGUIWndCommonShowScrollTextGridItem _itemWnd, int _itemIdx)
        {
            if (null == _itemWnd || null == _m_lStringList || _m_lStringList.Count <= _itemIdx)
                return;

            //显示窗口
            _itemWnd.showWnd();
            //设置信息
            _itemWnd.setInfo(_m_lStringList[_itemIdx]);
        }

        /// <summary>
        /// 添加信息并开始滚动
        /// </summary>
        /// <param name="_strList"></param>
        public void addInfo(List<string> _strList)
        {
            if (_strList == null)
                return;

            if (_m_lStringList == null)
                _m_lStringList = new List<string>();

            if (_m_lStringList.Count == 0)
                _m_fCurShowIndex = 0;

            _m_lStringList.AddRange(_strList);
            setItemCount(_m_lStringList.Count);
        }

        /// <summary>
        /// 设置开始滚动
        /// </summary>
        /// <param name="_duration">滚动时间</param>
        /// <param name="_checkCanContinue">检查是否可以继续</param>
        /// <param name="_onNewItemScroll">滚动出现新的item回调</param>
        /// <param name="_onScrollDone">滚动结束回调</param>
        public void setStartScroll(float _duration, Func<bool> _checkCanContinue, Action _onNewItemScroll, Action _onScrollDone = null)
        {
            if (wnd == null || wnd.scrollRect == null)
                return;

            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                _m_bIsScrolling = true;
                //操作序列号
                _m_lOpSerialize = ALSerializeOpMgr.next();
                long serialize = _m_lOpSerialize;
                //当前位置
                float curNormalized = wnd.scrollRect.verticalNormalizedPosition;
                //目标位置
                float targetNormalized = 0;
                
                //先销毁旧的滚动
                if (_m_tweener != null)
                {
                    _m_tweener.Kill();
                    _m_tweener = null;
                }
                if (_m_indexTweener != null)
                {
                    _m_indexTweener.Kill();
                    _m_indexTweener = null;
                }

                //开始新的滚动
                _m_tweener = DOTween.To(
                        value =>
                        {
                            if (wnd != null && wnd.scrollRect != null)
                                wnd.scrollRect.verticalNormalizedPosition = value;
                        },
                        curNormalized,
                        targetNormalized,
                        _duration)
                    .OnUpdate(() =>
                            {
                                if (_checkCanContinue != null && !_checkCanContinue())
                                {
                                    _m_lOpSerialize = ALSerializeOpMgr.next();
                                    _m_tweener?.Kill();
                                    _m_lStringList.Clear();
                                    setItemCount(0);
                                    moveToTop();
                                }
                            })
                    .SetEase(wnd.scrollSpeedCurve);
                //开始新的下标计算
                _m_indexTweener = DOTween.To(
                        value =>
                        {
                            if (Math.Ceiling(value) != Math.Ceiling(_m_fCurShowIndex))
                            {
                                _m_fCurShowIndex = value;
                                _onNewItemScroll?.Invoke();
                            }
                        },
                        _m_fCurShowIndex,
                        _m_lStringList.Count - 1,
                        _duration)
                    .OnUpdate(() =>
                            {
                                    if (_checkCanContinue != null && !_checkCanContinue())
                                    {
                                        _m_lOpSerialize = ALSerializeOpMgr.next();
                                        _m_indexTweener?.Kill();
                                    }
                            })
                    .SetEase(wnd.scrollSpeedCurve);

                //处理滚动完成回调，如果还在滚动中触发第二次滚动，则会等第二次滚动完成才触发回调
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    if (serialize != _m_lOpSerialize)
                        return;

                    //移动结束重置一下列表当前为最后一个显示
                    _resetToShowLast();
                    _m_bIsScrolling = false;

                    //执行回调
                    _onScrollDone?.Invoke();
                }, _duration);
            });
        }

        //重置到最后一个展示并清空列表
        private void _resetToShowLast()
        {
            if (_m_lStringList != null && _m_lStringList.Count > 0)
            {
                string lastStr = _m_lStringList.GetLast();
                _m_lStringList.Clear();
                _m_lStringList.Add(lastStr);
                _m_fCurShowIndex = 0;
                setItemCount(_m_lStringList.Count);
                moveToTop();
            }
        }

        //清空数据
        public void clear()
        {
            _m_lStringList?.Clear();
            setItemCount(0);
        }
    }
}