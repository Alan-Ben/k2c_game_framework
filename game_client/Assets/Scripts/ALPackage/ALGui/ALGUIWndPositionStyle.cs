using System;
using System.Collections.Generic;
using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    /** 窗口位置定位的参考点类型 */
    public enum ALGUIWndPositionSetting
    {
        LEFT_TOP,
        LEFT_MID,
        LEFT_BOTTOM,
        MID_TOP,
        MID_MID,
        MID_BOTTOM,
        RIGHT_TOP,
        RIGHT_MID,
        RIGHT_BOTTOM,
    }

    /** 长度度量类型 */
    public enum ALGUILengthType
    {
        PARENT_PERCENT,     //根据父容器对应长度以百分比计算
        NUM,                //绝对值
        CUT_NUM,            //与总宽度存在一定差值
    }

    public class ALGUILengthStyle
    {
        public ALGUILengthType lengthType;
        public float num;

        public ALGUILengthStyle()
        {
            lengthType = ALGUILengthType.NUM;
            num = 0;
        }
        public ALGUILengthStyle(ALGUILengthStyle _src)
        {
            lengthType = _src.lengthType;
            num = _src.num;
        }

        public void setNum(int _num)
        {
            lengthType = ALGUILengthType.NUM;
            num = _num;
        }
        public void setPercent(int _percent)
        {
            lengthType = ALGUILengthType.PARENT_PERCENT;
            num = _percent;
        }

        /** 根据父节点的长度变量计算实际的长度值 */
        public float getLength(float _parentLen)
        {
            if (lengthType == ALGUILengthType.NUM)
                return num;
            else if (lengthType == ALGUILengthType.CUT_NUM)
                return _parentLen - num;
            else
                return num * _parentLen / 100;
        }
    }

    /**************************
     * 存储本UI框架下窗口位置属性的对象
     **************************/
    public class ALGUIWndPositionStyle
    {
        /** 窗口位置定位的参考点 */
        public ALGUIWndPositionSetting wndRectRefPositionType;

        /** 根据定位参考点定位后的坐标偏移 */
        public float x;
        public float y;

        /** 长宽计算对象 */
        public ALGUILengthStyle width;
        public ALGUILengthStyle height;

        /** 默认构建的窗口定位类型对象 */
        /** 默认构建的窗口定位类型对象 */
        public ALGUIWndPositionStyle()
        {
            wndRectRefPositionType = ALGUIWndPositionSetting.LEFT_TOP;

            x = 0;
            y = 0;

            width = new ALGUILengthStyle();
            height = new ALGUILengthStyle();
        }
        public ALGUIWndPositionStyle(ALGUIWndPositionStyle _src)
        {
            wndRectRefPositionType = _src.wndRectRefPositionType;

            x = _src.x;
            y = _src.y;

            width = new ALGUILengthStyle(_src.width);
            height = new ALGUILengthStyle(_src.height);
        }

        /**********************
         * 将窗口样式的宽度和高度设置为指定值
         **********************/
        public void setSize(float _width, float _height)
        {
            if (ALGUILengthType.NUM == width.lengthType)
                width.num = _width;

            if (ALGUILengthType.NUM == height.lengthType)
                height.num = _height;
        }

        /**********************
         * 根据父窗口区域，计算当前窗口位置
         **********************/
        public Rect ALGUIGetNewRect(Rect _parentRect)
        {
            float realWidth = width.getLength(_parentRect.width);
            float realHeight = height.getLength(_parentRect.height);

            Vector2 leftTopPoint = new Vector2(0, 0);

            //计算参考的起始点坐标
            switch(wndRectRefPositionType)
            {
                case ALGUIWndPositionSetting.LEFT_TOP:
                    {
                        leftTopPoint = new Vector2(0, 0);
                        break;
                    }
                case ALGUIWndPositionSetting.LEFT_MID:
                    {
                        float realY = (_parentRect.height - realHeight) / 2;
                        leftTopPoint = new Vector2(0, realY);
                        break;
                    }
                case ALGUIWndPositionSetting.LEFT_BOTTOM:
                    {
                        float realY = _parentRect.height - realHeight;
                        leftTopPoint = new Vector2(0, realY);
                        break;
                    }
                case ALGUIWndPositionSetting.MID_TOP:
                    {
                        float realX = (_parentRect.width - realWidth) / 2;
                        leftTopPoint = new Vector2(realX, 0);
                        break;
                    }
                case ALGUIWndPositionSetting.MID_MID:
                    {
                        float realX = (_parentRect.width - realWidth) / 2;
                        float realY = (_parentRect.height - realHeight) / 2;
                        leftTopPoint = new Vector2(realX, realY);
                        break;
                    }
                case ALGUIWndPositionSetting.MID_BOTTOM:
                    {
                        float realX = (_parentRect.width - realWidth) / 2;
                        float realY = _parentRect.height - realHeight;
                        leftTopPoint = new Vector2(realX, realY);
                        break;
                    }
                case ALGUIWndPositionSetting.RIGHT_TOP:
                    {
                        float realX = _parentRect.width - realWidth;
                        leftTopPoint = new Vector2(realX, 0);
                        break;
                    }
                case ALGUIWndPositionSetting.RIGHT_MID:
                    {
                        float realX = _parentRect.width - realWidth;
                        float realY = (_parentRect.height - realHeight) / 2;
                        leftTopPoint = new Vector2(realX, realY);
                        break;
                    }
                case ALGUIWndPositionSetting.RIGHT_BOTTOM:
                    {
                        float realX = _parentRect.width - realWidth;
                        float realY = _parentRect.height - realHeight;
                        leftTopPoint = new Vector2(realX, realY);
                        break;
                    }
                default :
                    leftTopPoint = new Vector2(0, 0);
                    break;
            }

            //计算实际的区域
            Rect realRect = new Rect(leftTopPoint.x + x, leftTopPoint.y + y, realWidth, realHeight);

            return realRect;
        }
    }
}

#endif
