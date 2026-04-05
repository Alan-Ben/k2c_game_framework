package NPGameRes.GameObjs.Mars.UsMarsBattle;

import Common.MarsObj.MarsBattleV2_MemberInfo;
import Common.ServerObj.ServerObj_MarsBattleV2_MemberFightResult;
import Common.ServerObj.ServerObj_MarsBattleV2_PropertyBonus;
import Common.ServerObj.ServerObj_MarsBattleV2_TeamFightResult;
import Common.ServerObj.ServerObj_MarsBattleV2_TeamInfo;
import NPEnum.ENPPlayerPropertyType;
import NPGameRes.Refs.RefGeneral;

import java.util.ArrayList;

/**
 * 火星战斗计算处理类
 */
public class UsMarsBattleCal
{
    /**
     * PVP战斗结果计算
     *
     * 计算过程：
     【公式】【战损】=MEDIAN(下限值,防守方战力/进攻方战力,上限值)，*取小数点后4位 - MEDIAN 取中间的值
     1	战损值有上限和下限值

     4	战斗伤害计算
     1	胜败判断：
     【公式】【进攻方损失精兵】=【防守方当前精兵】*【战损】
     1	当【进攻方损失精兵】≤【进攻方当前精兵】，战胜
     2	当【进攻方损失精兵】＞【进攻方当前精兵】，战败
     2	实际伤害计算：
     【公式】【战败方剩余精兵】=败方当前精兵*败方精兵实际损失系数
     败方精兵实际损失系数=败方基础损失系数*(1+实力碾压对败方损失加成*(胜方战力/败方战力))
     注:败方精兵实际损失系数有上限(general表配置),超过损失上限时,败方精兵实际损失系数=上限值
     【公式】【战胜方损失精兵】=胜方当前精兵*胜方精兵实际损失系数
     胜方精兵实际损失系数=胜方基础损失系数*(1-胜方损失减免系数*(胜方战力-败方战力)/(胜方战力+败方战力))
     注:胜方有损失上限,胜方损失超过败方损失时 胜方损失=败方损失

     败方基础损失系数(general表配置)
     实力碾压对败方损失加成(general表配置)
     胜方基础损失系数(general表配置)
     胜方损失减免系数(general表配置)
     败方精兵实际损失系数上限(general表配置)
     *
     * @param _attaker
     * @param _defencer
     * @return
     */
    public static FightResult calFight(_IUsMarsBattleObj _attaker, _IUsMarsBattleObj _defencer)
    {
        if(null == _attaker)
            return null;

        boolean win;
        FightResult result = new FightResult();

        do
        {
            //防守方战力
            long defenceTroopNum = _defencer == null ? 0 : _defencer.getTeamSoldierNum();
            long defencePower = _defencer == null ? 0 : defenceTroopNum * _defencer.getTeamSoldierPower();

            result.defencerOriSoldierNum = defenceTroopNum;
            result.defencerSingleSoldierPower = _defencer == null ? 0 : _defencer.getTeamSoldierPower();

            //进攻方当前精兵
            long attackTroopNum = _attaker.getTeamSoldierNum();
            //进攻方当前精兵
            long attackPower = attackTroopNum * _attaker.getTeamSoldierPower();

            result.attackerOriSoldierNum = attackTroopNum;
            result.attackerSingleSoldierPower = _attaker.getTeamSoldierPower();

            //使用双方最小数量的最低比例作为最低战损，不单独使用比例处理战损
            long minHurtNum = Math.min(attackTroopNum, defenceTroopNum) * RefGeneral.Ref().mars_explore_team_loss_min_per / 10000;
            if(minHurtNum < 1)
                minHurtNum = 1;

            if(0 >= attackPower)
            {
                win = false;
                //日志记录
                _attaker.logBattle(true, false, _defencer, attackPower, 0, attackTroopNum, 0, 0, 0);
                break;
            }

            //如果防守方战力为0，直接判攻击方胜利
            if(null == _defencer || defencePower <= 0)
            {
                win = true;
                //日志记录
                _attaker.logBattle(true, true, _defencer, attackPower, 0, attackTroopNum, 0, 0, 0);

                break;
            }

            //防守方战力/进攻方战力
            long powerPer = defencePower * 10000 / attackPower;
//            ALServerLog.Sys("RefGeneral.Ref().mars_explore_team_loss_max_per - " + RefGeneral.Ref().mars_explore_team_loss_max_per);
//            ALServerLog.Sys("RefGeneral.Ref().mars_explore_team_loss_min_per - " + RefGeneral.Ref().mars_explore_team_loss_min_per);

            //【公式】【进攻方损失精兵】=【防守方当前精兵】*【战损】
            //long attackLoseValue = defenceTroopNum * powerPer / 10000;
            //当【进攻方损失精兵】＞【进攻方当前精兵】，战败
            //alzq : 直接修改未战力决定胜负
            if(defencePower > attackPower)
            {
                //【公式】【战败方剩余精兵】=败方当前精兵*败方精兵实际损失系数
                //计算损失万分比
                long failCostPer = RefGeneral.Ref().mars_explore_team_base_cost_per * (
                                        10000 + (RefGeneral.Ref().mars_explore_team_fail_cost_per * powerPer / 10000)
                                ) / 10000;

                if(failCostPer > RefGeneral.Ref().mars_explore_team_loss_max_per)
                    failCostPer = RefGeneral.Ref().mars_explore_team_loss_max_per;

                //计算上下限
                //败方精兵实际损失系数=败方基础损失系数*(1+实力碾压对败方损失加成*(胜方战力/败方战力))
                long failCostValue =
                        attackTroopNum * failCostPer / 10000;

                if(failCostValue < minHurtNum)
                    failCostValue = minHurtNum;

                //进攻方增加伤兵数量
                _attaker.addHurtSoldierNum(failCostValue);
                result.attackerHurtSoldierNum = failCostValue;
//                ALServerLog.Sys("RefGeneral.Ref().mars_explore_team_base_cost_per - " + RefGeneral.Ref().mars_explore_team_base_cost_per);
//                ALServerLog.Sys("RefGeneral.Ref().mars_explore_team_fail_cost_per - " + RefGeneral.Ref().mars_explore_team_fail_cost_per);

                //【公式】【战胜方损失精兵】=胜方当前精兵*胜方精兵实际损失系数
                //计算损失万分比
                long winCostPer = RefGeneral.Ref().mars_explore_team_win_cost_per * (
                                        10000 - (RefGeneral.Ref().mars_explore_team_win_red_per * (
                                                (defencePower - attackPower) * 10000 / (defencePower + attackPower)
                                        ) / 10000)
                                ) / 10000;

                if(winCostPer > RefGeneral.Ref().mars_explore_team_loss_max_per)
                    winCostPer = RefGeneral.Ref().mars_explore_team_loss_max_per;

                //胜方精兵实际损失系数=胜方基础损失系数*(1-胜方损失减免系数*(胜方战力-败方战力)/(胜方战力+败方战力))
                long winCostValue =
                        defenceTroopNum * winCostPer / 10000;

                //注:胜方有损失上限,胜方损失超过败方损失时 胜方损失=败方损失
                //winCostValue = Math.min(winCostValue, failCostValue);

                if(winCostValue < minHurtNum)
                    winCostValue = minHurtNum;

                //防御方增加损失
                _defencer.addHurtSoldierNum(winCostValue);
                result.defencerHurtSoldierNum = winCostValue;
//                ALServerLog.Sys("RefGeneral.Ref().mars_explore_team_win_cost_per - " + RefGeneral.Ref().mars_explore_team_win_cost_per);
//                ALServerLog.Sys("RefGeneral.Ref().mars_explore_team_win_red_per - " + RefGeneral.Ref().mars_explore_team_win_red_per);

                win = false;

                //日志记录
                _attaker.logBattle(true, false, _defencer, attackPower, failCostValue, attackTroopNum, defencePower, winCostValue, defenceTroopNum);
                _defencer.logBattle(false, true, _attaker, attackPower, failCostValue, attackTroopNum, defencePower, winCostValue, defenceTroopNum);
            }
            else //战胜
            {
                //【公式】【战败方剩余精兵】=败方当前精兵*败方精兵实际损失系数
                //计算损失万分比
                long failCostPer = RefGeneral.Ref().mars_explore_team_base_cost_per * (
                                    10000 + RefGeneral.Ref().mars_explore_team_fail_cost_per * (attackPower * 10000 / defencePower) / 10000
                            ) / 10000;

                if(failCostPer > RefGeneral.Ref().mars_explore_team_loss_max_per)
                    failCostPer = RefGeneral.Ref().mars_explore_team_loss_max_per;

                //败方精兵实际损失系数=败方基础损失系数*(1+实力碾压对败方损失加成*(胜方战力/败方战力))
                long failCostValue =
                        defenceTroopNum * failCostPer / 10000;

                if(failCostValue < minHurtNum)
                    failCostValue = minHurtNum;

                //防御方增加损失
                _defencer.addHurtSoldierNum(failCostValue);
                result.defencerHurtSoldierNum = failCostValue;
//                ALServerLog.Sys("RefGeneral.Ref().mars_explore_team_base_cost_per - " + RefGeneral.Ref().mars_explore_team_base_cost_per);
//                ALServerLog.Sys("RefGeneral.Ref().mars_explore_team_fail_cost_per - " + RefGeneral.Ref().mars_explore_team_fail_cost_per);

                //【公式】【战胜方损失精兵】=胜方当前精兵*胜方精兵实际损失系数
                //计算损失万分比
                long winCostPer = RefGeneral.Ref().mars_explore_team_win_cost_per * (
                                        10000 - RefGeneral.Ref().mars_explore_team_win_red_per * (
                                                (attackPower - defencePower) * 10000 / (defencePower + attackPower)
                                        ) / 10000
                                ) / 10000;

                if(winCostPer > RefGeneral.Ref().mars_explore_team_loss_max_per)
                    winCostPer = RefGeneral.Ref().mars_explore_team_loss_max_per;

                //胜方精兵实际损失系数=胜方基础损失系数*(1-胜方损失减免系数*(胜方战力-败方战力)/(胜方战力+败方战力))
                long winCostValue =
                        attackTroopNum * winCostPer / 10000;
                //注:胜方有损失上限,胜方损失超过败方损失时 胜方损失=败方损失
                //winCostValue = Math.min(winCostValue, failCostValue);

                if(winCostValue < minHurtNum)
                    winCostValue = minHurtNum;

                //进攻方增加伤兵数量
                _attaker.addHurtSoldierNum(winCostValue);
                result.attackerHurtSoldierNum = winCostValue;
//                ALServerLog.Sys("RefGeneral.Ref().mars_explore_team_win_cost_per - " + RefGeneral.Ref().mars_explore_team_win_cost_per);
//                ALServerLog.Sys("RefGeneral.Ref().mars_explore_team_win_red_per - " + RefGeneral.Ref().mars_explore_team_win_red_per);

                win = true;

                //日志记录
                _attaker.logBattle(true, true, _defencer, attackPower, winCostValue, attackTroopNum, defencePower, failCostValue, defenceTroopNum);
                _defencer.logBattle(false, false, _attaker, attackPower, winCostValue, attackTroopNum, defencePower, failCostValue, defenceTroopNum);
            }
        } while(false);

        result.win = win;


        //返回战斗结果
        return result;
    }

    /**
     * 战斗结果类
     * 用于返回PVP战斗的计算结果，包含进攻方和防守方的战斗损失信息
     *
     * 字段说明：
     * - oriSoldierNum: 原士兵数
     * - hurtSoldierNum: 受伤士兵数
     * - singleSoldierPower: 单个士兵战力
     */
    public static class FightResult
    {
        /** 是否进攻方胜利 */
        public boolean win;

        /** 进攻方原始士兵数量 */
        public long attackerOriSoldierNum;
        /** 进攻方受伤（损失）士兵数量 */
        public long attackerHurtSoldierNum;
        /** 进攻方单个士兵战力 */
        public long attackerSingleSoldierPower;

        /** 防守方原始士兵数量 */
        public long defencerOriSoldierNum;
        /** 防守方受伤（损失）士兵数量 */
        public long defencerHurtSoldierNum;
        /** 防守方单个士兵战力 */
        public long defencerSingleSoldierPower;
    }

    /****************************
     * ============== 战斗计算V1版切换到V2计算方式的切换函数 - 开始 ==============
     */
    /**
     * 当前使用的战斗计算方法，这是一个入口函数，根据配置选择合适的战斗算法
     * @param _attaker 进攻方对象
     * @param _defencer 防守方对象
     * @return 战斗结果
     */
    public static FightResult curUseCalFight(_IUsMarsBattleObj _attaker, _IUsMarsBattleObj _defencer)
    {
        return calFight(_attaker, _defencer);
    }

    /**
     * V1版本转换为V2版本的第一种转换方式
     * 使用V2计算方法calFight_v1，该方法每回合后重新规整所有Blob数据
     * @param _attaker 进攻方对象
     * @param _defencer 防守方对象
     * @return 战斗结果
     */
    public static FightResult calFight_v1Var_use_v2_1(_IUsMarsBattleObj _attaker, _IUsMarsBattleObj _defencer)
    {
        //参数空检查
        if(null == _attaker || null == _defencer)
            return null;

        //构建v2参数
        ServerObj_MarsBattleV2_TeamInfo v2Att = new ServerObj_MarsBattleV2_TeamInfo();
        v2Att.getTeamMajorBonus().add(new ServerObj_MarsBattleV2_PropertyBonus(ENPPlayerPropertyType.MARS_TEAM_SOLDIER_POWER.ordinal(), _attaker.getTeamSoldierPower()));
        v2Att.getMemberList().add(new MarsBattleV2_MemberInfo(0, 1, _attaker.getTeamSoldierNum(), 0));

        //构建v2参数
        ServerObj_MarsBattleV2_TeamInfo v2def = new ServerObj_MarsBattleV2_TeamInfo();
        v2def.getTeamMajorBonus().add(new ServerObj_MarsBattleV2_PropertyBonus(ENPPlayerPropertyType.MARS_TEAM_SOLDIER_POWER.ordinal(), _defencer.getTeamSoldierPower()));
        v2def.getMemberList().add(new MarsBattleV2_MemberInfo(0, 1, _defencer.getTeamSoldierNum(), 0));

        //构造结果数据
        FightResult result = new FightResult();
        result.attackerOriSoldierNum = _attaker.getTeamSoldierNum();
        result.attackerSingleSoldierPower = _attaker.getTeamSoldierPower();
        result.defencerOriSoldierNum = _defencer.getTeamSoldierNum();
        result.defencerSingleSoldierPower = _defencer.getTeamSoldierPower();

        //计算战斗结果
        FightV2_Result fightV2Res = calFight_v1(v2Att, v2def);

        //从结果构造返回数据
        result.win = fightV2Res.isAttWin;
        result.attackerHurtSoldierNum = fightV2Res.attackerResult.getMemberList().get(0).getSoldierFinalLossValue();
        result.defencerHurtSoldierNum = fightV2Res.defencerResult.getMemberList().get(0).getSoldierFinalLossValue();

        //返回转变后的结果
        return result;
    }

    /**
     * V1版本转换为V2版本的第二种转换方式
     * 使用V2计算方法calFight_v2，该方法只在战斗开始时规整一次Blob，后续每回合只进行补员处理
     * @param _attaker 进攻方对象
     * @param _defencer 防守方对象
     * @return 战斗结果
     */
    public static FightResult calFight_v1Var_use_v2_2(_IUsMarsBattleObj _attaker, _IUsMarsBattleObj _defencer)
    {
        //参数空检查
        if(null == _attaker || null == _defencer)
            return null;

        //构建v2参数
        ServerObj_MarsBattleV2_TeamInfo v2Att = new ServerObj_MarsBattleV2_TeamInfo();
        v2Att.getTeamMajorBonus().add(new ServerObj_MarsBattleV2_PropertyBonus(ENPPlayerPropertyType.MARS_TEAM_SOLDIER_POWER.ordinal(), _attaker.getTeamSoldierPower()));
        v2Att.getMemberList().add(new MarsBattleV2_MemberInfo(0, 1, _attaker.getTeamSoldierNum(), 0));

        //构建v2参数
        ServerObj_MarsBattleV2_TeamInfo v2def = new ServerObj_MarsBattleV2_TeamInfo();
        v2def.getTeamMajorBonus().add(new ServerObj_MarsBattleV2_PropertyBonus(ENPPlayerPropertyType.MARS_TEAM_SOLDIER_POWER.ordinal(), _defencer.getTeamSoldierPower()));
        v2def.getMemberList().add(new MarsBattleV2_MemberInfo(0, 1, _defencer.getTeamSoldierNum(), 0));

        //构造结果数据
        FightResult result = new FightResult();
        result.attackerOriSoldierNum = _attaker.getTeamSoldierNum();
        result.attackerSingleSoldierPower = _attaker.getTeamSoldierPower();
        result.defencerOriSoldierNum = _defencer.getTeamSoldierNum();
        result.defencerSingleSoldierPower = _defencer.getTeamSoldierPower();

        //计算战斗结果
        FightV2_Result fightV2Res = calFight_v2(v2Att, v2def);

        //从结果构造返回数据
        result.win = fightV2Res.isAttWin;
        result.attackerHurtSoldierNum = fightV2Res.attackerResult.getMemberList().get(0).getSoldierFinalLossValue();
        result.defencerHurtSoldierNum = fightV2Res.defencerResult.getMemberList().get(0).getSoldierFinalLossValue();

        //返回转变后的结果
        return result;
    }

    /****************************
     * ============== 战斗计算V1版切换到V2计算方式的切换函数 - 结束 ==============
     */

    /****************************
     * ============== 战斗计算V2版 - 开始 ==============
     */

    /**
     * 带入进攻者和防御者，进行战斗结果计算
     * @param _attacker
     * @param _defencer
     */
    //战斗回合数限制，超过此回合数则停止战斗计算
    public static int C_FIGHTV2_Round = 5;
    //Blob分块的大小，每个Blob最多包含的士兵数量
    public static int C_FIGHTV2_BlobSize = 10000;
    //总损失比例达到此阈值（万分比）之后该对象判定为无效
    public static int C_FIGHTV2_FailTotalLossPer = 5000;

    /**
     * 计算方式1，每次战斗回合后都重新规整Blob，再进行下一轮战斗
     * 该方法会进行多个回合的循环战斗，每个回合内进攻方和防守方都有机会进攻
     * 每个回合后会重新规整所有Blob数据，重新核算兵力分配
     * @param _attacker 进攻方队伍信息
     * @param _defencer 防守方队伍信息
     * @return 战斗结果对象，包含胜负和双方的战斗损失
     */
    public static FightV2_Result calFight_v1(ServerObj_MarsBattleV2_TeamInfo _attacker, ServerObj_MarsBattleV2_TeamInfo _defencer)
    {
        //构造结果对象
        FightV2_Result result = new FightV2_Result(_attacker, _defencer);

        //构造战斗计算的进攻对象和防御对象
        FightV2_CalTeamInfo attackTeam = new FightV2_CalTeamInfo(result.attackerResult);
        FightV2_CalTeamInfo defencerTeam = new FightV2_CalTeamInfo(result.defencerResult);

        //开始根据战斗规则进行处理
        for(int i = 0; i < C_FIGHTV2_Round; i++)
        {
            //每个回合先重新核算和分配双方Blob
            attackTeam.rebuildAllMemberBlob();
            defencerTeam.rebuildAllMemberBlob();

            //进行进攻方回合计算
            _roundCal(attackTeam, defencerTeam);

            //在切换双方计算角色前，结算所有Blob，重新划分
            attackTeam.calResultAllMember();
            defencerTeam.calResultAllMember();
            //判断是否已经决出胜负
            if(!defencerTeam.judgeTeamEnable())
            {
                //进攻方获胜
                result.isAttWin = true;
                break;
            }
            if(!attackTeam.judgeTeamEnable())
            {
                //防御方获胜
                result.isAttWin = false;
                break;
            }

            //每个回合先重新核算和分配双方Blob
            attackTeam.rebuildAllMemberBlob();
            defencerTeam.rebuildAllMemberBlob();

            //进行防御方进攻结算
            _roundCal(defencerTeam, attackTeam);

            //结算双方结果
            attackTeam.calResultAllMember();
            defencerTeam.calResultAllMember();
            //判断是否已经决出胜负
            if(!defencerTeam.judgeTeamEnable())
            {
                //进攻方获胜
                result.isAttWin = true;
                break;
            }
            if(!attackTeam.judgeTeamEnable())
            {
                //防御方获胜
                result.isAttWin = false;
                break;
            }
        }

        return result;
    }

    /**
     * 计算方式2，只在战斗开始的时候做一次规整，每个回合之后做的事情只是将溢出兵力填充到前面的blob中
     * 尽量保证既有Blob满员，相比v1版本更加高效
     * 该方法会进行多个回合的循环战斗，每个回合内只进行补员处理而不是重新规整
     * @param _attacker 进攻方队伍信息
     * @param _defencer 防守方队伍信息
     * @return 战斗结果对象，包含胜负和双方的战斗损失
     */
    public static FightV2_Result calFight_v2(ServerObj_MarsBattleV2_TeamInfo _attacker, ServerObj_MarsBattleV2_TeamInfo _defencer)
    {
        //构造结果对象
        FightV2_Result result = new FightV2_Result(_attacker, _defencer);

        //构造战斗计算的进攻对象和防御对象
        FightV2_CalTeamInfo attackTeam = new FightV2_CalTeamInfo(result.attackerResult);
        FightV2_CalTeamInfo defencerTeam = new FightV2_CalTeamInfo(result.defencerResult);

        //初始构建Blob
        attackTeam.rebuildAllMemberBlob();
        defencerTeam.rebuildAllMemberBlob();

        //开始根据战斗规则进行处理
        for(int i = 0; i < C_FIGHTV2_Round; i++)
        {
            //进行进攻方回合计算
            _roundCal(attackTeam, defencerTeam);

            //在切换双方计算角色前，尝试给Blob补员
            attackTeam.tryRefitAllMemberBlob();
            defencerTeam.tryRefitAllMemberBlob();
            //判断是否已经决出胜负
            if(!defencerTeam.judgeMemberHasBlobEnable(C_FIGHTV2_BlobSize))
            {
                //进攻方获胜
                result.isAttWin = true;
                break;
            }
            if(!attackTeam.judgeMemberHasBlobEnable(C_FIGHTV2_BlobSize))
            {
                //防御方获胜
                result.isAttWin = false;
                break;
            }

            //进行防御方进攻结算
            _roundCal(defencerTeam, attackTeam);

            //在切换双方计算角色前，尝试给Blob补员
            attackTeam.tryRefitAllMemberBlob();
            defencerTeam.tryRefitAllMemberBlob();
            //判断是否已经决出胜负
            if(!defencerTeam.judgeMemberHasBlobEnable(C_FIGHTV2_BlobSize))
            {
                //进攻方获胜
                result.isAttWin = true;
                break;
            }
            if(!attackTeam.judgeMemberHasBlobEnable(C_FIGHTV2_BlobSize))
            {
                //防御方获胜
                result.isAttWin = false;
                break;
            }
        }

        //结束前结算一次战斗损失
        attackTeam.calResultAllMember();
        defencerTeam.calResultAllMember();

        return result;
    }

    /**
     * 每个回合的计算过程
     * 该方法负责进行一个完整回合的战斗计算，进攻方的每个成员的每个Blob轮询对防御方进行攻击
     * 防御方轮询每个成员的每个Blob进行防御
     * 防御方轮询到最后一个之后会重新从头开始轮询
     * @param _attackTeam 进攻方队伍对象
     * @param _defencerTeam 防守方队伍对象
     */
    protected static void _roundCal(FightV2_CalTeamInfo _attackTeam, FightV2_CalTeamInfo _defencerTeam)
    {
        if(null == _attackTeam || null == _defencerTeam)
            return ;

        //获取当前进攻方对象
        FightV2_CalMemberInfo attackMember = _attackTeam.getCurCalMemberInfo();
        FightV2_CalMemberInfo defencerMember = _defencerTeam.getCurCalMemberInfo();
        while(null != attackMember)
        {
            //获取进攻方Blob
            FightV2_CalTeamInfo_Blob attackBlob = attackMember.getCurBlob(C_FIGHTV2_BlobSize);
            //如果数据为空，则取下一个member
            while(null == attackBlob)
            {
                //获取下一个成员
                _attackTeam.moveNextCalMemberInfo();
                //获取成员当前blob
                attackMember = _attackTeam.getCurCalMemberInfo();
                //如果已经取到空直接返回
                if(null == attackMember)
                    break;

                //无效成员不获取战斗Blob
                if(attackMember.judgeMemberEnable())
                    attackBlob = attackMember.getCurBlob(C_FIGHTV2_BlobSize);
            }

            //如果member为空表示本轮循环结束
            if(null == attackMember)
                break ;

            //获取防御方的Blob对象
            FightV2_CalTeamInfo_Blob defencerBlob = defencerMember.getCurBlob(C_FIGHTV2_BlobSize);
            //是否已经经过一个循环，如果已经经过循环还需要重置时，此时表示防御方已无效，直接返回
            boolean hasLooped = false;
            //如果数据为空，则取下一个member
            while(null == defencerBlob)
            {
                //获取下一个成员
                _defencerTeam.moveNextCalMemberInfo();
                //获取成员当前blob
                defencerMember = _defencerTeam.getCurCalMemberInfo();
                //如果已经取到空直接返回
                if(null == defencerMember)
                {
                    //防御方如果为空，则重头开始取
                    _defencerTeam.resetCalIndex();
                    if(!hasLooped) {
                        hasLooped = true;
                        defencerMember = _defencerTeam.getCurCalMemberInfo();
                    }
                    else
                    {
                        break;
                    }
                }

                //无效成员跳过，不获取Blob
                if(defencerMember != null && defencerMember.judgeMemberEnable())
                    defencerBlob = defencerMember.getCurBlob(C_FIGHTV2_BlobSize);
            }

            //如果防御 Blob为空，表示已经结束
            if(null == defencerBlob)
                break ;

            //开始使用Blob对防御方的Blob进行攻击
            _blobCal(_attackTeam, attackMember, attackBlob, _defencerTeam, defencerMember, defencerBlob);

            //获取下一个进攻Blob
            attackMember.moveNextBlob();
            defencerMember.moveNextBlob();
        }
    }

    /**
     * 单个Blob计算的过程函数
     * 负责两个Blob之间的战斗损伤计算
     * 根据双方的战力和配置参数计算损失比例，然后计算具体损失人数
     * @param _attackTeam 进攻方队伍对象
     * @param _attackMember 进攻方成员对象
     * @param _attackBlob 进攻方分块对象
     * @param _defenceTeam 防守方队伍对象
     * @param _defenceMember 防守方成员对象
     * @param _defencerBlob 防守方分块对象
     */
    protected static void _blobCal(FightV2_CalTeamInfo _attackTeam, FightV2_CalMemberInfo _attackMember, FightV2_CalTeamInfo_Blob _attackBlob
            , FightV2_CalTeamInfo _defenceTeam, FightV2_CalMemberInfo _defenceMember, FightV2_CalTeamInfo_Blob _defencerBlob)
    {
        if(null == _attackBlob || null == _defencerBlob
            || null == _attackMember || null == _defenceMember
            || null == _attackTeam || null == _defenceTeam)
            return ;

        //TODO ： 获取各自兵种等级，并根据等级及属性计算最终战力参数

        //获取各自加成数据
        long attSoldierPower = _attackTeam.getPropertyBonusNum(ENPPlayerPropertyType.MARS_TEAM_SOLDIER_POWER);
        long attSoldierPowerPer = _attackTeam.getPropertyBonusNum(ENPPlayerPropertyType.MARS_TEAM_SOLDIER_POWER_PER);
        long defSoldierPower = _defenceTeam.getPropertyBonusNum(ENPPlayerPropertyType.MARS_TEAM_SOLDIER_POWER);
        long defSoldierPowerPer = _defenceTeam.getPropertyBonusNum(ENPPlayerPropertyType.MARS_TEAM_SOLDIER_POWER_PER);

        //计算进攻方及防守方单兵实力计算值
        //临时使用属性计算单兵战力
        long attackSoldierPowerFinal = attSoldierPower * (10000 + attSoldierPowerPer) / 10000;
        long defenceSoldierPowerFinal = defSoldierPower * (10000 + defSoldierPowerPer) / 10000;

        //进行战斗计算
        do
        {
            long attackTroopNum = _attackBlob.getBlobSoldierNum();
            long defenceTroopNum = _defencerBlob.getBlobSoldierNum();  // 修复: 从防守方Blob获取兵力数量

            long attackPower = attackTroopNum * attackSoldierPowerFinal;
            long defencePower = defenceTroopNum * defenceSoldierPowerFinal;

            if(attackPower <= 0 || defencePower <= 0)
            {
                break;
            }

            //使用双方最小数量的最低比例作为最低战损，不单独使用比例处理战损
            //使用旧方法时由于最小战损会涉及N个回合，因此以最大回合数将最小战损切割，避免实际最小战损超过目标最小战损
            long minHurtNum = Math.min(attackTroopNum, defenceTroopNum) * RefGeneral.Ref().mars_explore_team_loss_min_per / C_FIGHTV2_Round / 10000;
            if(minHurtNum < 1)
                minHurtNum = 1;

            //防守方战力/进攻方战力
 //           long powerPer = defencePower * 10000 / attackPower;
//            ALServerLog.Sys("RefGeneral.Ref().mars_explore_team_loss_max_per - " + RefGeneral.Ref().mars_explore_team_loss_max_per);
//            ALServerLog.Sys("RefGeneral.Ref().mars_explore_team_loss_min_per - " + RefGeneral.Ref().mars_explore_team_loss_min_per);

            //【公式】【进攻方损失精兵】=【防守方当前精兵】*【战损】
            //long attackLoseValue = defenceTroopNum * powerPer / 10000;
            //当【进攻方损失精兵】＞【进攻方当前精兵】，战败
            //alzq : 直接修改未战力决定胜负
            if(defencePower > attackPower)
            {
                //【公式】【战败方剩余精兵】=败方当前精兵*败方精兵实际损失系数
                //计算损失万分比
                long failCostPer = RefGeneral.Ref().mars_explore_team_base_cost_per * (
                        10000 + (RefGeneral.Ref().mars_explore_team_fail_cost_per * defencePower * 10000 / attackPower / 10000)
                ) / 10000;

                if(failCostPer > RefGeneral.Ref().mars_explore_team_loss_max_per)
                    failCostPer = RefGeneral.Ref().mars_explore_team_loss_max_per;

                //计算上下限
                //败方精兵实际损失系数=败方基础损失系数*(1+实力碾压对败方损失加成*(胜方战力/败方战力))
                long failCostValue =
                        attackTroopNum * failCostPer / 10000;

                if(failCostValue < minHurtNum)
                    failCostValue = minHurtNum;

                //进攻方增加伤兵数量
                _attackBlob.lossSoldier(failCostValue);

                //【公式】【战胜方损失精兵】=胜方当前精兵*胜方精兵实际损失系数
                //计算损失万分比
                long winCostPer = RefGeneral.Ref().mars_explore_team_win_cost_per * (
                        10000 - (RefGeneral.Ref().mars_explore_team_win_red_per * (
                                (defencePower - attackPower) * 10000 / (defencePower + attackPower)
                        ) / 10000)
                ) / 10000;

                if(winCostPer > RefGeneral.Ref().mars_explore_team_loss_max_per)
                    winCostPer = RefGeneral.Ref().mars_explore_team_loss_max_per;

                //胜方精兵实际损失系数=胜方基础损失系数*(1-胜方损失减免系数*(胜方战力-败方战力)/(胜方战力+败方战力))
                long winCostValue =
                        defenceTroopNum * winCostPer / 10000;

                //注:胜方有损失上限,胜方损失超过败方损失时 胜方损失=败方损失
                //winCostValue = Math.min(winCostValue, failCostValue);

                if(winCostValue < minHurtNum)
                    winCostValue = minHurtNum;

                //防御方增加损失
                _defencerBlob.lossSoldier(winCostValue);
            }
            else //战胜
            {
                //【公式】【战败方剩余精兵】=败方当前精兵*败方精兵实际损失系数
                //计算损失万分比
                long failCostPer = RefGeneral.Ref().mars_explore_team_base_cost_per * (
                        10000 + RefGeneral.Ref().mars_explore_team_fail_cost_per * (attackPower * 10000 / defencePower) / 10000
                ) / 10000;

                if(failCostPer > RefGeneral.Ref().mars_explore_team_loss_max_per)
                    failCostPer = RefGeneral.Ref().mars_explore_team_loss_max_per;

                //败方精兵实际损失系数=败方基础损失系数*(1+实力碾压对败方损失加成*(胜方战力/败方战力))
                long failCostValue =
                        defenceTroopNum * failCostPer / 10000;

                if(failCostValue < minHurtNum)
                    failCostValue = minHurtNum;

                //防御方增加损失
                _defencerBlob.lossSoldier(failCostValue);

                //【公式】【战胜方损失精兵】=胜方当前精兵*胜方精兵实际损失系数
                //计算损失万分比
                long winCostPer = RefGeneral.Ref().mars_explore_team_win_cost_per * (
                        10000 - RefGeneral.Ref().mars_explore_team_win_red_per * (
                                (attackPower - defencePower) * 10000 / (defencePower + attackPower)
                        ) / 10000
                ) / 10000;

                if(winCostPer > RefGeneral.Ref().mars_explore_team_loss_max_per)
                    winCostPer = RefGeneral.Ref().mars_explore_team_loss_max_per;

                //胜方精兵实际损失系数=胜方基础损失系数*(1-胜方损失减免系数*(胜方战力-败方战力)/(胜方战力+败方战力))
                long winCostValue =
                        attackTroopNum * winCostPer / 10000;
                //注:胜方有损失上限,胜方损失超过败方损失时 胜方损失=败方损失
                //winCostValue = Math.min(winCostValue, failCostValue);

                if(winCostValue < minHurtNum)
                    winCostValue = minHurtNum;

                //进攻方增加伤兵数量
                _attackBlob.lossSoldier(winCostValue);
            }
        } while(false);
    }

    /**
     * 战斗计算V2版本的队伍信息对象
     * 负责管理一个队伍在战斗中的成员计算信息、属性加成等
     * 包含成员列表索引管理和属性查询等功能
     */
    public static class FightV2_CalTeamInfo
    {
        //数据结构中的信息对象，存储原始的队伍战斗结果数据
        private ServerObj_MarsBattleV2_TeamFightResult _m_tiTeamInfo;
        //队伍内的成员计算对象列表，用于战斗计算
        private ArrayList<FightV2_CalMemberInfo> _m_listMemberInfos;

        //当前计算的成员索引，用于轮询计算
        private int _m_iCalIndex;

        /**
         * 构造函数，初始化队伍信息对象
         * @param _teamInfo 队伍战斗结果信息对象
         */
        public FightV2_CalTeamInfo(ServerObj_MarsBattleV2_TeamFightResult _teamInfo)
        {
            _m_tiTeamInfo = _teamInfo;

            //逐个添加成员信息
            _m_listMemberInfos = new ArrayList<FightV2_CalMemberInfo>();
            for(ServerObj_MarsBattleV2_MemberFightResult memberInfo : _teamInfo.getMemberList())
            {
                if(null == memberInfo)
                    continue;

                FightV2_CalMemberInfo calMemberInfo = new FightV2_CalMemberInfo(memberInfo);
                _m_listMemberInfos.add(calMemberInfo);
            }
        }

        /**
         * 获取队伍战斗结果信息对象
         * @return 队伍信息对象
         */
        public ServerObj_MarsBattleV2_TeamFightResult getTeamInfo() {return _m_tiTeamInfo;}

        /**
         * 判断整个队伍的成员是否都无效，如果都无效则表示队伍无效
         * @return true表示队伍有效成员，false表示队伍已全灭
         */
        public boolean judgeTeamEnable()
        {
            for(FightV2_CalMemberInfo memberInfo : _m_listMemberInfos)
            {
                if(null == memberInfo)
                    continue;

                if(memberInfo.judgeMemberEnable())
                    return true;
            }

            return false;
        }

        /**
         * 针对所有成员的Blob进行有效性检查，在v2计算模式里没有每轮结算结果，因此不能使用总数核算有效性
         * @param _blobSize Blob的大小
         * @return true表示存在有效的Blob，false表示所有Blob都无效
         */
        public boolean judgeMemberHasBlobEnable(long _blobSize)
        {
            for(FightV2_CalMemberInfo memberInfo : _m_listMemberInfos)
            {
                if(null == memberInfo)
                    continue;

                if(memberInfo.judgeMemberHasBlobEnable(_blobSize))
                    return true;
            }

            return false;
        }

        /**
         * 重新构建每个成员的Blob数据
         * 该操作会根据成员当前兵力和设定的BlobSize重新分块
         */
        public void rebuildAllMemberBlob()
        {
            for(FightV2_CalMemberInfo memberInfo : _m_listMemberInfos)
            {
                if(null == memberInfo)
                    continue;

                //根据分块大小重新分块
                memberInfo.rebuildBlob(C_FIGHTV2_BlobSize);
            }
        }

        /**
         * 每个对象尝试给每个Blob补员
         * 用于在战斗过程中维持Blob的满员状态
         */
        public void tryRefitAllMemberBlob()
        {
            for(FightV2_CalMemberInfo memberInfo : _m_listMemberInfos)
            {
                if(null == memberInfo)
                    continue;

                //尝试补员处理
                memberInfo.tryRefitAllBlob(C_FIGHTV2_BlobSize);
            }
        }

        /**
         * 结算所有成员的Blob战斗数据
         * 将Blob中的损失士兵数量计入成员的总损失中
         */
        public void calResultAllMember()
        {
            for(FightV2_CalMemberInfo memberInfo : _m_listMemberInfos)
            {
                if(null == memberInfo)
                    continue;

                //结算当前成员所有Blob结果
                memberInfo.calBlobResult();
            }
        }

        /**
         * 获取当前计算的成员信息对象
         * @return 当前索引指向的成员信息对象，如果索引越界则返回null
         */
        public FightV2_CalMemberInfo getCurCalMemberInfo()
        {
            if(_m_iCalIndex < 0 || _m_iCalIndex >= _m_listMemberInfos.size())
                return null;

            return _m_listMemberInfos.get(_m_iCalIndex);
        }

        /**
         * 获取某个属性加成的具体值
         * @param _propertyType 属性类型
         * @return 属性加成值，如果不存在则返回0
         */
        public long getPropertyBonusNum(ENPPlayerPropertyType _propertyType)
        {
            if(null == getTeamInfo().getTeamMajorBonus())
                return 0;

            //从队伍加成中检索
            for(ServerObj_MarsBattleV2_PropertyBonus bonus : getTeamInfo().getTeamMajorBonus())
            {
                if(null == bonus)
                    continue;

                if(bonus.getType() == _propertyType.ordinal())
                    return bonus.getValue();
            }

            return 0;
        }

        /**
         * 移动到下一个成员用于计算
         */
        public void moveNextCalMemberInfo()
        {
            _m_iCalIndex++;
        }

        /**
         * 重置成员计算索引，使其指向第一个成员
         */
        public void resetCalIndex()
        {
            _m_iCalIndex = 0;
        }

        /**
         * 获取指定ID的成员计算对象
         * @param _memberId 成员ID
         * @return 对应的成员计算对象，如果不存在则返回null
         */
        public FightV2_CalMemberInfo getMemberInfo(long _memberId)
        {
            for(FightV2_CalMemberInfo memberInfo : _m_listMemberInfos)
            {
                if(null == memberInfo)
                    continue;

                if(memberInfo._m_tiMemberInfo.getMemberId() == _memberId)
                    return memberInfo;
            }

            return null;
        }
    }


    /**
     * 战斗V2版本的成员计算信息对象
     * 用于管理队伍中单个成员的战斗分块、损失统计等信息
     * 每个成员可以包含多个分块(Blob)，用于分阶段计算战斗损失
     */
    public static class FightV2_CalMemberInfo
    {
        //数据结构中的信息对象，存储原始的成员战斗结果数据
        private ServerObj_MarsBattleV2_MemberFightResult _m_tiMemberInfo;
        //划分的分组（分块），每个战斗回合开始前重新分组
        private ArrayList<FightV2_CalTeamInfo_Blob> _m_listBlobs;

        //构造时的初始士兵数量（用于计算损失比例）
        private long _m_lSrcSoldierNum;

        //当前战斗选取Blob的索引，用于轮询每个Blob进行战斗
        private int _m_iCurBlobIndex;

        /**
         * 构造函数，初始化成员计算信息对象
         * @param _memberInfo 成员战斗结果信息对象
         */
        public FightV2_CalMemberInfo(ServerObj_MarsBattleV2_MemberFightResult _memberInfo)
        {
            _m_tiMemberInfo = _memberInfo;
            _m_listBlobs = new ArrayList<FightV2_CalTeamInfo_Blob>();
            _m_lSrcSoldierNum = _memberInfo.getSoldierNum() - _memberInfo.getSoldierFinalLossValue();
            _m_iCurBlobIndex = 0;
        }

        /**
         * 判断这个参战成员是否还有效
         * 成员有效需要满足：兵力大于0且损失比例未达到上限
         * @return true表示成员有效，false表示成员已无效
         */
        public boolean judgeMemberEnable()
        {
            long curSoldierNum = _m_tiMemberInfo.getSoldierNum() - _m_tiMemberInfo.getSoldierFinalLossValue();

            if(curSoldierNum <= 0)
                return false;

            //判断万分比是否有效
            return curSoldierNum * 10000 / _m_lSrcSoldierNum >= C_FIGHTV2_FailTotalLossPer;
        }

        /**
         * 判断这个成员的所有Blob是否有效
         * @param _blobSize Blob的大小
         * @return true表示存在有效的Blob，false表示所有Blob都无效
         */
        public boolean judgeMemberHasBlobEnable(long _blobSize)
        {
            for(FightV2_CalTeamInfo_Blob blob : _m_listBlobs)
            {
                if(null == blob)
                    continue;

                if(blob.judgeEnable(_blobSize))
                    return true;
            }

            return false;
        }

        /**
         * 获取当前分块（Blob）对象
         * @param _blobSize Blob的大小标准
         * @return 当前有效的Blob对象，如果没有有效Blob则返回null
         */
        public FightV2_CalTeamInfo_Blob getCurBlob(long _blobSize)
        {
            if(_m_iCurBlobIndex < 0 || _m_iCurBlobIndex >= _m_listBlobs.size())
                return null;

            FightV2_CalTeamInfo_Blob blob = _m_listBlobs.get(_m_iCurBlobIndex);
            //判断blob比例是否有效
            while(null != blob && !blob.judgeEnable(_blobSize))
            {
                //如果当前Blob无效，继续取下一个Blob
                moveNextBlob();
                if(_m_iCurBlobIndex < 0 || _m_iCurBlobIndex >= _m_listBlobs.size())
                    return null;

                blob = _m_listBlobs.get(_m_iCurBlobIndex);
            }

            return blob;
        }

        /**
         * 移动到下一个分块（Blob）
         */
        public void moveNextBlob()
        {
            _m_iCurBlobIndex++;
        }

        /**
         * 重置Blob索引到第一个分块
         */
        public void resetBlobIndex()
        {
            _m_iCurBlobIndex = 0;
        }

        /**
         * 根据分块的大小，重新对成员进行分块
         * 该操作会先结算当前所有Blob的损失，然后根据剩余兵力重新分块
         * @param _blobSize 每个分块的大小
         */
        public void rebuildBlob(long _blobSize)
        {
            //进行一次结算
            calBlobResult();

            //异常数据直接不分块
            if(_blobSize <= 0)
                return ;

            long totalSoldierNum = _m_tiMemberInfo.getSoldierNum() - _m_tiMemberInfo.getSoldierFinalLossValue();
            //大于分块则添加一个完整块
            while(totalSoldierNum > _blobSize)
            {
                FightV2_CalTeamInfo_Blob blob = new FightV2_CalTeamInfo_Blob(_blobSize);
                _m_listBlobs.add(blob);

                totalSoldierNum -= _blobSize;
            }

            //如果队列为空，且分块剩余数量大于0，则添加一个分块
            if(_m_listBlobs.size() <= 0 && totalSoldierNum > 0)
            {
                FightV2_CalTeamInfo_Blob blob = new FightV2_CalTeamInfo_Blob(totalSoldierNum);
                _m_listBlobs.add(blob);
            }
        }

        /**
         * 尝试补充所有Blob到满员状态
         * 用于在战斗过程中维持Blob的兵力，使用剩余的未分配兵力进行补员
         * @param _blobSize 每个分块应该达到的大小
         */
        public void tryRefitAllBlob(long _blobSize)
        {
            //异常数据直接不分块
            if(_blobSize <= 0)
                return ;

            //获取总兵力
            long totalSoldierNumLeft = _m_tiMemberInfo.getSoldierNum() - _m_tiMemberInfo.getSoldierFinalLossValue();
            //总兵力去除所有blob分配的兵力，求剩余兵力
            for(FightV2_CalTeamInfo_Blob blob : _m_listBlobs)
            {
                if(null == blob)
                    continue;

                totalSoldierNumLeft -= blob.getBlobSoldierNum();
            }

            //每个Blob尝试增加兵力，让剩余兵力达到满员状态
            for(FightV2_CalTeamInfo_Blob blob : _m_listBlobs)
            {
                if(null == blob)
                    continue;

                //判断当前Blob是否满员
                if(blob.getBlobSoldierNum() - blob.getBlobLossValue() < _blobSize)
                {
                    //需要增加的兵力数量
                    long needRefitNum = _blobSize - (blob.getBlobSoldierNum() - blob.getBlobLossValue());
                    //实际增加的兵力数量不能超过剩余兵力数量
                    long realRefitNum = Math.min(needRefitNum, totalSoldierNumLeft);
                    if(realRefitNum > 0)
                    {
                        //增加Blob的兵力数量
                        blob.refitSoldier(realRefitNum);
                        //减少剩余兵力数量
                        totalSoldierNumLeft -= realRefitNum;
                    }
                }

                //如果剩余兵力已经没有了，直接退出
                if(totalSoldierNumLeft <= 0)
                    break;
            }

            //重置Blob索引，为下一轮循环做准备
            resetBlobIndex();
        }

        /**
         * 结算当前所有分块的战斗结果
         * 将所有Blob的损失士兵数量计入成员的最终战斗结果中
         */
        public void calBlobResult()
        {
            //每个Blob结算
            for(FightV2_CalTeamInfo_Blob blob : _m_listBlobs)
            {
                if(null == blob)
                    continue;

                //增加当前损失量
                _m_tiMemberInfo.setSoldierLossValue(
                        _m_tiMemberInfo.getSoldierLossValue() + blob.getBlobLossValue()
                );
                //增加最终损失数量
                _m_tiMemberInfo.setSoldierFinalLossValue(
                        _m_tiMemberInfo.getSoldierFinalLossValue() + blob.getBlobLossValue()
                );
            }

            //清空队列
            _m_listBlobs.clear();

            //重置Blob索引，为下一轮循环做准备
            resetBlobIndex();
        }
    }

    /**
     * 战斗V2版本的分块(Blob)对象
     * 用于表示一个战斗单位，每个分块代表一个独立的战斗小单位
     * 分块中记录兵力数量和损失数量，用于分阶段计算战斗损失
     */
    public static class FightV2_CalTeamInfo_Blob
    {
        /** 分块士兵总数量（包括已损失的） */
        private long _m_lBlobSoldierNum;
        /** 分块战斗中损失的士兵数量 */
        private long _m_lBlobLossValue;

        /**
         * 构造函数，初始化一个分块对象
         * @param _blobSoldierNum 分块的初始士兵数量
         */
        public FightV2_CalTeamInfo_Blob(long _blobSoldierNum)
        {
            _m_lBlobSoldierNum = _blobSoldierNum;
            _m_lBlobLossValue = 0;
        }

        /**
         * 获取分块的士兵总数量
         * @return 分块的士兵数量
         */
        public long getBlobSoldierNum() {return _m_lBlobSoldierNum;}

        /**
         * 获取分块的损失士兵数量
         * @return 分块中已损失的士兵数量
         */
        public long getBlobLossValue() {return _m_lBlobLossValue;}

        /**
         * 获取分块的有效（存活）士兵数量
         * @return 分块中存活的士兵数量（总数-损失数）
         */
        public long getEnableSoldierNum() {return _m_lBlobSoldierNum - _m_lBlobLossValue;}

        /**
         * 分块失去士兵
         * 这里同时记录失去的数量，并从分块士兵总数中减少对应数量
         * @param _lossValue 损失的士兵数量
         */
        public void lossSoldier(long _lossValue)
        {
            _m_lBlobSoldierNum -= _lossValue;
            _m_lBlobLossValue += _lossValue;
        }

        /**
         * 补充分块兵力，只增加总数，不增加损失数（用于补员操作）
         * @param _refitValue 补充的士兵数量
         */
        public void refitSoldier(long _refitValue)
        {
            _m_lBlobSoldierNum += _refitValue;
        }

        /**
         * 判断分块是否有效，当分块损失数量达到一定比例时即表示无效
         * 判断依据是有效兵力与分块规定大小的比例是否达到有效下限
         * @param _blobSize Blob的规定大小
         * @return true表示分块有效，false表示分块已无效
         */
        public boolean judgeEnable(long _blobSize)
        {
            if(getEnableSoldierNum() <= 0)
                return false;

            //判断万分比是否有效
            return getEnableSoldierNum() * 10000 / Math.min(_m_lBlobSoldierNum, _blobSize) >= C_FIGHTV2_FailTotalLossPer;
        }
    }

    /**
     * 战斗V2版本的战斗结果对象
     * 用于返回V2版本战斗计算的最终结果，包含进攻方和防守方的战斗结果
     */
    public static class FightV2_Result
    {
        //是否进攻方胜利
        public boolean isAttWin;

        /** 进攻方的战斗结果信息 */
        public ServerObj_MarsBattleV2_TeamFightResult attackerResult;
        /** 防守方的战斗结果信息 */
        public ServerObj_MarsBattleV2_TeamFightResult defencerResult;

        /**
         * 构造函数，根据初始的进攻方和防守方信息构造结果对象
         * @param _attacker 进攻方队伍信息
         * @param _defencer 防守方队伍信息
         */
        public FightV2_Result(ServerObj_MarsBattleV2_TeamInfo _attacker, ServerObj_MarsBattleV2_TeamInfo _defencer)
        {
            attackerResult = new ServerObj_MarsBattleV2_TeamFightResult();
            attackerResult.getTeamMajorBonus().addAll(_attacker.getTeamMajorBonus());
            //逐个成员赋值
            for(MarsBattleV2_MemberInfo memberInfo : _attacker.getMemberList())
            {
                if(null == memberInfo)
                    continue;

                //构造结果初始对象
                ServerObj_MarsBattleV2_MemberFightResult memberFightResult
                        = new ServerObj_MarsBattleV2_MemberFightResult(
                                memberInfo.getMemberId(), memberInfo.getTeamSoldierLvl(), memberInfo.getSoldierNum(), 0, memberInfo.getSoldierLossValue());
                attackerResult.getMemberList().add(memberFightResult);
            }

            defencerResult = new ServerObj_MarsBattleV2_TeamFightResult();
            defencerResult.getTeamMajorBonus().addAll(_defencer.getTeamMajorBonus());
            //逐个成员赋值
            for(MarsBattleV2_MemberInfo memberInfo : _defencer.getMemberList())
            {
                if(null == memberInfo)
                    continue;

                //构造结果初始对象
                ServerObj_MarsBattleV2_MemberFightResult memberFightResult
                        = new ServerObj_MarsBattleV2_MemberFightResult(
                        memberInfo.getMemberId(), memberInfo.getTeamSoldierLvl(), memberInfo.getSoldierNum(), 0, memberInfo.getSoldierLossValue());
                defencerResult.getMemberList().add(memberFightResult);
            }
        }
    }


    /**
     * 从属性加成结构体中获取对应的属性加成值
     * 该方法遍历属性加成列表，查找指定类型的属性加成值
     * @param _propertyBonus 属性加成列表
     * @param _propertyType 要查询的属性类型
     * @return 属性加成值，如果不存在则返回0
     */
    public long getValue(ArrayList<ServerObj_MarsBattleV2_PropertyBonus> _propertyBonus, ENPPlayerPropertyType _propertyType)
    {
        for(ServerObj_MarsBattleV2_PropertyBonus bonus : _propertyBonus)
        {
            if(bonus.getType() == _propertyType.ordinal())
                return bonus.getValue();
        }

        return 0;
    }

    /**
     * 从战斗结果成员队列中，获取对应成员的战斗信息结构体
     * 用于根据成员ID在结果列表中查找特定成员的战斗结果
     * @param _memberList 战斗结果成员列表
     * @param _memberId 要查询的成员ID
     * @return 对应的成员战斗结果对象，如果不存在则返回null
     */
    public ServerObj_MarsBattleV2_MemberFightResult getMemberInfo(ArrayList<ServerObj_MarsBattleV2_MemberFightResult> _memberList, long _memberId)
    {
        for(ServerObj_MarsBattleV2_MemberFightResult _result : _memberList)
        {
            if(_result.getMemberId() == _memberId)
                return _result;
        }

        return null;
    }


    /****************************
     * ============== 战斗计算V2版结束 ==============
     */
}
