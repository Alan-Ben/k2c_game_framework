package NPGameRes.GameObjs.Tower;

import NPGameRes.Refs.Tower.RefTowerChapter;
import NPGameRes.Refs.Tower.RefTowerChapterStage;

public class TowerStageRefObj
{
    //总索引
    private int _m_stageIndex;
    //总起始等级
    private int _m_totalStartLevel;
    //章节起始等级
    private int _m_chapterStartLevel;
    //章节配表
    private RefTowerChapter _m_refChapter;
    //关卡配表
    private RefTowerChapterStage _m_refChapterStage;

    public TowerStageRefObj(int _m_stageIndex, int _m_totalStartLevel, int _m_chapterStartLevel,
                            RefTowerChapter _m_refChapter, RefTowerChapterStage _m_refChapterStage)
    {
        this._m_stageIndex = _m_stageIndex;
        this._m_totalStartLevel = _m_totalStartLevel;
        this._m_chapterStartLevel = _m_chapterStartLevel;
        this._m_refChapter = _m_refChapter;
        this._m_refChapterStage = _m_refChapterStage;
    }

    public int getStageIndex()
    {
        return _m_stageIndex;
    }

    public int getTotalStartLevel()
    {
        return _m_totalStartLevel;
    }

    public int getChapterStartLevel()
    {
        return _m_chapterStartLevel;
    }

    public RefTowerChapter getRefChapter()
    {
        return _m_refChapter;
    }

    public RefTowerChapterStage getRefChapterStage()
    {
        return _m_refChapterStage;
    }

    public int getTotalEndLevel()
    {
        return _m_totalStartLevel + _m_refChapterStage.levels_in_range_count - 1;
    }

    /**
     * 每关的实力值=实力基础值*实力增长万分比^(当前关卡-关卡初始值)
     * @param _level
     * @return
     */
    public long calPower(int _level)
    {
        int relativeLevel = Math.max(0, _level - _m_chapterStartLevel);

        return (long) (getRefChapterStage().base_power_per_level * Math.pow((1 + 1.0f * getRefChapterStage().power_growth_ratio / 10000f), relativeLevel));
    }

    /**
     * 每日迷宫币产出提升: 每一关的产出值= 每日迷宫币基础值+增长值*(当前关卡-关卡初始值)
     * @param _level
     * @return
     */
    public long calDailyCoinReward(int _level)
    {
        int relativeLevel = Math.max(0, _level - _m_chapterStartLevel);

        return getRefChapterStage().daily_tower_coin_output_base + (long) getRefChapterStage().tower_coin_growth_value * relativeLevel;
    }

    /**
     * 计算通过关卡的迷宫币
     * 迷宫币奖励=到达当前关卡的每日迷宫币产出值*迷宫币奖励万分比系数
     * @param _level
     * @return
     */
    public long calPassedTowerCoin(int _level)
    {
        long towerCoin = calDailyCoinReward(_level);
        if (towerCoin <= 0)
            return 0;

        return (long) Math.ceil(1.0 * towerCoin * getRefChapterStage().tower_coin_reward_ratio_per_level / 10000f);
    }
}
