using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsbaBank.Core.Queries
{
  public interface IQueryProcessor
    {
        TResult Handle<TResult>(IQuery<TResult> query);
    }
}
