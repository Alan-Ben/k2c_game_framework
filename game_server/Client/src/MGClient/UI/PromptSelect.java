package MGClient.UI;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

@SuppressWarnings("serial")
public class PromptSelect extends javax.swing.JDialog
{
    private boolean _m_bIsOK = false;
    @SuppressWarnings("rawtypes")
    private JComboBox comboSelects = null;

    @SuppressWarnings("rawtypes")
    public PromptSelect()
    {
        getContentPane().setLayout(new FlowLayout(FlowLayout.LEFT, 10, 20));

        JLabel label = new JLabel("    输入：");
        getContentPane().add(label);

        JLabel label_2 = new JLabel("");
        getContentPane().add(label_2);

        JButton button = new JButton("确定");
        button.addActionListener(new ActionListener()
        {
            public void actionPerformed(ActionEvent arg0)
            {
                _m_bIsOK = true;
                setVisible(false);
            }
        });

        comboSelects = new JComboBox();
        getContentPane().add(comboSelects);
        getContentPane().add(button);

        JLabel label_5 = new JLabel("");
        getContentPane().add(label_5);
    }

    public String getInput()
    {

        return (String) comboSelects.getSelectedItem();
    }

    public static String prompt(String _title, String[] options)
    {
        return prompt(_title, 450, 100, options);
    }

    public static String prompt(String _title, int _width, int _height, String[] options)
    {
        PromptSelect dlg = new PromptSelect();
        dlg.setModal(true);
        dlg.setSize(_width, _height);
        dlg.setLocation(500, 500);
        dlg.init(_title, options);
        dlg.setVisible(true);


        if (dlg._m_bIsOK)
        {
            return dlg.getInput();
        } else
        {
            return "";
        }
    }

    @SuppressWarnings("unchecked")
    private void init(String _title, String[] options)
    {
        this.setTitle(_title);
        for (String str : options)
        {
            comboSelects.addItem(str);
        }

    }
}
