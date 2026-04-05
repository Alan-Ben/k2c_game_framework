package NPCommon.RefData.Ref.Matcher;

import ALServerLog.ALServerLog;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;

public class NumberRange
{
    public static NumberRange parse(String data)
    {
        NumberRange range = new NumberRange();
        if (data == null || "".equals(data))
        {
            range.from = 0;
            range.to = 9999;
        } else
        {
            range.parseFrom(data, '~');
        }
        return range;
    }

    public static NumberRange parse(String data, char token)
    {
        NumberRange range = new NumberRange();
        if (data == null || "".equals(data))
        {
            range.from = 0;
            range.to = 9999;
        } else
        {
            range.parseFrom(data, token);
        }
        return range;
    }

    int from;
    int to;

    public NumberRange()
    {
        from = 0;
        to = 0;
    }

    public NumberRange(int f, int t)
    {
        from = f;
        to = t;
    }

    public boolean within(int r)
    {
        return (r >= from) && (r <= to || to == -1);
    }

    public int getLow()
    {
        return from;
    }

    public int getTop()
    {
        return to;
    }

    public boolean isNarrow()
    {
        return from == to;
    }

    public void parseFrom(String data, char token)
    {
        if (data == null)
        {
            ALServerLog.Error("NumberRange failed to parse String 'null'");
            return;
        }
        String[] split = CommonFunc.charSplit(data, token);
        try
        {
            if (split.length == 1)
            {
                from = to = Integer.parseInt(data.trim());
            } else if (split.length == 2)
            {
                int a = Integer.parseInt(split[0].trim());
                int b = Integer.parseInt(split[1].trim());
                from = Math.min(a, b);
                to = Math.max(a, b);
            }
        } catch (Exception e)
        {
            ALServerLog.Error("NumberRange failed to parse String '" + data + "'");
            ALServerLog.Error(e.toString());
        }
    }

    public int getRandomValueInRange()
    {
        ArrayList<Integer> range = new ArrayList<>();
        range.add(getLow());
        range.add(getTop());
        return CommonFunc.getRandomValueInRange(range);
    }

    @Override
    public String toString()
    {
        return "[ " + from + " ~ " + to + " ]";
    }
}
