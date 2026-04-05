package MGClient.Logic.LogicObjs;

import MGClient.Logic.LogicBase;
import NPCommon.Util.CommClass;

import java.util.ArrayList;
import java.util.List;
import java.util.Set;

public class LogicFactory
{
    private static LogicFactory _instance = new LogicFactory();

    public static LogicFactory getInstance()
    {
        return _instance;
    }


    private ArrayList<Class<?>> _m_lClasses = new ArrayList<Class<?>>();

    private LogicFactory()
    {
        String pack = LogicFactory.class.getPackage().getName();
        Set<Class<?>> dealers = CommClass.getClasses(pack);
        for (Class<?> cs : dealers)
        {
            if (LogicBase.class.isAssignableFrom(cs))
            {
                _m_lClasses.add(cs);
            }
        }
    }

    public List<String> enumName()
    {
        ArrayList<String> retArray = new ArrayList<String>();
        for (Class<?> cs : _m_lClasses)
        {
            retArray.add(cs.getSimpleName());
        }
        return retArray;
    }

    public LogicBase createLogic(String name)
    {
        for (Class<?> cs : _m_lClasses)
        {
            if (cs.getSimpleName().compareToIgnoreCase(name.trim()) == 0)
            {
                try
                {
                    return (LogicBase) (cs.newInstance());
                } catch (InstantiationException e)
                {
                    return null;
                } catch (IllegalAccessException e)
                {
                    return null;
                }
            }
        }
        return null;

    }

}
