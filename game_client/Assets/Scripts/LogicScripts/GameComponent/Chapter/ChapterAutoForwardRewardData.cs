using System.Collections.Generic;
using NPCommon;

namespace GOE
{
    /// <summary>
    /// 关卡自动前进的奖励数据
    /// </summary>
    public class ChapterAutoForwardRewardData
    {
        private long _m_startChapterId; 
        private int _m_startPoint;
        
        private long _m_endChapterId;
        private int _m_endPoint;
            
        private long _m_costGoldNum;
        private long _m_rewardHeroExp;
        private long _m_rewardPlayerExp;
        private List<NPCommon.NPCommon_ItemInfo> _m_itemList;

        public long startChapterId
        {
            get { return _m_startChapterId; }
        }

        public int startPoint
        {
            get { return _m_startPoint; }
        }

        public long endChapterId
        {
            get { return _m_endChapterId; }
        }

        public int endPoint
        {
            get { return _m_endPoint; }
        }

        public long costGoldNum
        {
            get { return _m_costGoldNum; }
        }

        public long rewardHeroExp
        {
            get { return _m_rewardHeroExp; }
        }

        public long rewardPlayerExp
        {
            get { return _m_rewardPlayerExp; }
        }

        public ChapterAutoForwardRewardData(long _startChapterId, int _startPoint)
        {
            _m_startChapterId = _startChapterId;
            _m_startPoint = _startPoint;
        }
        
        public void setEndChapterInfo(long _endChapterId, int _endPoint)
        {
            _m_endChapterId = _endChapterId;
            _m_endPoint = _endPoint;
        }
        
        public void addCostGoldNum(long _num)
        {
            _m_costGoldNum += _num;
        }
        
        public void addRewardHeroExp(long _exp)
        {
            _m_rewardHeroExp += _exp;
        }

        public void addRewardPlayerExp(long _exp)
        {
            _m_rewardPlayerExp += _exp;
        }
        
        public void addRewardItem(List<NPCommon.NPCommon_ItemInfo> _itemList)
        {
            if(null == _itemList)
                return;
            
            if (_m_itemList == null)
            {
                _m_itemList = new List<NPCommon.NPCommon_ItemInfo>();
            }
            _m_itemList.AddRange(_itemList);
        }
        
        public List<NPCommonCostItem> getRewardItemList()
        {
            return GCommon.getCombineItemList(_m_itemList);;
        }
    }
}