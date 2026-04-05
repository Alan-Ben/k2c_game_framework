using CommonEnum;
using GOE.BonusSpace;
using System;
using System.Text;

namespace GOE
{

    public class BuildingCommon
    {
        /// <summary>
        /// 计算经营建筑总收益
        /// </summary>
        /// <param name="_buildingInfo"></param>
        /// <param name="_sb"></param>
        /// <returns></returns>
        public static long calBusinessBuildingEarnings(BusinessBuildingInfo _buildingInfo, StringBuilder _sb = null)
        {
            if (_sb != null)
                _sb.Append("产出速度万分比加成：\n");

            //总加成数值=店铺升级加成+农田加成+伙伴驻扎加成+外部属性加成
            int profitAddPer = calBusinessBuildingEarningsAddPer(_buildingInfo, _sb);

            //单个建筑收益=[(伙伴总实力/1000)+员工基础收益]*(1+收益百分比加成)，结果向上取整

            //**每个员工收益
            long earningsPerPerson = _buildingInfo.getEmployeeEarningsPerPerson();
            if (_sb != null)
                _sb.Append("单个员工收益：").Append(earningsPerPerson).Append("\n");
            //**员工基础收益=建筑员工数量*每个员工收益
            long baseWorkerProfit = earningsPerPerson * _buildingInfo.employeeNum;
            if (_sb != null)
                _sb.Append("员工基础收益：").Append(baseWorkerProfit).Append("\n");
            //**伙伴总实力=所有伙伴实力之和
            long totalPower = NPPlayer.instance.heroComponent.totalPower; 
            if (_sb != null)
                _sb.Append("伙伴总实力：").Append(totalPower).Append("\n");
            //总收益数值=员工建筑总收益+伙伴经营总收益
            long outputBS = (long)Math.Ceiling(((double)totalPower / 1000 + baseWorkerProfit) * (10000 + profitAddPer) / 10000);
            if (_sb != null)
                _sb.Append("总收益数值：").Append(outputBS).Append("\n");
            //**Bonus中的绝对值加成
            long bonusValue = NPPlayer.instance.playerBonusMgr.getTotalPropertyBonus(EBonusPropertyType.BONUS, _buildingInfo.bonusJudgeParts);
            if (_sb != null)
                _sb.Append("Bonus 绝对值加成：").Append(bonusValue).Append("\n");

            return outputBS + bonusValue;
        }

        /// <summary>
        /// 计算经营建筑总收益加成万分比
        /// </summary>
        /// <param name="_buildingInfo"></param>
        /// <param name="_sb"></param>
        /// <returns></returns>
        public static int calBusinessBuildingEarningsAddPer(BusinessBuildingInfo _buildingInfo, StringBuilder _sb = null)
        {
            //店铺升级加成（万分比）
            int upgradeAddPer = (int)(_buildingInfo.levelRef?.earning_rate ?? 0);
            if (_sb != null)
                _sb.Append(" 店铺升级加成：").Append(upgradeAddPer).Append("\n");
            // 客户端农田加成归入外部属性了
            // //农田加成（万分比）
            // int farmAddPer = getBuildingInfo().getComp().getTotalEarningRate();
            //伙伴驻扎加成（万分比）
            int heroPlaceAddPer = (int)_buildingInfo.getAllPlacedHeroEarningBonus();
            if (_sb != null)
                _sb.Append(" 伙伴驻扎加成：").Append(heroPlaceAddPer).Append("\n");
            //外部属性加成（万分比）
            int bonusAddPer = (int)NPPlayer.instance.playerBonusMgr.getTotalPropertyBonus(EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _buildingInfo.bonusJudgeParts);
            if (_sb != null)
                _sb.Append(" bonus属性加成（包含农场、联盟、权益卡）：").Append(bonusAddPer).Append("\n");
            //联盟bonus属性加成（万分比）
            _AUnionBonusMgr guildBonusMgr = NPPlayer.instance.playerBonusMgr.findMgrByTag(EUnionBonusMgrTag.GUILD);
            int guildBonusAddPer = guildBonusMgr == null ? 0 : (int)guildBonusMgr.getTotalPropertyBonus(EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _buildingInfo.bonusJudgeParts);
            if (_sb != null)
                _sb.Append("    联盟bonus属性加成：").Append(guildBonusAddPer).Append("\n");
            //农田bonus属性加成（万分比）
            _AUnionBonusMgr farmBonusMgr = NPPlayer.instance.playerBonusMgr.findMgrByTag(EUnionBonusMgrTag.FARMING);
            int farmBonusAddPer = farmBonusMgr == null ? 0 : (int)farmBonusMgr.getTotalPropertyBonus(EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _buildingInfo.bonusJudgeParts);
            if (_sb != null)
                _sb.Append("    农田bonus属性加成：").Append(farmBonusAddPer).Append("\n");
            //权益卡加成（万分比）
            _AUnionBonusMgr privilegeCardBonusMgr = NPPlayer.instance.playerBonusMgr.findMgrByTag(EUnionBonusMgrTag.PRIVILEGE_CARD);
            int privilegeCardBonusAddPer = privilegeCardBonusMgr == null ? 0 : (int)privilegeCardBonusMgr.getTotalPropertyBonus(EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _buildingInfo.bonusJudgeParts);
            if (_sb != null)
                _sb.Append("    权益卡属性加成：").Append(privilegeCardBonusAddPer).Append("\n");
            //爬塔加成（万分比）
            int towerAddPer = (int)NPPlayer.instance.towerComp.towerEarningAddPer;
            if (_sb != null)
                _sb.Append(" 爬塔关卡属性加成：").Append(towerAddPer).Append("\n");

            //总加成数值=店铺升级加成+农田加成+伙伴驻扎加成+外部属性加成
            int profitAddPer = upgradeAddPer /* + farmAddPer */ + heroPlaceAddPer + bonusAddPer + towerAddPer;
            if (_sb != null)
                _sb.Append("总万分比加成数值：").Append(profitAddPer).Append("\n");

            return profitAddPer;
        }

        /// <summary>
        /// 处理重新计算经营建筑收益
        /// </summary>
        /// <param name="_m_buildingInfo"></param>
        /// <param name="_isDealNextFrame"></param>
        public static void dealRecalculateBuildingEarnings(BusinessBuildingInfo _m_buildingInfo, bool _isDealNextFrame)
        {
            if(_m_buildingInfo == null)
                return;

            _m_buildingInfo.updateEarnings(BuildingCommon.calBusinessBuildingEarnings(_m_buildingInfo));

            NPPlayer.instance.buildingComp._invokeOnBusinessBuildingChg(_m_buildingInfo);

            //计算总赚速
            NPPlayer.instance.specialItemComp.goldData.recalEarnings(_isDealNextFrame);
        }
    }
}