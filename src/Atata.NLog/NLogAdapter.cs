namespace Atata.NLog;

internal static class NLogAdapter
{
    private static readonly object s_configurationSyncLock = new();

    internal static FileTarget CreateFileTarget(string targetName, string filePath, string layout) =>
        new(targetName)
        {
            FileName = Layout.FromString(filePath),
            Layout = Layout.FromString(layout)
        };

    internal static LoggingRule AddConfigurationRuleForAllLevels(string ruleName, Target target, string loggerNamePattern)
    {
        lock (s_configurationSyncLock)
        {
            var configuration = NLogManager.Configuration;

            if (configuration is null)
            {
                configuration = new LoggingConfiguration();
                NLogManager.Configuration = configuration;
            }

            LoggingRule loggingRule = new(loggerNamePattern, NLogLevel.Trace, NLogLevel.Fatal, target)
            {
                Final = true,
                RuleName = ruleName
            };

            configuration.AddRule(loggingRule);

            NLogManager.ReconfigExistingLoggers();

            return loggingRule;
        }
    }

    internal static void RemoveConfigurationRule(LoggingRule rule)
    {
        lock (s_configurationSyncLock)
        {
            var configuration = NLogManager.Configuration!;
            configuration.LoggingRules.Remove(rule);
        }
    }

    internal static NLogEventInfo CreateLogEventInfo(LogEventInfo eventInfo)
    {
        NLogEventInfo otherEventInfo = new()
        {
            TimeStamp = eventInfo.Timestamp,
            Level = ConvertLogLevel(eventInfo.Level),
            Message = eventInfo.Message ?? string.Empty,
            Exception = eventInfo.Exception
        };

        var properties = otherEventInfo.Properties;

        foreach (var item in eventInfo.GetProperties())
            properties[item.Key] = item.Value;

        return otherEventInfo;
    }

    private static NLogLevel ConvertLogLevel(LogLevel level) =>
        level switch
        {
            LogLevel.Trace => NLogLevel.Trace,
            LogLevel.Debug => NLogLevel.Debug,
            LogLevel.Info => NLogLevel.Info,
            LogLevel.Warn => NLogLevel.Warn,
            LogLevel.Error => NLogLevel.Error,
            LogLevel.Fatal => NLogLevel.Fatal,
            _ => throw Guard.CreateArgumentExceptionForUnsupportedValue(level)
        };
}
