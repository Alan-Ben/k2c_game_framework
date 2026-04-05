package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPCommonItem;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;

public class NPPlayerEffect_S_GAIN_ITEM_FORM extends _ANPPlayerEffectInfo
{
	//itemType类型
    private NPCommonItem _m_ciItem;
    //好感度数值高级公式
    private NPPlayerVariableGroupObj _m_voCountVarObj;
    //倍数，默认1
    private int _m_iMultiple = 1;

    public NPPlayerEffect_S_GAIN_ITEM_FORM()
    {
    	_m_ciItem = new NPCommonItem();
    }
    
    public NPCommonItem getItem() {return _m_ciItem;}
    public NPPlayerVariableGroupObj getCountVariableGroupObj() {return _m_voCountVarObj;}
    public int getMulti() {return _m_iMultiple;}

    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_GAIN_ITEM_FORM;
    }

    public static NPPlayerEffect_S_GAIN_ITEM_FORM readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_GAIN_ITEM_FORM[" + _reader.getSrcString() + "]");
            return null;
        }

        String itemStr = _reader.readItem(':');
        String countObjStr = _reader.readItem(':');

        if (null == itemStr || null == countObjStr)
        {
            ALServerLog.Error("can not get effect S_GAIN_ITEM_FORM[" + _reader.getSrcString() + "]");
            return null;
        }

        NPPlayerEffect_S_GAIN_ITEM_FORM effect = new NPPlayerEffect_S_GAIN_ITEM_FORM();
        
        effect._m_ciItem.parseFromString(itemStr);
        effect._m_voCountVarObj = NPPlayerVariableGroupObj.readVariable(countObjStr);

        effect._m_iMultiple = 1;
        String multipleStr = _reader.readItem(':');
        if(null != multipleStr)
        {
        	effect._m_iMultiple = Integer.parseInt(multipleStr);
        }
        
        return effect;
    }
}
