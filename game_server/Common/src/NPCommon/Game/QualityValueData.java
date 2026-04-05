package NPCommon.Game;

import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPEnum.EQuality;

/**
 * 品质权重数据类
 * 用于快速查询指定品质的权重值
 * green:5000;blue:8000;purple:11000;orange:12000;red:12000
 */
public class QualityValueData implements _IParseFromStringable, Cloneable
{
    private int[] _m_qualityList = new int[EQuality.EQuality_Length];

    @Override
    public boolean parseFromString(String _str)
    {
        String[] _strList = _str.split(";");
        for (String _item : _strList)
        {
            String[] _itemList = _item.split(":");
            if (_itemList.length != 2)
            {
                CommLog.error("QualityWeightData parseFromString had item illegal, item:{}", _item);
                return false;
            }

            EQuality quality = EQuality.valueOf(_itemList[0].toUpperCase());
            int weight = Integer.parseInt(_itemList[1]);

            if (quality.ordinal() < _m_qualityList.length)
            {
                if (_m_qualityList[quality.ordinal()] != 0)
                    CommLog.warn("QualityWeightData parseFromString quality weight repeat, item:{}", _item);

                _m_qualityList[quality.ordinal()] += weight;
            }
        }
        return true;
    }

    @Override
    protected Object clone() throws CloneNotSupportedException
    {
        QualityValueData clone = (QualityValueData) super.clone();
        clone._m_qualityList = _m_qualityList.clone();
        return clone;
    }

    /**
     * 公共的拷贝方法
     * @return
     */
    public QualityValueData copy() {
        try {
            return (QualityValueData) this.clone();
        } catch (CloneNotSupportedException e) {
            CommLog.error("QualityValueData clone failed", e);
            return null;
        }
    }

    /**
     * 合并另一个 QualityValueData 的数据
     * @param _valueData
     */
    public void merge(QualityValueData _valueData)
    {
        for (int i = 0; i < EQuality.EQuality_Length; i++)
        {
            _m_qualityList[i] += _valueData._m_qualityList[i];
        }
    }

    /**
     * 按万分比乘法
     * @param _valueData 万分比数据，其中的值作为万分比使用，10000表示100%
     */
    public void multiply(QualityValueData _valueData)
    {
        if (_valueData == null)
            return;

        for (int i = 0; i < EQuality.EQuality_Length; i++)
        {
            _m_qualityList[i] = (int) Math.ceil(_m_qualityList[i] * _valueData._m_qualityList[i] / 10000d);
        }
    }

    /**
     * 构造权重列表
     * @return
     */
    public WeightQualityValueList buildWeightList()
    {
        WeightQualityValueList qualityList = new WeightQualityValueList();
        for (int i = 0; i < _m_qualityList.length; i++)
        {
            qualityList.add(EQuality.EQuality_FromInt(i), _m_qualityList[i]);
        }
        return qualityList;
    }

}
