using System;
using ALPackage;
using System.Collections.Generic;
using NPCommon;
using UnityEngine;
using UnityEngine.UI;


namespace GOE
{
    [System.Serializable]
	public class NPGGUICustomFollowingQuestMono
	{
	    [ALHeader("任务步骤状态")]
	    public ENPQuestStepStatusEnum status;
	    [ALHeader("对应Go List")]
	    public List<GameObject> goList;
	    [ALHeader("对应文本")]
	    public Text targetTxt;
	}

	/// <summary>
	/// 显示正在跟踪的任务
	/// </summary>
	public class NPGGUICustomMonoQuestFollowingQuest : MonoBehaviour
    {
		[ALHeader("任务序号")]
        public Text txtQuestNum;
		[ALHeader("当前无追踪任务时显示的物体")]
		public List<GameObject> goListShowOnEmpty;
		[ALHeader("当前无追踪任务时隐藏的物体")]
		public List<GameObject> goListHideOnEmpty;

        [ALHeader("完成所有任务时需要显示的GO列表")]
        public List<GameObject> goFinishAllQuestShowList;
        [ALHeader("完成所有任务时需要隐藏的GO列表")]
        public List<GameObject> goFinishAllQuestHideList;

        [ALHeader("任务进度文本")]
        public Text txtProgress;

		[ALHeader("任务状态列表")]
	    public List<NPGGUICustomFollowingQuestMono> statusList;

	    [ALHeader("完成任务按钮")]
	    public GameObject finishQuestBtn;
	    
	    [ALHeader("任务前往按钮")]
	    public GameObject gotoQuestBtn;

		[ALHeader("打开任务按钮")]
		public GameObject openQuestBtn;

        [ALHeader("限时任务倒计时文本")]
	    public Text leftTimeTxt;

	    [ALHeader("进行中的任务是限时任务显示Go List")]
	    public List<GameObject> leftTimeShowGoList;

	    [ALHeader("播放动画倒计时秒数")]
	    public int playAniSec;

	    [ALHeader("达到秒数时播放的动画")]
	    public Animation playAni;


        [ALHeader("接受新任务时播放的特效父节点")]
		public Transform addNewQuestSfxParent;
        [ALHeader("接受新任务时播放的特效id")]
		public long addNewQuestSfxId;
        [ALHeader("接受新任务时播放的音效id")]
		public long addNewQuestAudioId;

        [ALHeader("粒子开始位置")]
        public RectTransform particleStartRectTransform;
        [ALHeader("领奖特效父节点")]
        public Transform getRewardSfxParent;
        [ALHeader("领奖特效id")]
        public long getRewardSfxId;

        //该脚本是否使用于tip预制体上
        [NonSerialized]
        public bool isUseInTipPrefab = false;

        //是否播放了动画
        private bool _m_isPlay;

	    //是否需要刷新
	    private bool _m_needCheck;

	    //每秒检测数据状态的任务控制对象
	    private ALCommonEnableTaskController _m_tcTickTaskController;

        //音效资源实例id
        private long insAudioId = 0;

#if NP_GAME

		//当前显示的任务
		private _IFollowableQuest _m_curFollow;
        //特效容器
		private CommonUISfxObj _m_sfxObj;
        //领奖特效列表
        private List<CommonUISfxObj> _m_lGetRewardSfxObjList;

#endif
        private void Awake()
        {
#if NP_GAME
            ALUGUICommon.combineBtnClick(finishQuestBtn, _finishQuestBtnDidClick);
            ALUGUICommon.combineBtnClick(openQuestBtn, _openQuestBtnDidClick);
            ALUGUICommon.combineBtnClick(gotoQuestBtn, _gotoQuestBtnDidClick);
#endif
        }

        private void OnDestroy()
        {
#if NP_GAME
            ALUGUICommon.uncombineBtnClick(finishQuestBtn, _finishQuestBtnDidClick);
            ALUGUICommon.uncombineBtnClick(openQuestBtn, _openQuestBtnDidClick);
            ALUGUICommon.uncombineBtnClick(gotoQuestBtn, _gotoQuestBtnDidClick);
#endif
        }

        private void OnEnable()
        {
#if NP_GAME
            //非入口tip挂载了这个脚本，需要标记当前任务入口已经显示
            if (!isUseInTipPrefab)
            {
                NPPlayer.instance.questComp.questEntryIsShow = true;
				WinMsg.SendMsg(WinMsgType.ON_QUEST_ENTRY_SHOW);
            }

            _refresh(); 
			_addListenMsg();
			WinMsg.RegisterMsgAct(WinMsgType.QUEST_FOLLOW, _onQuestChg);
			WinMsg.RegisterMsgAct(WinMsgType.ON_ACCEPT_NEW_QUEST_SHOW, _onAcceptQuestShow);
			WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_RET_MAIN_QUEST_REWARD, _simulateTetMainQuestReward);
			WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_MAIN_QUEST_GO_TO, _simulateClickMainQuestGoTo);
#endif
	    }

        private void OnDisable()
        {
#if NP_GAME
            //非入口tip挂载了这个脚本，需要标记当前任务入口已经隐藏
            if (!isUseInTipPrefab)
                NPPlayer.instance.questComp.questEntryIsShow = false;

            if (_m_sfxObj != null)
	            _m_sfxObj.forceDiscard();
            _m_sfxObj = null;

            if (_m_lGetRewardSfxObjList != null)
            {
                foreach (CommonUISfxObj sfxObj in _m_lGetRewardSfxObjList)
                {
                    sfxObj?.forceDiscard();
                }
                _m_lGetRewardSfxObjList.Clear();
                _m_lGetRewardSfxObjList = null;
            }

            PlayAudioMgr.instance.stopClip(insAudioId);

			_stopTask();
			_removeListenMsg();
			WinMsg.UnregisterMsgAct(WinMsgType.QUEST_FOLLOW, _onQuestChg);
			WinMsg.UnregisterMsgAct(WinMsgType.ON_ACCEPT_NEW_QUEST_SHOW, _onAcceptQuestShow);
			WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_RET_MAIN_QUEST_REWARD, _simulateTetMainQuestReward);
			WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_MAIN_QUEST_GO_TO, _simulateClickMainQuestGoTo);
#endif
		}

#if NP_GAME

		/// <summary>
		/// 添加监听
		/// </summary>
		private void _addListenMsg()
        {
			//防止重复添加，先移除
			_removeListenMsg();

			if (_m_curFollow != null)
			{
				List<WinMsgType> listenMsgList = _m_curFollow.getListenMsgList();
				if (listenMsgList != null)
				{
					for (int i = 0; i < listenMsgList.Count; i++)
					{
						WinMsg.RegisterMsgAct(listenMsgList[i], _onQuestChg);
					}
				}
			}
		}

		/// <summary>
		/// 移除监听
		/// </summary>
		private void _removeListenMsg()
        {
			if (_m_curFollow != null)
			{
				List<WinMsgType> listenMsgList = _m_curFollow.getListenMsgList();
				if (listenMsgList != null)
				{
					for (int i = 0; i < listenMsgList.Count; i++)
					{
						WinMsg.UnregisterMsgAct(listenMsgList[i], _onQuestChg);
					}
				}
			}
		}

		/// <summary>
		/// 任务状态变更
		/// </summary>
		private void _onQuestChg()
        {
			//判断是否已经准备检测，避免当帧过多检测
			if (_m_needCheck)
				return;

			_m_needCheck = true;

			//到管理对象中进行处理
			ALCommonActionMonoTask.addNextFrameTask(_refresh);
		}

		/// <summary>
		/// 接受支线任务时需要展示效果
		/// </summary>
        private void _onAcceptQuestShow()
        {
            if(null != _m_sfxObj)
	            _m_sfxObj.forceDiscard();
            
			//播放特效
			_m_sfxObj = PlaySfxMgr.instance.playUISfx(addNewQuestSfxId, addNewQuestSfxParent);

			//播放音效
            insAudioId = PlayAudioMgr.instance.playClip(addNewQuestAudioId);
		}

	    /// <summary>
	    /// 刷新任务内容
	    /// </summary>
	    private void _refresh()
	    {
	        _m_needCheck = false;

	        //先取消限时任务相关展示
	        ALUGUICommon.setGameObjEnable(leftTimeShowGoList, false);
			_stopTask();

			//当前追踪任务为空，或者已经改变了，尝试获取最新数据
			_IFollowableQuest newestFollow = QuestFollowMgr.instance.curFollow;
			if (_m_curFollow == null || !_m_curFollow.Equals(newestFollow))
            {
				_removeListenMsg();
				_m_curFollow = newestFollow;
				_addListenMsg();
			}

            if (_m_curFollow != null)
            {
				//设置排序id
                ALUGUICommon.setLabelTxt(txtQuestNum, _m_curFollow.sortId);
				//设置没有任务的显隐
                ALUGUICommon.setGameObjEnable(goListShowOnEmpty, false);
				ALUGUICommon.setGameObjEnable(goListHideOnEmpty, true);
                //设置完成所有任务的显隐
                ALUGUICommon.setGameObjEnable(goFinishAllQuestHideList,true);
				ALUGUICommon.setGameObjEnable(goFinishAllQuestShowList,false);

				ENPQuestStepStatusEnum status = _m_curFollow.questStepStatus;
				string showStr = _m_curFollow.showStr;
				string progressStr = _m_curFollow.progressStr;
				NPGGUICustomFollowingQuestMono statusMono = null;
				for (int i = 0; i < statusList.Count; i++)
				{
					if (null == statusList[i])
						continue;

					if(status == statusList[i].status)
                        statusMono = statusList[i];

					//设置显隐
					ALUGUICommon.setGameObjEnable(statusList[i].goList, false);

					if (status == ENPQuestStepStatusEnum.QUEST_NONE)
						continue;

					//正在进行中任务，显示标题和进度
                    if (status == ENPQuestStepStatusEnum.QUEST_ISGOING)
                        ALUGUICommon.setLabelTxt(statusList[i].targetTxt, TextTranslate.instance.getLanguage(TransKeyConst.quest_nameAndProgress_str_str, showStr, progressStr));
					else
						ALUGUICommon.setLabelTxt(statusList[i].targetTxt, showStr);
				}
				if(statusMono != null)
                    ALUGUICommon.setGameObjEnable(statusMono.goList, true);

				//如果是限时任务
				if (_m_curFollow.expireTimeTagS > 0)
				{
					ALUGUICommon.setGameObjEnable(leftTimeShowGoList, true);
					_startTask();
				}

                //设置任务进度文本
                ALUGUICommon.setLabelTxt(txtProgress, progressStr);
            }
            else
			{
				ALUGUICommon.setGameObjEnable(leftTimeShowGoList, false);
                //设置完成所有任务的显隐
                ALUGUICommon.setGameObjEnable(goFinishAllQuestHideList, false);
                ALUGUICommon.setGameObjEnable(goFinishAllQuestShowList, true);

                //当前没有追踪任务显隐
                ALUGUICommon.setGameObjEnable(goListHideOnEmpty, false);
				ALUGUICommon.setGameObjEnable(goListShowOnEmpty, true);
			}
            //刷新入口红点
            NPPlayer.instance.questComp.refreshRedTip();
		}

	    //开启限时任务计时
	    private void _startTask()
	    {
	        //停止原先任务
	        _m_tcTickTaskController.setDisable();

	        //开启任务进行数据逻辑的处理
	        _m_tcTickTaskController = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_refreshLeftTime, 1f);
	    }

	    //停止倒计时
	    private void _stopTask()
	    {
	        //重置动画播放
	        _m_isPlay = false;

	        _m_tcTickTaskController.setDisable();
	    }
	    /// <summary>
	    /// 刷新限时任务倒计时
	    /// </summary>
	    private void _refreshLeftTime()
	    {
			if (_m_curFollow == null)
				return;

			long leftTimeMs = _m_curFollow.expireTimeTagS*1000 - FpsAndPingMgr.instance.serverTimeTag;

			if (leftTimeMs > 0)
	        {
	            ALUGUICommon.setLabelTxt(leftTimeTxt, TimeUtil.millisecondsToTime_hms(leftTimeMs));
	            if (leftTimeMs <= (playAniSec*1000) && !_m_isPlay)
	            {
	                playAni.Play();
	                _m_isPlay = true;
	            }
	        }
	        else
	        {
	            _stopTask();
	            _refresh();
	        }
	    }

        /// <summary>
        /// 播放领奖特效
        /// </summary>
        private void _playGetRewardSfx()
        {
            if (_m_lGetRewardSfxObjList == null)
                _m_lGetRewardSfxObjList = new List<CommonUISfxObj>();

            //播放处理成功特效
            if (getRewardSfxId > 0 && getRewardSfxParent != null)
            {
                CommonUISfxObj newSfxObj = PlaySfxMgr.instance.playUISfx(getRewardSfxId, getRewardSfxParent);
                _m_lGetRewardSfxObjList.Add(newSfxObj);
            }
        }

        //模拟点击主线任务奖励
        private void _simulateTetMainQuestReward()
	    {
		    _finishQuestBtnDidClick(null);
	    }

        //模拟点击主线任务前往
        private void _simulateClickMainQuestGoTo()
		{
			_gotoQuestBtnDidClick(null);
        }

        /// <summary>
        /// 点击完成按钮
        /// </summary>
        private void _finishQuestBtnDidClick(GameObject _go)
		{
			if (_m_curFollow == null)
				return;

			//不是可领奖状态不处理
			if (_m_curFollow.questStepStatus != ENPQuestStepStatusEnum.QUEST_CANGET)
				return;

			_m_curFollow.dealFinish(_retGetReward);
	    }

		/// <summary>
		/// 点击前往按钮
		/// </summary>
		/// <param name="_go"></param>
	    private void _gotoQuestBtnDidClick(GameObject _go)
	    {
		    if (null == _m_curFollow)
			    return;
			    
		    _m_curFollow.dealQuestGoto();
			WinMsg.SendMsg(WinMsgType.ON_QUEST_CLICK_GOTO);
	    }

		/// <summary>
		/// 点击打开按钮
		/// </summary>
		/// <param name="_go"></param>
		private void _openQuestBtnDidClick(GameObject _go)
		{
            GCommon.enterUIMainNodeShow(ESysSceneType.QUEST);
        }

		/// <summary>
		/// 领取奖励回包
		/// </summary>
		/// <param name="_itemList"></param>
        private void _retGetReward(List<NPCommon_ItemInfo> _itemList)
        {
            if (particleStartRectTransform == null || _itemList == null || _itemList.Count == 0)
                return;

            //展示粒子效果
			GCommon.showItemParticle(_itemList, particleStartRectTransform);

            //播放领奖特效
            _playGetRewardSfx();

            //领取完主线任务奖励
            WinMsg.SendMsg(WinMsgType.ON_GET_QUEST_REWARD);
        }
#endif
	}
}