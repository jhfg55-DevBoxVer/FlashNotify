using System;
using System.Diagnostics;

namespace FlashNotify
{
    public static class Logger
    {
        public static void Log(string message, EventLogEntryType entryType = EventLogEntryType.Information)
        {
            const string source = "FlashNotifyService";
            const string logName = "Application";

            try
            {
                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, logName);
                }
                EventLog.WriteEntry(source, message, entryType);
            }
            catch
            {
                // ����¼���־д��ʧ�ܣ������������¼��ʽ
            }
        }
    }
}
