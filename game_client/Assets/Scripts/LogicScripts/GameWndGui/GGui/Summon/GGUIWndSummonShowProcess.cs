using System;
using ALPackage;
using NPEnum;
using Spine;

namespace GOE
{
    /// <summary>
    /// 抽卡表现流程
    /// </summary>
    public class GGUIWndSummonShowProcess : _ANPGGUIBasicWnd<GGUIMonoSummonShowProcess>
    {
        private static GGUIWndSummonShowProcess _g_instance;
        public static GGUIWndSummonShowProcess instance { get { return _g_instance ??= new GGUIWndSummonShowProcess(); } }

        private long _m_lShowSerializeId;
        private long _m_lVideoMonitorSerializeOp;
        
        private GGUIWndSimpleVideo _m_wSimpleVideoWnd;
        
        public GGUIWndSummonShowProcess() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoSummonShowProcess.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoSummonShowProcess.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            if(wnd.monoSimpleVideo != null)
                _m_wSimpleVideoWnd = new GGUIWndSimpleVideo(wnd.monoSimpleVideo);

            if (wnd.videoConfigList != null)
            {
                // 对wnd.videoConfigList按照quality品质进行排序，quality值越大，优先级越高
                wnd.videoConfigList.Sort((a, b) => b.quality.CompareTo(a.quality));
            }
            
            // _setSpineShow(wnd.idleSpineConfig, true);
        }
        
        protected override void _onDiscard()
        {
            _m_wSimpleVideoWnd?.discard();
            _m_wSimpleVideoWnd = null;
        }
        
        protected override void _onShowWnd()
        {
            _m_lShowSerializeId = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerializeId = ALSerializeOpMgr.next();
            _m_lVideoMonitorSerializeOp = ALSerializeOpMgr.next();
            
            _m_wSimpleVideoWnd?.hideWnd();

            //抽卡表现完成
            WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.SUMMON_SHOW_DONE);
        }

        protected override void _onReset()
        {
            _m_wSimpleVideoWnd?.reset();
        }

        /// <summary>
        /// 是否抽到大奖
        /// </summary>
        /// <param name="_isTenDraw"></param>
        public void setData(bool _isTenDraw, GS2GC.p007_CommOp.GS2GC_007_026_RetGachaRoll _msg)
        {
            if (wnd == null || _msg == null)
            {
                _closeWnd();
                return;
            }

            // ESummonResultType resultType = ESummonResultType.NONE;
            // bool hasGreatReward = GRefdataCoreMgr.instance.gachaCheckHasGreatReward(_msg.getPoolItemIdList());
            // if (_isTenDraw)
            // {
            //     if (hasGreatReward)
            //         resultType = ESummonResultType.TEN_DRAW_GREAT_REWARD;
            //     else
            //         resultType = ESummonResultType.TEN_DRAW_NORMAL_REWARD;
            // }
            // else
            // {
            //     if(hasGreatReward)
            //         resultType = ESummonResultType.ONE_DRAW_GREAT_REWARD;
            //     else
            //         resultType = ESummonResultType.ONE_DRAW_NORMAL_REWARD;
            // }
            //
            // _dealShowProgress(wnd.getSummonProcessShowConfig(resultType));

            EQuality getItemMaxQuality = EQuality.NONE;//获取到的物品中最高品质
            if (_msg.getItemList() != null)
            {
                foreach (var item in _msg.getItemList())
                {
                    if(item == null)
                        continue;
                    
                    EQuality quality = GCommon.getItemQuality((ENPItemType)item.getItemType(), item.getSubId());
                    if(quality > getItemMaxQuality)
                        getItemMaxQuality = quality;
                }
            }
            
            _dealShowProgress(_getNeedPlayVideoConfig(getItemMaxQuality));
        }

        private void _dealShowProgress(SummonVideoConfig _videoConfig)
        {
            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess((_complete) =>
                {
                    _playVideo(_videoConfig, _complete);
                })
                .addProcess(_closeWnd);
         
            process.dealProcess();
        }
        
        /// <summary>
        /// 获取需要播放的视频配置
        /// </summary>
        private SummonVideoConfig _getNeedPlayVideoConfig(EQuality _quality)
        {
            if (wnd == null || wnd.videoConfigList == null)
                return null;
            
            // 因为wnd.videoConfigList已经按照quality从大到小排序, 所以只需要按顺序找到第一个quality小于等于当前quality的配置即可
            foreach (var config in wnd.videoConfigList)
            {
                if (config != null && config.quality <= _quality)
                    return config;
            }

            return null;
        }
        
        private void _playVideo(SummonVideoConfig _videoConfig, Action _playDone)
        {
            if (wnd == null || _m_wSimpleVideoWnd == null || _videoConfig == null || _videoConfig.videoClipIndex == null || !_videoConfig.videoClipIndex.isValid())
            {
                _playDone?.Invoke();
                return;
            }

            _m_lVideoMonitorSerializeOp = ALSerializeOpMgr.next();
            long serializeOp = _m_lVideoMonitorSerializeOp;

            if (_m_wSimpleVideoWnd != null && wnd != null)
            {
                _m_wSimpleVideoWnd.showWnd();
                _m_wSimpleVideoWnd.setVideoClip(_videoConfig.videoClipIndex);
                _m_wSimpleVideoWnd.playVideo();
                _m_wSimpleVideoWnd.monitorPlay(() =>
                {
                    if(serializeOp != _m_lVideoMonitorSerializeOp)
                        return;
                    
                    _playDone?.Invoke();
                });
            }
        }
        
        // /// <summary>
        // /// 开始表现过程
        // /// </summary>
        // private void _dealShowProgress(SummonProcessShowConfig _showConfig)
        // {
        //     if (_showConfig == null)
        //     {
        //         _closeWnd();
        //         return;
        //     }
        //     
        //     ALProcess process = ALProcess.CreateProcess();
        //     process
        //         .addDelegateProcess((_complete) =>
        //         {
        //             _setSpineShow(_showConfig.spineShowConfig, false, _complete);
        //         })
        //         .addDelegateProcess((_complete) =>
        //         {
        //             if(wnd == null)//若这时wnd为空, 也不需要继续表现了
        //                 return;
        //             
        //             _playWndAnimation(_showConfig.afterSpineShowAnimationName, _complete);
        //         })
        //         .addProcess(_closeWnd);
        //  
        //     process.dealProcess();
        // }
        //
        // /// <summary>
        // /// 设置spine表现
        // /// </summary>
        // /// <param name="_onShowDone"></param>
        // private void _setSpineShow(SummonSpineShowConfig _spineShowConfig, bool _isLoop, Action _onShowDone = null)
        // {
        //     if (_spineShowConfig == null || wnd == null || wnd.spineGraphic == null)
        //     {
        //         _onShowDone?.Invoke();
        //         return;
        //     }
        //
        //     try
        //     {
        //         if (!string.IsNullOrEmpty(_spineShowConfig.spineSKinName))
        //         {
        //             Skeleton skeleton = wnd.spineGraphic.Skeleton;
        //             Skin newSKin = skeleton.Data.FindSkin(_spineShowConfig.spineSKinName);
        //             if (newSKin != null)
        //             {
        //                 skeleton.SetSkin(newSKin);
        //                 skeleton.SetSlotsToSetupPose();
        //                 wnd.spineGraphic.UpdateMesh();
        //             }    
        //         }
        //     
        //         if (_spineShowConfig.spineAnimationList != null && _spineShowConfig.spineAnimationList.Count > 0)
        //         {
        //             SummonSpineAnimationConfig spineAnimationConfig = _spineShowConfig.spineAnimationList.GetRandomItem();
        //             _playSpineAnimation(spineAnimationConfig, _isLoop, () =>
        //             {
        //                 Action action = _onShowDone;
        //                 _onShowDone = null;
        //                 action?.Invoke();
        //             });
        //         }
        //         else
        //         {
        //             Action action = _onShowDone;
        //             _onShowDone = null;
        //             action?.Invoke();
        //         }
        //     }
        //     catch (Exception e)
        //     {
        //         Debug.LogError($"[GGUIWndSummonShowProcess _setSpineShow] error: {e}");
        //         _onShowDone?.Invoke();
        //     }
        // }
        //
        // /// <summary>
        // /// 播放spine动画
        // /// </summary>
        // private void _playSpineAnimation(SummonSpineAnimationConfig _spineAnimationConfig, bool _isLoop, Action _onPlayDone)
        // {
        //     if (wnd == null || wnd.spineGraphic == null || wnd.spineGraphic.AnimationState == null || _spineAnimationConfig == null)
        //     {
        //         _onPlayDone?.Invoke();
        //         return;
        //     }
        //
        //     long serializeId = _m_lShowSerializeId;
        //     wnd.spineGraphic.AnimationState.SetAnimation(_spineAnimationConfig.trackIndex, _spineAnimationConfig.spineAnimationName, _isLoop);
        //     ALCommonTaskController.CommonActionAddMonoTask(() =>
        //     {
        //         if(_m_lShowSerializeId != serializeId)
        //             return;
        //         
        //         _onPlayDone?.Invoke();
        //     }, _spineAnimationConfig.animationTime);
        // }

        /// <summary>
        /// 播放窗口动画
        /// </summary>
        /// <param name="_animationName"></param>
        private void _playWndAnimation(string _animationName, Action _onPlayDone)
        {
            if (wnd == null || wnd.wndAnimation == null || string.IsNullOrEmpty(_animationName))
            {
                _onPlayDone?.Invoke();
                return;
            }

            wnd.wndAnimation.Play(_animationName, _onPlayDone);
        }
        
        private void _closeWnd()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_SUMMON_SHOW_PROCESS);
        }
    }
}