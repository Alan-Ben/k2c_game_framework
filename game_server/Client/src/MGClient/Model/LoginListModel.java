package MGClient.Model;

import javax.swing.*;
import java.util.ArrayList;

@SuppressWarnings({"serial", "rawtypes"})
public class LoginListModel extends AbstractListModel
{
    private ArrayList<LoginEntry> _mArr = new ArrayList<>();

    @Override
    public synchronized Object getElementAt(int index)
    {

        if (index >= _mArr.size())
            return null;
        return _mArr.get(index);
    }

    @Override
    public synchronized int getSize()
    {
        return _mArr.size();

    }

    public synchronized LoginEntry addEntry(String name, String password)
    {
        LoginEntry entry = new LoginEntry();
        entry.name = name;
        entry.password = password;
        _mArr.add(entry);
        return entry;

    }

    public synchronized void removeAt(int i)
    {

        _mArr.remove(i);

    }

    public synchronized void removeEntry(LoginEntry entry)
    {
        _mArr.remove(entry);

    }

    public synchronized void clear()
    {
        _mArr.clear();


    }

    public synchronized LoginEntry lookupEntryByName(String name)
    {

        for (LoginEntry entry : _mArr)
        {
            if (entry.name.compareToIgnoreCase(name) == 0)
                return entry;
        }
        return null;
    }

    public void setEntryLogined(String name)
    {

    }

}  