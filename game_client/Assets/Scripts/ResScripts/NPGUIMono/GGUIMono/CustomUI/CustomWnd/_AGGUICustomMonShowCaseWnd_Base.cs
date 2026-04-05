using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GOE
{
    [System.Serializable]
    public class GGUICustomMonShowCaseAniData
    {
        [ALHeader("单位位置索引")]
        public int index;
        [ALHeader("动画名称")]
        public string name;
    }
    
    /// <summary>
    /// 自定义显示render model角色场景图片
    /// </summary>
    public abstract class _AGGUICustomMonShowCaseWnd_Base : MonoBehaviour
    {
        [ALHeader("正在Loading时，显示的go")]
        public GameObject goLoading;
        [ALHeader("渲染图片+背景图")]
        public RawImage imageBodyAndBG;
        [ALHeader("舞台配置")]
        public NPGShowcaseIndex templateIndex;
        [ALHeader("应该用哪一种摄像机")]
        public EShowcaseCameraType cameraType = EShowcaseCameraType.MainCamera;
        [ALHeader("是否根据RawImage的大小调整RT的大小")]
        public bool isResizeRTByRect = false;
        
        [ALHeader("显示时候播放的动画")]
        public List<GGUICustomMonShowCaseAniData> showAniDataList;

        
        [ALHeader("拖拽点击区域")]
        public GameObject btnDragArea;
        
        [ALHeader("是否允许拖拽旋转")]
        public bool isRotate;
        [ALHeader("拖拽旋转的单位索引（第几个index）")]
        public List<int> rotateUnitIndexList;
        [ALHeader("拖拽旋转速度（ui上移动的单位距离 * rotateSpeed = 旋转角度）")]
        [Min(0.1f)]
        public float rotateSpeed = 0.2f;
        
        [ALHeader("UI视觉中心（设置了值会开启相机跟随UI视觉中心位置进行偏移)")]
        [ALHeader("--------MainCamera类型跟随UI视觉中心偏移--------")]
        [Tooltip("1.只针对Camera Type为Main Camera的有用\n 2.设置了值就开启相机跟随UI视觉中心偏移 \n 3.需要配合ViewCenterY参数来计算\n")]
        public RectTransform viewCenterRect;
#if NP_GAME
        private NPGGuiWndTexture _m_wBodyAndBGWnd;
        private ShowcaseInfo _m_showcaseInfo;

        private void Awake()
        {
            ALUGUICommon.combineBtnClick(btnDragArea, _onClickShowcase);
            ALUGUICommon.combineBeginDrag(btnDragArea, _onBeginDragShowcase);
            ALUGUICommon.combineEndDrag(btnDragArea, _onEndDragShowcase);
            ALUGUICommon.combineDrag(btnDragArea, _onDragShowcase);
        }


        private void OnDestroy()
        {
            ALUGUICommon.uncombineBtnClick(btnDragArea, _onClickShowcase);
            ALUGUICommon.uncombineBeginDrag(btnDragArea, _onBeginDragShowcase);
            ALUGUICommon.uncombineEndDrag(btnDragArea, _onEndDragShowcase);
            ALUGUICommon.uncombineDrag(btnDragArea, _onDragShowcase);
        }

        //有效和无效的时候分别注册和注销显示对象
        private void OnEnable()
        {
            _AShowCaseUnitInfoObj[] itemList = _getUnitInfoObjList();
            if(null == itemList)
                return;
            
            ALUGUICommon.setGameObjEnable(goLoading, true);

            //获取showcase对象
            if(null == _m_showcaseInfo){
                _m_showcaseInfo = ShowCaseMgr.instance.popAvailableInfo();
            }

            if (cameraType == EShowcaseCameraType.MainCamera)
            {
                _m_showcaseInfo.setUIViewCenter(viewCenterRect);
                _m_showcaseInfo.enableShowCase(templateIndex, itemList, cameraType, _getDescriptor());
                _m_showcaseInfo.regAllDoneDelegate(() =>
                {
                    //播放显示时候播放的动画
                    _playShowAni();
                    
                    ALUGUICommon.setGameObjEnable(goLoading, false);
                });
            }
            else if (cameraType == EShowcaseCameraType.RTCamera)
            {
                _m_showcaseInfo.enableShowCase(templateIndex, itemList, cameraType, _getDescriptor());
                _m_showcaseInfo.regAllDoneDelegate(() =>
                {
                    if(null == _m_wBodyAndBGWnd)
                        _m_wBodyAndBGWnd = new NPGGuiWndTexture(imageBodyAndBG);
                    _m_wBodyAndBGWnd.showWnd();
                    _m_wBodyAndBGWnd.setTexture(_m_showcaseInfo.getRenderTexture());

                    //播放显示时候播放的动画
                    _playShowAni();
                    
                    ALUGUICommon.setGameObjEnable(goLoading, false);
                });
            }
        }
        

        private void OnDisable()
        {
            _disableAll();
        }

        /// <summary>
        /// 播放显示时候播放的动画
        /// </summary>
        private void _playShowAni()
        {
            if(null == _m_showcaseInfo)
                return;
            
            if(null == showAniDataList || showAniDataList.Count == 0)
                return;
            
            foreach (GGUICustomMonShowCaseAniData itemData in showAniDataList)
            {
                if(null == itemData)
                    continue;
                
                _m_showcaseInfo.playAnim(itemData.index, itemData.name);
            }
        }
        
        private void _disableAll()
        {
            if (null != _m_showcaseInfo)
            {
                _m_showcaseInfo.disableShowCase();
                ShowCaseMgr.instance.pushBackInfo(_m_showcaseInfo);
                _m_showcaseInfo = null;
            }
            
            if (null != _m_wBodyAndBGWnd)
                _m_wBodyAndBGWnd.discard();
            _m_wBodyAndBGWnd = null;
        }

        private RenderTextureDescriptor _getDescriptor()
        {
            //用默认大小设置
            if (null == imageBodyAndBG || cameraType != EShowcaseCameraType.RTCamera || !isResizeRTByRect)
                return CommonQualityMgr.instance.getRTDescriptor();
            
            var rect = imageBodyAndBG.rectTransform.rect;
            RenderTextureDescriptor rt = CommonQualityMgr.instance.getRTDescriptor();
            float scale = CommonQualityMgr.instance.screenRTScaleFactor;
            rt.width = (int) (rect.width * scale);
            rt.height = (int) (rect.height * scale);
            return rt;
        }
        
        /// <summary>
        /// 点击showcase
        /// </summary>
        /// <param name="_go"></param>
        protected virtual void _onClickShowcase(GameObject _go)
        {
           
        }

        /// <summary>
        /// showcase开始拖拽事件
        /// </summary>
        /// <param name="_pointData"></param>
        private void _onBeginDragShowcase(PointerEventData _pointData)
        {
            
        }

        /// <summary>
        /// showcase拖拽事件
        /// </summary>
        /// <param name="_pointData"></param>
        private void _onDragShowcase(PointerEventData _pointData)
        {
            if (!enabled)
                return;

            if (null == _pointData || rotateUnitIndexList == null || null == _m_showcaseInfo)
                return;

            if (isRotate)
            {
                foreach (int index in rotateUnitIndexList)
                {
                    _m_showcaseInfo.rotateUnit(index, -_pointData.delta.x * rotateSpeed);
                }
            }
        }

        /// <summary>
        /// showcase停止拖拽事件
        /// </summary>
        /// <param name="_pointData"></param>
        private void _onEndDragShowcase(PointerEventData _pointData)
        {
            
        }

        /// <summary>
        /// 获取加载数据对象列表
        /// </summary>
        /// <returns></returns>
        protected abstract _AShowCaseUnitInfoObj[] _getUnitInfoObjList();
#endif

    }
}