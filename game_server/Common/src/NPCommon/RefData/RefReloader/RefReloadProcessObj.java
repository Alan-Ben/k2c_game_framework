package NPCommon.RefData.RefReloader;

import NPCommon.Log.CommLog;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.Util.CommFile;

import java.util.ArrayList;
import java.util.Map;

/**
 * @description: 配表加载时帮助处理的中间对象
 * @author: ricci
 * @date: 2023-03-13 16:02:22
 */
class RefReloadProcessObj
{
    /**
     * 对应的配表对象
     */
    private RefContainerBase<?> _m_refBase;
    /**
     * 从磁盘读取的配表全部内容
     */
    public String _m_diskText;
    /**
     * 一张表的txt内容读取按固定分隔符，划分出的临时表结构
     * <k-v>包括主键列
     */
    public ArrayList<RefBase> _m_tableLinesParseObjList;

    public RefReloadProcessObj(RefContainerBase<?> _refBase, String _txt)
    {
        _m_refBase = _refBase;
        _m_diskText = _txt;
        _m_tableLinesParseObjList = new ArrayList<>();
        __init();
    }

    public String getDiskText()
    {
        return _m_diskText;
    }

    public void setRefBase(RefContainerBase<?> _refBase)
    {
        _m_refBase = _refBase;
    }

    public RefContainerBase<?> getRefBase()
    {
        return _m_refBase;
    }

    public ArrayList<RefBase> getTableLinesParseObjList()
    {
        return _m_tableLinesParseObjList;
    }

    /**
     * 已经初始化txt数据和refBase数据后
     * 转换成Ref对象的过程
     */
    private void __init()
    {
        //每一行数据是一个map，整表数据是一个list
        ArrayList<Map<String, String>> tableKeyValMapList =
                CommFile.GetTableFromTxt(_m_refBase.getTableName(), _m_diskText, 0, 1);
        //遍历整表
        for (Map<String, String> keyValMap : tableKeyValMapList)
        {
            RefBase refIns;
            //把这一行转换成refBase实例
            try
            {
                refIns = _m_refBase.getRefClass().newInstance();
            } catch (Exception e)
            {
                CommLog.error("[RefReload] RefReloadProcessObj can not createNewInstance...");
                continue;
            }
            //设置新实例的所有值
            refIns.internalSetValue(keyValMap);
            //将新实例放入管理等待执行
            _m_tableLinesParseObjList.add(refIns);
        }
    }
}
