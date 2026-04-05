package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

import java.util.ArrayList;

public class NPPlayerEffect_S_GAIN_ITEM extends _ANPPlayerEffectInfo
{
    private ArrayList<NPCommonCostItem> _m_alItemObjList;

    public NPPlayerEffect_S_GAIN_ITEM()
    {
        _m_alItemObjList = new ArrayList<>();
    }

    public ArrayList<NPCommonCostItem> getItemObjList()
    {
        return _m_alItemObjList;
    }

    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_GAIN_ITEM;
    }

    public static NPPlayerEffect_S_GAIN_ITEM readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_GAIN_ITEM[" + _reader.getSrcString() + "]");
            return null;
        }

        NPPlayerEffect_S_GAIN_ITEM effect = new NPPlayerEffect_S_GAIN_ITEM();

        String itemStr = _reader.readItem('#');
        while (null != itemStr)
        {
            //构造读取对象
            NPStringReader itemReader = new NPStringReader(itemStr);

            String typeS = itemReader.readItem('-');
            String idS = itemReader.readItem(':');
            String countS = itemReader.readItem(':');

            if (null != typeS
                    && null != idS
                    && null != countS)
            {
                try
                {
                    NPCommonCostItem obj = new NPCommonCostItem(ENPItemType.valueOf(typeS.toUpperCase()), Long.parseLong(idS), Long.parseLong(countS));
                    effect._m_alItemObjList.add(obj);
                } catch (Exception e)
                {
                    e.printStackTrace();
                }
            } else
            {
                ALServerLog.Error("can not get effect S_GAIN_ITEM[" + itemStr + "]");
            }

            //读取下一个数据
            itemStr = _reader.readItem('#');
        }

        return effect;
    }
}
