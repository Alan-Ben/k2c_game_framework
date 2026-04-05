using System;
using ALPackage;
using CommonEnum;
using JetBrains.Annotations;

namespace GOE
{
    public class PlayerSpecialItemComponent : _ANPBasicPlayerComponent
    {
        [NotNull] private readonly _ASpecialItemData[] _m_specialItemDataArray;


        public PlayerSpecialItemComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_specialItemDataArray = new _ASpecialItemData[Enum.GetValues(typeof(ESpecialItemType)).Length];
            _m_specialItemDataArray[(int)ESpecialItemType.GOLD] = new SpecailItemData_GOLD();
            _m_specialItemDataArray[(int)ESpecialItemType.MARS_ENERGY] = new SpecailItemData_MARS_ENERGY();
        }
        

        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.SPECIAL_ITEM; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }
        public override bool canPreInit { get { return true; } }
        
        public SpecailItemData_GOLD goldData { get { return (SpecailItemData_GOLD)_m_specialItemDataArray[(int)ESpecialItemType.GOLD]; } }
        public SpecailItemData_MARS_ENERGY marsEnergyData { get { return (SpecailItemData_MARS_ENERGY)_m_specialItemDataArray[(int)ESpecialItemType.MARS_ENERGY]; } }
        
        
        public override void presendInitProtocol()
        {
            Action dataPreInitFunc = null;
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(_m_specialItemDataArray.Length + 1);
            stepCounter.regAllDoneDelegate(() =>
            {
                dealPreInitFunc(() =>
                {
                    dataPreInitFunc?.Invoke();
                    dataPreInitFunc = null;
                    setInitDone();
                });
            });

            foreach (_ASpecialItemData data in _m_specialItemDataArray)
            {
                if (data == null)
                {
                    stepCounter.addDoneStepCount();
                    continue;
                }
                
                data.presendInitProtocol(_preInitFunc =>
                {
                    dataPreInitFunc += _preInitFunc;
                    stepCounter.addDoneStepCount();
                });
            }
            
            stepCounter.addDoneStepCount();
        }
        protected override void _dealInit()
        {
        }
        protected override void _onInitDone()
        {
            foreach (_ASpecialItemData data in _m_specialItemDataArray)
                data?.init();
        }
        protected override void _onInitFail()
        {
        }
        protected override void _discard()
        {
            foreach (_ASpecialItemData data in _m_specialItemDataArray)
                data?.discard();
        }
        public override void onAllCompInited()
        {
            foreach (_ASpecialItemData data in _m_specialItemDataArray)
                data?.onAllCompInited();
        }


        public long getValue(ESpecialItemType _type)
        {
            int typeIndex = (int)_type;
            if (typeIndex < 0 || typeIndex >= _m_specialItemDataArray.Length)
                return 0;
            
            _ASpecialItemData data = _m_specialItemDataArray[typeIndex];
            if (data == null)
            {
#if UNITY_EDITOR
                Debug.LogError("未支持的特殊物品枚举： " + _type);
#endif
                return 0;
            }

            return data.getValue();
        }
    }
}