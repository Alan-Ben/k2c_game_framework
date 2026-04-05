package MGClient.UI;

import com.google.gson.JsonArray;
import com.google.gson.JsonPrimitive;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.ArrayList;
import java.util.List;

@SuppressWarnings("serial")
public class PromptInputList extends JDialog
{
    private boolean _m_bIsOK = false;
    private List<JTextField> _m_txtInputList;

    public PromptInputList(int _num)
    {
        getContentPane().setLayout(new FlowLayout(FlowLayout.LEFT, 10, 20));

        _m_txtInputList = new ArrayList<>();
        for (int i = 0; i < _num; i++)
        {
            JTextField txt = new JTextField();
            txt.setColumns(20);
            getContentPane().add(txt);
            _m_txtInputList.add(txt);
        }

        JButton button = new JButton("确定");
        button.addActionListener(new ActionListener()
        {
            public void actionPerformed(ActionEvent arg0)
            {
                _m_bIsOK = true;
                setVisible(false);
            }
        });
        getContentPane().add(button);
    }

    public JsonArray getJsonArrayInput()
    {
        JsonArray jsonArray = new JsonArray();
        for (JTextField field : _m_txtInputList)
        {
            jsonArray.add(new JsonPrimitive(field.getText()));
        }
        return jsonArray;
    }

    public static JsonArray promptJsonArray(String _title, int _num)
    {
        PromptInputList dlg = new PromptInputList(_num);
        dlg.setModal(true);
        dlg.setSize(450, 100 * _num);
        dlg.setLocation(500, 500);
        dlg.init(_title);
        dlg.setVisible(true);


        if (dlg._m_bIsOK)
        {
            return dlg.getJsonArrayInput();
        } else
        {
            return null;
        }
    }

    private void init(String _title)
    {
        this.setTitle(_title);
    }
}
