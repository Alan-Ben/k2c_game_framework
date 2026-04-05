package NPUSServer.NPUSUserMgr.UserComp.ArenaComp.Battle;

import Common.ArenaObj.Arena_SingleBuffInfo;

import java.util.List;

/**
 * 竞技场战斗处理器
 * 统一处理竞技场战斗相关的逻辑，避免重复代码
 */
public class ArenaBattleProcessor
{

    /**
     * 执行战斗计算
     * @param attackerPower 攻击者战力
     * @param defenderPower 防御者战力
     * @return 战斗结果，包含剩余血量和是否胜利
     */
    public static BattleResult processBattle(long attackerPower, long defenderPower)
    {
        long attackerHp = attackerPower;
        long defenderHp = defenderPower;
        long deductHp = 0;

        // 战斗计算
        while (attackerHp > 0 && defenderHp > 0)
        {
            long attackerDamage = attackerHp;
            long defenderDamage = defenderHp;

            attackerHp -= defenderDamage;
            deductHp += defenderDamage;
            defenderHp -= attackerDamage;
        }

        return new BattleResult(attackerHp, defenderHp, deductHp, defenderHp <= 0);
    }

    /**
     * 计算Buff加成
     * @param buffList  Buff列表
     * @param basePower 基础战力
     * @return 加成后的战力
     */
    public static long calculateBuffedPower(List<Arena_SingleBuffInfo> buffList, long basePower)
    {
        return (long) Math.ceil((basePower * (10000d + BuffProcessor.getBuffAddPercentage(buffList))) / 10000);
    }

    /**
     * 战斗结果类
     */
    public static class BattleResult
    {
        private final long attackerRemainingHp;
        private final long defenderRemainingHp;
        private final long deductedHp;
        private final boolean victory;

        public BattleResult(long attackerRemainingHp, long defenderRemainingHp, long deductedHp, boolean victory)
        {
            this.attackerRemainingHp = attackerRemainingHp;
            this.defenderRemainingHp = defenderRemainingHp;
            this.deductedHp = deductedHp;
            this.victory = victory;
        }

        public long getAttackerRemainingHp()
        {
            return attackerRemainingHp;
        }

        public long getDefenderRemainingHp()
        {
            return defenderRemainingHp;
        }

        public long getDeductedHp()
        {
            return deductedHp;
        }

        public boolean isVictory()
        {
            return victory;
        }
    }
}