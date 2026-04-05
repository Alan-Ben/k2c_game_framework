using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 改变UI大小为与UI根节点FullCanvas相同大小
    /// </summary>
    public class NPGGUICustomMonoChangeSizeToFullCanvas : MonoBehaviour
    {
        [Header("改变大小的对象")]
        public List<RectTransform> transList;

        private void Awake()
        {
        }

        private void OnDestroy()
        {
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            //注册屏幕变动回调
            ALGUIMain.instance.screenSizeChgDelegate += _onClientScreenOnSize;
#endif
            _refreshSize();
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            //注册屏幕变动回调
            ALGUIMain.instance.screenSizeChgDelegate -= _onClientScreenOnSize;
#endif
        }

        private void _refreshSize()
        {
#if NP_GAME
            if (transList == null || _AALMonoMain.instance.fullCanvas == null || _AALMonoMain.instance.uiRootRectTrans == null)
                return;

            var rect = _AALMonoMain.instance.uiRootRectTrans.rect;
            float fullCanvasWidth = rect.width;
            float fullCanvasHeight = rect.height;
            Vector3 fullCanvasWorldPosition = _AALMonoMain.instance.uiRootRectTrans.position;
            
            RectTransform trans = null;
            for (int i = 0; i < transList.Count; i++)
            {
                trans = transList[i];
                if(trans == null)
                    continue;

                Transform parent = trans.parent;
                Vector3 fullCanvasLocalPosition = parent == null ? fullCanvasWorldPosition : parent.InverseTransformPoint(fullCanvasWorldPosition);
                trans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fullCanvasWidth);
                trans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, fullCanvasHeight);
                Vector3 centerOffset = new Vector3(trans.rect.center.x, trans.rect.center.y, 0);
                trans.localPosition = fullCanvasLocalPosition - centerOffset;
                // Debug.Log($"fullCanvasWorldPosition:{fullCanvasWorldPosition} fullCanvasLocalPosition:{fullCanvasLocalPosition} trans.rect.center:{trans.rect.center}");
            }
#endif
        }

        private void _onClientScreenOnSize(int _width, int _height)
        {
            _refreshSize();
        }
    }
}