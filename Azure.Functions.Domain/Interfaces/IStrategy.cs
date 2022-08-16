using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Functions.Domain.Interfaces
{
    public interface IStrategy<TInput, TResult>
    {
        public Task<TResult> ExecuteAsync(TInput input);
    }
}
