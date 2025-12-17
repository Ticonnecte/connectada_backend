using System;
using System.Threading;
using Xunit;
using Xunit.Abstractions;
using LongLivedMarshalByRefObject = Xunit.Sdk.LongLivedMarshalByRefObject;

namespace CidConnectada.Tests
{
    public class TestExecutionSink : LongLivedMarshalByRefObject, IMessageSink
    {
        private readonly Action<ITestFailed> _onTestFailed;
        private readonly Action<ITestSkipped> _onTestSkipped;

        public TestExecutionSink(Action<ITestFailed> onTestFailed, Action<ITestSkipped> onTestSkipped)
        {
            _onTestFailed = onTestFailed;
            _onTestSkipped = onTestSkipped;
        }
        public ManualResetEvent Finished { get; } = new ManualResetEvent(false);

        public bool OnMessage(IMessageSinkMessage message)
        {
            if (message is ITestFailed failed)
            {
                _onTestFailed(failed);
            }
            else if (message is ITestSkipped skipped)
            {
                _onTestSkipped(skipped);
            }
            else if (message is ITestAssemblyExecutionFinished)
            {
                Finished.Set();
            }

            return true;
        }
    }
}