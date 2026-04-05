using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 妃子列表
    /// </summary>
    public class GGUIWndConsortListGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoConsortListGridItem, GGUIMonoConsortListGrid, GGUIWndConsortListGridItem>, _IScrollerSmoothMovable
    {
        //已解锁的妃子卡牌列表
        [NotNull] private List<GGottenConsortInfo> _m_lUnlockConsortInfoList = new List<GGottenConsortInfo>();
        // 未解锁的妃子卡牌列表
        [NotNull] private List<ConsortRefShowInfo> _m_lLockConsortInfoList = new List<ConsortRefShowInfo>();

        private long _m_lInviteShowSerializeId;//邀约表现序列化id
        private long _m_lScrollMoveSerialize;//滚动序列化id
        
        /// <summary>
        /// 被邀约的妃子id
        /// </summary>
        private long _m_lByInviteConsortId = 0;
        
        // 已解锁妃子bar
        private GGUIWndConsortListBarController _m_UnlockBarController;
        // 未解锁妃子bar
        private GGUIWndConsortListBarController _m_LockBarController;

        //是否需要刷新bar
        private bool _m_bNeedRefreshBar;

        private int _m_iSerialize;
        
        public event Action<GGUIWndConsortListGridItem> clickDelegate;
        public event Action<float> onVerticalNormalizedPositionChg; 

        public GGUIWndConsortListGrid(GGUIMonoConsortListGrid _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onDiscard()
        {
            if (null != _m_UnlockBarController)
            {
                removeBar(_m_UnlockBarController);
                _m_UnlockBarController.discard();
                _m_UnlockBarController = null;
            }
            if (null != _m_LockBarController)
            {
                removeBar(_m_LockBarController);
                _m_LockBarController.discard();
                _m_LockBarController = null;
            }

            _m_lUnlockConsortInfoList.Clear();
            _m_lLockConsortInfoList.Clear();

            _m_lByInviteConsortId = 0;
            
            clickDelegate = null;
            onVerticalNormalizedPositionChg = null;

            _m_bNeedRefreshBar = true;
        }

        protected override void _onHideWnd()
        {
            if(wnd != null && wnd.scrollRect != null)
                wnd.scrollRect.onValueChanged.RemoveListener(_onScrollNormalizedPositionChg);
            
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_CONSORT, _onSimulateClickConsort);

            _m_lInviteShowSerializeId = ALSerializeOpMgr.next();
            _m_lByInviteConsortId = 0;

            _m_iSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            _m_lByInviteConsortId = 0;
        }

        protected override void _onShowWnd()
        {
            // 放到show监听是因为_ATNPGGUIWndShowAnimGrid底层将scrollRect的所有onValueChanged事件都移除了
            if(wnd != null && wnd.scrollRect != null)
                wnd.scrollRect.onValueChanged.AddListener(_onScrollNormalizedPositionChg);

            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_CONSORT, _onSimulateClickConsort);
        }

        protected override void _onWndInitDone()
        {
            // 初始化容器
            setItemCount(0);

            _m_bNeedRefreshBar = true;
        }

        // 创建对象
        protected override GGUIWndConsortListGridItem _createItemWnd(GGUIMonoConsortListGridItem _itemMono)
        {
            // 创建对象
            GGUIWndConsortListGridItem gridItem = new GGUIWndConsortListGridItem(_itemMono);
            // 注册物品点击事件
            gridItem.clickDelegate += _onClickCardItem;
            return gridItem;
        }

        // 刷新对象
        protected override void _onRefreshItemWnd(GGUIWndConsortListGridItem _itemMono, int _itemIdx)
        {
            if (_itemMono == null || _m_lUnlockConsortInfoList.Count + _m_lLockConsortInfoList.Count <= _itemIdx)
                return;
    
            _IConsortShowInfo consortShowInfo = null;
            if (_m_lUnlockConsortInfoList.Count > _itemIdx)
                consortShowInfo = _m_lUnlockConsortInfoList[_itemIdx];
            else
            {
                _itemIdx -= _m_lUnlockConsortInfoList.Count;
                consortShowInfo = _m_lLockConsortInfoList[_itemIdx];
            }
            
            _itemMono.setData(consortShowInfo);
            _itemMono.setByInvite(consortShowInfo != null && consortShowInfo.consortId == _m_lByInviteConsortId);
            if (wnd != null)
            {
                if (_itemIdx % 2 == 0)
                {
                    _itemMono.setContentPositionOffset(wnd.evenIdxItemOffset);
                }
                else
                {
                    _itemMono.setContentPositionOffset(wnd.oddIdxItemOffset);
                }
            }
        }

        /// <summary>
        /// 初始化妃子列表
        /// </summary>
        /// <param name="_unlockConsortShowInfoList">已解锁妃子信息列表</param>
        /// <param name="_lockConsortShowInfoList">未解锁妃子信息列表</param>
        /// <param name="_needShowAni"></param>
        public void showConsortList(List<GGottenConsortInfo> _unlockConsortShowInfoList, List<ConsortRefShowInfo> _lockConsortShowInfoList, float _verticalNormalizedPosition, bool _needShowAni = false)
        {
            if (null == _unlockConsortShowInfoList && _lockConsortShowInfoList == null)
                return;

            //获取的列表已经是排序过的
            _m_lUnlockConsortInfoList.Clear();
            if(_unlockConsortShowInfoList != null)
                _m_lUnlockConsortInfoList.AddRange(_unlockConsortShowInfoList);

            _m_lLockConsortInfoList.Clear();
            if(_lockConsortShowInfoList != null)
                _m_lLockConsortInfoList.AddRange(_lockConsortShowInfoList);
            
            _m_bNeedRefreshBar = true;
            
            //刷新卡牌
            _refreshCardList();

            //调用showWnd显示子窗体动画
            if (_needShowAni)
                showWnd();

            scrollMoveToVerticalRate(_verticalNormalizedPosition);
        }

        // 刷新卡牌列表
        private void _refreshCardList()
        {
            //空物品提示
            ALUGUICommon.setUIObjScale(wnd.noneItemsTips, _m_lUnlockConsortInfoList.Count + _m_lLockConsortInfoList.Count <= 0 ? 1 : 0);

            //刷新grid
            setItemCount(_m_lUnlockConsortInfoList.Count + _m_lLockConsortInfoList.Count);

            //刷新bar
            if(_m_bNeedRefreshBar)
                _refreshBar();
        }

        private void _refreshBar()
        {
            if (_m_bNeedRefreshBar)
            {
                _m_bNeedRefreshBar = false;
                
                _showUnlockBar();
                _showLockBar();
                
                forceRefreshBar();
            }
        }
        
        private void _showUnlockBar()
        {
            // 已解锁妃子bar一定在下标为0位置
            int barIndex = 0;

            //添加bar
            if (_m_UnlockBarController == null)
            {
                _m_UnlockBarController = new GGUIWndConsortListBarController(wnd.gridAreaUIObj, wnd.unlockConsortBarAssetPathInfo);
                addBar(_m_UnlockBarController);
            }
            _m_UnlockBarController.regLoadDoneDelegate(() =>
            {
                _m_UnlockBarController.setInsertIndex(barIndex);
            });
        }
        
        //刷新bar
        private void _showLockBar()
        {
            //查找未解锁的妃子位置为bar的插入位置
            int barIndex = _m_lLockConsortInfoList.Count <= 0 ? -1 : _m_lUnlockConsortInfoList.Count;
            if (barIndex == -1)
            {
                if (null != _m_LockBarController)
                {
                    removeBar(_m_LockBarController);
                    _m_LockBarController.discard();
                    _m_LockBarController = null;
                }
                return;
            }

            //添加bar
            if (_m_LockBarController == null)
            {
                _m_LockBarController = new GGUIWndConsortListBarController(wnd.gridAreaUIObj, wnd.lockConsortBarAssetPathInfo);
                addBar(_m_LockBarController);
            }
            _m_LockBarController.regLoadDoneDelegate(() =>
            {
                _m_LockBarController.setInsertIndex(barIndex);
            });
                
        }

        //移动到顶部
        public void scrollMoveToTop()
        {
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                moveToTop();
            });
        }

        public void scrollMoveToVerticalRate(float _rate)
        {
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                moveToVerticalRate(_rate);
            });
        }
        
        //点击卡牌
        private void _onClickCardItem(GGUIWndConsortListGridItem _itemWnd)
        {
            if (null == _itemWnd)
                return;

            if (null != clickDelegate)
                clickDelegate(_itemWnd);
        }

        //滚动到指定位置
        public void scrollMoveToTarget(float _pos)
        {
            //到管理对象中进行处理
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                moveToVerticalRate(_pos);
            });
        }

        //获取当前位置
        public float getScrollCurPos()
        {
            if (wnd == null || wnd.scrollRect == null)
                return 0;
            return 1 - wnd.scrollRect.verticalNormalizedPosition;
        }

        /// <summary>
        /// 获取妃子下标
        /// </summary>
        /// <returns></returns>
        public int getConsortIndex(long _consortId)
        {
            _IConsortShowInfo consortShowInfo = null;
            for (int i = 0; i < _m_lUnlockConsortInfoList.Count; i++)
            {
                consortShowInfo = _m_lUnlockConsortInfoList[i];
                if (consortShowInfo != null && consortShowInfo.consortId == _consortId)
                    return i;
            }

            int unlockConsortCount = _m_lUnlockConsortInfoList.Count;
            for (int i = 0; i < _m_lLockConsortInfoList.Count; i++)
            {
                consortShowInfo = _m_lLockConsortInfoList[i];
                if(consortShowInfo != null && consortShowInfo.consortId == _consortId)
                    return i + unlockConsortCount;
            }

            return -1;
        }

        private void _onScrollNormalizedPositionChg(Vector2 _)
        {
            onVerticalNormalizedPositionChg?.Invoke(getScrollCurPos());
        }
        
        #region 邀约

        /// <summary>
        /// 设置被邀约妃子id
        /// </summary>
        /// <param name="_consortId"></param>
        public void setByInviteConsortId(long _consortId, bool _needMoveToConsort, Action _comlete = null)
        {
            if (_needMoveToConsort && wnd != null)
            {
                long serializeId = _m_lInviteShowSerializeId = ALSerializeOpMgr.next();
                
                // 获取妃子所在位置
                Vector2 consortPos = getItemPos(getConsortIndex(_consortId));

                //计算可移动区域高度
                float canMoveHeight = wnd.gridAreaMaskObj == null ? allHeight : allHeight - wnd.gridAreaMaskObj.rect.height;

                float tmpHeight = canMoveHeight - (wnd.byInviteConsortItemMoveToPosY - consortPos.y);
                float verticalRate = Mathf.Clamp(tmpHeight / canMoveHeight, 0f, 1f);
                
                new ScrollerSmoothMoveTaskVertical(this, verticalRate, wnd.smoothMoveTime, ()=>
                {
                    if(serializeId != _m_lInviteShowSerializeId)
                        return;
                    
                    _m_lByInviteConsortId = _consortId;//移动完成后再设置被邀约妃子, 否则妃子item被邀约表现会在移动中就展示

                    // 移动完成后刷新一次item
                    forceRefreshAllItem();
                    _comlete?.Invoke();
                }).deal();
            }
            else
            {
                _m_lByInviteConsortId = _consortId;

                forceRefreshAllItem();
                _comlete?.Invoke();
            }
        }

        #endregion

        #region Scroll滚动到指定妃子

        /// <summary>
        /// 获取第一个拥有可升级加护技能的已解锁妃子下标，找不到时回退到第一个妃子（下标0），列表为空则返回-1
        /// </summary>
        private int _getCanUpgradeBlessSkillConsortIndex()
        {
            for (int i = 0; i < _m_lUnlockConsortInfoList.Count; i++)
            {
                GGottenConsortInfo gottenConsortInfo = _m_lUnlockConsortInfoList[i] as GGottenConsortInfo;
                if (gottenConsortInfo == null || gottenConsortInfo.blessSkillInfoList == null)
                    continue;

                foreach (ConsortBlessSkillInfo blessSkillInfo in gottenConsortInfo.blessSkillInfoList)
                {
                    if (blessSkillInfo != null && blessSkillInfo.checkHasCanLevelUp(gottenConsortInfo))
                        return i;
                }
            }
            // 找不到目标妃子时，回退到第一个妃子
            return _m_lUnlockConsortInfoList.Count > 0 ? 0 : -1;
        }

        /// <summary>
        /// 根据目标类型获取对应妃子下标
        /// </summary>
        private int _getTargetConsortIndex(EConsortMainTargetConsortType _type, string _params)
        {
            switch (_type)
            {
                case EConsortMainTargetConsortType.CAN_UPGRADE_BLESS_SKILL:
                    return _getCanUpgradeBlessSkillConsortIndex();
                case EConsortMainTargetConsortType.SPECIFIED_ID:
                    long.TryParse(_params, out long consortId);
                    return _getSpecifiedIdConsortIndex(consortId);
                default:
                    return -1;
            }
        }

        /// <summary>
        /// 滚动到指定类型的妃子位置
        /// </summary>
        /// <param name="_type">目标妃子类型</param>
        /// <param name="_params">不同类型需要参数</param>
        /// <param name="_smoothTime">平滑移动时间</param>
        /// <param name="_complete">移动完成回调</param>
        public void scrollMoveToTargetConsort(EConsortMainTargetConsortType _type, string _params, float _smoothTime = 0.25f, Action _complete = null)
        {
            if (wnd == null)
            {
                _complete?.Invoke();
                return;
            }

            int consortIndex = _getTargetConsortIndex(_type, _params);
            if (consortIndex < 0)
            {
                _complete?.Invoke();
                return;
            }

            long serializeId = _m_lScrollMoveSerialize = ALSerializeOpMgr.next();

            // 获取妃子所在位置
            Vector2 consortPos = getItemPos(consortIndex);

            // 计算可移动区域高度
            float canMoveHeight = wnd.gridAreaMaskObj == null ? allHeight : allHeight - wnd.gridAreaMaskObj.rect.height;

            // 计算目标垂直位置比率（滚动到item顶部）
            float tmpHeight = canMoveHeight - (-consortPos.y);
            float verticalRate = Mathf.Clamp(tmpHeight / canMoveHeight, 0f, 1f);

            new ScrollerSmoothMoveTaskVertical(this, verticalRate, _smoothTime, () =>
            {
                if (serializeId != _m_lScrollMoveSerialize)
                    return;

                // 移动完成后刷新一次所有item
                forceRefreshAllItem();
                _complete?.Invoke();
            }).deal();
        }

        #endregion

        /// <summary>
        /// 模拟点击指定类型的妃子（消息处理）
        /// 参数：_objs[0] 为 EConsortMainTargetConsortType 枚举（已由发送方解析）
        /// 参数：_objs[1] 为妃子ID字符串（string，仅当 SPECIFIED_ID 时有效，由本方解析）
        /// </summary>
        private void _onSimulateClickConsort(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || _objs[0] == null)
                return;

            EConsortMainTargetConsortType targetType;
            if (_objs[0] is EConsortMainTargetConsortType consType)
                targetType = consType;
            else if (_objs[0] is string s && ALCommon.TryEnumParse(typeof(EConsortMainTargetConsortType), s, out targetType))
            { }
            else
                return;

            string param = string.Empty;
            if (_objs.Length >= 2 && _objs[1] is string)
                param = (string) _objs[1];

            simulateClickConsortByTargetType(targetType, param);
        }

        #region 模拟点击

        /// <summary>
        /// 通过指定的目标妃子类型模拟点击妃子
        /// </summary>
        /// <param name="_params">不同类型需要参数</param>
        public void simulateClickConsortByTargetType(EConsortMainTargetConsortType _targetType, string _params)
        {
            GGUIWndConsortListGridItem targetItem = _getTargetConsortItemWnd(_targetType, _params);
            if (targetItem != null)
                _onClickCardItem(targetItem);
        }

        /// <summary>
        /// 根据目标类型获取对应妃子的 ItemWnd
        /// </summary>
        private GGUIWndConsortListGridItem _getTargetConsortItemWnd(EConsortMainTargetConsortType _type, string _params)
        {
            switch (_type)
            {
                case EConsortMainTargetConsortType.CAN_UPGRADE_BLESS_SKILL:
                    return _getCanUpgradeBlessSkillConsortItemWnd();
                case EConsortMainTargetConsortType.SPECIFIED_ID:
                    long.TryParse(_params, out long consortId);
                    return _getSpecifiedIdConsortItemWnd(consortId);
                default:
                    return null;
            }
        }

        #endregion

        public ScrollRect scrollRect { get { return wnd == null ? null : wnd.scrollRect; } }
        public long serialize { get { return _m_iSerialize; } }

        #region 获取RectTransform

        /// <summary>
        /// 获取指定ID妃子在列表中的下标，找不到时回退到第一个已解锁妃子，列表为空则返回-1
        /// </summary>
        private int _getSpecifiedIdConsortIndex(long _consortId)
        {
            for (int i = 0; i < _m_lUnlockConsortInfoList.Count; i++)
            {
                if (_m_lUnlockConsortInfoList[i]?.consortId == _consortId)
                    return i;
            }
            // 找不到时回退到第一个已解锁妃子
            return _m_lUnlockConsortInfoList.Count > 0 ? 0 : -1;
        }

        /// <summary>
        /// 获取指定ID的妃子 ItemWnd，找不到时回退到第一个可见已解锁妃子
        /// </summary>
        private GGUIWndConsortListGridItem _getSpecifiedIdConsortItemWnd(long _consortId)
        {
            GGUIWndConsortListGridItem targetItem = null;
            GGUIWndConsortListGridItem firstUnlockItem = null;

            refreshAllItem((_itemWnd, _idx) =>
            {
                if (_itemWnd == null || _itemWnd.consortShowInfo == null)
                    return;

                GGottenConsortInfo gottenConsortInfo = NPPlayer.instance?.consortComp?.getConsortInfo(_itemWnd.consortShowInfo.consortId);
                if (gottenConsortInfo == null)
                    return;

                if (firstUnlockItem == null)
                    firstUnlockItem = _itemWnd;

                if (targetItem == null && _itemWnd.consortShowInfo.consortId == _consortId)
                    targetItem = _itemWnd;
            });

            return targetItem ?? firstUnlockItem;
        }

        /// <summary>
        /// 获取第一个拥有可升级加护技能的已解锁妃子的 ItemWnd，找不到时回退到第一个可见已解锁妃子
        /// </summary>
        private GGUIWndConsortListGridItem _getCanUpgradeBlessSkillConsortItemWnd()
        {
            GGUIWndConsortListGridItem targetItem = null;
            GGUIWndConsortListGridItem firstUnlockItem = null;

            // 使用refreshAllItem遍历当前可见的item，找到第一个有可升级加护技能的妃子
            refreshAllItem((_itemWnd, _idx) =>
            {
                if (_itemWnd == null || _itemWnd.consortShowInfo == null)
                    return;

                // 只处理已解锁的妃子
                GGottenConsortInfo gottenConsortInfo = NPPlayer.instance?.consortComp?.getConsortInfo(_itemWnd.consortShowInfo.consortId);
                if (gottenConsortInfo == null)
                    return;

                // 记录第一个已解锁妃子作为回退目标
                if (firstUnlockItem == null)
                    firstUnlockItem = _itemWnd;

                if (targetItem != null)
                    return;

                // 检查是否有可以升级的加护技能
                if (gottenConsortInfo.blessSkillInfoList == null)
                    return;

                foreach (ConsortBlessSkillInfo blessSkillInfo in gottenConsortInfo.blessSkillInfoList)
                {
                    if (blessSkillInfo != null && blessSkillInfo.checkHasCanLevelUp(gottenConsortInfo))
                    {
                        targetItem = _itemWnd;
                        break;
                    }
                }
            });

            // 找不到目标妃子时，回退到第一个可见已解锁妃子
            return targetItem ?? firstUnlockItem;
        }

        /// <summary>
        /// 根据目标类型获取对应妃子的 RectTransform
        /// </summary>
        public RectTransform getTargetConsortRectTransform(EConsortMainTargetConsortType _type, string _params)
        {
            switch (_type)
            {
                case EConsortMainTargetConsortType.CAN_UPGRADE_BLESS_SKILL:
                    return _getCanUpgradeBlessSkillConsortItemWnd()?.rectTransform;
                case EConsortMainTargetConsortType.SPECIFIED_ID:
                    long.TryParse(_params, out long consortId);
                    return _getSpecifiedIdConsortItemWnd(consortId)?.rectTransform;
                default:
                    return null;
            }
        }

        #endregion
    }
}