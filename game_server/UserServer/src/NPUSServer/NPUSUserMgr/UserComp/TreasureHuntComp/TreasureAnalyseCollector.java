package NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp;

import NPEnum.EQuality;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntStationLevel;

import java.util.ArrayList;
import java.util.List;

/**
 * 太空寻宝分析数据收集器
 * <p>
 * 主要功能：
 * 1. 收集和存储太空寻宝过程中的分析数据
 * 2. 按类型和品质分类查询分析数据
 * 3. 提供数据统计和清理功能
 * <p>
 * 使用场景：
 * - 太空寻宝奇物分析结果收集
 * - 分析数据统计和展示
 * - 临时数据缓存和处理
 */
public class TreasureAnalyseCollector
{
    private int wantNum; // 期望数量
    private RefTreasureHuntStationLevel wantLevelRef; // 期望矿石数量

    // 分析数据列表
    private int num;
    private List<TreasureAnalyseData> analyseDataList;

    public TreasureAnalyseCollector(RefTreasureHuntStationLevel _wantLevelRef, int _wantNum)
    {
        wantLevelRef = _wantLevelRef;
        wantNum = _wantNum;
        num = 0;
        analyseDataList = new ArrayList<>();
    }

    public int getWantNum()
    {
        return wantNum;
    }

    public RefTreasureHuntStationLevel getWantLevelRef()
    {
        return wantLevelRef;
    }

    public void addAnalyseData(ETreasureHuntDataAnalyseType type, EQuality quality, long id)
    {
        analyseDataList.add(new TreasureAnalyseData(type, quality, id));
    }

    public void addNum(int num)
    {
        this.num += num;
    }

    @Override
    public String toString()
    {
        int totalCount = num;

        // 统计各类型数量
        int oreCount = 0;
        int treasureCount = 0;
        int treasureRepeatCount = 0;
        int itemCount = 0;

        // 统计各品质数量
        int[] oreQualityCount = new int[EQuality.EQuality_Length]; // GREEN, BLUE, PURPLE, ORANGE, RED
        int[] treasureQualityCount = new int[EQuality.EQuality_Length];
        int[] treasureRepeatQualityCount = new int[EQuality.EQuality_Length];

        for (TreasureAnalyseData data : analyseDataList)
        {
            switch (data.getType())
            {
                case ORE:
                    oreCount++;
                    if (data.getQuality() != null)
                    {
                        oreQualityCount[data.getQuality().ordinal()]++;
                    }
                    break;
                case TREASURE:
                    treasureCount++;
                    if (data.getQuality() != null)
                    {
                        treasureQualityCount[data.getQuality().ordinal()]++;
                    }
                    break;
                case TREASURE_REPEAT:
                    treasureRepeatCount++;
                    if (data.getQuality() != null)
                    {
                        treasureRepeatQualityCount[data.getQuality().ordinal()]++;
                    }
                    break;
                case REWARD:
                    itemCount++;
                    break;
            }
        }

        StringBuilder sb = new StringBuilder();
        sb.append("总寻宝次数:").append(totalCount).append("\n");

        // 矿石统计
        sb.append("  矿石数量").append(oreCount).append(", ");
        for (int i = 0; i < oreQualityCount.length; i++)
        {
            int count = oreQualityCount[i];
            if (count > 0)
            {
                sb.append(EQuality.values()[i].name()).append(":").append(count).append(";");
            }
        }
        sb.append("\n");

        // 奇物统计
        sb.append("  奇物数量").append(treasureCount).append(",");
        for (int i = 0; i < treasureCount; i++)
        {
            int count = treasureQualityCount[i];
            if (count > 0)
            {
                sb.append(EQuality.values()[i].name()).append(":").append(count).append(";");
            }
        }
        sb.append("\n");

        // 重复奇物统计
        sb.append("  重复奇物数量").append(treasureRepeatCount).append(",");
        for (int i = 0; i < treasureRepeatQualityCount.length; i++)
        {
            int count = treasureRepeatQualityCount[i];
            if (count > 0)
            {
                sb.append(EQuality.values()[i].name()).append(":").append(count).append(";");
            }
        }
        sb.append("\n");

        // 道具统计
        sb.append("  道具数量").append(itemCount).append("\n");

        // 具体随机结果
        sb.append("具体随机结果:\n");
        for (TreasureAnalyseData data : analyseDataList)
        {
            String typeName = "";
            switch (data.getType())
            {
                case ORE:
                    typeName = "矿石";
                    break;
                case TREASURE:
                    typeName = "奇物";
                    break;
                case REWARD:
                    typeName = "道具";
                    break;
                case TREASURE_REPEAT:
                    typeName = "重复奇物";
                    break;
            }

            sb.append(typeName);
            sb.append(",").append(data.getQuality().name());
            sb.append(",").append(data.getId()).append("\n");
        }

        return sb.toString();
    }


    public static class TreasureAnalyseData
    {
        private ETreasureHuntDataAnalyseType type;
        private EQuality quality;
        private long id;

        public TreasureAnalyseData(ETreasureHuntDataAnalyseType type, EQuality quality, long id)
        {
            this.type = type;
            this.quality = quality;
            this.id = id;
        }

        public ETreasureHuntDataAnalyseType getType()
        {
            return type;
        }

        public EQuality getQuality()
        {
            return quality;
        }

        public long getId()
        {
            return id;
        }
    }

    public enum ETreasureHuntDataAnalyseType
    {
        ORE, // 矿石
        TREASURE, // 奇物
        REWARD, // 道具
        TREASURE_REPEAT // 奇物重复
    }
}
