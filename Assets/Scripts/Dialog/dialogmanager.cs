using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuildMaster.Windows;
using GuildMaster.Dialogs;
using System.Threading.Tasks;

public class DialogManager
{

    public DialogManager(DialogUI dialogUI, Dialog dialog)
    {
        _dialogUI = dialogUI;
        _dialog = dialog;
    }
    public async Task Play()
    {
        _dialogUI.Open();
        foreach(var line in _dialog.contents)
        {
            _dialogUI.Printtext(line);
            Debug.Log("1");
            await _dialogUI.WaitNextLineTriggered();
        }
        _dialogUI.Close();
    }
    private readonly DialogUI _dialogUI;
    private readonly Dialog _dialog;
}
