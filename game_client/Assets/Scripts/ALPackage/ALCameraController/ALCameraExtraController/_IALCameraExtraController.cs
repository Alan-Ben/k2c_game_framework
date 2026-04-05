using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

namespace ALPackage
{
    /********************
     * 附加控制对象，根据本对象返回的不同的位置或焦点附加位移对摄像头进行附加调整
     **/
    public interface _IALCameraExtraController
    {
        /**************
         * 本控制对象是否还有效，如无效将自动移除
         **/
        bool enable { get; }

        /***************
         * 获取额外的位置偏移信息
         **/
        Vector3 getExtraPosition();
        /***************
         * 获取额外的目标点偏移信息
         **/
        Vector3 getExtraFocusPosition();
        /***************
         * 获取额外的绝对位置偏移信息
         **/
        Vector3 getExtraOriginPosition();
        /***************
         * 获取额外的绝对目标点偏移信息
         **/
        Vector3 getExtraOriginFocusPosition();
    }
}
