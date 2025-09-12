#pragma warning disable S103 // Lines should not be too long

namespace Atata.NLog;

/// <summary>
/// A log consumer that writes log to file using NLog.
/// </summary>
public class NLogFileConsumer : IInitializableLogConsumer, IDisposable
{
    /// <summary>
    /// The default file name, which is <c>"Trace.log"</c>.
    /// </summary>
    public const string DefaultFileName = "Trace.log";

    private static int s_globalCounter;

    private Logger _logger = null!;

    private LoggingRule _loggingRule = null!;

    /// <summary>
    /// Gets or sets the file name template.
    /// The default value is <c>"Trace.log"</c>.
    /// </summary>
    public string FileNameTemplate { get; set; } = DefaultFileName;

    /// <summary>
    /// Gets or sets the layout of log event.
    /// The default value is <c>@"${event-property:time-elapsed:format=hh\\\:mm\\\:ss\\.fff} ${event-property:execution-unit-id} ${uppercase:${level}:padding=5} ${event-property:log-nesting-text}${when:when='${event-property:log-source}'!='':inner={${event-property:log-source}\} }${when:when='${event-property:log-category}'!='':inner=[${event-property:log-category}] }${when:when='${message}'!='':inner=${message}${onexception:inner= }${exception:format=ToString:flattenException=false}:else=${exception:format=ToString:flattenException=false}"</c>.
    /// </summary>
    /// <remarks>
    /// If you want to replace "time elapsed" column in layout with "timestamp", you can replace the value
    /// <c>"{event-property:time-elapsed:format=hh\\\:mm\\\:ss\\.fff}"</c> with
    /// <c>"{date:format=yyyy-MM-dd HH\:mm\:ss.fff}"</c>.
    /// </remarks>
    public string Layout { get; set; } =
        @"${event-property:time-elapsed:format=hh\\\:mm\\\:ss\\.fff} ${event-property:execution-unit-id} ${uppercase:${level}:padding=5} ${event-property:log-nesting-text}${when:when='${event-property:log-source}'!='':inner={${event-property:log-source}\} }${when:when='${event-property:log-category}'!='':inner=[${event-property:log-category}] }${when:when='${message}'!='':inner=${message}${onexception:inner= }${exception:format=ToString:flattenException=false}:else=${exception:format=ToString:flattenException=false}";

    void IInitializableLogConsumer.Initialize(AtataContext context)
    {
        int loggerNumber = Interlocked.Increment(ref s_globalCounter);
        string uniqueLoggerName = $"{nameof(NLogFileConsumer)}{loggerNumber}";
        string filePath = BuildFilePath(context);

        FileTarget target = NLogAdapter.CreateFileTarget(uniqueLoggerName, filePath, Layout);

        _loggingRule = NLogAdapter.AddConfigurationRuleForAllLevels(uniqueLoggerName, target, uniqueLoggerName);

        _logger = NLogManager.GetLogger(uniqueLoggerName);
    }

    void ILogConsumer.Log(LogEventInfo eventInfo)
    {
        NLogEventInfo otherEventInfo = NLogAdapter.CreateLogEventInfo(eventInfo);
        _logger.Log(otherEventInfo);
    }

    object ICloneable.Clone() =>
        new NLogFileConsumer
        {
            FileNameTemplate = FileNameTemplate,
            Layout = Layout
        };

    [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>")]
    void IDisposable.Dispose() =>
        NLogAdapter.RemoveConfigurationRule(_loggingRule);

    private string BuildFilePath(AtataContext context)
    {
        string fileName = context.Variables.FillPathTemplateString(FileNameTemplate);
        return Path.Combine(context.ArtifactsPath, fileName);
    }
}
