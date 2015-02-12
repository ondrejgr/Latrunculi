using Latrunculi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi
{
    public delegate void BrainComputationStartedEvent();
    public delegate void BrainComputationFinishedEvent(Move bestMove, string errorMessage, bool isCancelled);
}
