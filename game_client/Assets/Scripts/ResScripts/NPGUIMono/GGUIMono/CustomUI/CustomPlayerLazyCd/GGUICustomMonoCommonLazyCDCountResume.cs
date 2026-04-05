using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUICustomMonoCommonLazyCDCountResume : MonoBehaviour
    {
        [ALHeader("lazyCD id")]
        public long lazyCDId;
        
        [ALHeader("倒计时文本")] 
        public TextEx txtTimeDown;
        [ALHeader("数量文本")] 
        public TextEx txtCount;
        [ALHeader("在显示数量时是否显示能获取的最多数量")]
        public bool needShowMaxCount = true;
        [ALHeader("数量文本Key(当needShowMaxCount为true时需要两个参数, 1.当前数量 2.数量上限; 当needShowMaxCount为false时需要一个参数, 1.当前数量)")] 
        public string txtCountKey;
        [ALHeader("数量进度条")]
        public Slider countSlider;
        [ALHeader("数量不足的时候显示，充足就隐藏")] 
        public List<GameObject> goListCDingShow;
        [ALHeader("数量不足的时候隐藏，充足就显示")] 
        public List<GameObject> goListCDingHide;
        [ALHeader("数量满的时候显示，不满就隐藏")] 
        public List<GameObject> goListMaxShow;
        [ALHeader("特效父节点")] 
        public Transform sfxParent;
        [ALHeader("数量增加显示的特效id")] 
        public long addCountSfxId;

        [ALHeader("当数量满时是否需要设置当前数量文本颜色，这个勾选下面两个颜色才有效")]
        public bool isSetCurTextColorWhenFull;
        [ALHeader("当数量满时当前数量文本颜色")]
        public Color curTextFullColor = Color.white;
        [ALHeader("当数量未满时当前数量文本颜色")]
        public Color curTextNotFullColor = Color.white;
        
        [ALHeader("恢复按钮")]
        public GameObject btnResume;
        
        [ALHeader("查看详情按钮")]
        public GameObject btnDetailInfo;
        [ALHeader("详情弹窗资源id")]
        public long detailInfoToolTipAssetPathId = UIResPathConst.WIN_COMMON_LAZYCD_RESOURCES_TIP;
        [ALHeader("详情弹窗跟随物体")]
        public RectTransform detailInfoToolTipFollower;

#if NP_GAME
        private int _m_serialize;
        private long _m_lastCount = -1;//-1表示初始化显示
        private CommonUISfxObj _m_sfxObj;
        private bool _m_isSfxPlaying = false;
        
        private void Awake()
        {
            ALUGUICommon.combineBtnClick(btnResume, _onClickResume);
            ALUGUICommon.combineBtnClick(btnDetailInfo, _onClickDetailInfo);
        }

        private void OnDestroy()
        {
            ALUGUICommon.uncombineBtnClick(btnResume, _onClickResume);
            ALUGUICommon.uncombineBtnClick(btnDetailInfo, _onClickDetailInfo);
            
            if (_m_sfxObj != null) 
                _m_sfxObj.forceDiscard();
            _m_sfxObj = null;
            _m_isSfxPlaying = false;
        }

        private void OnEnable()
        {
            _refreshWnd();
            WinMsg.RegisterMsg(WinMsgType.ON_LAZY_CD_CHG, _onLazyCDChg);
        }

        private void OnDisable()
        {
            _m_serialize = ALSerializeOpMgr.next();
            _m_lastCount = -1;
            
            if (_m_sfxObj != null) 
                _m_sfxObj.forceDiscard();
            _m_sfxObj = null;
            
            _m_isSfxPlaying = false;
            WinMsg.UnregisterMsg(WinMsgType.ON_LAZY_CD_CHG, _onLazyCDChg);
        }
        
        private void _onLazyCDChg(object[] _objs)
        {
            if (_objs.Length == 0)
                return;
            long cdId = (long) _objs[0];
            if (cdId != lazyCDId)
                return;
            _refreshEnergyCount();
        }
        
        private void _refreshWnd()
        {
            if (!gameObject.activeInHierarchy)
                return;
            
            _m_serialize = ALSerializeOpMgr.next();
            _refreshEnergyCount();
        }


        private void _refreshEnergyCount()
        {
            if (!gameObject.activeInHierarchy)
                return;
            
            int serialize = _m_serialize;
            //看消耗cd  consort_rand_call_cd
            PlayerLazyCDInfo lazyCdInfo = NPPlayer.instance.lazyCdComp.getLazyCDInfo(lazyCDId);
            if(null == lazyCdInfo)
                return;
            int curCount = lazyCdInfo.getCount();
            if (curCount <= 0)
            {
                ALUGUICommon.setLabelTxt(txtTimeDown,  TimeUtil.millisecondsToTime_dhms(lazyCdInfo.getRemainMs()));
            }

            string curCountStr = curCount.ToString();
            //当前数量是否需要设置颜色
            if (isSetCurTextColorWhenFull)
            {
                //根据是否满设置当前数量文本颜色
                if (curCount >= lazyCdInfo.MaxCount)
                    curCountStr = GCommon.addColorForRichText(curCountStr, curTextFullColor);
                else
                    curCountStr = GCommon.addColorForRichText(curCountStr, curTextNotFullColor);
            }
            string countKey = string.Empty;
            if (needShowMaxCount)
            {
                //需要显示最大数量
                countKey = string.IsNullOrEmpty(txtCountKey) ? TransKeyConst.common_currentTotalNum_num_num : txtCountKey;
                ALUGUICommon.setLabelTxt(txtCount,   TextTranslate.instance.getLanguage(countKey, curCountStr, lazyCdInfo.MaxCount));
            }
            else
            {
                //不需要显示最大数量
                countKey = string.IsNullOrEmpty(txtCountKey) ? TransKeyConst.common_value : txtCountKey;
                ALUGUICommon.setLabelTxt(txtCount,   TextTranslate.instance.getLanguage(countKey, curCountStr));
            }
            if (countSlider != null)
            {
                float sliderValue = lazyCdInfo.MaxCount <= 0 ? 0 : (float)curCount / lazyCdInfo.MaxCount;
                ALUGUICommon.setSliderScale(countSlider, sliderValue);
            }
            
            ALUGUICommon.setGameObjEnable(goListCDingShow,curCount <= 0);
            ALUGUICommon.setGameObjEnable(goListCDingHide,curCount > 0);
            ALUGUICommon.setGameObjEnable(goListMaxShow,curCount >= lazyCdInfo.MaxCount);

            if (_m_lastCount != -1 && _m_lastCount < curCount && !_m_isSfxPlaying)//非初始记录，说明数值有变化，播放恢复特效
            {
                _m_isSfxPlaying = true;
                _m_sfxObj = PlaySfxMgr.instance.playUISfx(addCountSfxId, sfxParent);
                if (_m_sfxObj != null)
                    _m_sfxObj.regPlayCompleteDelegate(() => { _m_isSfxPlaying = false; });
                else
                {
                    _m_isSfxPlaying = false;
                }
            }
            _m_lastCount = curCount;
            if (curCount < lazyCdInfo.MaxCount)
            {
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if (serialize != _m_serialize)
                        return;
                    _refreshEnergyCount();
                },1f);
            }
        }
        
                
        /// <summary>
        /// 恢复按钮
        /// </summary>
        /// <param name="_gameObject"></param>
        private void _onClickResume(GameObject _gameObject)
        {
            GCommon.popItemAccessWays(ENPItemType.LAZY_CD, lazyCDId);
        }
        
        /// <summary>
        /// 点击详情按钮
        /// </summary>
        /// <param name="_gameObject"></param>
        private void _onClickDetailInfo(GameObject _gameObject)
        {
            NPGNodeCommonToolTip_LazyCdItemDetail toolTip = new NPGNodeCommonToolTip_LazyCdItemDetail(detailInfoToolTipAssetPathId,lazyCDId, detailInfoToolTipFollower, 0);
            QueueMgr.instance.AddNode(toolTip);
        }
#endif
        
    }
}