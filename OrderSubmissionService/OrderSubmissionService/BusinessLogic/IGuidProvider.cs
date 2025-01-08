using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSubmissionService.BusinessLogic
{
    public interface IGuidProvider
    {
        public Guid NewGuid { get; }
    }

    public class GuidProvider : IGuidProvider
    {
        public Guid NewGuid => Guid.NewGuid();
    }
}
