package MGClient.Model;

import MGClient.ClientPlayer.ClientPlayer;
import NPCommon.Log.CommLog;
import NPEnum.ENPPlayerParam;

import javax.swing.event.TableModelListener;
import javax.swing.table.TableModel;
import java.util.ArrayList;

public class OnlinePlayerTableModel implements TableModel
{

    public OnlinePlayerTableModel()

    {
    }

    public static enum ETableColumns
    {
        COL_NAME,
        COL_CID,
        COL_LVL,

    }

    public interface _ICellFactory
    {
        public _ACellData createCellData();
    }

    public static class ColumnDesc
    {
        public ETableColumns eColumn;
        public Class<?> clazz;
        public String columnName;
        public ENPPlayerParam eRelateParam;
        public _ICellFactory cellFactory;
    }


    public interface _ACellData
    {
        public abstract Object getData();

        public abstract void setData(Object aValue);

    }

    public static class StringCellData implements _ACellData
    {
        private String _value = "";

        @Override
        public Object getData()
        {
            return _value;
        }

        @Override
        public void setData(Object aValue)
        {
            _value = (String) aValue;
        }


        private static _ICellFactory _factory = null;

        public static _ICellFactory getCellFactory()
        {
            if (null == _factory)
            {
                _factory = new _ICellFactory()
                {

                    @Override
                    public _ACellData createCellData()
                    {
                        return new StringCellData();
                    }

                };
            }
            return _factory;
        }

    }

    public static class LongCellData implements _ACellData
    {
        long _value;

        @Override
        public Object getData()
        {
            return _value;
        }

        @Override
        public void setData(Object aValue)
        {
            _value = (long) aValue;
        }

        private static _ICellFactory _factory = null;

        public static _ICellFactory getCellFactory()
        {
            if (null == _factory)
            {
                _factory = new _ICellFactory()
                {

                    @Override
                    public _ACellData createCellData()
                    {
                        return new LongCellData();
                    }

                };
            }
            return _factory;
        }

    }

    public static class FloatCellData implements _ACellData
    {
        float _value;

        @Override
        public Object getData()
        {
            return _value;
        }

        @Override
        public void setData(Object aValue)
        {
            _value = (float) aValue;
        }

        private static _ICellFactory _factory = null;

        public static _ICellFactory getCellFactory()
        {
            if (null == _factory)
            {
                _factory = new _ICellFactory()
                {

                    @Override
                    public _ACellData createCellData()
                    {
                        return new FloatCellData();
                    }

                };
            }
            return _factory;
        }

    }

    public static class PlayerTableRowData
    {
        private _ACellData[] cells = new _ACellData[_g_columnCount];

        public PlayerTableRowData()
        {
            for (int i = 0; i < _g_columnCount; i++)
            {
                cells[i] = _g_column_descs[i].cellFactory.createCellData();
            }
        }

        public Object getData(ETableColumns eColumn)
        {
            return cells[eColumn.ordinal()].getData();
        }

        public void setData(ETableColumns eColumn, Object aValue)
        {
            cells[eColumn.ordinal()].setData(aValue);
        }

        public void initRow(ClientPlayer _player)
        {
            setData(ETableColumns.COL_NAME, _player.getName());
            setData(ETableColumns.COL_CID, _player.getCid());
            initParams(_player);
        }

        public void initParams(ClientPlayer _player)
        {
            for (ColumnDesc desc : _g_column_descs)
            {
                if (desc.eRelateParam != null)
                {
                    setData(desc.eColumn, _player.getParam(desc.eRelateParam));
                }
            }
            onParamUpdate();
        }

        public void updateParam(ENPPlayerParam eParam, long value)
        {
            for (ColumnDesc desc : _g_column_descs)
            {
                if (desc.eRelateParam == eParam)
                {
                    setData(desc.eColumn, value);
                }
            }
            onParamUpdate();
        }

        private void onParamUpdate()
        {
            //float M2Score =0.f;
            //setData(ETableColumns.COL_M2_SCORE,M2Score);
        }
    }

    private static class _ColumnInit
    {
        public _ColumnInit()
        {
            initColumn(ETableColumns.COL_NAME, String.class, StringCellData.getCellFactory(), "登录名", null);
//			initColumn(ETableColumns.COL_STATE,EWCGUserState.class,EWCGUserStateCellData.getCellFactory(),"状态",null);
            initColumn(ETableColumns.COL_CID, Long.class, LongCellData.getCellFactory(), "CID", null);
            initColumn(ETableColumns.COL_LVL, Long.class, LongCellData.getCellFactory(), "等级", ENPPlayerParam.LEVEL);
//			initColumn(ETableColumns.COL_STAR,Long.class,LongCellData.getCellFactory(),"星耀",EPlayerParam.STARHONER);
//			initColumn(ETableColumns.COL_LEGENDSCORE,Long.class,LongCellData.getCellFactory(),"传说积分",EPlayerParam.LEGENDSCORE);
//			initColumn(ETableColumns.COL_TOTALWIN,Long.class,LongCellData.getCellFactory(),"胜利次数",EPlayerParam.TOTALVICTORY);
//			initColumn(ETableColumns.COL_TOTALLOSE,Long.class,LongCellData.getCellFactory(),"失败次数",EPlayerParam.TOTALFAIL);
//			initColumn(ETableColumns.COL_TOTALDRAW,Long.class,LongCellData.getCellFactory(),"平局次数",EPlayerParam.TOTALDOGFALL);
//			initColumn(ETableColumns.COL_SERAILWIN,Long.class,LongCellData.getCellFactory(),"连胜",EPlayerParam.SERIALVICTORY);
//			initColumn(ETableColumns.COL_SERIALLOSE,Long.class,LongCellData.getCellFactory(),"连败",EPlayerParam.SERIALFAIL);
//			initColumn(ETableColumns.COL_WINRATE,Float.class,FloatCellData.getCellFactory(),"胜率",null);
//			initColumn(ETableColumns.COL_GRADE_SCROE,Long.class,LongCellData.getCellFactory(),"段位积分",null);
//			initColumn(ETableColumns.COL_M2_SCORE,Float.class,FloatCellData.getCellFactory(),"M2积分",null);
//			initColumn(ETableColumns.COL_MATCH_TIME,Float.class,FloatCellData.getCellFactory(),"匹配耗时(秒)",null);
        }

        public void initColumn(ETableColumns eColumns, Class<?> clazz, _ICellFactory _facotry, String name, ENPPlayerParam param)
        {
            ColumnDesc desc = new ColumnDesc();
            desc.eColumn = eColumns;
            desc.clazz = clazz;
            desc.columnName = name;
            desc.eRelateParam = param;
            desc.cellFactory = _facotry;
            _g_column_descs[eColumns.ordinal()] = desc;
        }
    }

    public static final int _g_columnCount = ETableColumns.values().length;
    public static final ColumnDesc[] _g_column_descs = new ColumnDesc[_g_columnCount];
    @SuppressWarnings("unused")
    private static final _ColumnInit _g_init = new _ColumnInit();

    private ArrayList<PlayerTableRowData> _m_lRows = new ArrayList<>();

    @Override
    public void addTableModelListener(TableModelListener l)
    {

    }

    @Override
    public Class<?> getColumnClass(int columnIndex)
    {
        return _g_column_descs[columnIndex].clazz;
    }

    @Override
    public int getColumnCount()
    {
        return _g_columnCount;
    }

    @Override
    public String getColumnName(int columnIndex)
    {
        return _g_column_descs[columnIndex].columnName;
    }

    @Override
    public synchronized int getRowCount()
    {
        return _m_lRows.size();
    }

    @Override
    public synchronized Object getValueAt(int rowIndex, int columnIndex)
    {
        PlayerTableRowData row = _m_lRows.get(rowIndex);
        ColumnDesc desc = _g_column_descs[columnIndex];
        return row.getData(desc.eColumn);

    }

    @Override
    public boolean isCellEditable(int rowIndex, int columnIndex)
    {
        return false;
    }

    @Override
    public void removeTableModelListener(TableModelListener l)
    {

    }

    @Override
    public synchronized void setValueAt(Object aValue, int rowIndex, int columnIndex)
    {
        PlayerTableRowData row = _m_lRows.get(rowIndex);
        ColumnDesc desc = _g_column_descs[columnIndex];
        row.setData(desc.eColumn, aValue);

    }

    public synchronized void addRow(PlayerTableRowData row)
    {
        _m_lRows.add(row);

    }

    public synchronized PlayerTableRowData lookupRowByUid(long uid)
    {
        for (PlayerTableRowData rowData : _m_lRows)
        {
            if ((long) rowData.getData(ETableColumns.COL_CID) == uid)
            {
                return rowData;
            }
        }
        return null;
    }

    public synchronized void removeRowByUid(long uid)
    {

        for (PlayerTableRowData rowData : _m_lRows)
        {
            if ((long) rowData.getData(ETableColumns.COL_CID) == uid)
            {
                _m_lRows.remove(rowData);
                return;
            }
        }
        CommLog.error("removeRowByUid can not find cid:{}", uid);

    }

    public synchronized void clear()
    {
        _m_lRows.clear();
    }

}
