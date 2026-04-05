using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 带奖励列表的进度条
    /// </summary>
    public class GGUIWndCommonRewardSlider : _ANPGGUIBasicSubWnd<GGUIMonoCommonRewardSlider>
    {
        //奖励宝箱列表
        private GGUIWndCommonRewardSliderContainer _m_wRewardSliderContainer;
        //点击item
        private Action<GGUIWndCommonRewardSliderContainerItem> _m_aOnClickItem;
        //领奖状态变化
        private Action<GGUIWndCommonRewardSliderContainerItem> _m_aOnRewardStateChg;
        //当前分数
        private long _m_lCurScore;
        //最大分数
        private long _m_lMaxScore;
        //slider的最大分数
        private float _m_lSliderMaxScore;
        //进度奖励信息列表
        private List<_ISliderRewardItemInfo> _m_lItemInfoList;
        //item列表位置信息
        private List<float> _m_lItemPos;
        //最后一个需要特殊展示的item
        private GGUIWndCommonRewardSliderContainerItem _m_wLastSpecItem;
        //进度条满的特效
        private CommonUISfxObj _m_fullSfxObj;
        
        private string _m_curScroeKey;
        
        /// <summary>
        /// 点击item
        /// </summary>
        public Action<GGUIWndCommonRewardSliderContainerItem> onClickItem
        {
            get { return _m_aOnClickItem; }
            set { _m_aOnClickItem = value; }
        }

        /// <summary>
        /// 奖励状态变化
        /// </summary>
        public Action<GGUIWndCommonRewardSliderContainerItem> onRewardStateChg
        {
            get { return _m_aOnRewardStateChg; }
            set { _m_aOnRewardStateChg = value; }
        }

        /// <summary>
        /// 当前分数
        /// </summary>
        public long curScore
        {
            get { return _m_lCurScore; }
        }

        /// <summary>
        /// 进度条可以展示的最大值
        /// </summary>
        public float sliderMaxScore
        {
            get { return _m_lSliderMaxScore; }
        }

        public GGUIWndCommonRewardSlider(GGUIMonoCommonRewardSlider _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            if(_m_wRewardSliderContainer != null)
                _m_wRewardSliderContainer.hideWnd();

            if(_m_wLastSpecItem != null)
                _m_wLastSpecItem.hideWnd();

            _m_fullSfxObj?.forceDiscard();
            _m_fullSfxObj = null;
        }

        protected override void _onReset()
        {
            if (_m_wRewardSliderContainer != null)
                _m_wRewardSliderContainer.resetWnd();

            if (_m_wLastSpecItem != null)
                _m_wLastSpecItem.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_wRewardSliderContainer != null)
                _m_wRewardSliderContainer.discard();
            _m_wRewardSliderContainer = null;

            if (_m_wLastSpecItem != null)
                _m_wLastSpecItem.discard();
            _m_wLastSpecItem = null;

            _m_fullSfxObj?.forceDiscard();
            _m_fullSfxObj = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoRewardSliderContainer != null)
            {
                _m_wRewardSliderContainer = new GGUIWndCommonRewardSliderContainer(wnd.monoRewardSliderContainer);
                _m_wRewardSliderContainer.onClickItem += _onClickItem;
                _m_wRewardSliderContainer.onRewardStateChg += _onRewardStateChg;
            }

            if (wnd.monoLastSpecItem != null)
            {
                _m_wLastSpecItem = new GGUIWndCommonRewardSliderContainerItem(wnd.monoLastSpecItem);
                _m_wLastSpecItem.onClickItem += _onClickItem;
                _m_wLastSpecItem.onRewardStateChg += _onRewardStateChg;
            }
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_curScore">当前分数</param>
        /// <param name="_itemInfoList">item信息列表</param>
        public void setInfo(long _curScore, List<_ISliderRewardItemInfo> _itemInfoList, string _curScroeKey = default)
        {
            if (wnd == null || _itemInfoList == null)
                return;

            //设置分数
            _m_lCurScore = _curScore;
            _m_curScroeKey = _curScroeKey;
            _m_lMaxScore = 0;
            for (int i = 0; i < _itemInfoList.Count; i++)
            {
                if (_itemInfoList[i] != null && _m_lMaxScore < _itemInfoList[i].score)
                    _m_lMaxScore = _itemInfoList[i].score;
            }
            _m_lSliderMaxScore = _m_lMaxScore * 1.0f / (wnd.lastRewardBoxPosPercentage / 100f);

            //设置item信息列表
            _m_lItemInfoList = _itemInfoList;
            _m_lItemInfoList.Sort((_a, _b) =>
            {
                //按分数从小到大排序
                return _a.score.CompareTo(_b.score);
            });
            //设置每个item位置
            _m_lItemPos = new List<float>();
            if (wnd.sldProcess != null && _itemInfoList.Count != 0)
            {
                //获取进度条的长度
                RectTransform sldRect = (RectTransform)wnd.sldProcess.transform;
                float sliderWidth = sldRect != null ? sldRect.rect.width : 0f;
                //是否是均分的
                if (wnd.isDivideEqually)
                {
                    //每个item间隔
                    float eachPos = sliderWidth * (wnd.lastRewardBoxPosPercentage / 100f) / _itemInfoList.Count;
                    //item目标位置
                    float itemPos = 0;
                    for (int i = 0; i < _itemInfoList.Count; i++)
                    {
                        itemPos += eachPos;
                        _m_lItemPos.Add(itemPos);
                    }
                }
                else
                {
                    for (int i = 0; i < _itemInfoList.Count; i++)
                    {
                        if(_itemInfoList[i] == null)
                            continue;

                        //根据分数比例计算item位置
                        float itemPos = _m_lSliderMaxScore != 0 ? _itemInfoList[i].score * 1.0f / _m_lSliderMaxScore * sliderWidth : 0;
                        _m_lItemPos.Add(itemPos);
                    }
                }
            }

            //刷新窗口
            _refreshWnd();
        }

        /// <summary>
        /// 根据分数刷新全部状态
        /// </summary>
        /// <param name="_curScore"></param>
        public void refreshAllStateByScore(long _curScore)
        {
            _m_lCurScore = _curScore;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新全部状态
        /// </summary>
        public void refreshAllState()
        {
            _refreshWnd();
        }

        /// <summary>
        /// 根据分数刷新进度及该分数下的宝箱状态
        /// </summary>
        /// <param name="_curScore"></param>
        public void refreshStateByScore(long _curScore)
        {
            refreshStateByScore((float)_curScore);
        }

        /// <summary>
        /// 根据分数刷新进度及该分数下的宝箱状态
        /// </summary>
        /// <param name="_curScore"></param>
        public void refreshStateByScore(float _curScore)
        {
            _m_lCurScore = (long)Math.Floor(_curScore);

            if (_m_wRewardSliderContainer != null)
                _m_wRewardSliderContainer.refreshStateByScore(_m_lCurScore);

            if(_m_wLastSpecItem != null && _m_wLastSpecItem.itemInfo != null && _m_lCurScore >= _m_wLastSpecItem.itemInfo.score)
                _m_wLastSpecItem.refreshState();

            _refreshSlider(_curScore);
        }

        /// <summary>
        /// 获取进度条的值
        /// </summary>
        /// <returns></returns>
        public float getSliderValueByScore(float _curScore)
        {
            if (wnd == null || _m_lItemInfoList == null || _m_lItemInfoList.Count == 0)
                return 0 ;

            //奖励item间隔是否均分
            if (wnd.isDivideEqually)
            {
                //目标slider值
                float targetSldValue = 0f;
                //平均Slider值
                float divideValue = (wnd.lastRewardBoxPosPercentage / 100f) / _m_lItemInfoList.Count;
                //上个奖励item分数
                long lastScore = 0;
                for (int i = 0; i < _m_lItemInfoList.Count; i++)
                {
                    if (_m_lItemInfoList[i] == null)
                        continue;

                    //如果分数在某个区间，计算该分数在该区间的比例，加入进度值
                    if (_curScore >= lastScore && _curScore < _m_lItemInfoList[i].score)
                    {
                        targetSldValue += ((_curScore - lastScore) * 1.0f / (_m_lItemInfoList[i].score - lastScore) * divideValue);
                        break;
                    }
                    targetSldValue += divideValue;
                    lastScore = _m_lItemInfoList[i].score;
                }

                //计算超过最后一个宝箱的进度条值，如果有的话
                if (_curScore > _m_lMaxScore && _m_lSliderMaxScore > _m_lMaxScore)
                    targetSldValue += ((_curScore - _m_lMaxScore) * 1.0f / (_m_lSliderMaxScore - _m_lMaxScore) * ((100 - wnd.lastRewardBoxPosPercentage) / 100f));

                if (targetSldValue > 1)
                    targetSldValue = 1;
                if (targetSldValue < 0)
                    targetSldValue = 0;

                return targetSldValue;
            }
            else
            {
                float sldValue = _m_lSliderMaxScore != 0 ? _curScore * 1.0f / _m_lSliderMaxScore : 0f;
                if (sldValue > 1)
                    sldValue = 1;
                if (sldValue < 0)
                    sldValue = 0;

                return sldValue;
            }
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshSlider();
            _refreshItemList();
            _refreshState();
            _refreshLastSpecItem();
        }

        //刷新进度条
        private void _refreshSlider()
        {
            _refreshSlider(_m_lCurScore);
        }
        
        //刷新进度条
        private void _refreshSlider(float _curScore)
        {
            if (wnd == null)
                return;

            string scrollKey = string.IsNullOrEmpty(_m_curScroeKey) ? TransKeyConst.common_value : _m_curScroeKey;
            ALUGUICommon.setLabelTxt(wnd.txtCurProcess, TextTranslate.instance.getLanguage(scrollKey, _curScore));
            ALUGUICommon.setSliderScale(wnd.sldProcess, getSliderValueByScore(_curScore));

            //播放满的特效
            if (_curScore >= _m_lSliderMaxScore && _m_fullSfxObj == null && wnd.fullSfxId > 0 )
            {
                _m_fullSfxObj = PlaySfxMgr.instance.playUISfx(wnd.fullSfxId, wnd.fullSfxParent);
            }
            if (_curScore < _m_lSliderMaxScore)
            {
                _m_fullSfxObj?.forceDiscard();
                _m_fullSfxObj = null;
            }
        }

        //刷新进度奖励列表
        private void _refreshItemList()
        {
            if (_m_wRewardSliderContainer != null)
            {
                _m_wRewardSliderContainer.showWnd();
                _m_wRewardSliderContainer.showItemList(_m_lItemInfoList, _m_lItemPos, _m_wLastSpecItem == null);
            }
        }

        //刷新进度奖励状态
        private void _refreshState()
        {
            if (_m_wRewardSliderContainer != null)
                _m_wRewardSliderContainer.refreshState();
        }

        //刷新最后一个特殊展示item
        private void _refreshLastSpecItem()
        {
            if (_m_wLastSpecItem != null && _m_lItemInfoList != null && _m_lItemInfoList.Count > 0)
            {
                _ISliderRewardItemInfo info = _m_lItemInfoList[_m_lItemInfoList.Count - 1];
                _m_wLastSpecItem.showWnd();
                _m_wLastSpecItem.setInfo(info);
            }
        }

        //奖励状态变化
        private void _onRewardStateChg(GGUIWndCommonRewardSliderContainerItem _item)
        {
            if (_m_aOnRewardStateChg != null)
                _m_aOnRewardStateChg(_item);
        }

        #region 点击事件

        //点击item
        private void _onClickItem(GGUIWndCommonRewardSliderContainerItem _item)
        {
            if (_m_aOnClickItem != null)
                _m_aOnClickItem(_item);
        }

        #endregion
    }
}
