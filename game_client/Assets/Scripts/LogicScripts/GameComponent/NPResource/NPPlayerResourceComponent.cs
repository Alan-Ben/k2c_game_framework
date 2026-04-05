using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using ALPackage;
using System.Text;
using CommonEnum;
using NPEnum;


namespace GOE
{
    //玩家资源管理器组件
    public class NPPlayerResourceComponent : _ANPBasicPlayerComponent
    {
        public static readonly int g_ResourceTypeCount = Enum.GetValues(typeof(CommonEnum.ECurrency)).Length;

        //所有资源数量列表
        private long[] _m_lResourceCountList = new long[g_ResourceTypeCount];
        private NPCurrencyResObj[] _m_lPlayerResList = new NPCurrencyResObj[g_ResourceTypeCount];


        /* 当前资源数量变化的事件Action<enum,srcValue, destValue>*/
        public Action<CommonEnum.ECurrency, long, long> onResourceCountChg;

        public override bool isMustInit { get { return true; } }

        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.RESOURCE; } }

        public override ENPPlayerCompType[] dependCompList { get { return null; } }

        public NPPlayerResourceComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            //初始化所有属性
            for(int i = 0; i < g_ResourceTypeCount; i++)
            {
                _m_lResourceCountList[i] = 0;
            }

            onResourceCountChg = default(Action<CommonEnum.ECurrency, long, long>);
        }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            //请求玩家资源
            reqCurrencyList();
        }

        protected override void _dealInit()
        {
            //获取所有货币数据对象
            for(int i = 0; i < g_ResourceTypeCount; i++)
            {
                _m_lPlayerResList[i] = GRefdataCoreMgr.instance.currencyResCore.getRef(i);
            }
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("NPPlayerResourceComponent init Fail!!!");
        }
        //释放资源函数
        protected override void _discard()
        {
            clear();
        }


        public void clear()
        {
            for(int i = 0; i < g_ResourceTypeCount; i++)
            {
                _m_lResourceCountList[i] = 0;
            }

            onResourceCountChg = default(Action<CommonEnum.ECurrency, long, long>);
        }

        //获取指定类型的资源数量
        public long getValue(CommonEnum.ECurrency _type)
        {
            // 金币特殊处理，数值在另外一个组件中，这边特殊处理方便原有结构取值
            if (_type == ECurrency.SILVER)
                return NPPlayer.instance.specialItemComp.goldData.getValue();

            // 火星能量特殊处理，数值在另外一个组件中，这边特殊处理方便原有结构取值
            if (_type == ECurrency.MARS_ENERGY)
                return NPPlayer.instance.specialItemComp.marsEnergyData.getValue();

            int idx = (int)_type;
            if(idx >= _m_lResourceCountList.Length)
            {
#if UNITY_EDITOR
                Debug.LogError("数据不匹配， 货币枚举： " + _type);
#endif
                return 0;
            }
            return _m_lResourceCountList[idx];
        }
        public long getShowValue(CommonEnum.ECurrency _type)
        {
            int idx = (int)_type;
            if(idx < 0 || idx >= _m_lPlayerResList.Length)
            {
#if UNITY_EDITOR
                Debug.LogError("数据不匹配， 货币枚举： " + _type);
#endif
                return 0;
            }
            NPCurrencyResObj currencyRef = _m_lPlayerResList[idx];

            if (null == currencyRef || currencyRef.multiple == 0)
                return getValue(_type);
            else
                return getValue(_type) / currencyRef.multiple;
        }

        public void setValue(CommonEnum.ECurrency _type, long _value)
        {
            if (_type == ECurrency.SILVER)
            {
                ALLog.Error("[Currency] 金币数据被特殊处理了，不允许在这边设值，使用 PlayerSpecialComponent.goldData 进行处理。");
                return;
            }

            if (_type == ECurrency.MARS_ENERGY)
            {
                ALLog.Error("[Currency] 火星能量数据被特殊处理了，不允许在这边设值，使用 PlayerSpecialComponent.marsEnergyData 进行处理。");
                return;
            }

            if((int)_type >= _m_lResourceCountList.Length)
                return;

            long srcValue = _m_lResourceCountList[(int)_type];
            _m_lResourceCountList[(int)_type] = _value;

            if(onResourceCountChg != null)
                onResourceCountChg(_type, srcValue, _value);
            if(_type == CommonEnum.ECurrency.NONE)
            {
                GCommon.reloadCustomLoadPrefab();
            }
            //事件广播
            WinMsg.SendMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _type, srcValue, _value);
            
            NPPlayer.instance.commonItemCountComp.sendCommonItemCountChgMsg(ENPItemType.CURRENCY, (long)_type, _value);
        }

        public void addValue(CommonEnum.ECurrency _type, long _addValue)
        {
            long curValue = getValue(_type);
            setValue(_type, curValue + _addValue);
        }

        #region S2C

        //从服务器加载数据
        public void setData(List<NPCommon.NPCommon_CurrencyInfo> _currencys)
        {
            for(int i = 0; i < _currencys.Count; ++i)
            {
                setValue(_currencys[i]);
            }

            //初始化结束必须要设置状态
            setInitDone();
        }
        //设置值，服务器更新
        public void setValue(NPCommon.NPCommon_CurrencyInfo _currency)
        {
            setValue((CommonEnum.ECurrency)_currency.getType(), _currency.getCount());
        }


        #endregion

        #region C2S
        //请求卡牌列表
        public void reqCurrencyList()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_003_ReqCurrencyList());
        }
        #endregion
    }
}
