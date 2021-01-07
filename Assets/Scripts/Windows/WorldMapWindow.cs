using GuildMaster.Exploration;
using GuildMaster.Windows;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WorldMapWindow : DraggableWindow
{
    public class Response
    {
        public enum ActionEnum
        {
            Cancel,
            GoBack,
            GoNext,
        }

        public ActionEnum NextAction;
    }


    public async Task<Response> GetResponse()
    {
        _responseTaskCompletionSource.TrySetResult(new Response {NextAction = Response.ActionEnum.Cancel});
        _responseTaskCompletionSource = new TaskCompletionSource<Response>();
        return await _responseTaskCompletionSource.Task;
    }
    
    public void GoNext()
    {
        _responseTaskCompletionSource.TrySetResult(new Response
        {
            NextAction = Response.ActionEnum.GoNext,
        });
        Close();
    }

    public void Back()
    {
        _responseTaskCompletionSource.TrySetResult(new Response
        {
            NextAction = Response.ActionEnum.GoBack,
        });
        Close();
    }

    protected override void OnClose()
    {
        _responseTaskCompletionSource.TrySetResult(new Response
        {
            NextAction = Response.ActionEnum.Cancel,
        });
    }

    private TaskCompletionSource<Response> _responseTaskCompletionSource = new TaskCompletionSource<Response>();
}
