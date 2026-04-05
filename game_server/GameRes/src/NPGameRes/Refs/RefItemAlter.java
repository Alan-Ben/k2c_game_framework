package NPGameRes.Refs;

import NPCommon.Enum.NPCommonEnum.ENPInsteadItemType;
import NPCommon.Log.CommLog;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;
import NPEnum.ENPItemType;

import java.util.ArrayList;
import java.util.HashMap;

@RefTable(tableName = "item_alter")
public class RefItemAlter extends RefBase
{
    private static RefItemAlterMgr _g_mgr = new RefItemAlterMgr();

    public static RefItemAlterMgr getMgr()
    {
        return _g_mgr;
    }

    /**********
     * 管理某一物品替换信息，根据存放的表示替换表还是被替换表决定替换的信息如何
     * @author mj
     */
    private static class ItemAlterInfo
    {
        //根据替换类型对应的队列
        private ArrayList<RefItemAlter> _m_lAlterList;

        public ItemAlterInfo()
        {
            _m_lAlterList = new ArrayList<RefItemAlter>();
        }

        /***********
         *把替换物品添加的替换列表中，允许类型为NONE的替换物品加入
         * @param ref
         */
        public void addAlterRef(RefItemAlter _ref)
        {
            //检测是否已有数据
            if (!checkAlterItemExists(_ref))
            {
                _m_lAlterList.add(_ref);
            } else
            {
                CommLog.error("duplicate item alter for src_item:{}-{},alter_item:{}-{}"
                        , _ref.src_item_type, _ref.src_item_id, _ref.alter_item_type, _ref.alter_item_id, new Exception());
            }
        }

        /************
         * 检测替换物品是否已经在列表中
         * @param alterList
         * @param alter_item_type
         * @param alter_item_id
         * @return
         */
        private boolean checkAlterItemExists(RefItemAlter _ref)
        {
            if (null == _ref)
                return false;

            for (int i = 0; i < _m_lAlterList.size(); i++)
            {
                if (_m_lAlterList.get(i).alter_item_type == _ref.alter_item_type && _m_lAlterList.get(i).alter_item_id == _ref.alter_item_id
                        && _m_lAlterList.get(i).src_item_type == _ref.src_item_type && _m_lAlterList.get(i).src_item_id == _ref.src_item_id)
                    return true;
            }
            return false;
        }

        /**********
         * 根据替换类型查询可替换的队列
         * @param _itemType
         * @param _itemId
         * @return
         */
        public void getAlterList(ENPInsteadItemType _alterType, ArrayList<RefItemAlter> _recList)
        {
            if (null == _recList)
                return;

            //比对合适的队列
            for (int i = 0; i < _m_lAlterList.size(); i++)
            {
                if (_m_lAlterList.get(i).type == _alterType)
                    _recList.add(_m_lAlterList.get(i));
            }
        }
    }

    public static class RefItemAlterMgr extends RefListContainer<RefItemAlter>
    {
        //根据被替换物品类型ID区分的表
        private HashMap<Long, ItemAlterInfo> _m_hmAlterEDMap;
        //根据可替换物品类型ID区分的表
        private HashMap<Long, ItemAlterInfo> _m_hmAlterItemMap;

        public RefItemAlterMgr()
        {
            _m_hmAlterEDMap = new HashMap<>();
            _m_hmAlterItemMap = new HashMap<>();
        }

        @Override
        public void onLoaded()
        {
            RefItemAlter refObj = null;
            for (int i = 0; i < getList().size(); i++)
            {
                refObj = getList().get(i);
                if (null == refObj)
                    continue;

                //逐个放入对应数据集合中
                long tmpId = refObj.alterEDId();
                ItemAlterInfo alterInfo = _m_hmAlterEDMap.get(refObj.alterEDId());
                if (null == alterInfo)
                {
                    alterInfo = new ItemAlterInfo();
                    _m_hmAlterEDMap.put(tmpId, alterInfo);
                }
                alterInfo.addAlterRef(refObj);

                tmpId = refObj.alterEDId();
                alterInfo = _m_hmAlterItemMap.get(refObj.alterEDId());
                if (null == alterInfo)
                {
                    alterInfo = new ItemAlterInfo();
                    _m_hmAlterItemMap.put(tmpId, alterInfo);
                }
                alterInfo.addAlterRef(refObj);
            }
        }

        /**********
         * 根据替换类型，物品类型，物品id，获取物品的替换列表。提供外部调用。
         * @param _alterType
         * @param _itemType
         * @param _itemId
         * @return
         */
        public ArrayList<RefItemAlter> lookupItemAlterEDList(ENPInsteadItemType _alterType, ENPItemType _itemType, long _itemId)
        {
            ArrayList<RefItemAlter> list = new ArrayList<RefItemAlter>();

            //查询对象可替换数据集合
            long id = _alterEDId(_itemType, _itemId);
            //获取对应数据
            ItemAlterInfo alterInfo = _m_hmAlterEDMap.get(id);
            if (null == alterInfo)
                return list;

            //获取对应的替换队列
            alterInfo.getAlterList(_alterType, list);
            return list;
        }

        protected long _alterEDId(ENPItemType _itemType, long _itemId)
        {
            return _itemId * 100 + _itemType.ordinal();
        }

        protected long _alterItemId(ENPItemType _itemType, long _itemId)
        {
            return _itemId * 100 + _itemType.ordinal();
        }
    }

    //////////////////////////////
    @Override
    public RefItemAlterMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefItemAlterMgr) _mgr;
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefItemAlter newRef = (RefItemAlter) _newRef;
        type = newRef.type;
        src_item_type = newRef.src_item_type;
        src_item_id = newRef.src_item_id;
        alter_item_type = newRef.alter_item_type;
        alter_item_id = newRef.alter_item_id;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return src_item_id * 10000 + (src_item_type.ordinal() * 100) + type.ordinal();
    }

    public long alterEDId()
    {
        return src_item_id * 100 + src_item_type.ordinal();
    }

    public long alterItemId()
    {
        return alter_item_id * 100 + alter_item_type.ordinal();
    }

    //////////////////////////////
    public ENPInsteadItemType type; //替换功能枚举
    public ENPItemType src_item_type;//源物品类型
    public long src_item_id;//源物品ID
    public ENPItemType alter_item_type;//代替物品类型
    public long alter_item_id;//代替物品ID

}
