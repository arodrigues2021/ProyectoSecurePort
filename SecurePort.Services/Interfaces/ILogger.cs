using System;

namespace SecurePort
{
    public interface ILogger
    {
        void PublishException(Exception exception);

        void WriteVerbose(string category, string message);

        void WriteInfo(string category, string message);

        void WriteWarning(string category, string message);

        void TraceError(string category, string message);

    }
}
