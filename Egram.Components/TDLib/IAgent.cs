using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Egram.Components.TDLib
{
    public interface IAgent
    {
        event EventHandler<TD.Structure> Received;

        Task<TResult> ExecuteAsync<TResult>(TD.Method<TResult> method)
            where TResult : TD.Structure;
    }
}
