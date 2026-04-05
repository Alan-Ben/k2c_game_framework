package NPGameRes.Refs.TreasureHunt;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;

import java.util.ArrayList;

/**
 * 寻宝矿石质量奖励档位列表解析类
 * 格式: 200;250|currency-1:100;300|currency-1:200;350|currency-1:300;400
 * 表示: 200-250无奖励;250-300的奖励是currency-1:100;300-350的奖励是currency-1:200;350-400的奖励是currency-1:300
 */
public class TreasureHuntMassGradeList implements _IParseFromStringable
{
    /**
     * 根据质量值获取对应的奖励档位索引
     * @param _weight
     * @return
     */
    public int getGradeIndexByWeight(int _weight)
    {
        for (int i = 0; i < _m_rewardGradeList.size(); i++)
        {
            MassGradeItem item = _m_rewardGradeList.get(i);
            if (item.isInRange(_weight))
            {
                return i;
            }
        }
        return -1; // 如果没有找到对应的档位，返回-1
    }

    /**
     * 根据索引获取对应的奖励档位
     * @param index
     * @return
     */
    public MassGradeItem getGradeByIndex(int index)
    {
        if (index < 0 || index >= _m_rewardGradeList.size())
        {
            return null; // 索引越界
        }
        return _m_rewardGradeList.get(index);
    }

    /**
     * 奖励档位项
     */
    public static class MassGradeItem
    {
        public int _m_minMass;        // 最小质量值（包含）
        public int _m_maxMass;        // 最大质量值（不包含）
        public NPCommonCostItem _m_reward; // 奖励物品，如果为null表示无奖励
        
        public MassGradeItem(int minMass, int maxMass, NPCommonCostItem reward)
        {
            _m_minMass = minMass;
            _m_maxMass = maxMass;
            _m_reward = reward;
        }
        
        /**
         * 检查指定质量值是否在此档位范围内
         * @param mass 质量值
         * @return 是否在范围内
         */
        public boolean isInRange(int mass)
        {
            return mass >= _m_minMass && mass < _m_maxMass;
        }

        /**
         * 随机生成一个在此档位范围内的质量值
         * @return
         */
        public int randomMass()
        {
            return (int) (Math.random() * (_m_maxMass - _m_minMass)) + _m_minMass;
        }

        @Override
        public String toString()
        {
            StringBuilder sb = new StringBuilder();
            sb.append(_m_minMass).append("-").append(_m_maxMass);
            if (_m_reward != null)
            {
                sb.append("|").append(_m_reward);
            }
            return sb.toString();
        }
    }
    
    private ArrayList<MassGradeItem> _m_rewardGradeList;
    
    public TreasureHuntMassGradeList()
    {
        _m_rewardGradeList = new ArrayList<>();
    }
    
    /**
     * 获取奖励档位列表
     * @return 奖励档位列表
     */
    public ArrayList<MassGradeItem> getRewardGradeList()
    {
        return _m_rewardGradeList;
    }
    
    /**
     * 根据质量值获取对应的奖励
     * @param mass 质量值
     * @return 奖励物品，如果没有对应档位或档位无奖励则返回null
     */
    public NPCommonCostItem getRewardByMass(int mass)
    {
        for (MassGradeItem item : _m_rewardGradeList)
        {
            if (item.isInRange(mass))
            {
                return item._m_reward;
            }
        }
        return null;
    }
    
    @Override
    public boolean parseFromString(String sValue)
    {
        _m_rewardGradeList.clear();
        
        if (null == sValue || sValue.trim().isEmpty())
            return true;
            
        try
        {
            // 按分号分割各个档位
            String[] gradeSegments = sValue.split(";");
            
            // 解析所有分割点和奖励信息
            int[] massValues = new int[gradeSegments.length];
            NPCommonCostItem[] rewards = new NPCommonCostItem[gradeSegments.length];
            
            for (int i = 0; i < gradeSegments.length; i++)
            {
                String segment = gradeSegments[i];
                
                if (segment.contains("|"))
                {
                    String[] parts = segment.split("\\|", 2);
                    massValues[i] = Integer.parseInt(parts[0]);
                    
                    // 解析奖励物品，这个奖励属于从这个质量值开始的区间
                    String rewardStr = parts[1];
                    if (!rewardStr.isEmpty())
                    {
                        rewards[i] = new NPCommonCostItem();
                        if (!rewards[i].parseFromString(rewardStr))
                        {
                            CommLog.error("TreasureHuntMassGradeList Failed to parse reward item: " + rewardStr);
                            return false;
                        }
                    }
                }
                else
                {
                    massValues[i] = Integer.parseInt(segment);
                    rewards[i] = null; // 没有奖励信息
                }
            }
            
            // 创建区间：每个区间的奖励取自该区间起始点的奖励
            for (int i = 0; i < massValues.length - 1; i++)
            {
                int minMass = massValues[i];
                int maxMass = massValues[i + 1];
                
                if (minMass > maxMass)
                {
                    CommLog.error("TreasureHuntMassGradeList Invalid mass range: " + minMass + " > " + maxMass);
                    return false;
                }
                
                // 每个区间的奖励取自该区间起始点（左边界）的奖励
                NPCommonCostItem reward = rewards[i];
                
                _m_rewardGradeList.add(new MassGradeItem(minMass, maxMass, reward));
            }
            
            return true;
        }
        catch (Exception e)
        {
            _m_rewardGradeList.clear();
            CommLog.error("TreasureHuntMassGradeList Exception in parseFromString, e:{} ", e);
            return false;
        }
    }

    /**
     * 测试用例
     * @return
     */
    public static void main(String[] args)
    {
        System.out.println("=== 测试用例1: 第一个区间无奖励 ===");
        testCase("200;250|currency-1:100;300|currency-1:200;350|currency-1:300;400",
                "200-250无奖励; 250-300的奖励是currency-1:100; 300-350的奖励是currency-1:200; 350-400的奖励是currency-1:300");
        
        System.out.println("\n=== 测试用例2: 第一个区间有奖励 ===");
        testCase("200|currency-1:50;250|currency-1:100;300|currency-1:200;350|currency-1:300;400",
                "200-250的奖励是currency-1:50; 250-300的奖励是currency-1:100; 300-350的奖励是currency-1:200; 350-400的奖励是currency-1:300");
    }
    
    private static void testCase(String testStr, String expected)
    {
        TreasureHuntMassGradeList list = new TreasureHuntMassGradeList();
        
        System.out.println("Input: " + testStr);
        System.out.println("Expected: " + expected);
        System.out.println();
        
        if (list.parseFromString(testStr))
        {
            System.out.println("Parse successful!");
            System.out.println("Parsed grades:");
            
            for (int i = 0; i < list.getRewardGradeList().size(); i++)
            {
                MassGradeItem item = list.getRewardGradeList().get(i);
                String rewardDesc = (item._m_reward == null) ? "无奖励" : item._m_reward.toString();
                System.out.println("  Grade " + i + ": " + item._m_minMass + "-" + item._m_maxMass + " 奖励: " + rewardDesc);
            }
            
            System.out.println();
            System.out.println("Test queries:");
            System.out.println("  质量225的奖励: " + (list.getRewardByMass(225) == null ? "无奖励" : list.getRewardByMass(225)));
            System.out.println("  质量275的奖励: " + (list.getRewardByMass(275) == null ? "无奖励" : list.getRewardByMass(275)));
            System.out.println("  质量325的奖励: " + (list.getRewardByMass(325) == null ? "无奖励" : list.getRewardByMass(325)));
            System.out.println("  质量375的奖励: " + (list.getRewardByMass(375) == null ? "无奖励" : list.getRewardByMass(375)));
        }
        else
        {
            System.out.println("Parse failed!");
        }
    }
    
    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        for (MassGradeItem item : _m_rewardGradeList)
        {
            sb.append(item.toString()).append(";");
        }
        return sb.toString();
    }
}
