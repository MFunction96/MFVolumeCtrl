using MFVolumeCtrl.Models;
using MFVolumeService.Interfaces;
using System;

namespace MFVolumeService.Controllers.Operators
{
    public class ScriptOperator : IOperator
    {

        protected ScriptModel Script { get; set; }

        public ScriptOperator(SocketMessage message)
        {
            if (!(message.Body is ScriptModel script))
                throw new ArgumentException(nameof(message.Body));
            Script = script;

        }

        public void Dispose()
        {
            Script?.Dispose();
        }

        public SocketMessage Operate()
        {
            Script.Run();
            return new SocketMessage();
        }
    }
}
