using ALPackage;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴列表
    /// </summary>
    public class GGUIWndHeroListGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoHeroListGridItem, GGUIMonoHeroListGrid, GGUIWndHeroListGridItem>, _IScrollerSmoothMovable
    {
        //总的卡牌列表
        [NotNull] private List<HeroCardShowInfo> _m_lCardList = new List<HeroCardShowInfo>();
        //点击Item回调
        private Action<GGUIWndHeroListGridItem> _m_dClickDelegate;
        //伙伴列表已获得bar
        private GGUIWndHeroListOwnBarController _m_ownBarController;
        //伙伴列表未解锁bar
        private GGUIWndHeroListBarController _m_otherBarController;
        //是否需要刷新bar
        private bool _m_bNeedRefreshBar;
        //检查是否是特殊展示红点的伙伴方法
        private Func<long, bool> _m_checkIsSpecialHeroFunc;

        private long _m_lScrollMoveSerialize;

        public Action<GGUIWndHeroListGridItem> clickDelegate { get { return _m_dClickDelegate; } set { _m_dClickDelegate = value; } }

        public GGUIWndHeroListGrid(GGUIMonoHeroListGrid _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onDiscard()
        {
            if (null != _m_ownBarController)
            {
                removeBar(_m_ownBarController);
                _m_ownBarController.discard();
                _m_ownBarController = null;
            }

            if (null != _m_otherBarController)
            {
                removeBar(_m_otherBarController);
                _m_otherBarController.discard();
                _m_otherBarController = null;
            }

            if (_m_lCardList != null)
                _m_lCardList.Clear();

            _m_dClickDelegate = null;

            _m_bNeedRefreshBar = true;
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_HERO, _onSimulateClickHero);
            
            _m_lScrollMoveSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_HERO, _onSimulateClickHero);
        }

        protected override void _onWndInitDone()
        {
            // 初始化容器
            setItemCount(0);

            _m_bNeedRefreshBar = true;
        }

        // 创建对象
        protected override GGUIWndHeroListGridItem _createItemWnd(GGUIMonoHeroListGridItem _itemMono)
        {
            // 创建对象
            GGUIWndHeroListGridItem gridItem = new GGUIWndHeroListGridItem(_itemMono, _m_checkIsSpecialHeroFunc);
            // 注册物品点击事件
            gridItem.clickDelegate += _onClickCardItem;
            return gridItem;
        }

        // 刷新对象
        protected override void _onRefreshItemWnd(GGUIWndHeroListGridItem _itemMono, int _itemIdx)
        {
            if (_m_lCardList.Count <= _itemIdx)
                return;

            HeroCardShowInfo heroShowInfo = _m_lCardList[_itemIdx];
            _itemMono.setInfo(heroShowInfo);
        }

        /// <summary>
        /// 初始化伙伴列表
        /// </summary>
        /// <param name="_cardList"></param>
        public void showHeroList(List<HeroCardShowInfo> _cardList, Func<long, bool> _checkIsSpecialShowRedTipHero)
        {
            if (null == _cardList)
                return;

            _m_bNeedRefreshBar = true;
            //获取的列表已经是排序过的
            _m_lCardList.Clear();
            _m_lCardList.AddRange(_cardList);
            _m_checkIsSpecialHeroFunc = _checkIsSpecialShowRedTipHero;
            //刷新卡牌
            _refreshCardList();
            //滚到上边
            scrollMoveToTop();
        }

        // 刷新卡牌列表
        private void _refreshCardList()
        {
            //空物品提示
            ALUGUICommon.setUIObjScale(wnd.noneItemsTips, _m_lCardList.Count <= 0 ? 1 : 0);

            //刷新grid
            setItemCount(_m_lCardList.Count);

            //刷新bar
            if(_m_bNeedRefreshBar)
                _showListBar();

            ALCommonActionMonoTask.addNextFrameLaterTask(() =>
            {
                //列表加载完成
                WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.HERO_LIST_LOAD_DONE);
            });
        }

        //刷新bar
        private void _showListBar()
        {
            _m_bNeedRefreshBar = false;

            //添加已解锁bar
            if (_m_ownBarController == null)
            {
                _m_ownBarController = new GGUIWndHeroListOwnBarController(wnd.gridAreaUIObj);
                addBar(_m_ownBarController);
            }
            long ownCount = 0;
            for (int i = 0; i < _m_lCardList.Count; i++)
            {
                if (_m_lCardList[i] != null && _m_lCardList[i].isUnlock)
                    ownCount++;
            }
            _m_ownBarController.regLoadDoneDelegate(() =>
            {
                _m_ownBarController.setInsertIndex(0);
                _m_ownBarController.setInfo(ownCount);
                forceRefreshBar();
            });

            //查找未解锁的伙伴位置为bar的插入位置
            int barIndex = -1;
            for (int i = 0; i < _m_lCardList.Count; i++)
            {
                if (_m_lCardList[i] != null && _m_lCardList[i].heroRefObj != null && !_m_lCardList[i].isUnlock)
                {
                    barIndex = i;
                    break;
                }
            }

            if (barIndex == -1)
            {
                if (null != _m_otherBarController)
                {
                    removeBar(_m_otherBarController);
                    _m_otherBarController.discard();
                    _m_otherBarController = null;
                }
                return;
            }

            //添加未解锁bar
            if (_m_otherBarController == null)
            {
                _m_otherBarController = new GGUIWndHeroListBarController(wnd.gridAreaUIObj);
                addBar(_m_otherBarController);
            }
            _m_otherBarController.regLoadDoneDelegate(() =>
            {
                _m_otherBarController.setInsertIndex(barIndex);
                forceRefreshBar();
            });
        }

        //移动到顶部
        public void scrollMoveToTop()
        {
            _m_lScrollMoveSerialize = ALSerializeOpMgr.next();
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                moveToTop();
            });
        }

        //点击卡牌
        private void _onClickCardItem(GGUIWndHeroListGridItem _itemWnd)
        {
            if (null == _itemWnd)
                return;

            if (null != _m_dClickDelegate)
                _m_dClickDelegate(_itemWnd);
        }

        //滚动到指定位置
        public void scrollMoveToTarget(float _pos)
        {
            _m_lScrollMoveSerialize = ALSerializeOpMgr.next();
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
        /// 获取伙伴在列表中的下标
        /// </summary>
        /// <param name="_heroId">伙伴ID</param>
        /// <returns>返回下标，未找到返回-1</returns>
        public int getHeroIndex(long _heroId)
        {
            for (int i = 0; i < _m_lCardList.Count; i++)
            {
                if (_m_lCardList[i] != null && _m_lCardList[i].id == _heroId)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// 获取等级最低的伙伴下标
        /// </summary>
        private int _getLevelMinHeroIndex()
        {
            if (_m_lCardList.Count <= 0)
                return 0;

            HeroCardShowInfo minLevelHero = null;
            int minHeroIndex = 0;

            for (int i = 0; i < _m_lCardList.Count; i++)
            {
                HeroCardShowInfo heroShowInfo = _m_lCardList[i];
                if (heroShowInfo == null || !heroShowInfo.isUnlock)
                    continue;

                if (minLevelHero == null || heroShowInfo.level < minLevelHero.level)
                {
                    minHeroIndex = i;
                    minLevelHero = heroShowInfo;
                }
            }

            return minHeroIndex;
        }

        /// <summary>
        /// 获取经营技能等级最低的已解锁伙伴下标
        /// </summary>
        private long _getBusinessSkillLevelMinHeroIndex()
        {
            if (_m_lCardList.Count <= 0)
                return 0;

            int minBusinessSkillLevel = int.MaxValue;
            int minHeroIndex = 0;

            for (int i = 0; i < _m_lCardList.Count; i++)
            {
                HeroCardShowInfo heroShowInfo = _m_lCardList[i];
                if (heroShowInfo == null || !heroShowInfo.isUnlock || heroShowInfo.heroInfo == null || heroShowInfo.heroRefObj == null)
                    continue;

                // 获取伙伴初始经营技能等级
                int businessSkillInfoLvl = heroShowInfo.heroInfo.heroBusinessSkillInfoMgr.getBusinessSkillInfo(heroShowInfo.heroRefObj.business_skill_id)?.level ?? 0;

                if (businessSkillInfoLvl < minBusinessSkillLevel)
                {
                    minBusinessSkillLevel = businessSkillInfoLvl;
                    minHeroIndex = i;
                }
            }

            return minHeroIndex;
        }

        /// <summary>
        /// 获取没有装备藏品的已解锁伙伴下标
        /// </summary>
        /// <returns></returns>
        private long _getNoWearEquipHeroIndex()
        {
            if (_m_lCardList.Count <= 0)
                return 0;

            int heroIndex = 0;
            for (int i = 0; i < _m_lCardList.Count; i++)
            {
                HeroCardShowInfo heroShowInfo = _m_lCardList[i];
                if (heroShowInfo == null || !heroShowInfo.isUnlock || heroShowInfo.heroInfo == null || heroShowInfo.heroRefObj == null)
                    continue;

                EquipInfo equipInfo = NPPlayer.instance.equipComp.getEquipInfoByHeroId(heroShowInfo.id);
                if(equipInfo  == null)
                {
                    heroIndex = i;
                    break;
                }
            }
            return heroIndex;
        }

        #region 获取大臣itemWnd

        /// <summary>
        /// 通过指定的
        /// </summary>
        /// <returns></returns>
        public GGUIWndHeroListGridItem getItemWndByTargetHeroType(EHeroMainTargetHeroType _targetHeroType)
        {
            switch (_targetHeroType)
            {
                case EHeroMainTargetHeroType.LEVEL_MIN_HERO:
                    return _getMinLevelHeroItemWnd();
                case EHeroMainTargetHeroType.BUSINESS_SKILL_LEVEL_MIN_HERO:
                    return _getMinBusinessSkillLevelHeroItemWnd();
                case EHeroMainTargetHeroType.NO_WEAR_EQUIP_HERO:
                    return _getNoWearEquipHeroItemWnd();
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取等级最低的已解锁伙伴的 ItemWnd
        /// </summary>
        private GGUIWndHeroListGridItem _getMinLevelHeroItemWnd()
        {
            GGUIWndHeroListGridItem targetItem = null;
                
            refreshAllItem((_itemWnd, _idx) =>
            {
                if(_itemWnd == null || _itemWnd.heroShowInfo == null || !_itemWnd.heroShowInfo.isUnlock)
                    return;
                
                if(targetItem == null || targetItem.heroShowInfo == null || !targetItem.heroShowInfo.isUnlock || 
                   targetItem.heroShowInfo.level > _itemWnd.heroShowInfo.level)
                {
                    targetItem = _itemWnd;
                }
            });

            return targetItem;
        }

        /// <summary>
        /// 获取经营技能等级最低的已解锁伙伴的 ItemWnd
        /// </summary>
        public GGUIWndHeroListGridItem _getMinBusinessSkillLevelHeroItemWnd()
        {
            GGUIWndHeroListGridItem targetItem = null;
            int minBusinessSkillLevel = int.MaxValue;
                
            refreshAllItem((_itemWnd, _idx) =>
            {
                if(_itemWnd == null || _itemWnd.heroShowInfo == null || _itemWnd.heroShowInfo.heroInfo == null
                   || _itemWnd.heroShowInfo.heroRefObj == null || !_itemWnd.heroShowInfo.isUnlock)
                    return;

                int heorBusinessSkillLevel =
                    _itemWnd.heroShowInfo.heroInfo.heroBusinessSkillInfoMgr.getBusinessSkillInfo(_itemWnd.heroShowInfo.heroRefObj.business_skill_id)?.level ?? 0;
                
                if(targetItem == null || minBusinessSkillLevel > heorBusinessSkillLevel)
                {
                    targetItem = _itemWnd;
                    minBusinessSkillLevel = heorBusinessSkillLevel;
                }
            });

            return targetItem;
        }

        /// <summary>
        /// 获取没有佩戴藏品的已解锁伙伴的 ItemWnd
        /// </summary>
        public GGUIWndHeroListGridItem _getNoWearEquipHeroItemWnd()
        {
            GGUIWndHeroListGridItem firstItem = null;
            GGUIWndHeroListGridItem targetItem = null;
                
            refreshAllItem((_itemWnd, _idx) =>
            {
                if(_itemWnd == null || _itemWnd.heroShowInfo == null || _itemWnd.heroShowInfo.heroInfo == null
                   || _itemWnd.heroShowInfo.heroRefObj == null || !_itemWnd.heroShowInfo.isUnlock)
                    return;
                
                //记录第一个
                if (firstItem == null)
                    firstItem = _itemWnd;

                //查找没有装备藏品的伙伴
                EquipInfo equipInfo = NPPlayer.instance.equipComp.getEquipInfoByHeroId(_itemWnd.heroShowInfo.id);
                if(targetItem == null && equipInfo == null)
                {
                    targetItem = _itemWnd;
                }
            });

            //如果没找到 取第一个
            if(targetItem == null)
                targetItem = firstItem;

            return targetItem;
        }
        
        #endregion
        
        
        #region Scroll滚动

        /// <summary>
        /// 滚动到等级最低的伙伴位置
        /// </summary>
        /// <param name="_smoothTime"></param>
        /// <param name="_complete"></param>
        public void scrollMoveToMinLevelHero(float _smoothTime = 0.25f, Action _complete = null)
        {
            if (wnd == null)
            {
                _complete?.Invoke();
                return;
            }

            // 获取等级最低的伙伴下标
            int heroIndex = _getLevelMinHeroIndex();
            scrollMoveToIndex(heroIndex, _smoothTime, _complete);
        }

        /// <summary>
        /// 滚动到经营技能等级最低的伙伴位置
        /// </summary>
        /// <param name="_smoothTime"></param>
        /// <param name="_complete"></param>
        public void scrollMoveToMinBusinessSkillLevelHero(float _smoothTime = 0.25f, Action _complete = null)
        {
            if (wnd == null)
            {
                _complete?.Invoke();
                return;
            }

            // 获取经营技能等级最低的伙伴下标
            int heroIndex = (int)_getBusinessSkillLevelMinHeroIndex();
            scrollMoveToIndex(heroIndex, _smoothTime, _complete);
        }

        /// <summary>
        /// 滚动到没有装备藏品的伙伴位置
        /// </summary>
        /// <param name="_smoothTime"></param>
        /// <param name="_complete"></param>
        public void scrollMoveToNoWearEquipHero(float _smoothTime = 0.25f, Action _complete = null)
        {
            if (wnd == null)
            {
                _complete?.Invoke();
                return;
            }

            // 获取没有装备藏品的伙伴下标
            int heroIndex = (int)_getNoWearEquipHeroIndex();
            scrollMoveToIndex(heroIndex, _smoothTime, _complete);
        }
            
        /// <summary>
        /// 滚动到指定伙伴ID的位置
        /// </summary>
        /// <param name="_heroId">伙伴ID</param>
        /// <param name="_smoothTime">平滑移动时间，默认0.25秒</param>
        /// <param name="_complete">移动完成回调</param>
        public void scrollMoveToHero(long _heroId, float _smoothTime = 0.25f, Action _complete = null)
        {
            if (wnd == null)
            {
                _complete?.Invoke();
                return;
            }

            // 获取伙伴所在索引
            int heroIndex = getHeroIndex(_heroId);
            scrollMoveToIndex(heroIndex, _smoothTime, _complete);
        }

        /// <summary>
        /// 滚动到指定下标的位置
        /// </summary>
        /// <param name="_index">下标</param>
        /// <param name="_smoothTime">平滑移动时间，默认0.25秒</param>
        /// <param name="_complete">移动完成回调</param>
        public void scrollMoveToIndex(int _index, float _smoothTime = 0.25f, Action _complete = null)
        {
            if (wnd == null || _index < 0)
            {
                _complete?.Invoke();
                return;
            }

            long serializeId = _m_lScrollMoveSerialize = ALSerializeOpMgr.next();

            // 获取伙伴所在位置
            Vector2 itemPos = getItemPos(_index);

            // 计算可移动区域高度
            float canMoveHeight = wnd.gridAreaMaskObj == null ? allHeight : allHeight - wnd.gridAreaMaskObj.rect.height;

            // 计算目标垂直位置比率
            float tmpHeight = canMoveHeight - (-itemPos.y);
            float verticalRate = Mathf.Clamp(tmpHeight / canMoveHeight, 0f, 1f);

            // 使用平滑移动任务
            new ScrollerEaseMoveTaskVertical(this, verticalRate, EaseType.OutSine, _smoothTime, () =>
            {
                // 检查序列化ID是否匹配，避免多次调用冲突
                if (serializeId != _m_lScrollMoveSerialize)
                    return;

                // 移动完成后刷新一次所有item
                forceRefreshAllItem();
                _complete?.Invoke();
            }).deal();
        }
        
        #endregion

        #region 获取RectTransform

        /// <summary>
        /// 获取等级最低的已解锁伙伴的 RectTransform
        /// </summary>
        public RectTransform getMinLevelHeroRectTransform()
        {
            GGUIWndHeroListGridItem targetItem = _getMinLevelHeroItemWnd();
            return targetItem?.rectTransform;
        }

        /// <summary>
        /// 获取经营技能等级最低的已解锁伙伴的 RectTransform
        /// </summary>
        public RectTransform getMinBusinessSkillLevelHeroRectTransform()
        {
            GGUIWndHeroListGridItem targetItem = _getMinBusinessSkillLevelHeroItemWnd();
            return targetItem?.rectTransform;
        }

        /// <summary>
        /// 获取没有佩戴藏品的已解锁伙伴的 RectTransform
        /// </summary>
        public RectTransform getNoWearEquipHeroRectTransform()
        {
            GGUIWndHeroListGridItem targetItem = _getNoWearEquipHeroItemWnd();
            return targetItem?.rectTransform;
        }
        
        #endregion

        /// <summary>
        /// 模拟点击指定类型的伙伴（消息处理）
        /// 参数：_objs[0] 为 EHeroMainTargetHeroType 枚举类型
        /// </summary>
        private void _onSimulateClickHero(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || _objs[0] == null)
                return;

            EHeroMainTargetHeroType targetType;
            if(_objs[0] is EHeroMainTargetHeroType)
                targetType = (EHeroMainTargetHeroType)_objs[0];
            else if(_objs[0] is string)
                ALCommon.TryEnumParse(typeof(EHeroMainTargetHeroType), (string)_objs[0], out targetType);
            else
                return;
            
            simulateClickHeroByTargetHeroType(targetType);
        }

        #region 模拟点击

        /// <summary>
        /// 通过指定的目标伙伴类型模拟点击伙伴
        /// </summary>
        public void simulateClickHeroByTargetHeroType(EHeroMainTargetHeroType _targetHeroType)
        {
            GGUIWndHeroListGridItem targetItem = getItemWndByTargetHeroType(_targetHeroType);
            if (targetItem != null)
            {
                _onClickCardItem(targetItem);
            }
        }

        #endregion
        
        public ScrollRect scrollRect { get { return wnd == null ? null : wnd.scrollRect; } }
        public long serialize { get { return _m_lScrollMoveSerialize; } }
    }
}
