using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public interface _ISliderRewardItemInfoPro
    {
        /// <summary>
        /// id
        /// </summary>
        long id { get; }
        /// <summary>
        /// 分数
        /// </summary>
        long score { get; }
        /// <summary>
        /// item加载路径
        /// </summary>
        NPCommonAssetPathInfo rewardItemAssetPath { get; }
        /// <summary>
        /// 显示数值使用的格式化类型
        /// </summary>
        EValueFormatType valueFormatType { get; }
        /// <summary>
        /// 显示使用的分数(若为null或string.empty, 会显示score值, 否则显示该字符串)
        /// </summary>
        string showScore { get; }
        /// <summary>
        /// 奖励领取状态
        /// </summary>
        ESliderRewardState rewardState { get; }
        /// <summary>
        /// 宝箱图标
        /// </summary>
        NPGTextureIndex icon { get; }
        /// <summary>
        /// 需要展示的奖励item
        /// </summary>
        _IItem showRewardItem { get; }
    }
    
    public class GGUIWndCommonRewardSliderPro : _ANPGGUIBasicSubWnd<GGUIMonoCommonRewardSliderPro>
    {
        private float _m_fSldWidth;//进度条宽度
        
        //点击item
        private Action<GGUIWndCommonRewardSliderItemPro> _m_aOnClickItem;
        //领奖状态变化
        private Action<GGUIWndCommonRewardSliderItemPro> _m_aOnRewardStateChg;
        //当前分数
        private long _m_lCurScore;
        //slider最小分数
        private long _m_lSliderMinScore;
        //slider的最大分数
        private long _m_lSliderMaxScore;
        //进度奖励信息列表
        private List<_ISliderRewardItemInfoPro> _m_lItemInfoList;
        //item列表位置信息
        private List<float> _m_lItemPos;
        /// <summary>
        /// 奖励item列表
        /// </summary>
        [NotNull] private List<GGUIWndCommonRewardSliderItemPro> _m_lRewardItemWndList = new List<GGUIWndCommonRewardSliderItemPro>();
        private long _m_lInitItemWndSerialize = 0;
        
        //进度条满的特效
        private CommonUISfxObj _m_fullSfxObj;
        
        private string _m_curScroeKey;
        
        /// <summary>
        /// 点击item
        /// </summary>
        public Action<GGUIWndCommonRewardSliderItemPro> onClickItem
        {
            get { return _m_aOnClickItem; }
            set { _m_aOnClickItem = value; }
        }

        /// <summary>
        /// 奖励状态变化
        /// </summary>
        public Action<GGUIWndCommonRewardSliderItemPro> onRewardStateChg
        {
            get { return _m_aOnRewardStateChg; }
            set { _m_aOnRewardStateChg = value; }
        }

        public GGUIWndCommonRewardSliderPro(GGUIMonoCommonRewardSliderPro _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_fullSfxObj?.forceDiscard();
            _m_fullSfxObj = null;
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _discradAllItemWnd();
            
            _m_fullSfxObj?.forceDiscard();
            _m_fullSfxObj = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.sldProcess != null)
            {
                wnd.sldProcess.minValue = 0;
                wnd.sldProcess.maxValue = 1;
                wnd.sldProcess.wholeNumbers = false;
                
                _m_fSldWidth = ((RectTransform) wnd.sldProcess.transform).rect.width;
            }
            else
            {
                _m_fSldWidth = 0;
            }
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_curScore">当前分数</param>
        /// <param name="_itemInfoList">item信息列表</param>
        public void setInfo(long _sldMinValue, long _sldMaxValue, long _curScore, List<_ISliderRewardItemInfoPro> _itemInfoList, string _curScroeKey = default)
        {
            if (wnd == null || _itemInfoList == null)
                return;

            //设置分数
            _m_lCurScore = _curScore;
            _m_curScroeKey = _curScroeKey;
            
            //设置item信息列表
            _m_lItemInfoList = _itemInfoList;
            _m_lItemInfoList.Sort((_a, _b) =>
            {
                //按分数从小到大排序
                return _a.score.CompareTo(_b.score);
            });

            _m_lSliderMinScore = long.MaxValue;
            _m_lSliderMaxScore = long.MinValue;
            
            _ISliderRewardItemInfoPro minRewardItemScore = _m_lItemInfoList.GetFirst();
            _ISliderRewardItemInfoPro maxRewardItemScore = _m_lItemInfoList.GetLast();
            _m_lSliderMinScore = Math.Min(_sldMinValue, minRewardItemScore?.score ?? long.MaxValue);
            _m_lSliderMaxScore = Math.Max(_sldMaxValue, maxRewardItemScore?.score ?? long.MinValue);
            if (_m_lSliderMinScore > _m_lSliderMaxScore)
                _m_lSliderMinScore = _m_lSliderMaxScore = 0;

            //设置每个item位置
            _m_lItemPos = new List<float>();
            if (wnd.sldProcess != null && _m_lItemInfoList.Count != 0)
            {
                //是否是均分的
                if (wnd.isDivideEqually)
                {
                    int partNum = 1;//需要分成的份数
                    long perScore = _m_lSliderMinScore;//上一次奖励点位分数
                    // 遍历item列表，计算需要分成的份数, 因为上面已经将_m_lItemInfoList列表按分数从小到大排序
                    for(int i = 0, count = _m_lItemInfoList.Count; i < count; i++)
                    {
                        _ISliderRewardItemInfoPro itemInfo = _m_lItemInfoList[i];
                        if(itemInfo == null)
                            continue;
                        
                        // 若需要分数大于进度条最大分数，则不计算位置
                        if(itemInfo.score >= _m_lSliderMaxScore)
                            continue;

                        if (itemInfo.score > perScore)
                        {
                            partNum++;
                            perScore = itemInfo.score;
                        }
                    }
                    
                    //每个不同奖励分数间隔
                    float eachPos = _m_fSldWidth / partNum;
                    //item目标位置
                    float itemPos = 0;
                    perScore = _m_lSliderMinScore;//上一次奖励点位分数
                    for (int i = 0; i < _m_lItemInfoList.Count; i++)
                    {
                        _ISliderRewardItemInfoPro itemInfo = _m_lItemInfoList[i];
                        if (itemInfo == null || itemInfo.score <= perScore)
                        {
                            _m_lItemPos.Add(itemPos);
                            continue;
                        }
                        
                        itemPos += eachPos;
                        perScore = itemInfo.score;
                        _m_lItemPos.Add(itemPos);
                    }
                }
                else
                {
                    for (int i = 0; i < _m_lItemInfoList.Count; i++)
                    {
                        if(_m_lItemInfoList[i] == null)
                            continue;

                        //根据分数比例计算item位置
                        float itemPos = (_m_lItemInfoList[i].score - _m_lSliderMinScore) * 1.0f / (_m_lSliderMaxScore - _m_lSliderMinScore) * _m_fSldWidth;
                        _m_lItemPos.Add(itemPos);
                    }
                }
            }

            _initAllItemWnd(() =>
            {
                //刷新窗口
                _refreshWnd();       
            });
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

            _refreshAllItemStateByScore(_m_lCurScore);
            _refreshSlider(_curScore);
        }

        /// <summary>
        /// 获取进度条的值
        /// </summary>
        /// <returns></returns>
        public float getSliderValueByScore(float _curScore)
        {
            if (wnd == null || _m_lItemInfoList == null || _m_lItemInfoList.Count == 0 || _m_lSliderMinScore == _m_lSliderMaxScore 
            || _m_lItemPos == null || _m_lItemPos.Count <= 0 || _m_fSldWidth <= 0)
                return 0 ;

            //奖励item间隔是否均分
            if (wnd.isDivideEqually)
            {
                float scorePos = 0;//当前分数位置
                long lastScore = _m_lSliderMinScore;//上个奖励item分数
                float lastItemPos = 0;//上一个item位置
                for (int i = 0, count = _m_lItemInfoList.Count; i < count; i++)
                {
                    _ISliderRewardItemInfoPro itemInfo = _m_lItemInfoList[i];
                    if(itemInfo == null)
                        continue;

                    if (itemInfo.score < _curScore)
                    {
                        lastScore = itemInfo.score;
                        scorePos = lastItemPos = _m_lItemPos?.SafeGet(i) ?? 0;
                        continue;
                    }

                    if (itemInfo.score >= _curScore)
                    {
                        float curItemPos = _m_lItemPos?.SafeGet(i) ?? 0;
                        //计算当前分数位置
                        scorePos = (_curScore - lastScore) / (itemInfo.score - lastScore) * (curItemPos - lastItemPos) + lastItemPos;
                        break;
                    }
                }
             
                return scorePos / _m_fSldWidth;
            }
            else
            {
                float sldValue = (_curScore - _m_lSliderMinScore) / (_m_lSliderMaxScore - _m_lSliderMinScore);
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
            _refreshAllItemState();
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

        //奖励状态变化
        private void _onRewardStateChg(GGUIWndCommonRewardSliderItemPro _item)
        {
            if (_m_aOnRewardStateChg != null)
                _m_aOnRewardStateChg(_item);
        }

        #region item列表

        private void _discardItemWnd(GGUIWndCommonRewardSliderItemPro _itemWnd)
        {
            if(_itemWnd == null)
                return;

            //设置点击事件
            _itemWnd.onClickItem = null;
            //设置领奖状态变化事件
            _itemWnd.onRewardStateChg = null;
            
            GGUIMonoCommonRewardSliderItemPro mono = _itemWnd.wnd;
            _itemWnd.discard();

            if (_itemWnd.itemInfo == null || mono == null)
            {
                ALUnityCommon.releaseGameObj(mono);
                return;
            }
            
            GAssetPathCacheMgr.instance.pushbackItem(_itemWnd.itemInfo.rewardItemAssetPath, mono.gameObject);
        }

        private void _loadItemWnd(_ISliderRewardItemInfoPro _info, float _posX, Action<GGUIWndCommonRewardSliderItemPro> _onLoadDone)
        {
            if (wnd == null || wnd.sldProcess == null || _info == null)
            {
                Debug.LogError("GGUIWndCommonRewardSliderPro _loadItemWnd error, wnd or sldProcess is null or info is null");
                _onLoadDone?.Invoke(null);
                return;
            }

            GAssetPathCacheMgr.instance.popItem(_info.rewardItemAssetPath, (_go) =>
            {
                if (_go == null)
                {
                    Debug.LogError($"GGUIWndCommonRewardSliderPro _loadItemWnd error, _go is null, info: {_info}");
                    _onLoadDone?.Invoke(null);
                    return;
                }

                GGUIMonoCommonRewardSliderItemPro mono = _go.GetComponent<GGUIMonoCommonRewardSliderItemPro>();
                if (mono == null)
                {
                    Debug.LogError(
                        $"GGUIWndCommonRewardSliderPro _loadItemWnd error, 资源:{_info.rewardItemAssetPath} 上没有挂载脚本 GGUIMonoCommonRewardSliderItemPro");
                    GAssetPathCacheMgr.instance.pushbackItem(_info.rewardItemAssetPath, _go);
                    _onLoadDone?.Invoke(null);
                    return;
                }

                mono.transform.parent = wnd.sldProcess.transform;
                mono.transform.localPosition = new Vector3(_posX, 0);
                mono.transform.localScale = Vector3.one;

                GGUIWndCommonRewardSliderItemPro itemWnd = new GGUIWndCommonRewardSliderItemPro(mono);
                itemWnd.initWnd();
                
                //设置点击事件
                itemWnd.onClickItem = _onClickItem;
                //设置领奖状态变化事件
                itemWnd.onRewardStateChg = _onRewardStateChg;
                
                itemWnd.setInfo(_info);
                _onLoadDone?.Invoke(itemWnd);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        private void _discradAllItemWnd()
        {
            _m_lInitItemWndSerialize = ALSerializeOpMgr.next();
            
            foreach (var itemWnd in _m_lRewardItemWndList)
            {
                _discardItemWnd(itemWnd);
            }
            
            _m_lRewardItemWndList.Clear();
        }

        /// <summary>
        /// 初始化全部item窗口
        /// </summary>
        private void _initAllItemWnd(Action _onInitDone = null)
        {
            _discradAllItemWnd();
            long serialize = _m_lInitItemWndSerialize = ALSerializeOpMgr.next();
            
            if (_m_lItemInfoList == null)
            {
                _onInitDone?.Invoke();
                return;
            }
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(_onInitDone);
            
            for(int i = 0, count = _m_lItemInfoList.Count;i < count; i++)
            {
                _ISliderRewardItemInfoPro item = _m_lItemInfoList[i];
                if (item == null)
                    continue;

                stepCounter.chgTotalStepCount(1);
                
                //加载item窗口
                _loadItemWnd(item, _m_lItemPos?.SafeGet(i) ?? 0, (_itemWnd) =>
                {
                    if (_itemWnd == null)
                    {
                        stepCounter.addDoneStepCount();
                        return;
                    }

                    if (serialize != _m_lInitItemWndSerialize)
                    {
                        _discardItemWnd(_itemWnd);
                        stepCounter.addDoneStepCount();
                        return;
                    }
                    
                    _itemWnd.showWnd();
                    
                    //添加到列表
                    _m_lRewardItemWndList.Add(_itemWnd);
                    stepCounter.addDoneStepCount();
                });
            }
            
            stepCounter.addDoneStepCount();
        }

        //刷新进度奖励状态
        private void _refreshAllItemState()
        {
            foreach (var itemWnd in _m_lRewardItemWndList)
            {
                itemWnd?.refreshState();
            }
        }

        private void _refreshAllItemStateByScore(long _score)
        {
            foreach (var itemWnd in _m_lRewardItemWndList)
            {
                if (itemWnd != null && itemWnd.itemInfo != null &&
                    (( itemWnd.itemInfo.score <= _score) || 
                     (itemWnd.itemInfo.score > _score && itemWnd.lastState != ESliderRewardState.CAN_NOT_GET))) 
                    itemWnd.refreshState();
            }
        }
        
        #endregion
        
        #region 点击事件

        //点击item
        private void _onClickItem(GGUIWndCommonRewardSliderItemPro _item)
        {
            if (_m_aOnClickItem != null)
                _m_aOnClickItem(_item);
        }

        #endregion
    }
}