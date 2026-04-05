using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using static GOE.NPUINoticeMgr.NPUINoticeCacheMgr;

namespace GOE
{
    /// <summary>
    /// Np中的提示类型，注意实际用于比对的使用需要使用二进制处理
    /// 此处最多配置32个枚举类型！
    /// </summary>
    public enum ENoticeType
    {
        NONE,       //无效的提示类型
        NORMAL,     //日常的基础类型提示
        BUILDING,  //主城
        CHAPTER_MAP, //关卡
        SPACE_STATION,//空间站
        ADULT,//成年子嗣
    }

    /// <summary>
    /// 这边直接加好几种notice汇总类型静态数据
    /// 后续dealer不要每次new数组，直接来这边取值不会出错
    /// </summary>
    public static class NPNoticeType
    {
        /**
         * 这边的汇总是给dealer用的，不是给node用的，node只需要标记自己是那个界面就行
         * node不要引用这个类，这个类是给dealer用的，dealer根据自己的需求来选择
         * notice上标记自己在哪些node可以展示
         */
        
        //默认全部地方都展示
        public static ENoticeType[] g_AllTypeArr = { ENoticeType.NORMAL, ENoticeType.BUILDING, ENoticeType.CHAPTER_MAP, ENoticeType.SPACE_STATION, ENoticeType.ADULT };
        //仅在主城展示
        public static ENoticeType[] g_buildingOrRoomTypeArr = { ENoticeType.BUILDING };
        //仅在主城或者成年子嗣展示
        public static ENoticeType[] g_adultOrBuildingOrRoomTypeArr = { ENoticeType.BUILDING, ENoticeType.ADULT };
        //仅在主城或者卧室或者华尔街展示
        public static ENoticeType[] g_buildingRoomWallStreetTypeArr = { ENoticeType.BUILDING, ENoticeType.SPACE_STATION };
        //仅在华尔街展示的
        public static ENoticeType[] g_SpaceStationTypeArr = { ENoticeType.SPACE_STATION };
        //仅在关卡地图展示的
        public static ENoticeType[] g_chapterTypeArr = { ENoticeType.CHAPTER_MAP };
    }
    
    /**********************
     * UI提示信息管理对象
     **/
    public class NPUINoticeMgr
    {
        private static NPUINoticeMgr _g_instance = new NPUINoticeMgr();
        public static NPUINoticeMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPUINoticeMgr();

                return _g_instance;
            }
        }

        //Notice缓存管理对象
        private NPUINoticeCacheMgr _m_ncmNoticeCacheMgr;

        //优先处理的队列
        [NotNull]private List<_ANPUINoticeDealer> _m_lPriorityDealerList;
        private bool _m_bIsPriorityDealing;

#if UNITY_EDITOR
        //编辑器下方便检查问题存储的当前处理对象
        private _ANPUINoticeDealer _m_dCurNoticeDealer;
#endif

        //是否允许处理提示信息，如果不允许在开启的时候会自动开始继续执行
        [NotNull] private CommonOpMaskLogic _m_oCanDealNoticeMask;

        //处理队列
        [NotNull]private List<_ANPUINoticeDealer> _m_lDealerList;
        //是否正在处理普通队列的节点中，注意！这个变量不包含优先级节点
        private bool _m_bIsDealing;
        //是否允许优先节点展示，这个变量只在已经展示的节点上设置。是为了避免在已经展示普通节点的情况下展示了优先节点
        private bool _m_bCanPriorityDealing;

        //处理完成之后的后续处理对象
        private Action _m_dDoneAllNoticeDelegate;

        //允许展示的提示类型二进制比对值
        private int _m_iEnableNoticeBinaryValue;
        //每个类型中允许展示的统计次数，避免多次允许一次取消就导致取消的情况
        private int[] _m_arrEnableNoticeCount;
        private int _m_iEnumCount = ALCommon.getEnumCount(typeof(ENoticeType));
        /// <summary>
        /// 是否有notice正在处理中
        /// </summary>
        public bool isDealing { get { return _m_bIsDealing || _m_bIsPriorityDealing; } }
        /// <summary>
        /// 是否开启了操作遮罩
        /// </summary>
        public bool isOpenOpMasking { get { return _m_oCanDealNoticeMask.isOpMasking; } }
        
        public NPUINoticeMgr()
        {
            _m_ncmNoticeCacheMgr = new NPUINoticeCacheMgr(this);
            _m_lPriorityDealerList = new List<_ANPUINoticeDealer>();
            _m_lDealerList = new List<_ANPUINoticeDealer>();
            _m_oCanDealNoticeMask = new CommonOpMaskLogic();

            _m_dDoneAllNoticeDelegate = null;

            _m_bIsDealing = false;
            _m_bIsPriorityDealing = false;

            _m_iEnableNoticeBinaryValue = 0;
            _m_arrEnableNoticeCount = new int[32];
        }

        public void reset()
        {
            _m_ncmNoticeCacheMgr.reset();

            _m_lPriorityDealerList = new List<_ANPUINoticeDealer>();
            _m_lDealerList = new List<_ANPUINoticeDealer>();
            _m_oCanDealNoticeMask.forceCloseOpMask();

            _m_dDoneAllNoticeDelegate = null;

            _m_bIsDealing = false;
            _m_bIsPriorityDealing = false;

            _m_iEnableNoticeBinaryValue = 0;
        }

        /// <summary>
        /// 设置当前notice完成，正常不会调用，只有在外部要return时候调
        /// </summary>
        /// <param name="_isPriorityDealer"></param>
        protected void _setCurNoticeDone(bool _isPriorityDealer)
        {
            if (_isPriorityDealer)
            {
                _m_bIsPriorityDealing = false;
            }
            else
            {
                _m_bIsDealing = false;
            }
        }
        
        /// <summary>
        /// 将现有数据全部放入缓存处理
        /// </summary>
        /// <returns></returns>
        public long addCache()
        {
            if (null == _m_ncmNoticeCacheMgr)
                return 0;

            return _m_ncmNoticeCacheMgr.pushToCache();
        }

        /// <summary>
        /// 将对应序列号的缓存放回管理器中
        /// </summary>
        /// <param name="_serialize"></param>
        public void restoreCache(long _serialize)
        {
            if (null == _m_ncmNoticeCacheMgr)
                return;

            _m_ncmNoticeCacheMgr.restoreCache(_serialize);
        }

        #region 状态设置
        /// <summary>
        /// 设置无法进行提示处理
        /// </summary>
        public int setNoticeDisable()
        {
            return _m_oCanDealNoticeMask.openOpMask();
        }

        /// <summary>
        /// 设置无法进行提示处理
        /// </summary>
        public void setNoticeEnable(int _serializeId)
        {
            _m_oCanDealNoticeMask.closeOpMask(_serializeId);
            if (_m_oCanDealNoticeMask.isOpMasking)
                return;

            //此时如果不在处理中则尝试开启
            if(!_m_bIsDealing)
                _dealTryPopNoticeNextFrame();
        }

        /// <summary>
        /// 添加有效的展示类型
        /// </summary>
        /// <param name="_type"></param>
        public void addEnableNotice(ENoticeType[] _type)
        {
            if (null == _type)
                return;

            for(int i = 0; i < _type.Length; i++)
            {
                addEnableNotice(_type[i]);
            }
        }
        public void addEnableNotice(ENoticeType _type)
        {
            int idx = (int)_type;

            //左移做或处理，此时不需要判断原来是否有效，都做处理不会错
            _m_iEnableNoticeBinaryValue = _m_iEnableNoticeBinaryValue | (1 << idx);
            //增加次数
            _m_arrEnableNoticeCount[idx] = _m_arrEnableNoticeCount[idx] + 1;
        }
        public void addEnableNotice(int _noticeTypeValue)
        {
            //左移做或处理，此时不需要判断原来是否有效，都做处理不会错
            _m_iEnableNoticeBinaryValue = _m_iEnableNoticeBinaryValue | _noticeTypeValue;
            //增加次数
            for(int i = 0; i < _m_iEnumCount; i++)
            {
                if((_noticeTypeValue & (1 << i)) != 0)
                {
                    _m_arrEnableNoticeCount[i] = _m_arrEnableNoticeCount[i] + 1;
                }
            }
        }
        /// <summary>
        /// 去除对应有效的展示类型
        /// </summary>
        /// <param name="_type"></param>
        public void rmvEnableNotice(ENoticeType[] _type)
        {
            if (null == _type)
                return;

            for (int i = 0; i < _type.Length; i++)
            {
                rmvEnableNotice(_type[i]);
            }
        }
        public void rmvEnableNotice(ENoticeType _type)
        {
            int idx = (int)_type;

            //增加次数
            _m_arrEnableNoticeCount[idx] = _m_arrEnableNoticeCount[idx] - 1;

            //当数量已经到0的时候，需要从比对中去除
            if (_m_arrEnableNoticeCount[idx] == 0)
            {
                _m_iEnableNoticeBinaryValue = _m_iEnableNoticeBinaryValue & ~(1 << idx);
            }

            //判断是否低于0，是则弹错误提示
            if(_m_arrEnableNoticeCount[idx] < 0)
            {
                ALLog.Error($"notice type [{_type}] count < 0!");
                _m_arrEnableNoticeCount[idx] = 0;
            }
        }
        public void rmvEnableNotice(int _noticeTypeValue)
        {
            //增加次数
            for (int i = 0; i < _m_iEnumCount; i++)
            {
                if ((_noticeTypeValue & (1 << i)) != 0)
                {
                    _m_arrEnableNoticeCount[i] = _m_arrEnableNoticeCount[i] - 1;

                    //当数量已经到0的时候，需要从比对中去除
                    if (_m_arrEnableNoticeCount[i] == 0)
                    {
                        _m_iEnableNoticeBinaryValue = _m_iEnableNoticeBinaryValue & ~(1 << i);
                    }

                    //判断是否低于0，是则弹错误提示
                    if (_m_arrEnableNoticeCount[i] < 0)
                    {
                        ALLog.Error($"notice type [{i}] count < 0!");
                        _m_arrEnableNoticeCount[i] = 0;
                    }
                }
            }
        }

        /// <summary>
        /// 带入弹窗提示对应的二进制标识，判断是否允许弹出对应的提示
        /// </summary>
        /// <param name="_noticeValue"></param>
        public bool canShowNotice(ENoticeType _noticeType)
        {
            if ((_m_iEnableNoticeBinaryValue & (1 << (int)_noticeType)) != 0)
                return true;

            return false;
        }
        public bool canShowNotice(ENoticeType[] _noticeTypeArr)
        {
            if (null == _noticeTypeArr)
                return false;

            for(int i = 0; i < _noticeTypeArr.Length; i++)
            {
                if (canShowNotice(_noticeTypeArr[i]))
                    return true;
            }

            return false;
        }
        public bool canShowNotice(int _noticeValue)
        {
            if((_m_iEnableNoticeBinaryValue & _noticeValue) != 0)
                return true;

            return false;
        }
        #endregion

        /// <summary>
        /// 尝试增加新的处理对象 
        /// </summary>
        /// <param name="_dealer"></param>
        public void addDealer(_ANPUINoticeDealer _dealer)
        {
            if(null == _dealer)
                return;

            //stage和ui scene都可看则处理
            if(_dealer.isPriorityDealer)
            {
                _m_lPriorityDealerList.Add(_dealer);
                //尝试弹出提示
                _dealTryPopPriorityNoticeNextFrame();
            }
            else
            {
                _m_lDealerList.Add(_dealer);
                //尝试弹出提示
                _dealTryPopNoticeNextFrame();
            }
        }
        public void addDealerList(List<_ANPUINoticeDealer> _dealerList, bool _addListReverse = false)
        {
            if(null == _dealerList || _dealerList.Count == 0)
                return;

            if (!_addListReverse)
            {
                foreach (var dealer in _dealerList)
                {
                    if(dealer == null)
                        continue;
                
                    if(dealer.isPriorityDealer)
                    {
                        _m_lPriorityDealerList.Add(dealer);
                    }
                    else
                    {
                        _m_lDealerList.Add(dealer);
                    }
                }
            }
            else
            {
                for(int i = _dealerList.Count - 1;i >= 0; i--)
                {
                    _ANPUINoticeDealer dealer = _dealerList[i];
                    if(dealer == null)
                        continue;

                    if (dealer.isPriorityDealer)
                    {
                        _m_lPriorityDealerList.Add(dealer);
                    }
                    else
                    {
                        _m_lDealerList.Add(dealer);
                    }
                }
            }
            
            //尝试弹出提示
            _dealTryPopNoticeNextFrame();
        }
        /// <summary>
        /// 从前面增加新的处理对象
        /// </summary>
        /// <param name="_dealerList"></param>
        public void addDealerListFront(List<_ANPUINoticeDealer> _dealerList, bool _addListReverse = false)
        {
            if(null == _dealerList || _dealerList.Count == 0)
                return;

            _ANPUINoticeDealer dealer = null;
            if (_addListReverse)
            {
                // 因为是从前面插入, 使用Insert每次插入第一个元素, 所以这边正序遍历的情况插入后就是反转顺序了
                for(int i = 0, count = _dealerList.Count;i < count; i++)
                {
                    dealer = _dealerList[i];
                    if(dealer == null)
                        continue;

                    if (dealer.isPriorityDealer)
                    {
                        _m_lPriorityDealerList.Insert(0, dealer);
                    }
                    else
                    {
                        _m_lDealerList.Insert(0, dealer);
                    }
                }
            }
            else
            {
                // 因为是从前面插入, 使用Insert每次插入第一个元素, 所以这边逆序遍历的情况插入后就是正序了
                for(int i = _dealerList.Count - 1;i >= 0; i--)
                {
                    dealer = _dealerList[i];
                    if(dealer == null)
                        continue;

                    if (dealer.isPriorityDealer)
                    {
                        _m_lPriorityDealerList.Insert(0, dealer);
                    }
                    else
                    {
                        _m_lDealerList.Insert(0, dealer);
                    }
                }
            }
            
            //尝试弹出提示
            _dealTryPopNoticeNextFrame();
        }
        /// <summary>
        /// 尝试增加新的处理对象 
        /// </summary>
        /// <param name="_dealer"></param>
        protected internal void _reAddDealer(_ANPUINoticeDealer _dealer)
        {
            if (null == _dealer)
                return;

            //放入队列后不触发展示，本函数执行位置是内部位置，后续会有单独的检测和处理
            if (_dealer.isPriorityDealer)
            {
                _m_lPriorityDealerList.Insert(0, _dealer);
            }
            else
            {
                _m_lDealerList.Insert(0, _dealer);
            }
        }

        /// <summary>
        /// 在处理过程中进行的中间处理，此时需要进行判断处理
        /// </summary>
        /// <param name="_doneDelegate"></param>
        public void dealTryPopNotice(Action _doneDelegate = null)
        {
            //引导中不弹出，直接结束
            if (Game.instance.isInTutorial)
            {
                if (null != _doneDelegate)
                    _doneDelegate();
                return;
            }


            //判断是否在处理过程，如在则不进行处理
            if (_m_bIsDealing || _m_bIsPriorityDealing)
            {
                addDoneDelegate(_doneDelegate);
                return;
            }

            //调用直接弹出的处理
            directDealTryPopNotice(_doneDelegate);
        }
        /// <summary>
        /// 直接弹出notice的处理，这个函数不受其他状态的控制和判断，直接进行弹出处理
        /// </summary>
        /// <param name="_doneDelegate"></param>
        public void directDealTryPopNotice(Action _doneDelegate = null)
        {
            addDoneDelegate(_doneDelegate);

            //优先弹出优先提示，如果成功弹出优先节点则不做后续处理
            if (__popNextPriorityNotice())
                return;

            //尝试弹出非优先队列
            __popNextNotice();

            //检查是否全部完成
            _checkDone();
        }

        /// <summary>
        /// 检查是否完成所有处理，是则处理后续函数
        /// </summary>
        /// <param name="_idleAction"></param>
        public void checkDone(Action _idleAction)
        {
            if (!_m_bIsDealing && !_m_bIsPriorityDealing)
            {
                //如果无需要处理的则处理完成
                if (null != _idleAction)
                    _idleAction();
                _idleAction = null;
            }
        }

        /// <summary>
        /// 根据实例从队列中删除
        /// </summary>
        public void removeDealer(_ANPUINoticeDealer _dealer)
        {
            if(null == _dealer)
                return;
            
            for(int i = _m_lDealerList.Count - 1; i >= 0; i--)
            {
                if(_m_lDealerList[i] == _dealer)
                {
                    _m_lDealerList.RemoveAt(i);
                }
            }

            for(int i = _m_lPriorityDealerList.Count - 1; i >= 0; i--)
            {
                if(_m_lPriorityDealerList[i] == _dealer)
                {
                    _m_lPriorityDealerList.RemoveAt(i);
                }
            }
        }

        public void removeDealer(Predicate<_ANPUINoticeDealer> _match)
        {
            if(_match == null)
                return;
            
            for(int i = _m_lDealerList.Count - 1; i >= 0; i--)
            {
                if(_match(_m_lDealerList[i]))
                {
                    _m_lDealerList.RemoveAt(i);
                }
            }

            for(int i = _m_lPriorityDealerList.Count - 1; i >= 0; i--)
            {
                if(_match(_m_lPriorityDealerList[i]))
                {
                    _m_lPriorityDealerList.RemoveAt(i);
                }
            }
        }
        
        /// <summary>
        /// 根据类型从队列中删除
        /// </summary>
        public void removeDealerByType(Type _type)
        {
            if(null == _type)
                return;
            
            _ANPUINoticeDealer tmpDealer = null;
            for(int i = _m_lDealerList.Count - 1; i >= 0; i--)
            {
                tmpDealer = _m_lDealerList[i];
                if(null != tmpDealer && tmpDealer.GetType() == _type)
                {
                    _m_lDealerList.RemoveAt(i);
                }
            }

            for(int i = _m_lPriorityDealerList.Count - 1; i >= 0; i--)
            {
                tmpDealer = _m_lPriorityDealerList[i];
                if(null != tmpDealer && tmpDealer.GetType() == _type)
                {
                    _m_lPriorityDealerList.RemoveAt(i);
                }
            }
        }
        
        /// <summary>
        /// 根据字符串从队列中删除
        /// </summary>
        public void removeDealerByTag(string _strTag)
        {
            if(string.IsNullOrEmpty(_strTag))
                return;
            
            _ANPUINoticeDealer tmpDealer = null;
            for(int i = _m_lDealerList.Count - 1; i >= 0; i--)
            {
                tmpDealer = _m_lDealerList[i];
                if(null == tmpDealer || string.IsNullOrEmpty(tmpDealer.noticeTag))
                    continue;
                
                if(tmpDealer.noticeTag == _strTag)
                {
                    _m_lDealerList.RemoveAt(i);
                }
            }

            for(int i = _m_lPriorityDealerList.Count - 1; i >= 0; i--)
            {
                tmpDealer = _m_lPriorityDealerList[i];
                if(null == tmpDealer || string.IsNullOrEmpty(tmpDealer.noticeTag))
                    continue;
                
                if(tmpDealer.noticeTag == _strTag)
                {
                    _m_lPriorityDealerList.RemoveAt(i);
                }
            }
        }
        
        /*****************
         * 设置结束的处理
         **/
        public void addDoneDelegate(Action _delegate)
        {
            if (null == _delegate)
                return;

            if (null == _m_dDoneAllNoticeDelegate)
                _m_dDoneAllNoticeDelegate = _delegate;
            else
                _m_dDoneAllNoticeDelegate += _delegate;
        }

        //添加了处理对象后的尝试弹出提示处理
        protected void _dealTryPopPriorityNoticeNextFrame()
        {
            //下一帧才处理展示
            ALCommonActionMonoTask.addNextFrameTask(() => { _tryPopPriorityNotice(); }); //下一帧显示
        }
        protected void _dealTryPopNoticeNextFrame()
        {
            //下一帧才处理展示
            ALCommonActionMonoTask.addNextFrameTask(() => { _tryPopNotice(); }); //下一帧显示
        }

        /** 弹出提示信息 */
        protected void _tryPopPriorityNotice()
        {
            if (Game.instance.isInTutorial)
                return;

            if (__tryDealPopPriorityNotice())
            {
                //检查是否全部完成
                _checkDone();
            }
        }
        protected void _tryPopNotice()
        {
            if (Game.instance.isInTutorial)
                return;

            //优先先弹出优先提示
            if (__tryDealPopPriorityNotice())
                return;

            //判断是否在处理过程，如在则不进行处理
            if (_m_bIsDealing || _m_bIsPriorityDealing)
                return;

            //弹出提示
            __popNextNotice();

            //检查是否全部完成
            _checkDone();
        }
        /// <summary>
        /// 尝试处理弹出优先提示，返回是否有进行实际处理
        /// </summary>
        private bool __tryDealPopPriorityNotice()
        {
            //判断是否在处理过程，如在则不进行处理
            if (_m_bIsPriorityDealing)
                return false;

            //如果在执行非优先队列，且不可插入优先处理的时候不做处理
            if (_m_bIsDealing && !_m_bCanPriorityDealing)
                return false;

            //弹出提示
            return __popNextPriorityNotice();
        }

        /** 尝试弹出下一个对应提示 */

        /// <summary>
        /// 在优先节点完成的时候调用的展示下一个节点的处理
        /// </summary>
        protected void _onPriorityNoticeDone()
        {
            //优先弹出优先提示
            if (__popNextPriorityNotice())
                return;

            //在非优先并没有处理的时候，尝试弹出非优先队列
            if (!_m_bIsDealing)
                __popNextNotice();

            //检查是否全部完成
            _checkDone();
        }
        /// <summary>
        /// 在非优先节点结束的时候调用的处理下一个函数
        /// 会重置在非优先级节点情况下的优先级节点是否有效的状态
        /// 同时根据是否在处理优先级节点判断是否需要展示优先级节点
        /// </summary>
        protected void _onNoPriorityNoticeDone()
        {
            //允许优先插入
            _m_bCanPriorityDealing = true;
            //如果此时在进行优先节点的处理，则重置状态
            if (_m_bIsPriorityDealing)
            {
                //如果当前有优先节点在处理，需要重置不同节点处理状态
                _m_bIsDealing = false;
                return;
            }

            //先优先弹出优先提示
            if (__popNextPriorityNotice())
            {
                //如果当前优先节点处理成功，需要重置不同节点处理状态
                _m_bIsDealing = false;
                return;
            }

            //尝试弹出非优先队列
            __popNextNotice();

            //检查是否全部完成
            _checkDone();
        }

        /** 弹出提示信息 */
        private bool __popNextPriorityNotice()
        {
            //如果不允许触发notice则直接返回
            if (_m_oCanDealNoticeMask.isOpMasking)
            {
                //直接当做没有提示返回
                //设置不在处理中
                _m_bIsPriorityDealing = false;

                return false;
            }

            _ANPUINoticeDealer dealer = null;

            for(int i = 0; i < _m_lPriorityDealerList.Count; )
            {
                dealer = _m_lPriorityDealerList[i];

                //如果数据无效则删除
                if (null == dealer || dealer.isDone)
                {
                    ALLog.Error($"Notice: {dealer} in list is done!");
                    _m_lPriorityDealerList.RemoveAt(i);
                    dealer = null;
                    continue;
                }
                
                //如果数据无效则删除
                if (!dealer.isEnable)
                {
                    _m_lPriorityDealerList.RemoveAt(i);
                    dealer = null;
                    continue;
                }

                //找到不为空，且真正被允许在此时弹出的窗口才跳出
                if (dealer != null && dealer.canCurShow
                    && (canShowNotice(dealer.noticeType) || canShowNotice(dealer.noticeTypeSingle)))
                {
                    _m_lPriorityDealerList.RemoveAt(i);
                    break;
                }

                //全部轮完都没有选到，dealer置空，否则会选最后一个
                dealer = null;

                //累加下标
                i++;
            }

            //进行处理
            if(null != dealer)
            {
                //设置在处理中
                _m_bIsPriorityDealing = true;
#if UNITY_EDITOR
                //编辑器下方便检查问题存储的当前处理对象
                _m_dCurNoticeDealer = dealer;
#endif
                dealer.showNotice();

                return true;
            }
            else
            {
                //设置不在处理中
                _m_bIsPriorityDealing = false;

                return false;
            }
        }
        /** 弹出提示信息 */
        private bool __popNextNotice()
        {
            //如果不允许触发notice则直接返回
            if(_m_oCanDealNoticeMask.isOpMasking)
            {
                //直接当做没有提示返回
                //设置不在处理中
                _m_bIsDealing = false;
                _m_bCanPriorityDealing = true;

                return false;
            }

            _ANPUINoticeDealer dealer = null;

            for(int i = 0; i < _m_lDealerList.Count; )
            {
                dealer = _m_lDealerList[i];

                //如果数据无效则删除
                if (null == dealer || dealer.isDone)
                {
                    ALLog.Error($"Notice: {dealer} in list is done!");
                    _m_lDealerList.RemoveAt(i);
                    dealer = null;
                    continue;
                }

                //如果数据无效则删除
                if (!dealer.isEnable)
                {
                    _m_lDealerList.RemoveAt(i);
                    dealer = null;
                    continue;
                }

                //找到不为空，且真正被允许在此时弹出的窗口才跳出
                if (dealer != null && dealer.canCurShow
                    && (canShowNotice(dealer.noticeType) || canShowNotice(dealer.noticeTypeSingle)))
                {
                    _m_lDealerList.RemoveAt(i);
                    break;
                }

                //全部轮完都没有选到，dealer置空，否则会选最后一个
                dealer = null;

                //累加下标
                i++;
            }

            //进行处理
            if(null != dealer)
            {
                //设置在处理中
                _m_bIsDealing = true;
                _m_bCanPriorityDealing = dealer.canPlayPriority;
#if UNITY_EDITOR
                //编辑器下方便检查问题存储的当前处理对象
                _m_dCurNoticeDealer = dealer;
#endif

                dealer.showNotice();

                return true;
            }
            else
            {
                //设置不在处理中
                _m_bIsDealing = false;
                _m_bCanPriorityDealing = true;

                return false;
            }
        }

        /** 检查是否完成所有处理 */
        private void _checkDone()
        {
            if(!_m_bIsDealing && !_m_bIsPriorityDealing)
            {
                //如果无需要处理的则处理完成
                Action doneDelegate = _m_dDoneAllNoticeDelegate;
                _m_dDoneAllNoticeDelegate = null;
                if(null != doneDelegate)
                    doneDelegate();
            }
        }
        
        /// <summary>
        /// 游戏中UI提示信息处理对象
        /// </summary>
        public abstract class _ANPUINoticeDealer
        {
            //判断notice状态是否变更
            private long _m_lNoticeDealSerialize;

            //实际的处理Node
            private NPGAddQueueNoticeDealerNode _m_nDealNode;

            //是否已经完结
            private bool _m_bIsDone = false;

            public long noticeDealSerialize { get { return _m_lNoticeDealSerialize; } }

            //返回是否已完成
            public bool isDone { get { return _m_bIsDone; } }

            //是否在这个notice中可以回退
            public virtual bool noticeCanDoESC { get { return true; } }
            
            // 这个 node 是否是全屏
            public virtual bool isNoticeFullScreen { get { return false; } } 
            
            //这个 node是否需要自动删除
            public virtual bool needAutoRemove { get { return false; } } 

            // 这个 node 是否是是纯ui的
            public virtual bool isOnlyUINode { get { return false; } }

            /// <summary>
            ///  设置本处理过程完结
            /// </summary>
            public void setDealerDone()
            {
                setDealerDone(0);
            }
            public void setDealerDone(long _dealSerialize)
            {
                if (_m_bIsDone)
                    return;

                //判断序列号有效且是否一致，避免错误处理
                if (_dealSerialize > 0 && _dealSerialize != _m_lNoticeDealSerialize)
                    return;

                //设置已完成
                _m_bIsDone = true;

                //刷新显示序列号，表示问题处理完毕
                _m_lNoticeDealSerialize = ALSerializeOpMgr.next();

                //触发函数
                _onDealerDone();

                //关闭节点
                if (null != _m_nDealNode)
                {
                    QueueMgr.instance.forceCloseNode(_m_nDealNode);
                    _m_nDealNode = null;
                }

                // 若是在引导中，则不处理下一个
                if (Game.instance.isInTutorial)
                {
                    NPUINoticeMgr.instance._setCurNoticeDone(isPriorityDealer);;
                    return;
                }
                
                //处理下一个
                if (isPriorityDealer)
                    NPUINoticeMgr.instance._onPriorityNoticeDone();
                else
                    NPUINoticeMgr.instance._onNoPriorityNoticeDone();
            }

            /// <summary>
            /// 重置Notice状态，本窗口会关闭，同时不会完成Notice，并根据优先级放回对应队列第一个
            /// </summary>
            public void resetDealer()
            {
                if (_m_bIsDone)
                    return;

                //刷新显示序列号，避免Node关闭的时候设置本对象完成
                _m_lNoticeDealSerialize = ALSerializeOpMgr.next();

                //将本对象还原到队列首位
                NPUINoticeMgr.instance._reAddDealer(this);

                //关闭节点
                if (null != _m_nDealNode)
                {
                    QueueMgr.instance.forceCloseNode(_m_nDealNode);
                    _m_nDealNode = null;
                }

                // 若是在引导中，则不处理下一个
                if (Game.instance.isInTutorial)
                    return;

                //尝试处理下一个
                if (isPriorityDealer)
                    NPUINoticeMgr.instance._onPriorityNoticeDone();
                else
                    NPUINoticeMgr.instance._onNoPriorityNoticeDone();
            }

            //展示本节点的提示信息
            public virtual void showNotice()
            {
                //刷新显示序列号，表示问题处理完毕
                _m_lNoticeDealSerialize = ALSerializeOpMgr.next();

                //关闭原先的node
                if (null != _m_nDealNode)
                {
                    QueueMgr.instance.forceCloseNode(_m_nDealNode);
                    _m_nDealNode = null;
                }
                //开启一个新node
                _m_nDealNode = new NPGAddQueueNoticeDealerNode(this);

                //进入处理Node
                QueueMgr.instance.AddNode(_m_nDealNode);
            }

            /// <summary>
            /// 点击遮罩的回调，默认关闭自己的node
            /// </summary>
            public virtual void clickBkAction()
            {
                QueueMgr.instance.forceCloseNode(_m_nDealNode);
            }

            /// <summary>
            /// 不允许esc回退时调用
            /// </summary>
            public virtual void onCannotEscBack()
            {
            }
            
            /// <summary>
            /// 本提示对应的提示类型
            /// 默认所有界面都允许弹出
            /// 提示类型允许存在多个提示类型的特性，以更好的适应在不同位置弹出的需求
            /// </summary>
            public virtual ENoticeType[] noticeType { get { return NPNoticeType.g_AllTypeArr; } }

            /// <summary>
            /// 本提示对应的提示类型, 支持单一类型的快速访问, 避免只有一个类型时还要到NPNoticeType新增数组
            /// 与noticeType结合使用'或'判断, 只有一个满足即可, 因为noticeType默认为全部类型, 所有noticeTypeSingle就默认为无效类型
            /// </summary>
            public virtual ENoticeType noticeTypeSingle{ get { return ENoticeType.NONE; } }
            
            /// <summary>
            /// 是否需要蒙版遮罩
            /// </summary>
            public virtual bool needTransBk { get { return false; } }
            /// <summary>
            /// 是否还有效，如果无效会pop时候尝试删除
            /// </summary>
            public virtual bool isEnable { get { return true; } }
            /// <summary>
            /// 需要蒙版遮罩时关联的窗口控制，可以避免打开其他带蒙版窗口再返回时蒙版遮罩里有窗口的影子
            /// </summary>
            public virtual _AALBasicLoadUIWndBasicClass uiObj { get { return null; } }
            
            /// <summary>
            /// 字符串标记，可以用来根据tag删除notice
            /// </summary>
            public virtual string noticeTag { get { return string.Empty; } }
            /// <summary>
            /// 字符串标记，可以用来根据tag开区别node的tag
            /// </summary>
            public virtual string nodeTag { get { return string.Empty; } }
            
            //是否优先处理的对象
            public abstract bool isPriorityDealer { get; }
            public abstract bool canPlayPriority { get; }
            //是否现在可以从队列中提出来展示
            public abstract bool canCurShow { get; }
            
            //完结的触发函数
            protected abstract void _onDealerDone();
            //展示本节点的提示信息
            public abstract void dealShowNotice();
            //隐藏提示
            public abstract void dealHideNotice();
        }


        /// <summary>
        /// Notice缓存机制管理器
        /// </summary>
        public class NPUINoticeCacheMgr
        {
            /// <summary>
            /// 缓存存储的单元对象
            /// </summary>
            public class NPUINoticeCacheInfo
            {
                protected long _m_lSerialize;
                //优先处理的队列
                [NotNull] protected List<NPUINoticeMgr._ANPUINoticeDealer> _m_lPriorityDealerList;
                //处理队列
                [NotNull] protected List<NPUINoticeMgr._ANPUINoticeDealer> _m_lDealerList;

                public NPUINoticeCacheInfo(long _serialize, List<NPUINoticeMgr._ANPUINoticeDealer> _priorityList, List<NPUINoticeMgr._ANPUINoticeDealer> _dealerList)
                {
                    _m_lSerialize = _serialize;
                    _m_lPriorityDealerList = new List<NPUINoticeMgr._ANPUINoticeDealer>(_priorityList);
                    _m_lDealerList = new List<NPUINoticeMgr._ANPUINoticeDealer>(_dealerList);
                }

                public long serialize { get { return _m_lSerialize; } }
                public List<NPUINoticeMgr._ANPUINoticeDealer> priorityDealerList { get { return _m_lPriorityDealerList; } }
                public List<NPUINoticeMgr._ANPUINoticeDealer> dealersList { get { return _m_lDealerList; } }
            }


            //管理器
            private NPUINoticeMgr _m_nmNoticeMgr;
            //缓存队列
            private List<NPUINoticeCacheInfo> _m_lCacheList;

            public NPUINoticeCacheMgr(NPUINoticeMgr _noticeMgr)
            {
                _m_nmNoticeMgr = _noticeMgr;
                _m_lCacheList = new List<NPUINoticeCacheInfo>();
            }

            /// <summary>
            /// 将现有数据全部放入缓存处理
            /// </summary>
            /// <returns></returns>
            public long pushToCache()
            {
                if (null == _m_nmNoticeMgr)
                    return 0;

                long serialize = ALSerializeOpMgr.next();
                //将两个队列放入缓存
                NPUINoticeCacheInfo cacheInfo = new NPUINoticeCacheInfo(serialize, _m_nmNoticeMgr._m_lPriorityDealerList, _m_nmNoticeMgr._m_lDealerList);
                _m_lCacheList.Add(cacheInfo);

                //清空队列
                _m_nmNoticeMgr._m_lPriorityDealerList.Clear();
                _m_nmNoticeMgr._m_lDealerList.Clear();

                return serialize;
            }

            /// <summary>
            /// 将对应序列号的缓存放回管理器中
            /// </summary>
            /// <param name="_serialize"></param>
            public void restoreCache(long _serialize)
            {
                if (null == _m_nmNoticeMgr)
                    return;

                NPUINoticeCacheInfo cacheInfo = null;
                for (int i = _m_lCacheList.Count - 1; i >= 0; i--)
                {
                    cacheInfo = _m_lCacheList[i];
                    if (cacheInfo.serialize == _serialize)
                    {
                        //删除缓存
                        _m_lCacheList.RemoveAt(i);

                        //恢复数据
                        if (null != cacheInfo.priorityDealerList)
                            _m_nmNoticeMgr._m_lPriorityDealerList.AddRange(cacheInfo.priorityDealerList);
                        if (null != cacheInfo.dealersList)
                            _m_nmNoticeMgr._m_lDealerList.AddRange(cacheInfo.dealersList);

                        //尝试弹出notice
                        _m_nmNoticeMgr._dealTryPopNoticeNextFrame();

                        return;
                    }
                }

#if UNITY_EDITOR
                ALLog.Error($"restoreCache【{_serialize}】 has no info!");
#endif
            }

            /// <summary>
            /// 重置数据
            /// </summary>
            public void reset()
            {
                _m_lCacheList.Clear();
            }
        }
    }
}
