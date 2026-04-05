using CommonEnum;
using GOE.BonusSpace;

namespace GOE
{
    /// <summary>
    /// 对外的玩家总的属性管理器
    /// </summary>
    public class PlayerUnionBonusMgr : CommonUnionBonusMgr
    {
        public PlayerUnionBonusMgr() : base(EUnionBonusMgrTag.TOTAL)
        {
        }
        
        /// <summary>
        /// 属性变动通知
        /// </summary>
        /// <param name="_chgProperty"></param>
        protected internal override void _onPropertyChg(EBonusPropertyType _chgProperty)
        {
            base._onPropertyChg(_chgProperty);
            
            //根据不同属性进行处理
            switch (_chgProperty) 
            {
                //实力变化，所有大臣需要重新处理
                case EBonusPropertyType.TALENT:
                    
                case EBonusPropertyType.POWER:
                    
                case EBonusPropertyType.POWER_PER:
                    NPPlayer.instance.heroComponent.reCalcAllHero();
                    break;

                //收益加成需要所有建筑重新处理
                case EBonusPropertyType.BONUS:
                    
                case EBonusPropertyType.BUILDING_PROFIT_ADD_PER:
                case EBonusPropertyType.BUILDING_EMPLOYEE_PROFIT_ADD:
                    NPPlayer.instance.buildingComp.recalAllBusinessBuilding();
                    break;
                default:
                    break;
            }
        }
    }
}