package MGClient.Model;

import java.util.ArrayList;

public class LoginEntry
{
    public String name;
    public String password;
    public boolean isLogined = false;
    public ArrayList<String> commands = new ArrayList<String>();

    public String toString()
    {
        return String.format("%s[%s]", name, isLogined ? "OK" : "Pending");
    }
}