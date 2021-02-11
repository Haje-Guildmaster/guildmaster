using GuildMaster.Exploration;
using GuildMaster.Windows;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GuildMaster.Tools;
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

    public async Task<Response> GetResponse(CancellationToken cancellationToken = default)
    {
        return await _getResponseSingularRun.Run(async linkedCancellationToken =>
        {
            try
            {
                // 윈도우 열기.
                base.OpenWindow();

                // 입력 기다리기
                _responseTaskCompletionSource = new TaskCompletionSource<Response>();
                return await _responseTaskCompletionSource.CancellableTask(linkedCancellationToken);
            }
            finally
            {
                Close();
            }
        }, cancellationToken);
    }

    public void GoNext()
    {
        _responseTaskCompletionSource.TrySetResult(new Response
        {
            NextAction = Response.ActionEnum.GoNext,
        });
    }

    public void Back()
    {
        _responseTaskCompletionSource.TrySetResult(new Response
        {
            NextAction = Response.ActionEnum.GoBack,
        });
    }

    protected override void OnClose()
    {
        _responseTaskCompletionSource.TrySetResult(new Response
        {
            NextAction = Response.ActionEnum.Cancel,
        });
    }

    private readonly SingularRun _getResponseSingularRun = new SingularRun();
    private TaskCompletionSource<Response> _responseTaskCompletionSource = new TaskCompletionSource<Response>();
}