package MGCommon.Game.CommEvent.Events;

import MGCommon.Game.CommEvent.MetaData.Annotation.EventDesc;
import MGCommon.Game.CommEvent._ACommEventBase;

/****
 ${event.comment}
 ****/
 <%
 param_list = ['long _'+x.name for x in event.field_list]
 param_list =','.join(param_list)
 
 params=['"'+x.name+'"' for x in event.field_list]
 params =','.join(params)
 %>
@EventDesc(id=${event.id},name="${event.name}",params={${params}})
public class CE_${event.name} extends _ACommEventBase
{
	public static final int ID = ${event.id};
    public CE_${event.name}(${param_list})
    {
	% for field in event.field_list:
        set_${field.name}(_${field.name});
	% endfor
    }
    public CE_${event.name}(long[] _params)
    {
        super(_params);
    }

% for i in range(len(event.field_list)):
    //${event.field_list[i].comment}
    public void set_${event.field_list[i].name}(long _${event.field_list[i].name}){ setParamValue(${i}, _${event.field_list[i].name}); }
    public long get_${event.field_list[i].name}(){ return getParamValue(${i}); }
% endfor

}
