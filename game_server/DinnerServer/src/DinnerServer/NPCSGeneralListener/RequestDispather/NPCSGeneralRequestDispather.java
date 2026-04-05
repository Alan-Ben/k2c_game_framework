package DinnerServer.NPCSGeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher;

public class NPCSGeneralRequestDispather extends NPRequestDispatcher
{
    private static NPCSGeneralRequestDispather _g_instance = new NPCSGeneralRequestDispather();

    public static NPCSGeneralRequestDispather getInstance()
    {
        return _g_instance;
    }

    protected NPCSGeneralRequestDispather()
    {
        NPCSGeneral_255_RequestDispatcher.init(this);
    }
}
