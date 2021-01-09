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
        // 마지막 GetResponse가 안 끝났으면 취소시키기
        _responseTaskCompletionSource.TrySetResult(new Response {NextAction = Response.ActionEnum.Cancel});

        // 윈도우 열기.
        base.OpenWindow();

        // 입력 기다리기.
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
