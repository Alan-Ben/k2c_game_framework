using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    ///  自定义跑马灯显示窗口
    /// </summary>
    public class GGUICustomMonoMarquee : MonoBehaviour
    {
        [ALHeader("需要监听的跑马灯showPosId列表")]
        public List<int> showPosIdList;
        [ALHeader("连续点击需要特殊处理按钮")]
        public GGUIMonoContinuousClickBtn monoContinuousClickBtn;
        [ALHeader("跑马灯item父节点")]
        public Transform itemParent;
        [ALHeader("文字的可见区域")]
        public RectTransform transViewportArea;
        [ALHeader("每条跑马灯展示间隔时间(秒)")]
        public float intervalTimeSec;
        [ALHeader("跑马灯每秒的移动速度")]
        public float fMoveSpeed;
        [ALHeader("有跑马灯时需要展示的GO列表")]
        public List<GameObject> goHaveMarqueeShowList;
        [ALHeader("跑马灯显示动画")]
        public CommonAnimationSingleInfo marqueeShowAni;

#if NP_GAME
        //展示完成回调
        [System.NonSerialized]
        public Action<GameObject> onShowDone;
        //正在展示中的item列表
        private List<GGUIWndSubMarqueeItem> _m_lMovingItemList;
        //是否正在处理移动任务
        private bool _m_bIsDealingTask;
        //任务序列号
        private long _m_lSerialize;
        //连续点击特殊处理按钮
        private GGUIWndContinuousClickBtn _m_wContinuousClickBtn;
        //是否忽略正在展示记录，如果有外层mono控制显隐就需要忽略
        private bool _m_bIsIgnoreShowRecord;
#endif

        private void Awake()
        {
#if NP_GAME
            _m_lSerialize = ALSerializeOpMgr.next();
            _m_lMovingItemList = new List<GGUIWndSubMarqueeItem>();

            if (this.monoContinuousClickBtn != null)
            {
                _m_wContinuousClickBtn = new GGUIWndContinuousClickBtn(this.monoContinuousClickBtn);
                _m_wContinuousClickBtn.onNormalClick += _onClickClose;
                _m_wContinuousClickBtn.onSpecialClick += _onContinuousClickClose;
            }
#endif
        }

        private void OnEnable()
        {
#if NP_GAME
            WinMsg.RegisterMsg(WinMsgType.ON_ADD_MARQUEE, _onAddMarquee);
            _m_lSerialize = ALSerializeOpMgr.next();
            _m_bIsDealingTask = false;
            _m_wContinuousClickBtn?.hideWnd();
            ALUGUICommon.setGameObjEnable(this.goHaveMarqueeShowList, false);


            //记录到当前可见跑马灯预制体队列
            if (!_m_bIsIgnoreShowRecord)
                NPPlayer.instance.marqueeComp.recordShowInfo.addMarqueeQueueRecord(showPosIdList, this);

            //是否是在最上层的跑马灯ui，可能有窗口盖在该UI上但该UI没有隐藏，这时不需要显示跑马灯
            bool isTopUI = true;
            for (int i = 0; i < showPosIdList.Count; i++)
            {
                if (isTopUI && !NPPlayer.instance.marqueeComp.recordShowInfo.isTopMarqueeUI(showPosIdList[i], this))
                {
                    isTopUI = false;
                    break;
                }
            }

            //如果是在最上层的跑马灯ui，开始展示跑马灯
            if (isTopUI)
                _tryPopRecordMarquee();
#endif
        }

        private void OnDisable()
        {
#if NP_GAME
            WinMsg.UnregisterMsg(WinMsgType.ON_ADD_MARQUEE, _onAddMarquee);
            pauseAndRecord();

            //如果不忽略，移除当前跑马灯预制体控制的记录
            if(!_m_bIsIgnoreShowRecord)
                NPPlayer.instance.marqueeComp.recordShowInfo.removeMarqueeQueueRecord(showPosIdList, this);
#endif
        }
        private void OnDestroy()
        {
#if NP_GAME
            _m_lSerialize = ALSerializeOpMgr.next();
            _m_bIsDealingTask = false;
            _m_lMovingItemList = null;
            _m_bIsIgnoreShowRecord = false;

            if (_m_wContinuousClickBtn != null)
            {
                _m_wContinuousClickBtn.discard();
                _m_wContinuousClickBtn = null;
            }
#endif
        }

#if NP_GAME

        /// <summary>
        /// 设置忽略记录可见跑马灯预制体
        /// </summary>
        /// <param name="_isIgnore"></param>
        public void setIgnoreShowRecord(bool _isIgnore)
        {
            _m_bIsIgnoreShowRecord = _isIgnore;
        }

        /// <summary>
        /// 继续展示跑马灯
        /// </summary>
        public void resumeShow()
        {
            if (gameObject == null || !gameObject.activeInHierarchy)
                return;

            //如果还在处理任务中，不处理
            if (_m_bIsDealingTask)
                return;

            //先重置状态
            _m_lSerialize = ALSerializeOpMgr.next();
            _m_wContinuousClickBtn?.hideWnd();
            ALUGUICommon.setGameObjEnable(this.goHaveMarqueeShowList, false);

            //展示跑马灯
            _tryPopRecordMarquee();
        }

        /// <summary>
        /// 暂停并记录正在展示的跑马灯位置坐标
        /// </summary>
        public void pauseAndRecord()
        {
            _m_lSerialize = ALSerializeOpMgr.next();
            _m_bIsDealingTask = false;
            ALUGUICommon.setGameObjEnable(this.goHaveMarqueeShowList, false);

            if (_m_lMovingItemList == null)
                return;

            //先移除已记录的位置信息
            for (int i = 0; i < _m_lMovingItemList.Count; i++)
            {
                if (_m_lMovingItemList[i] != null && _m_lMovingItemList[i].marqueeInfo != null)
                    NPPlayer.instance.marqueeComp.recordShowInfo.removeInfo(_m_lMovingItemList[i].marqueeInfo.showPosId);
            }
            //如果还未超出显示范围，缓存相关展示信息
            for (int i = 0; i < _m_lMovingItemList.Count; i++)
            {
                if (!_checkTextIsOutView(_m_lMovingItemList[i]))
                    NPPlayer.instance.marqueeComp.recordShowInfo.addInfo(_m_lMovingItemList[i].marqueeInfo, _m_lMovingItemList[i].leftPosition);
            }

            _clearMovingItemList();
        }

        /// <summary>
        /// 获取下一个跑马灯
        /// </summary>
        /// <returns></returns>
        private MarqueeInfo _getNextMarqueeInfo()
        {
            if (showPosIdList == null)
                return null;

            for (int i = 0; i < showPosIdList.Count; i++)
            {
                MarqueeInfo curMarqueeInfo = NPPlayer.instance.marqueeComp.getMarqueeInfo(showPosIdList[i]);
                if (curMarqueeInfo != null)
                    return curMarqueeInfo;
            }

            return null;
        }

        /// <summary>
        /// 尝试从记录列表展示跑马灯，否则直接展示新的跑马灯
        /// </summary>
        private void _tryPopRecordMarquee()
        {
            if (showPosIdList != null && showPosIdList.Count > 0)
            {
                for (int i = 0; i < showPosIdList.Count; i++)
                {
                    int showPosId = showPosIdList[i];
                    //获取记录的跑马灯列表
                    List<MarqueeRecordShowInfo.MarqueeRecordShowData> infoList = NPPlayer.instance.marqueeComp.recordShowInfo.getInfoList(showPosId);
                    if (infoList != null && infoList.Count > 0)
                    {
                        //直接设置动画到最后一帧
                        if (marqueeShowAni != null)
                            marqueeShowAni.sample(1);

                        //根据记录的跑马灯开始展示
                        for (int j = 0; j < infoList.Count; j++)
                        {
                            Vector2 pos = infoList[j].pos;
                            _tryPopMarquee(infoList[j].info, pos, null);
                        }

                        //展示完，移除该posId全部记录
                        NPPlayer.instance.marqueeComp.recordShowInfo.removeInfo(showPosId);

                        return;
                    }
                }
                
                //新跑马灯数据
                MarqueeInfo info = _getNextMarqueeInfo();
                //播放展示动画
                if(info != null && marqueeShowAni != null)
                    marqueeShowAni.forcePlay();

                _tryPopMarquee(info, Vector2.zero, null);
            }
        }

        /// <summary>
        /// 尝试增加一个跑马灯展示
        /// </summary>
        private void _tryPopMarquee(MarqueeInfo _curMarqueeInfo, Vector2 _customPos, Action _onPopDone)
        {
            Action dealShowDone = () =>
            {
                _m_wContinuousClickBtn?.hideWnd();
                ALUGUICommon.setGameObjEnable(this.goHaveMarqueeShowList, false);
                onShowDone?.Invoke(this.gameObject);
            };

            //获取跑马灯数据
            MarqueeInfo curMarqueeInfo = _curMarqueeInfo;
            if (curMarqueeInfo == null)
            {
                if(_m_lMovingItemList.Count <= 0)
                    dealShowDone();
                _onPopDone?.Invoke();
                return;
            }

            //获取需要展示的ui样式
            long uiResId = curMarqueeInfo.uiResId;
            if (uiResId <= 0)
            {
                dealShowDone();
                _onPopDone?.Invoke();
                return;
            }

            long serialize = _m_lSerialize;
            //获取对应跑马灯item
            GMarqueeItemCacheMgr.instance.popItem(uiResId, this.itemParent, _item =>
            {
                if (_item == null)
                    return;

                if (gameObject == null || !gameObject.activeInHierarchy || serialize != _m_lSerialize)
                {
                    _pushBackMarquee(uiResId, _item);
                    return;
                }

                if (_m_lMovingItemList == null)
                    _m_lMovingItemList = new List<GGUIWndSubMarqueeItem>();

                _item.showWnd();
                _item.setInfo(curMarqueeInfo);
                _m_lMovingItemList.Add(_item);

                //设置开始展示位置在最右
                if (this.transViewportArea != null)
                    _item.leftPosition = Vector2.zero + this.transViewportArea.rect.width * Vector2.right;

                float passTimeSec = 0;
                //如果有自定义位置，设置自定义位置
                if (_customPos != Vector2.zero)
                {
                    //计算该跑马灯全部展示出来后已经经过的时间，用于后面计算下个跑马灯出现的时间
                    passTimeSec = -(_item.width + _customPos.x - _item.leftPosition.x) / this.fMoveSpeed;
                    _item.leftPosition = _customPos;
                }

                //打开移动任务
                ALUGUICommon.setGameObjEnable(this.goHaveMarqueeShowList, true);
                if (!_m_bIsDealingTask)
                {
                    _m_bIsDealingTask = true;
                    long taskSerialize = _m_lSerialize;
                    ALMonoTaskMgr.instance.addMonoTask(new MarqueeMoveTask(_m_lSerialize, this, () =>
                    {
                        if (taskSerialize == _m_lSerialize)
                        {
                            _m_bIsDealingTask = false;
                            dealShowDone();
                        }
                    }, passTimeSec));
                }

                _onPopDone?.Invoke();
            });
        }

        /// <summary>
        /// 回收跑马灯item
        /// </summary>
        /// <param name="_item"></param>
        private void _pushBackMarquee(GGUIWndSubMarqueeItem _item)
        {
            if (_item == null)
                return;

            if (_item.marqueeInfo != null)
                _pushBackMarquee(_item.marqueeInfo.uiResId, _item);
        }

        /// <summary>
        /// 回收跑马灯item
        /// </summary>
        /// <param name="_uiResId"></param>
        /// <param name="_item"></param>
        private void _pushBackMarquee(long _uiResId, GGUIWndSubMarqueeItem _item)
        {
            if (_item == null)
                return;

            if (_item.marqueeInfo != null)
                GMarqueeItemCacheMgr.instance.pushBackCacheItem(_uiResId, _item);

            _m_lMovingItemList?.Remove(_item);
        }

        /// <summary>
        /// 设置跑马灯已读
        /// </summary>
        /// <param name="_item"></param>
        private void _setReadMarque(GGUIWndSubMarqueeItem _item)
        {
            if (gameObject == null || _item == null || _item.marqueeInfo == null)
                return;

            //设置跑马灯已读
            float intervalTimeSec = this.intervalTimeSec > 0 ? this.intervalTimeSec : 1f;
            _item.marqueeInfo.setRead(intervalTimeSec);
            //检查是否还有效，无效则删除跑马灯
            if (!_item.marqueeInfo.checkIsValid())
                NPPlayer.instance.marqueeComp.checkRemoveMarquee(_item.marqueeInfo.showPosId, _item.marqueeInfo.instanceId);
        }

        /// <summary>
        /// 判断文字是否出了显示范围
        /// </summary>
        /// <param name="_item"></param>
        /// <returns></returns>
        private bool _checkTextIsOutView(GGUIWndSubMarqueeItem _item)
        {
            if (_item == null)
                return false;
            // 文字的最右端的X坐标小于0，那么就出范围了
            return (_item.width + _item.leftPosition.x) < 0;
        }

        /// <summary>
        /// 判断文字是否在显示范围内
        /// </summary>
        /// <param name="_item"></param>
        /// <returns></returns>
        private bool _checkTextIsInView(GGUIWndSubMarqueeItem _item)
        {
            if (gameObject == null || this.transViewportArea == null || _item == null)
                return false;

            return (_item.width + _item.leftPosition.x) < this.transViewportArea.rect.width;
        }

        //清空展示列表
        private void _clearMovingItemList()
        {
            if (_m_lMovingItemList != null)
            {
                for (int i = _m_lMovingItemList.Count - 1; i >= 0; i--)
                {
                    _pushBackMarquee(_m_lMovingItemList[i]);
                }
                _m_lMovingItemList.Clear();
            }
        }

        #region 点击事件

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        private void _onClickClose()
        {
            //停止任务，隐藏关闭按钮
            _m_lSerialize = ALSerializeOpMgr.next();
            _m_bIsDealingTask = false;
            _m_wContinuousClickBtn?.hideWnd();

            //对当前展示中的item清除
            if (_m_lMovingItemList != null)
            {
                for (int i = _m_lMovingItemList.Count - 1; i >= 0; i--)
                {
                    GGUIWndSubMarqueeItem item = _m_lMovingItemList[i];
                    if (item == null)
                        continue;

                    //移除数据
                    if (item.marqueeInfo != null)
                        NPPlayer.instance.marqueeComp.forceRemoveMarquee(item.marqueeInfo.showPosId, item.marqueeInfo.instanceId);

                    //回收item
                    _pushBackMarquee(item);
                }
                _m_lMovingItemList.Clear();
            }

            //尝试加载新的跑马灯
            _tryPopMarquee(_getNextMarqueeInfo(), Vector2.zero, null);
        }

        /// <summary>
        /// 一定时间内点击多次关闭按钮
        /// </summary>
        private void _onContinuousClickClose()
        {
            // //弹出确认弹窗
            // NPMesMgr.instance.showTwoBtnMes(
            //     TextTranslate.instance.getLanguage(TransKeyConst.marquee_deleteAllConfirmDesc_none),
            //     TextTranslate.instance.getLanguage(TransKeyConst.cancel),
            //     null,
            //     TextTranslate.instance.getLanguage(TransKeyConst.confirm),
            //     () =>
            //     {
            //         _m_lSerialize = ALSerializeOpMgr.next();
            //         _m_wContinuousClickBtn?.hideWnd();
            //         _clearMovingItemList();
            //         ALUGUICommon.setGameObjEnable(this.goHaveMarqueeShowList, false);
            //         for (int i = 0; i < this.showPosIdList.Count; i++)
            //         {
            //             NPPlayer.instance.marqueeComp.forceRemoveAllMarquee(showPosIdList[i]);
            //         }
            //     });
            _onClickClose();
        }

        #endregion

        #region 消息事件

        /// <summary>
        /// 新增跑马灯事件
        /// </summary>
        /// <param name="_objects"></param>
        private void _onAddMarquee(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _objects[0] == null)
                return;

            int showPosId = (int)_objects[0];

            //如果当前UI不是最上层的跑马灯预制体，则不展示
            if (!NPPlayer.instance.marqueeComp.recordShowInfo.isTopMarqueeUI(showPosId, this))
                return;

            for (int i = 0; i < this.showPosIdList.Count; i++)
            {
                if (showPosId == this.showPosIdList[i] && !_m_bIsDealingTask)
                {
                    //如果是当前位置的跑马灯，尝试展示
                    MarqueeInfo info = _getNextMarqueeInfo();
                    //播放展示动画
                    if (info != null && marqueeShowAni != null)
                        marqueeShowAni.forcePlay();
                    _tryPopMarquee(info, Vector2.zero, null);
                    break;
                }
            }
        }
        #endregion

        #region 跑马灯移动任务

        /// <summary>
        /// 跑马灯移动任务
        /// </summary>
        private class MarqueeMoveTask : _IALBaseMonoTask
        {
            //任务序列号
            private long _m_lTaskSerialize;
            //当前窗口
            private GGUICustomMonoMarquee _m_wInstance;
            //下一次创建跑马灯的时间点
            private float _m_fNextAddMarqueeTime;
            //全部展示完事件
            private Action _m_aOnAllShowDone;
            //是否正在添加新跑马灯
            private bool _m_isAddingItem;
            //有记录位置的跑马灯已经移动经过的时间
            private float _m_fPassTimeSec;

            public MarqueeMoveTask(long _serialize, GGUICustomMonoMarquee _instance, Action _onAllShowDone, float _passTimeSec)
            {
                _m_lTaskSerialize = _serialize;
                _m_wInstance = _instance;
                _m_aOnAllShowDone = _onAllShowDone;
                _m_fNextAddMarqueeTime = 0;
                _m_isAddingItem = false;
                _m_fPassTimeSec = _passTimeSec;
            }

            public void deal()
            {
                if (_m_wInstance == null ||
                    _m_wInstance._m_lSerialize != _m_lTaskSerialize ||
                    _m_wInstance.gameObject == null ||
                    _m_wInstance._m_lMovingItemList == null ||
                    (_m_wInstance._m_lMovingItemList.Count == 0 && _m_wInstance._getNextMarqueeInfo() == null && !_m_isAddingItem))
                {
                    _m_aOnAllShowDone?.Invoke();
                    return;
                }

                //判断最后一个跑马灯是否全部进入视野，计算下一个展示的时间点
                GGUIWndSubMarqueeItem lastItem = _m_wInstance._m_lMovingItemList.GetLast();
                if (lastItem != null && _m_wInstance._checkTextIsInView(lastItem) && _m_fNextAddMarqueeTime == 0)
                {
                    //全部进入视野，跑马灯设置为已读，如果刚进来就已经是全部进入视野，则不再设置已读，因为已经设置过
                    if (_m_fPassTimeSec == 0)
                        _m_wInstance._setReadMarque(lastItem);
                    //展示间隔时间
                    float intervalTimeSec = _m_wInstance.intervalTimeSec > 0 ? _m_wInstance.intervalTimeSec : 1f;
                    //下一次创建跑马灯的时间点
                    _m_fNextAddMarqueeTime = Time.realtimeSinceStartup + intervalTimeSec - _m_fPassTimeSec;
                }
                _m_fPassTimeSec = 0;

                //判断是否需要展示关闭按钮
                if (lastItem != null && lastItem.marqueeInfo != null && _m_wInstance._m_wContinuousClickBtn != null)
                {
                    if (lastItem.marqueeInfo.canDel)
                        _m_wInstance._m_wContinuousClickBtn.showWnd();
                    else
                        _m_wInstance._m_wContinuousClickBtn.hideWnd();
                }

                //下一个创建时间到，创建新的跑马灯，放入移动列表里
                if (_m_fNextAddMarqueeTime > 0 && Time.realtimeSinceStartup - _m_fNextAddMarqueeTime > 0)
                {
                    _m_isAddingItem = true;
                    _m_fNextAddMarqueeTime = 0f;
                    MarqueeInfo nextInfo = _m_wInstance._getNextMarqueeInfo();
                    _m_wInstance._tryPopMarquee(nextInfo, Vector2.zero, () =>
                    {
                        if (_m_wInstance._m_lSerialize == _m_lTaskSerialize)
                            _m_isAddingItem = false;
                    });
                }

                //每一帧判断，移动中的跑马灯，如果移出视野，就删除放回缓存池
                GGUIWndSubMarqueeItem tmpItemWnd = null;
                for (int i = _m_wInstance._m_lMovingItemList.Count - 1; i >= 0; i--)
                {
                    tmpItemWnd = _m_wInstance._m_lMovingItemList[i];
                    Vector2 pos = tmpItemWnd.leftPosition;
                    Vector2 offSet = Vector2.left * _m_wInstance.fMoveSpeed * Time.deltaTime;
                    pos += offSet;
                    tmpItemWnd.leftPosition = pos;

                    //判断文字是否出了显示范围
                    if (_m_wInstance._checkTextIsOutView(tmpItemWnd))
                        _m_wInstance._pushBackMarquee(tmpItemWnd);
                }

                //如果跑马灯移动列表里都跑出了可视视野，并且没有在添加新的跑马灯，跑马灯任务结束
                if ((_m_wInstance._m_lMovingItemList == null || _m_wInstance._m_lMovingItemList.Count == 0) && _m_wInstance._getNextMarqueeInfo() == null && !_m_isAddingItem)
                {
                    _m_aOnAllShowDone?.Invoke();
                    return;
                }

                ALMonoTaskMgr.instance.addNextFrameTask(this);
            }
        }

        #endregion
#endif

    }
}