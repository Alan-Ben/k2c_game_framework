using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 玩家升级成功弹窗属性变化item
    /// </summary>
    public class GGUIWndPlayerLvlUpSucContainerItem : _ATALBasicUISubWnd<GGUIMonoPlayerLvlUpSucContainerItem>
    {
        private PlayerLvlUpSucItemData _m_data;
        private float _m_fAddDelay;
        private long _m_lOpSerialize;//操作序列号
        private NPGGuiWndTexture _m_icon;

        /// <summary>
        /// item数据
        /// </summary>
        public PlayerLvlUpSucItemData itemData
        {
            get { return _m_data; }
        }

        public GGUIWndPlayerLvlUpSucContainerItem(GGUIMonoPlayerLvlUpSucContainerItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _m_lOpSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_icon?.hideWnd();
            _m_lOpSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            _m_icon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_icon?.discard();
            _m_icon = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgIcon != null)
                _m_icon = new NPGGuiWndTexture(wnd.imgIcon);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_addDelay"></param>
        public void setInfo(PlayerLvlUpSucItemData _data, float _addDelay)
        {
            _m_data = _data;
            _m_fAddDelay = _addDelay;
            _m_lOpSerialize = ALSerializeOpMgr.next();
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            //重置动画
            wnd?.aniShowTextChg?.resetAni();
            //设置 icon
            if (_m_icon != null)
            {
                if (_m_data.icon != null && _m_data.icon.isValid())
                {
                    _m_icon.showWnd();
                    _m_icon.setTexture(_m_data.icon);
                }
                else
                {
                    _m_icon.hideWnd();
                }
            }
            //不同类型处理
            if (_m_data.isSpecial)
            {
                //========特殊类型
                ALUGUICommon.setGameObjEnable(wnd.goAddShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.goNormalShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.goSpecialShowList,true);
                //设置名称
                ALUGUICommon.setLabelTxt(wnd.txtName, _m_data.name);
                //设置数值
                ALUGUICommon.setLabelTxt(wnd.txtLastNum, _m_data.specialValueStr);
                ALUGUICommon.setLabelTxt(wnd.txtCurNum, _m_data.specialValueStr);
            }
            else
            {
                //设置默认展示数值
                long addNum = _m_data.curValue - _m_data.lastValue;
                ALUGUICommon.setLabelTxt(wnd.txtAddNum, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, addNum));

                if (addNum <= 0)
                {
                    //========未变更类型
                    ALUGUICommon.setGameObjEnable(wnd.goSpecialShowList, false);
                    ALUGUICommon.setGameObjEnable(wnd.goAddShowList, false);
                    ALUGUICommon.setGameObjEnable(wnd.goNormalShowList, true);
                    //设置名称
                    ALUGUICommon.setLabelTxt(wnd.txtName, _m_data.name);
                    //设置数值
                    ALUGUICommon.setLabelTxt(wnd.txtLastNum, _m_data.lastValue);
                    ALUGUICommon.setLabelTxt(wnd.txtCurNum, _m_data.curValue);
                }
                else
                {
                    //========有新增类型
                    bool needShowValueLerp = wnd.showTextChgDurationSec > 0f;
                    ALUGUICommon.setGameObjEnable(wnd.goSpecialShowList, false);
                    ALUGUICommon.setGameObjEnable(wnd.goNormalShowList, false);
                    ALUGUICommon.setGameObjEnable(wnd.goAddShowList, true);
                    //设置名称
                    ALUGUICommon.setLabelTxt(wnd.txtName, _m_data.name);
                    //设置初始数值
                    ALUGUICommon.setLabelTxt(wnd.txtLastNum, _m_data.lastValue);
                    if (!needShowValueLerp)
                    {
                        ALUGUICommon.setLabelTxt(wnd.txtCurNum, _m_data.curValue);
                    }
                    else
                    {
                        ALUGUICommon.setLabelTxt(wnd.txtCurNum, _m_data.lastValue);

                        //判断是否需要延时，开始滚动
                        if (wnd.delayShowTextChgSec + _m_fAddDelay > 0)
                        {
                            long serialize = _m_lOpSerialize;
                            ALCommonActionMonoTask.addMonoTask(() =>
                            {
                                if (!isShow || serialize != _m_lOpSerialize)
                                    return;

                                _showValueLerp(_m_data.lastValue, _m_data.curValue);
                            }, wnd.delayShowTextChgSec + _m_fAddDelay);
                        }
                        else
                        {
                            _showValueLerp(_m_data.lastValue, _m_data.curValue);
                        }
                    }
                }
            }
        }

        //一段时间内，值从开始到结束的变化过程
        private void _showValueLerp(long _start, long _end)
        {
            if (null == wnd || _start == _end)
                return;

            //播放动画
            wnd?.aniShowTextChg?.forcePlay();

            NPMonoTaskLerpStartEndValueByTime.startLerpTask(() => { return wnd == null || !isShow; }
                , _m_lOpSerialize
                , _start
                , _end
                , Math.Abs(wnd.showTextChgDurationSec) < 0.01 ? 1.0f : wnd.showTextChgDurationSec
                , value =>
                {
                    ALUGUICommon.setLabelTxt(wnd.txtCurNum, value);
                }
                , () => { return _m_lOpSerialize; }
                , null);
        }
    }
}
