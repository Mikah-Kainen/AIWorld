using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld
{
    public interface IStateTransition<TransitionData>
    {
        public TransitionData GetData();
    }
}
