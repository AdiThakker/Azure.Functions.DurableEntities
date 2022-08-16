using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Functions.Domain.Interfaces
{
    public interface IEntity
    {
        public Task<int> AddAsync(string input);
        
        public Task<int> GetAsync(string input);
        
    }
}
