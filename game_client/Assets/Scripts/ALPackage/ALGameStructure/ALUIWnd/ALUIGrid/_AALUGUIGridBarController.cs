using System;

using UnityEngine;

/// <summary>
/// grid窗口中，横向占用整行空间的item对象，将插入在指定对象之后
/// </summary>
namespace ALPackage
{
    public abstract class _AALUGUIGridBarController : IComparable<_AALUGUIGridBarController>
    {
        /** 插入数据对象的索引 */
        protected int _m_iInsertItemIdx;
        /** 在实际的布局计算中的插入位置 */
        private int _m_iRealInsertItemIdx;
        /** 本区域展示的Item的结束数据序号 */
        private int _m_iShowItemDataEndIndex;

        /** 开始显示本bar的开始位置坐标 */
        private float _m_fShowStartPos;
        private float _m_fShowEndPos;

        protected _AALUGUIGridBarController()
        {
            _m_iInsertItemIdx = -1;
            _m_iRealInsertItemIdx = -1;
        }
        protected _AALUGUIGridBarController(int _insertIndex)
        {
            _m_iInsertItemIdx = _insertIndex;
            _m_iRealInsertItemIdx = _insertIndex;
        }

        /// <summary>
        /// 是否在本行结束后才插入
        /// </summary>
        public virtual bool isAfterLineBar { get { return false; } }

        public int insertItemIndex { get { return _m_iInsertItemIdx; } }
        public int realInsertItemIdx { get { return _m_iRealInsertItemIdx; } }
        public int showItemDataEndIndex { get { return _m_iShowItemDataEndIndex; } }
        public float showStartPos { get { return _m_fShowStartPos; } }
        public float showEndPos { get { return _m_fShowEndPos; } }

        /******************
         * 设置显示对象的位置索引
         **/
        public void setInsertIndex(int _index)
        {
            _m_iInsertItemIdx = _index;
        }
        /// <summary>
        /// 在实际的布局计算中，调整实际插入位置的设置函数
        /// </summary>
        /// <param name="_index"></param>
        public void setRealInsertIndex(int _index)
        {
            _m_iRealInsertItemIdx = _index;
        }
        /******************
         * 设置本对象的结束数据序号
         **/
        public void setShowItemDataEndIndex(int _index)
        {
            _m_iShowItemDataEndIndex = _index;
        }

        /***************
         * 重置窗口数据，代替原先的discard函数
         **/
        public void reset()
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UIGrid][{this.GetType().Name}] reset.");
            }
            _m_iInsertItemIdx = -1;

            //处理重置
            _reset();
        }

        /***************
         * 释放窗口资源相关对象
         **/
        public void discard()
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UIGrid][{this.GetType().Name}] discard.");
            }
            _m_iInsertItemIdx = -1;

            //处理重置
            _discard();
        }

        /// <summary>
        /// 窗口大小变化的处理
        /// </summary>
        /// <param name="_perUnitCount">带入每行的显示个数</param>
        /// <param name="_startShowCount">在本bar内开始显示的数据索引</param>
        /// <returns>返回本bar显示后的起始显示位置</returns>
        public float onResizeWnd(int _perUnitCount, int _startItemIndex, float _startShowPos, int _itemSize, float _space)
        {
            //计算当前bar之前需要显示的数量
            int showCount = _m_iInsertItemIdx - _startItemIndex;

            //计算当前bar需要显示的行数
            int lineCount = (showCount + _perUnitCount - 1) / _perUnitCount;
            if(lineCount < 0)
                lineCount = 0;

            //计算预期的实际插入位置，如果是行后插入，则将预期值设置为整行结束
            if(isAfterLineBar)
                _m_iRealInsertItemIdx = _startItemIndex + (lineCount * _perUnitCount);
            else
                _m_iRealInsertItemIdx = _m_iInsertItemIdx;
            //计算之前显示的高度
            _m_fShowStartPos = _startShowPos + (lineCount * (_itemSize + _space)) + _space;
            _m_fShowEndPos = _m_fShowStartPos + barHeight;

            return _m_fShowStartPos;
        }

        /// <summary>
        /// 排序函数
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(_AALUGUIGridBarController _other)
        {
            if(null == _other)
                return -1;

            //比较对应的index
            if(_other.insertItemIndex > insertItemIndex)
                return -1;
            else if(_other.insertItemIndex == insertItemIndex)
                return _compareWhenEqualIdx(_other);
            else
                return 1;
        }
        /// <summary>
        /// 当数据插入索引一致时的排序处理，可重载
        /// </summary>
        /// <param name="_other"></param>
        /// <returns></returns>
        protected virtual int _compareWhenEqualIdx(_AALUGUIGridBarController _other)
        {
            if(isAfterLineBar)
            {
                if(_other.isAfterLineBar)
                    return 0;
                else
                    return 1;
            }
            else if(_other.isAfterLineBar)
                return -1;
            else
                return 0;
        }

        /// <summary>
        /// 当前横栏对应的高度数据
        /// </summary>
        public abstract int barHeight { get; }
        /// <summary>
        /// 显隐控制函数，由于快速切换，需要用性能较高处理
        /// </summary>
        public abstract void show();
        public abstract void hide();
        /// <summary>
        /// 设置UI的坐标位置
        /// </summary>
        public abstract void setPos(float _x, float _y);
        /// <summary>
        /// 重置这个bar
        /// </summary>
        protected abstract void _reset();
        /// <summary>
        /// 释放相关资源
        /// </summary>
        protected abstract void _discard();
    }
}
