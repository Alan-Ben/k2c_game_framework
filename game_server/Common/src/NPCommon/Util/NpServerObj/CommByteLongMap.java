package NPCommon.Util.NpServerObj;

import Common.NpServerObj.NpServerObj_CommonLongMap;
import Common.NpServerObj.NpServerObj_CommonLongPair;
import NPCommon.Util.CommonFunc;

import java.nio.ByteBuffer;
import java.util.HashSet;
import java.util.Set;
import java.util.function.Consumer;
import java.util.function.Supplier;

/**
 * @description: NP 通用 long Map,用来往数据库存bytebuffer和内存map做转换
 * @author: ricci
 * @date: 2022-05-12 10:35:07
 */
public class CommByteLongMap extends _ACommArrayToMap<NpServerObj_CommonLongPair, Long, Long>
{
    /**
     * 获得源数据 bytes方法
     */
    protected final Supplier<byte[]> _m_getOriMapBytes;

    /**
     * 保存源数据 bytes 方法
     */
    protected final Consumer<byte[]> _m_saveOriMapBytes;

    /**
     * 源结构体 map
     */
    private final NpServerObj_CommonLongMap _m_oriStructureMap;

    /**
     * 数据是否有变更
     */
    private boolean _m_bHasChange;

    /**
     * 需要提供 ByteBuffer 数据源的获取和保存方法
     * @param _getOriMapBytes  获取方法
     * @param _saveOriMapBytes 保存方法
     */
    public CommByteLongMap(Supplier<byte[]> _getOriMapBytes, Consumer<byte[]> _saveOriMapBytes)
    {
        _m_getOriMapBytes = _getOriMapBytes;
        _m_saveOriMapBytes = _saveOriMapBytes;
        _m_oriStructureMap = new NpServerObj_CommonLongMap();
        _m_oriStructureMap.readPackage(ByteBuffer.wrap(_getOriMapBytes.get()));
    }

    //region getter and setter

    /**
     * 获取源数据
     * @return 数据库数据来源
     */
    public byte[] getOriByteBuffer()
    {
        return _m_getOriMapBytes.get();
    }

    /**
     * 获取内存中数据结构
     * @return NpServerObj_CommonLongMap
     */
    public NpServerObj_CommonLongMap getOriStructureMap()
    {
        return _m_oriStructureMap;
    }

    public Consumer<byte[]> getSaveOriMapBytes()
    {
        return _m_saveOriMapBytes;
    }

    //endregion


    public void clear()
    {
        _m_oriStructureMap.getLongMap().clear();
        _m_bHasChange = true;
    }

    @Override
    public Set<NpServerObj_CommonLongPair> getEntrySet()
    {
        return new HashSet<>(_m_oriStructureMap.getLongMap());
    }

    @Override
    protected Long getKey(NpServerObj_CommonLongPair _entry)
    {
        return _entry.getKey();
    }

    @Override
    protected Long getValue(NpServerObj_CommonLongPair _entry)
    {
        return _entry.getValue();
    }

    @Override
    protected void setValue(NpServerObj_CommonLongPair _entry, Long _value)
    {
        _entry.setValue(_value);
        _m_bHasChange = true;
    }

    @Override
    protected NpServerObj_CommonLongPair createNewEntry(Long _key, Long _value)
    {
        return new NpServerObj_CommonLongPair(_key, _value);
    }

    @Override
    protected void putInMap(NpServerObj_CommonLongPair _entry)
    {
        _m_oriStructureMap.addLongMap(_entry);
        _m_bHasChange = true;
    }

    @Override
    public void saveAllMark()
    {
        if (!_m_bHasChange)
        {
            return;
        }
        _m_bHasChange = false;
        getSaveOriMapBytes().accept(CommonFunc.ByteBfferToBytes(_m_oriStructureMap.makePackage()));
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        for (NpServerObj_CommonLongPair pair : getEntrySet())
        {
            if (pair == null)
            {
                continue;
            }
            sb.append("\n");
            sb.append("first: ");
            sb.append(pair.getKey());
            sb.append("second: ");
            sb.append(pair.getValue());
            sb.append("\n");
        }
        return "CommByteLongMap{" +
                ", _m_oriStructureMap=" + sb +
                ", _m_bHasChange=" + _m_bHasChange +
                '}';
    }
}
