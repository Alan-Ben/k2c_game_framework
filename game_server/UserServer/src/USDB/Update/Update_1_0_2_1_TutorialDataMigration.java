package USDB.Update;

import Common.TutorialData;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Collections;
import java.util.Map;

/**
 * Update_1_0_2_1_TutorialDataMigration - 引导数据旧版本迁移工具
 *
 * 主要功能：
 * 1. 将旧引导 ID 按 trans.txt 规则转换为新 ID
 * 2. 根据新的 lastFinishForceTutorial 重建 finishTutorialId 列表
 * 3. 保留原列表中 ID > 6400 的扩展引导
 */
public class Update_1_0_2_1_TutorialDataMigration
{
    // 按 list.txt 顺序排列的新版引导 ID 列表
    private static final List<Long> ORDERED_IDS = Arrays.asList(
        1000L, 1010L, 1020L,
        1100L, 1110L, 1200L, 1210L, 1300L, 1400L,
        2100L, 2110L, 2210L, 2300L, 2400L, 2410L, 2500L, 2600L, 2700L, 2800L, 2810L, 2900L,
        3000L, 3100L, 3110L, 3120L, 3130L, 3200L, 3300L, 3320L, 3400L, 3410L, 3420L, 3430L, 3440L, 3450L,
        4000L, 4200L, 4250L, 4300L, 4410L, 4420L, 4430L, 4900L,
        5010L, 5020L, 5030L, 5100L,
        6000L, 6010L, 6100L, 6110L, 6120L, 6130L, 6400L
    );

    // trans.txt 中需要变更的旧ID -> 新ID 映射
    private static final Map<Long, Long> TRANS_MAP = new HashMap<>();

    static
    {
        long[][] transEntries = {
            {1000L, 1010L},
            {2000L, 1400L}, {2010L, 1400L}, {2020L, 1400L},
            {2120L, 2110L},
            {2310L, 2600L},
            {2350L, 2700L}, {3000L, 2700L},
            {3100L, 2800L}, {3120L, 2800L},
            {3200L, 3300L},
            {3220L, 3320L},
            {3300L, 3200L}, {3310L, 3200L},
            {4100L, 4000L}, {4110L, 4000L}, {4120L, 4000L},
            {5100L, 6000L},
            {6101L, 6110L},
            {6102L, 6120L},
            {6110L, 6130L}
        };
        for (long[] entry : transEntries)
        {
            TRANS_MAP.put(entry[0], entry[1]);
        }
    }

    /**
     * 执行引导数据迁移
     *
     * @param _data 待迁移的 TutorialData
     * @return true=数据有变化已更新，false=无需迁移
     */
    public static boolean migrate(TutorialData _data)
    {
        long oldLastId = _data.getLastFinishForceTutorial();

        // 无引导进度，跳过
        if (oldLastId == 0) return false;

        // 转换 lastFinishForceTutorial
        long newLastId = TRANS_MAP.getOrDefault(oldLastId, oldLastId);

        // 收集原列表中 > 6400 的扩展引导（始终保留）
        List<Long> extraIds = new ArrayList<>();
        for (long id : _data.getFinishTutorialId())
        {
            if (id > 6400L)
            {
                extraIds.add(id);
            }
        }

        // 额外设置完成
        extraIds.add(10230L);
        extraIds.add(10650L);
        extraIds.add(12200L);

        // 构建新的 finishTutorialId
        int idx = ORDERED_IDS.indexOf(newLastId);
        List<Long> newList;
        if (idx >= 0)
        {
            // 找到位置：新版有序前缀 + 扩展ID
            newList = new ArrayList<>(ORDERED_IDS.subList(0, idx + 1));
            newList.addAll(extraIds);
        }
        else
        {
            // 找不到位置：仅保留扩展ID，清除无效的旧标准引导
            newList = new ArrayList<>(extraIds);
        }

        // 排序后比较，避免仅顺序不同导致无意义的更新
        List<Long> sortedNew = new ArrayList<>(newList);
        List<Long> sortedOld = new ArrayList<>(_data.getFinishTutorialId());
        Collections.sort(sortedNew);
        Collections.sort(sortedOld);

        boolean lastIdChanged = (newLastId != oldLastId);
        boolean listChanged = !sortedNew.equals(sortedOld);
        if (!lastIdChanged && !listChanged) return false;

        // 更新数据
        _data.setLastFinishForceTutorial(newLastId);
        _data.getFinishTutorialId().clear();
        _data.getFinishTutorialId().addAll(newList);

        return true;
    }
}
