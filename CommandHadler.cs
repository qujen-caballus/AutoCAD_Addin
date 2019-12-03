using System;
using Autodesk.Windows;
using Autodesk.AutoCAD.ApplicationServices;

public class CommandHandler : System.Windows.Input.ICommand

{

    public bool CanExecute(object parameter)

    {
        return true;

    }
    public event EventHandler CanExecuteChanged;
    public void Execute(object parameter)
    {
        Document doc = Application.DocumentManager.MdiActiveDocument;
        if (parameter is RibbonButton)
        {
            RibbonButton button = parameter as RibbonButton;
            RibbonCommandItem cmd = parameter as RibbonCommandItem;
            doc.SendStringToExecute((string)cmd.CommandParameter, true, false, true);
            doc.Editor.WriteMessage("\nRibbonButton Executed: " + button.Text + "\n");
        }

    }

}
