using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public interface _IContainerSideRedTipItemInfo
    {
        /// <summary>
        /// 该item的RectTransform
        /// </summary>
        RectTransform rectTransform { get; }
        /// <summary>
        /// 是否有红点提示
        /// </summary>
        bool haveRedTip { get; }
    }

    /// <summary>
    /// Container侧边红点提示信息
    /// </summary>
    [System.Serializable]
    public class ContainerSideRedTipInfo
    {
        [ALInfo("====左边====")]
        [ALHeader("左侧有红点页签需要超出显示区域多少距离才显示红点")]
        public float leftShowRedTipOverDistance;
        [ALHeader("列表左边在显示区域之外有红点的提示")]
        public GameObject goLeftRedTip;
        [ALHeader("左侧跳转红点按钮")]
        public GameObject btnLeftRedTip;
        [ALInfo("====右边====")]
        [ALHeader("右侧有红点页签需要超出显示区域多少距离才显示红点")]
        public float rightShowRedTipOverDistance;
        [ALHeader("列表右边在显示区域之外有红点的提示")]
        public GameObject goRightRedTip;
        [ALHeader("右侧跳转红点按钮")]
        public GameObject btnRightRedTip;


        /// <summary>
        /// 隐藏红点GO
        /// </summary>
        public void hideRedTipGo()
        {
            ALUGUICommon.setGameObjEnable(goRightRedTip, false);
            ALUGUICommon.setGameObjEnable(goLeftRedTip, false);
        }

        /// <summary>
        /// 刷新列表左右两边红点提示
        /// </summary>
        /// <param name="_itemList"></param>
        /// <param name="_containerRectTransform"></param>
        public void refreshContainerSideRedTip<T>(List<T> _itemList, RectTransform _containerRectTransform) where T: _IContainerSideRedTipItemInfo
        {
#if NP_GAME
            if (_itemList == null || _containerRectTransform == null)
                return;

            bool leftHaveRedTip = false;
            bool rightHaveRedTip = false;

            for (int i = 0; i < _itemList.Count; i++)
            {
                _IContainerSideRedTipItemInfo item = _itemList[i];
                if (item != null && item.rectTransform != null)
                {
                    //容器左边界
                    float leftEdge = GCommon.getUIRootPos(_containerRectTransform).x - (_containerRectTransform.pivot.x * _containerRectTransform.rect.width);
                    //容器右边界
                    float rightEdge = GCommon.getUIRootPos(_containerRectTransform).x + ((1 - _containerRectTransform.pivot.x) * _containerRectTransform.rect.width);
                    //item左边界 减去pivot偏移
                    float itemLeftX = GCommon.getUIRootPos(item.rectTransform).x - (item.rectTransform.pivot.x * item.rectTransform.rect.width);
                    //item右边界 加上pivot偏移
                    float itemRightX = GCommon.getUIRootPos(item.rectTransform).x + ((1 - item.rectTransform.pivot.x) * item.rectTransform.rect.width);

                    //item左边界超出是否有红点
                    if (leftEdge - itemLeftX > leftShowRedTipOverDistance && item.haveRedTip)
                        leftHaveRedTip = true;

                    //item右边界超出是否有红点
                    if (itemRightX - rightEdge > rightShowRedTipOverDistance && item.haveRedTip)
                        rightHaveRedTip = true;
                }
            }
            ALUGUICommon.setGameObjEnable(goLeftRedTip, leftHaveRedTip);
            ALUGUICommon.setGameObjEnable(goRightRedTip, rightHaveRedTip);
#endif
        }

        /// <summary>
        /// 查找第一个在显示区域外有红点的item的下标
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_itemList"></param>
        /// <param name="_containerRectTransform"></param>
        /// <param name="_isLeft"></param>
        public int findFirstRedTipItemIndexOutOfContaienr<T>(List<T> _itemList, RectTransform _containerRectTransform, bool _isLeft) where T : _IContainerSideRedTipItemInfo
        {
            int targetIndex = -1;
#if NP_GAME
            if (_itemList == null || _containerRectTransform == null)
                return -1;

            for (int i = 0; i < _itemList.Count; i++)
            {
                _IContainerSideRedTipItemInfo item = _itemList[i];
                if (item != null)
                {
                    //容器左边界
                    float leftEdge = GCommon.getUIRootPos(_containerRectTransform).x - (_containerRectTransform.pivot.x * _containerRectTransform.rect.width);
                    //容器右边界
                    float rightEdge = GCommon.getUIRootPos(_containerRectTransform).x + ((1 - _containerRectTransform.pivot.x) * _containerRectTransform.rect.width);
                    //item左边界 减去pivot偏移
                    float itemLeftX = GCommon.getUIRootPos(item.rectTransform).x - (item.rectTransform.pivot.x * item.rectTransform.rect.width);
                    //item右边界 加上pivot偏移
                    float itemRightX = GCommon.getUIRootPos(item.rectTransform).x + ((1 - item.rectTransform.pivot.x) * item.rectTransform.rect.width);

                    if (_isLeft)
                    {
                        //item左边界超出是否有红点
                        if (leftEdge - itemLeftX > leftShowRedTipOverDistance)
                        {
                            if (item.haveRedTip)
                                targetIndex = i;
                        }
                        else
                            break;
                    }
                    else
                    {
                        //item右边界超出是否有红点
                        if (itemRightX - rightEdge > rightShowRedTipOverDistance && item.haveRedTip)
                        {
                            targetIndex = i;
                            break;
                        }
                    }
                }
            }
#endif
            return targetIndex;
        }
    }
}