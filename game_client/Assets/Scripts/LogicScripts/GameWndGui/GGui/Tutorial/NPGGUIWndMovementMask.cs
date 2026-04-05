using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using System;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 新手引导运动遮罩窗口
    /// </summary>
    public class NPGGUIWndMovementMask : _ANPGGUIBasicWnd<NPGGUIMonoMovementMask>
    {
        private static NPGGUIWndMovementMask _g_instance = new NPGGUIWndMovementMask();
        public static NPGGUIWndMovementMask instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPGGUIWndMovementMask();
                return _g_instance;
            }
        }

        /**************
         * 窗口每个方向对象的信息
         **/
        public class NPGGUIWndMovementElementInfo
        {
            public ContractionController controller;
            public LayoutElement element;

            public NPGGUIWndMovementElementInfo()
            {
                controller = null;
                element = null;
            }
        }

        private int _m_iOpSerialize = 1;

        private List<NPGGUIWndMovementElementInfo> _m_lContractionControllerList;

        private float _m_iWidth;
        private float _m_iHeight;

        protected NPGGUIWndMovementMask()
            : base(EALUIWndLayer.TOP)
        {
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return NPGGUIMonoMovementMask.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoMovementMask.objName; } }

        public int opSerialize { get { return _m_iOpSerialize; } }

        /**************
        * 获取用于加载资源的管理对象
        **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        /******************
        * 显示窗口的事件函数
        **/
        protected override void _onShowWnd()
        {
        }

        /******************
        * 隐藏窗口的事件函数
        **/
        protected override void _onHideWnd()
        {
            _m_iOpSerialize++;
        }

        /******************
         * 重置窗口数据的事件函数
         **/
        protected override void _onReset()
        {
            _m_iOpSerialize++;
        }

        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {
            _m_iOpSerialize++;
        }

        /*************
        * 窗口初始化完成调用的函数
        * */
        protected override void _onWndInitDone()
        {
            _m_lContractionControllerList = new List<NPGGUIWndMovementElementInfo>();
            int enumCount = ALCommon.getEnumCount(typeof(TutorialMoveMaskElementType));
            for(int i = 0; i < enumCount; i++)
                _m_lContractionControllerList.Add(new NPGGUIWndMovementElementInfo());

            //遍历初始化element 
            if(null != wnd.elementList)
            {
                for(int i = 0; i < wnd.elementList.Length; i++)
                {
                    if(null == wnd.elementList[i])
                        continue;

                    _m_lContractionControllerList[(int)wnd.elementList[i].type].element = wnd.elementList[i].element;
                }
            }
        }

        //直接设置位置
        public void setFoucsTarget(RectTransform _rect)
        {
            if (wnd == null || wnd.elementList == null || null == _rect)
                return;

            _m_iWidth = rectTransform.rect.width;
            _m_iHeight = rectTransform.rect.height;

            //遍历处理element
            for (int i = 0; i < wnd.elementList.Length; i++)
            {
                //根据传入的Rect的UI坐标及长宽数据确定各模块最小长宽值变化曲线   //TODO想个办法统一Anchors设置
                dealElement(wnd.elementList[i].element
                    , GCommon.getUIRootPos(_rect, false)//找策划确认了, 引导遮罩一定是全屏的, guide_movementmask上要记得挂载脚本NPGGUICustomMonoChangeSizeToFullCanvas
                    , new Vector2(_rect.rect.size.x * _rect.localScale.x, _rect.rect.size.y * _rect.localScale.y)
                    , wnd.elementList[i].type
                    , true);

            }

            _m_iOpSerialize++;

            NPGGUIWndMovementMask.instance.refreshElement();
        }
        
        //计算并开始遮罩运动
        public void foucsTarget(RectTransform _rect)
        {
            if(wnd == null || wnd.elementList == null || null == _rect)
                return;

            _m_iWidth = rectTransform.rect.width;
            _m_iHeight = rectTransform.rect.height;

            //遍历处理element
            for(int i = 0; i < wnd.elementList.Length; i++)
            {
                //根据传入的Rect的UI坐标及长宽数据确定各模块最小长宽值变化曲线   //TODO想个办法统一Anchors设置
                dealElement(wnd.elementList[i].element
                    , GCommon.getUIRootPos(_rect, false)//找策划确认了, 引导遮罩一定是全屏的, guide_movementmask上要记得挂载脚本NPGGUICustomMonoChangeSizeToFullCanvas
                    , new Vector2(_rect.rect.size.x * _rect.localScale.x, _rect.rect.size.y * _rect.localScale.y), wnd.elementList[i].type);
            }

            _m_iOpSerialize++;

            //执行遮罩运动任务
            ALMonoTaskMgr.instance.addMonoTask(new NPMonoTaskMoveMask(_m_iOpSerialize, 1f));
        }

        //计算生成对应的ALRealTimeFloatFadeController
        private void dealElement(LayoutElement _layoutElement, Vector2 _vec, Vector2 _hightAndWight, TutorialMoveMaskElementType _type, bool _isFoucsDone = false)
        {
            float leftWidth = 0;
            float rightWidth = 0;
            if (_vec.x + _hightAndWight.x / 2 > _m_iWidth)
            {
                //右边超框了
                leftWidth = _m_iWidth - _hightAndWight.x;
                rightWidth = 0;
            }
            else if (_vec.x - _hightAndWight.x / 2 < 0)
            {
                //左边超框了
                leftWidth = 0;
                rightWidth = _m_iWidth - _hightAndWight.x;
            }
            else
            {
                leftWidth = _vec.x - _hightAndWight.x / 2;
                rightWidth = _m_iWidth - leftWidth - _hightAndWight.x;
            }

            float bottomHeight = 0;
            float topHeight = 0;
            if (_vec.y + _hightAndWight.y / 2 > _m_iHeight)
            {
                //上边超框了
                bottomHeight = _m_iHeight - _hightAndWight.y;
                topHeight = 0;
            }
            else if (_vec.y - _hightAndWight.y / 2 < 0)
            {
                //下边超框了
                bottomHeight = 0;
                topHeight = _m_iHeight - _hightAndWight.y;
            }
            else
            {
                bottomHeight = _vec.y - _hightAndWight.y / 2;
                topHeight = _m_iHeight - bottomHeight - _hightAndWight.y;
            }
            
   
            if(_type == TutorialMoveMaskElementType.LEFT)
                _m_lContractionControllerList[(int)_type].controller = _dealFloatFadeController(_layoutElement.minWidth, leftWidth, _type, _isFoucsDone);
            if(_type == TutorialMoveMaskElementType.RIGHT)
                _m_lContractionControllerList[(int)_type].controller = _dealFloatFadeController(_layoutElement.minWidth, rightWidth, _type, _isFoucsDone);
            if(_type == TutorialMoveMaskElementType.BOTTOM)
                _m_lContractionControllerList[(int)_type].controller = _dealFloatFadeController(_layoutElement.minHeight, bottomHeight, _type, _isFoucsDone);
            if(_type == TutorialMoveMaskElementType.TOP)
                _m_lContractionControllerList[(int)_type].controller = _dealFloatFadeController(_layoutElement.minHeight, topHeight, _type, _isFoucsDone);
        }

        public NPGGUIWndMovementElementInfo findCcWithType(TutorialMoveMaskElementType _type)
        {
            return _m_lContractionControllerList[(int)_type];
        }

        /************
         * 刷新当前节点的信息
         **/
        public void refreshElement()
        {
            float midW = _m_iWidth;
            float midH = _m_iHeight;

            NPGGUIWndMovementElementInfo tmpInfo = null;
            tmpInfo = findCcWithType(TutorialMoveMaskElementType.LEFT);
            if(null != tmpInfo)
            {
                //设置宽度
                tmpInfo.element.minWidth = tmpInfo.controller.curValue;
                //减少跨度
                midW -= tmpInfo.element.minWidth;
            }

            tmpInfo = findCcWithType(TutorialMoveMaskElementType.RIGHT);
            if(null != tmpInfo)
            {
                //设置宽度
                tmpInfo.element.minWidth = tmpInfo.controller.curValue;
                //减少跨度
                midW -= tmpInfo.element.minWidth;
            }

            tmpInfo = findCcWithType(TutorialMoveMaskElementType.TOP);
            if(null != tmpInfo)
            {
                //设置宽度
                tmpInfo.element.minHeight = tmpInfo.controller.curValue;
                //减少跨度
                midH -= tmpInfo.element.minHeight;
            }

            tmpInfo = findCcWithType(TutorialMoveMaskElementType.BOTTOM);
            if(null != tmpInfo)
            {
                //设置宽度
                tmpInfo.element.minHeight = tmpInfo.controller.curValue;
                //减少跨度
                midH -= tmpInfo.element.minHeight;
            }

            //设置中间
            tmpInfo = findCcWithType(TutorialMoveMaskElementType.MID);
            if(null != tmpInfo)
            {
                //设置宽度
                tmpInfo.element.minWidth = midW;
                tmpInfo.element.minHeight = midH;
            }
        }

        //根据传入的起始min值和目标min值确定ALRealTimeFloatFadeController
        private ContractionController _dealFloatFadeController(float _startNum, float _targetNum, TutorialMoveMaskElementType _type, bool _isFoucsDone)
        {
            //是否马上处理完
            if (_isFoucsDone)
            {
                return new ContractionController(_startNum, _targetNum, 0, 0, 0, 0, 500, _type);
            }
            else
            {
                return new ContractionController(_startNum, _targetNum, 0, 0.3f, 0, 0.2f, 500, _type);
            }        }

        /****************
        * 遮罩移动的定时任务
        **/
        protected class NPMonoTaskMoveMask : _IALBaseMonoTask
        {
            private int _m_iSerialize;
            private float _m_fEndTime;
            public NPMonoTaskMoveMask(int _serialize, float _time)
            {
                _m_iSerialize = _serialize;
                _m_fEndTime = Time.realtimeSinceStartup + _time;
            }

            public void deal()
            {
                if(_m_iSerialize != NPGGUIWndMovementMask.instance.opSerialize)
                    return;

                //调用界面刷新
                NPGGUIWndMovementMask.instance.refreshElement();

                //运动时间到则停止，否则下一帧继续执行
                if(Time.realtimeSinceStartup >= _m_fEndTime)
                    return;

                ALMonoTaskMgr.instance.addNextFrameTask(this);
            }
        }
    }

    public class ContractionController
    {

        /*********过渡阶段***********/

        /** 开始数值 */
        private float _m_fBinValue;
        /** 目标数值 */
        private float _m_fTargetValue;

        /** 开始运动的时间标记 */
        private float _m_fStartTimeTag;
        /** 初始化的速度 */
        private float _m_fSrcSpeed;
        /** 过渡总时间 */
        private float _m_fTotalTime;
        /** 过渡过程的加速时间 */
        private float _m_fFirstAccTime;
        /** 第一阶段的加速度 */
        private float _m_fFirstAccSpeed;
        /** 第二阶段的加速度 */
        private float _m_fSecAccSpeed;

        /*********回弹阶段***********/

        /** 回弹阶段的减速时间 */
        private float _m_fContractTime;
        /** 回弹阶段减速度值 */
        private float _m_fContractDesSpeed;
        /** 回弹阶段初始速度*/
        private float _m_fContractSrcSpeed;
        /** 回弹中心标记*/
        private bool _m_bIsMid;

        private TutorialMoveMaskElementType _m_eMaskElementType;

        /// <param name="_startNum">开始数值</param>
        /// <param name="_targetNum">目标数值</param>
        /// <param name="_srcSpeed">初始化的速度</param>
        /// <param name="_totalTime">过渡总时间</param>
        /// <param name="_firstAccSpeedTime">过渡过程的加速时间</param>
        /// <param name="_contractTime">回弹阶段减速时间</param>
        /// <param name="_contractSrcSpeed">回弹阶段初始速度</param>
        /// <param name="TutorialMoveMaskElementType">回弹类型标记</param>
        public ContractionController(float _startNum, float _targetNum, float _srcSpeed, float _totalTime, float _firstAccSpeedTime, float _contractTime, float _contractSrcSpeed, TutorialMoveMaskElementType _type)
        {
            _m_eMaskElementType = _type;
            _m_fBinValue = _startNum;
            _m_fTargetValue = _targetNum;

            _m_fContractTime = _contractTime;
            _m_fContractSrcSpeed = _contractSrcSpeed;
            _m_bIsMid = _m_eMaskElementType == TutorialMoveMaskElementType.MID;

            _m_fStartTimeTag = Time.realtimeSinceStartup;

            _m_fSrcSpeed = _srcSpeed;
            _m_fTotalTime = _totalTime;
            _m_fFirstAccTime = _firstAccSpeedTime;

            float moveDis = _m_fTargetValue - _m_fBinValue;
            float secAccTime = _m_fTotalTime - _m_fFirstAccTime;

            //计算回弹阶段减速度
            _m_fContractDesSpeed = _m_fContractSrcSpeed / _contractTime * 2;
            //计算加速度数值   如果第二阶段的时间小于0.0001f,那么整个过程都是加速的
            if(0.0001f > secAccTime)
            {
                //此时为加速运动
                _m_fSecAccSpeed = 0;
                _m_fFirstAccSpeed = 2 * (moveDis - (_m_fSrcSpeed * _m_fTotalTime)) / (_m_fTotalTime * _m_fTotalTime);
            }
            else if(0.0001f > _m_fFirstAccTime)
            {
                //若第二阶段时间小于0.0001f,此时为减速运动，初始速度需要重新计算
                //先计算加速度
                _m_fFirstAccSpeed = 0;
                _m_fSecAccSpeed = -(2 * moveDis) / (_m_fTotalTime * _m_fTotalTime);
                _m_fSrcSpeed = -_m_fSecAccSpeed * _m_fTotalTime;
            }
            else
            {
                _m_fSecAccSpeed = ((_m_fSrcSpeed * _m_fFirstAccTime) - (2 * moveDis)) / (_m_fTotalTime * secAccTime);
                _m_fFirstAccSpeed = -((_m_fSecAccSpeed * secAccTime) + _m_fSrcSpeed) / _m_fFirstAccTime;
            }
        }

        //根据时间节点取值
        public float getValueWithTimeTag(float _time)
        {
            if(_time >= _m_fTotalTime + _m_fContractTime)
                return _m_fTargetValue;

            if(_time < _m_fFirstAccTime)
            {
                //当运动轨迹在第一加速阶段时，使用加速度进行计算
                float dis = (_m_fSrcSpeed + (0.5f * _m_fFirstAccSpeed * _time)) * _time;
                //返回累计结果
                return (_m_fBinValue + dis);
            }
            else if(_m_fFirstAccTime < _time && _time < _m_fTotalTime)
            {
                //减速到达阶段
                //此时使用反向加速计算，更易得出结果
                float timeLeft = _m_fTotalTime - _time;
                float deltaDis = 0.5f * _m_fSecAccSpeed * timeLeft * timeLeft;
                //相减获得实际位置信息
                return (_m_fTargetValue + deltaDis);
            }
            else if(_time - _m_fTotalTime < _m_fContractTime / 2)
            {
                //弹起阶段
                //如果是回弹中心，min值增加
                if(_m_bIsMid)
                {
                    float timeExceeding = _time - _m_fTotalTime;
                    return _m_fTargetValue + 2 * (_m_fContractSrcSpeed * timeExceeding - 0.5f * _m_fContractDesSpeed * timeExceeding * timeExceeding);
                }
                else
                {
                    //否则是外围模块，min值降低
                    float timeExceeding = _time - _m_fTotalTime;
                    float value = _m_fTargetValue - (_m_fContractSrcSpeed * timeExceeding - 0.5f * _m_fContractDesSpeed * timeExceeding * timeExceeding);
                    return value > 0 ? value : 0;
                }
            }
            else
            {
                //回缩阶段  对照弹起阶段镜像回复
                return getValueWithTimeTag(2 * _m_fTotalTime + _m_fContractTime - _time);
            }
        }

        /*******************
        * 获取本过渡对象当前的数值信息
         **/
        public float curValue
        {
            get
            {
                float activeTime = Time.realtimeSinceStartup - _m_fStartTimeTag;
                return getValueWithTimeTag(activeTime);
            }
        }

        /*******************
        * 获取本过渡对象回弹标记类型
        **/
        public TutorialMoveMaskElementType getType()
        {
            return _m_eMaskElementType;
        }
    }
}
