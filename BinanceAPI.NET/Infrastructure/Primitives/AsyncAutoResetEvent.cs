﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Infrastructure.Primitives
{
    /// <summary>
    /// This is an implementation of an asynchronous AutoResetEvent based on work of Stephen Toub published in Microsoft's DevBlog : https://devblogs.microsoft.com/pfxteam/building-async-coordination-primitives-part-2-asyncautoresetevent/
    /// It is used to asynchronously wait for sending task to finish before writing to the "_sendBuffer" and vice versa (Used in WebSocketClient).
    /// </summary>
    public class AsyncAutoResetEvent
    {
        private readonly static Task s_completed = Task.FromResult(true);
        private readonly Queue<TaskCompletionSource<bool>> m_waits = new();
        private bool m_signaled;
        public Task WaitAsync()
        {
            lock (m_waits)
            {
                if (m_signaled)
                {
                    m_signaled = false;
                    return s_completed;
                }
                else
                {
                    var tcs = new TaskCompletionSource<bool>();
                    m_waits.Enqueue(tcs);
                    return tcs.Task;
                }
            }
        }
        public void Set()
        {
            TaskCompletionSource<bool> toRelease = null;

            lock (m_waits)
            {
                if (m_waits.Count > 0) toRelease = m_waits.Dequeue();
                else if (!m_signaled) m_signaled = true;
            }
            if (toRelease != null) toRelease.SetResult(true);

        }
    }
}
