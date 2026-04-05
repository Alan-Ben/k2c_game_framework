using System;
using System.Collections.Generic;
using ALPackage;
using Common.BuildingObj;
using DG.Tweening;
using GC2GS.p004_PlayerOp;
using GS2GC.p004_PlayerOp;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GOE
{
    /// <summary>
    /// 招聘体验界面
    /// </summary>
    public class GGUIWndHireMain : _ANPGGUIBasicWnd<GGUIMonoHireMain>
    {
        private static GGUIWndHireMain _g_instance;
        public static GGUIWndHireMain instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndHireMain();
                return _g_instance;
            }
        }

        //建筑id
        private long _m_lBuildingId;
        //招聘简历列表
        private List<HireRefObj> _m_lHireRefList;
        //第一个招聘简历item
        private GGUIWndHireResumeItem _m_wFirstHireResumeItem;
        //第二个招聘简历item
        private GGUIWndHireResumeItem _m_wSecondHireResumeItem;
        //当前简历索引
        private int _m_iCurResumeIndex;
        //同意的数量
        private int _m_iAgreeCount;
        //开始拖拽时手指的位置
        private Vector2 _m_vStartDragPos;
        //是否可以拖拽
        private bool _m_bCanDrag;
        //是否正在拖拽
        private bool _m_bIsDraging;
        //是否播放了右滑开始动画
        private bool _m_bIsPlayStartMoveRightAni;
        //是否播放了左滑开始动画
        private bool _m_bIsPlayStartMoveLeftAni;
        //屏蔽序号
        private int _m_iInputMaskSerialize;
        //处理移动
        private Tweener _m_tweener;
        //第一个item初始位置
        private Vector3 _m_initLocalPosition;
        //第一个item初始角度
        private Quaternion _m_initLocalRotation;

        public GGUIWndHireMain() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoHireMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoHireMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onShowWnd()
        {
            QueueMgr.instance.CloseRollBack(NodeESC_Const.C_QUEUE_ESC_HIRE_MAIN);
        }

        protected override void _onHideWnd()
        {
            QueueMgr.instance.OpenRollBack(NodeESC_Const.C_QUEUE_ESC_HIRE_MAIN);
            _m_wFirstHireResumeItem?.hideWnd();
            _m_wSecondHireResumeItem?.hideWnd();
            _m_tweener?.Kill();

            //如果同意数量大于0，则发送请求增加招聘人数
            if (_m_iAgreeCount > 0)
            {
                NPGSClientListener.sendRequestByLog(new GC2GS_004_025_ReqHireEmployee(_m_iAgreeCount, _m_lBuildingId),
                    new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_025_RetHireEmployee>(null));
            }

            //发送招聘完成
            WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.HIRE_MAIN_CLOSE);
        }

        protected override void _onReset()
        {
            _m_wFirstHireResumeItem?.resetWnd();
            _m_wSecondHireResumeItem?.resetWnd();
            _m_tweener?.Kill();
        }

        protected override void _onDiscard()
        {
            _m_tweener?.Kill();
            _m_lBuildingId = 0;
            _m_lHireRefList?.Clear();
            _m_lHireRefList = null;

            _m_wFirstHireResumeItem?.discard();
            _m_wFirstHireResumeItem = null;
            _m_wSecondHireResumeItem?.discard();
            _m_wSecondHireResumeItem = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnSkip, _onClickSkip);
            ALUGUICommon.uncombineBtnClick(wnd.btnAgree, _onClickAgree);
            ALUGUICommon.uncombineBtnClick(wnd.btnRefuse, _onClickRefuse);
            ALUGUICommon.uncombineBeginDrag(wnd.btnDrag, _onBeginDrag);
            ALUGUICommon.uncombineDrag(wnd.btnDrag, _onDrag);
            ALUGUICommon.uncombineEndDrag(wnd.btnDrag, _onEndDrag);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoFirstHireResumeItem != null)
            {
                _m_wFirstHireResumeItem = new GGUIWndHireResumeItem(wnd.monoFirstHireResumeItem);
                if(_m_wFirstHireResumeItem.wnd != null && _m_wFirstHireResumeItem.wnd.gameObject != null)
                {
                    _m_initLocalPosition = _m_wFirstHireResumeItem.wnd.gameObject.transform.localPosition;
                    _m_initLocalRotation = _m_wFirstHireResumeItem.wnd.gameObject.transform.localRotation;
                }
            }

            if(wnd.monoSecondHireResumeItem != null)
                _m_wSecondHireResumeItem = new GGUIWndHireResumeItem(wnd.monoSecondHireResumeItem);

            ALUGUICommon.combineBtnClick(wnd.btnSkip, _onClickSkip);
            ALUGUICommon.combineBtnClick(wnd.btnAgree, _onClickAgree);
            ALUGUICommon.combineBtnClick(wnd.btnRefuse, _onClickRefuse);
            ALUGUICommon.combineBeginDrag(wnd.btnDrag, _onBeginDrag);
            ALUGUICommon.combineDrag(wnd.btnDrag, _onDrag);
            ALUGUICommon.combineEndDrag(wnd.btnDrag, _onEndDrag);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_buildingId"></param>
        public void setInfo(long _buildingId)
        {
            _m_lBuildingId = _buildingId;
            _m_bCanDrag = true;
            _m_bIsDraging = false;
            _m_iCurResumeIndex = 0;
            _m_iAgreeCount = 0;
           _m_lHireRefList = new List<HireRefObj>();
            if (GRefdataCoreMgr.instance.hireRefCore.refList != null)
                _m_lHireRefList.AddRange(GRefdataCoreMgr.instance.hireRefCore.refList);

            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            //刷新简历数量
            ALUGUICommon.setLabelTxt(wnd.txtResumeNum,
                TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num,
                    _m_lHireRefList.Count - _m_iCurResumeIndex, _m_lHireRefList.Count));

            //筛选简历item
            HireRefObj firstHireRef = _m_lHireRefList != null && _m_lHireRefList.Count > _m_iCurResumeIndex
                ? _m_lHireRefList[_m_iCurResumeIndex]
                : null;
            if(firstHireRef != null)
            {
                _m_wFirstHireResumeItem?.showWnd();
                _m_wFirstHireResumeItem?.setInfo(firstHireRef);
            }
            HireRefObj secondHireRef = _m_lHireRefList != null && _m_lHireRefList.Count > _m_iCurResumeIndex + 1
                ? _m_lHireRefList[_m_iCurResumeIndex + 1]
                : null;
            if (secondHireRef != null)
            {
                _m_wSecondHireResumeItem?.showWnd();
                _m_wSecondHireResumeItem?.setInfo(secondHireRef);
            }

            //剩余1个简历时的显示状态
            ALUGUICommon.setGameObjEnable(wnd.goOneLeftShowList, secondHireRef == null);
            ALUGUICommon.setGameObjEnable(wnd.goOneLeftHideList, secondHireRef != null);

            //跳过按钮显隐状态
            ALUGUICommon.setGameObjEnable(wnd.goSkip, _m_iCurResumeIndex >= wnd.showSkipAfterOpNum);
        }

        #region 处理移动

        /// <summary>
        /// 处理拖拽移动
        /// </summary>
        /// <param name="_isAgree"></param>
        /// <param name="_dragDistanceX"></param>
        private void _dealDragMove(bool _isAgree, float _dragDistanceX)
        {
            if (wnd == null || _m_wFirstHireResumeItem == null || _m_wFirstHireResumeItem.wnd == null || _m_wFirstHireResumeItem.wnd.gameObject == null)
                return;

            //设置item显隐状态
            _m_wFirstHireResumeItem?.setAgreeAndRefuse(_isAgree, !_isAgree);

            //获取目标点
            RectTransform targetPoint = _isAgree ? wnd.rightTargetPoint : wnd.leftTargetPoint;
            if (targetPoint == null)
                return;

            //当前拖拽距离与目标点的距离比率
            float ratioValue = _dragDistanceX / targetPoint.localPosition.x;
            if (ratioValue < 0)
                ratioValue = -ratioValue;

            float targetX = _dragDistanceX + _m_initLocalPosition.x;
            float targetY = targetPoint.localPosition.y * ratioValue + _m_initLocalPosition.y;
            float targetRotationZ = (_isAgree ? 
                targetPoint.localRotation.eulerAngles.z : 
                targetPoint.localRotation.eulerAngles.z - 360f) * ratioValue + _m_initLocalRotation.eulerAngles.z;
            _m_wFirstHireResumeItem.wnd.gameObject.transform.localPosition = new Vector3(targetX, targetY);
            _m_wFirstHireResumeItem.wnd.gameObject.transform.localRotation = Quaternion.Euler(0, 0, targetRotationZ);
        }

        /// <summary>
        /// 处理自动移动
        /// </summary>
        /// <param name="_isAgree">是否同意</param>
        /// <param name="_moveToStart">是否移动回起点</param>
        private void _dealAutoMove(bool _isAgree, bool _moveToStart, float _moveTimeSec, Action _onComplete = null)
        {
            if (wnd == null || _m_wFirstHireResumeItem == null)
                return;

            //设置不可拖动状态
            _m_bCanDrag = false;

            //增加同意数量
            if (_isAgree && !_moveToStart)
                _m_iAgreeCount++;

            //开始位置
            RectTransform startPoint = _m_wFirstHireResumeItem.rectTransform;
            //获取目标点
            RectTransform targetPoint = _isAgree ? wnd.rightTargetPoint : wnd.leftTargetPoint;
            if (targetPoint == null)
                return;

            //移动距离
            float distanceX = startPoint.localPosition.x;
            _m_iInputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            _m_tweener?.Kill();
            _m_tweener = DOTween.To(() => distanceX, _v =>
            {
                distanceX = _v;
                _dealDragMove(_isAgree, distanceX);
            }, _moveToStart ? 0 : targetPoint.localPosition.x, _moveTimeSec).SetEase(Ease.Linear).OnComplete(() =>
            {
                //如果不是移动回起点，则更新当前简历索引
                if (!_moveToStart)
                {
                    _m_iCurResumeIndex++;
                    //把第一个item重置回原本的位置，并且刷新显示
                    if (_m_lHireRefList != null && _m_iCurResumeIndex < _m_lHireRefList.Count && _m_wFirstHireResumeItem != null)
                    {
                        _m_wFirstHireResumeItem.setInfo(_m_lHireRefList[_m_iCurResumeIndex]);
                        _m_wFirstHireResumeItem.setAgreeAndRefuse(false, false);
                        if (_m_wFirstHireResumeItem.wnd != null && _m_wFirstHireResumeItem.wnd.gameObject != null && _m_wFirstHireResumeItem.wnd.gameObject.transform != null)
                        {
                            _m_wFirstHireResumeItem.wnd.gameObject.transform.localPosition = _m_initLocalPosition;
                            _m_wFirstHireResumeItem.wnd.gameObject.transform.localRotation = _m_initLocalRotation;
                        }
                    }
                    //如果是同意，上浮提示招聘人数增加
                    if (_isAgree)
                    {
                        BuildingRefObj buildingRefObj = GRefdataCoreMgr.instance.buildingRefCore.getRef(_m_lBuildingId);
                        NPGUIAddSceneCenterTip.instance.showTextTip(TextTranslate.instance.getLanguage(TransKeyConst.building_getEmployeeCount_str_num, buildingRefObj?.name, 1));
                        //播放结束右滑动画
                        wnd?.moveAniTypeInfo?.forcePlay(EHireResumeMoveAniType.END_MOVE_RIGHT);
                    }
                    else//播放结束左滑动画
                        wnd?.moveAniTypeInfo?.forcePlay(EHireResumeMoveAniType.END_MOVE_LEFT);

                    //播放新简历摆正动画
                    wnd?.aniStraighten?.forcePlay();
                }
                //刷新窗口
                _refreshWnd();
                //关闭屏蔽
                MainCameraMono.selfInstance.closeAllInputMask(_m_iInputMaskSerialize);
                //完成回调
                _onComplete?.Invoke();

                //如果当前简历列表已经全处理完了，直接关闭窗口
                if (_m_lHireRefList == null || _m_iCurResumeIndex >= _m_lHireRefList.Count)
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_HIRE_MAIN);
            });
        }

        #endregion


        #region 点击事件

        //点击跳过
        private void _onClickSkip(GameObject obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_HIRE_MAIN);
        }

        //点击同意
        private void _onClickAgree(GameObject obj)
        {
            if (wnd == null || _m_bIsDraging)
                return;

            wnd?.moveAniTypeInfo?.forcePlay(EHireResumeMoveAniType.START_MOVE_RIGHT);
            _dealAutoMove(true, false, wnd.clickAutoMoveTime);
        }

        //点击拒绝
        private void _onClickRefuse(GameObject obj)
        {
            if (wnd == null || _m_bIsDraging)
                return;

            wnd?.moveAniTypeInfo?.forcePlay(EHireResumeMoveAniType.START_MOVE_LEFT);
            _dealAutoMove(false, false, wnd.clickAutoMoveTime);
        }

        #endregion

        #region 拖拽事件

        //开始拖拽
        private void _onBeginDrag(PointerEventData _eventData)
        {
            if (wnd == null || _eventData == null)
                return;

            _m_bIsDraging = true;
            _m_bCanDrag = true;
            _m_bIsPlayStartMoveRightAni = false;
            _m_bIsPlayStartMoveLeftAni = false;
            _m_vStartDragPos = _eventData.position;
        }

        //拖拽中
        private void _onDrag(PointerEventData _eventData)
        {
            if (wnd == null || _eventData == null || !_m_bCanDrag)
                return;

            float distanceX = _eventData.position.x - _m_vStartDragPos.x;
            if (distanceX > 0)
            {
                //往右拖拽，如果拖拽距离到达临界距离，则自动移动
                if (distanceX < wnd.moveRightCriticalDistance)
                {
                    _dealDragMove(true, distanceX);
                    if (!_m_bIsPlayStartMoveRightAni)
                    {
                        wnd?.moveAniTypeInfo?.forcePlay(EHireResumeMoveAniType.START_MOVE_RIGHT);
                        _m_bIsPlayStartMoveRightAni = true;
                        _m_bIsPlayStartMoveLeftAni = false;
                    }
                }
                else
                    _dealAutoMove(true, false, wnd.overCriticalAutoMoveTime);
            }

            if(distanceX < 0)
            {
                //往左拖拽
                if (-distanceX < wnd.moveLeftCriticalDistance)
                {
                    _dealDragMove(false, distanceX);
                    if (!_m_bIsPlayStartMoveLeftAni)
                    {
                        wnd?.moveAniTypeInfo?.forcePlay(EHireResumeMoveAniType.START_MOVE_LEFT);
                        _m_bIsPlayStartMoveLeftAni = true;
                        _m_bIsPlayStartMoveRightAni = false;
                    }
                }
                else
                    _dealAutoMove(false, false, wnd.overCriticalAutoMoveTime);
            }
        }

        //结束拖拽
        private void _onEndDrag(PointerEventData _eventData)
        {
            if (wnd == null || _eventData == null)
                return;

            _m_bIsDraging = false;
            float distanceX = _eventData.position.x - _m_vStartDragPos.x;

            //如果可以拖拽，说明没有触发自动移动，这时需要自动移动回去初始位置
            if (_m_bCanDrag)
            {
                bool isAgree = distanceX > 0;
                float moveTimeSec = isAgree
                    ? (distanceX / wnd.moveRightCriticalDistance * wnd.autoMoveBackTime)
                    : (-distanceX / wnd.moveRightCriticalDistance * wnd.autoMoveBackTime);
                _dealAutoMove(isAgree, true, moveTimeSec);
                if(isAgree)
                    wnd?.moveAniTypeInfo?.forcePlay(EHireResumeMoveAniType.END_MOVE_RIGHT);
                else
                    wnd?.moveAniTypeInfo?.forcePlay(EHireResumeMoveAniType.END_MOVE_LEFT);
            }
        }

        #endregion
    }
}