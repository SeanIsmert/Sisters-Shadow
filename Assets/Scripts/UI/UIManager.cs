using TMPro;

/// <summary>
/// Manager script intended to handle any UI elements.
/// Now it is split into parts that inherit the singleton base.
/// This way any logic can be handled there.
/// Written by: Kay
/// Modified by Sean
/// </summary>
public class UIManager: MonoSinglton<UIManager>
{
    protected void Awake()
    {
        
    }

    public virtual void OpenUI()
    {
        
    }

    public virtual void CloseUI()
    {
        
    }

    public virtual void ToggleUI()
    {
        //if (true)
            //CloseUI();
        //else
            //OpenUI();
    }

    public void SetText(TMP_Text textField, string text)
    {
        textField.text = text;
    }
}
