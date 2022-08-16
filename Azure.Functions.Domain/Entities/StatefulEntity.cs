using Azure.Functions.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Functions.Domain.Entities
{
    public class StatefulEntity : IEntity
    {
        public StatefulEntity()
        {
            
        }
        
        public Task<int> AddAsync(string input)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAsync(string input)
        {
            throw new NotImplementedException();
        }

        
    }
}
