
using System;
using Common.MarsEnum;
using Common.MarsObj;

namespace GOE
{
    /// <summary>
    /// 行军状态的额外数据
    /// </summary>
    public class MarsTeamEx_March : _ATMarsTeamBasicExData<MarsTeamState_March>
    {
        //根据不同行军目标，做的不同处理
        private _IMarsTeamExDataInterface _m_exMarchInterface;

        public _IMarsTeamExDataInterface exMarchDataObj { get { return _m_exMarchInterface; } }

        public MarsTeamEx_March(MarsExploreTeamInfo _tiTeamInfo, byte[] _exData)
           : base(_tiTeamInfo, _exData)
        {
            //判断目标状态
            if(data.getTargetState() == (int)EMarsExploreTeamState.COLLECT)
            {
                _m_exMarchInterface = new MarsTeamEx_March_Mine(this);
            }
        }

        /// <summary>
        /// 读取数据，并返回结构体
        /// </summary>
        /// <param name="_exData"></param>
        /// <returns></returns>
        protected override MarsTeamState_March _readExData(byte[] _exData)
        {
            MarsTeamState_March data = new MarsTeamState_March();
            data.readPackage(_exData);

            return data;
        }

        /****************** 扩展数据相关接口 **************/
        /// <summary>
        /// 更新扩展数据，如果状态没变化，则调用此接口
        /// </summary>
        /// <param name="_exData"></param>
        public override void onUpdateExData(_IMarsTeamExDataInterface _exData)
        {
            //矿点更新不需要处理
            _m_exMarchInterface?.onUpdateExData(_exData);
        }

        /// <summary>
        /// 进入本状态类的处理
        /// </summary>
        public override void onEnterExData()
        {
            _m_exMarchInterface?.onEnterExData();
        }

        /// <summary>
        /// 退出本状态类的处理
        /// </summary>
        public override void onExitExData()
        {
            _m_exMarchInterface?.onExitExData();
        }
        /****************** 扩展数据相关接口 End **************/

        public override EMarsBagItemUseTimeType timeType
        {
            get
            {
                EMarsBagItemUseTimeType exDataType = _m_exMarchInterface?.timeType ?? EMarsBagItemUseTimeType.NONE;
                if (exDataType != EMarsBagItemUseTimeType.NONE)
                    return exDataType;

                return EMarsBagItemUseTimeType.NONE; // todo: 如果以后通用的行军有枚举了，在这里返回
            }
        }
        public override long guildHelpId { get { return 0; } }
        public override void reqCompleteNow(Action<bool> _complete)
        {
            _complete?.Invoke(false);
        }
    }
}