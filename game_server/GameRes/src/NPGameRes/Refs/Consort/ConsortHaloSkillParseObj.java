package NPGameRes.Refs.Consort;

import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.Pair.CommPair;

public class ConsortHaloSkillParseObj extends CommPair<Long, Integer> implements _IParseFromStringable
{
    @Override
    public boolean parseFromString(String sValue)
    {
        if (null == sValue || sValue.isEmpty())
        {
            return true;
        }
        
        NPStringReader sr = new NPStringReader(sValue);
        
        try
        {
        	String skillIdStr = sr.readItem();
            if(null != skillIdStr)
            {
               this.first = Long.valueOf(skillIdStr);
            }
            
            String lvlStr = sr.readItem();
            if(null != lvlStr)
            {
                this.second = Integer.valueOf(lvlStr);
            }
            
            return true;
        }
        catch (Exception e) 
        {
        	CommLog.error("", e);
			return false;
		}
    }
}
