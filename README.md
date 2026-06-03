# Atata.NLog

[![Atata Templates](https://img.shields.io/badge/get-Atata_Templates-green.svg?color=4BC21F)](https://marketplace.visualstudio.com/items?itemName=YevgeniyShunevych.AtataTemplates)\
[![Slack](https://img.shields.io/badge/join-Slack-green.svg?colorB=4EB898)](https://join.slack.com/t/atata-framework/shared_invite/zt-5j3lyln7-WD1ZtMDzXBhPm0yXLDBzbA)
[![Atata docs](https://img.shields.io/badge/docs-Atata_Framework-orange.svg)](https://atata.io)
[![X](https://img.shields.io/badge/follow-@AtataFramework-blue.svg)](https://x.com/AtataFramework)

**Atata.NLog** is a .NET library that adds NLog logging to [Atata](https://github.com/atata-framework/atata).

*The package targets .NET 8.0 and .NET Framework 4.6.2.*

## Installation

Install the package via .NET CLI:

```bash
dotnet add package Atata.NLog
```

Or using Package Manager:

```powershell
Install-Package Atata.NLog
```

## Dependencies

- [Atata](https://www.nuget.org/packages/Atata)
- [NLog](https://www.nuget.org/packages/NLog)

## Usage

In order to add NLog logging to Atata,
call `AddNLog` (to use *NLog.config*) or `AddNLogFile` (no config file needed) extension method on the "base" `LogConsumersBuilder` instance in `GlobalFixture`.

```cs
public sealed class GlobalFixture : AtataGlobalFixture
{
    protected override void ConfigureAtataContextBaseConfiguration(AtataContextBuilder builder)
    {
        builder.LogConsumers.AddNLogFile();
    }
}
```

Also you can configure some of the settings:

```cs
builder.LogConsumers.AddNLogFile(x => x
    .WithMinLevel(LogLevel.Info)
    .WithFileNameTemplate("Info.log"));
```

## API

### `NLogConsumersBuilderExtensions`

Provides NLog extension methods for `LogConsumersBuilder`.

```cs
public static class NLogConsumersBuilderExtensions
{
    // Adds the NLogConsumer instance that uses NLog.Logger class for logging.
    public static AtataContextBuilder AddNLog(
        this LogConsumersBuilder builder,
        Action<LogConsumerBuilder<NLogConsumer>>? configure = null);

    // Adds the NLogFileConsumer instance that uses NLog.Logger class for logging into file.
    public static AtataContextBuilder AddNLogFile(
        this LogConsumersBuilder builder,
        Action<LogConsumerBuilder<NLogFileConsumer>>? configure = null);
}
```

### `NLogConsumerBuilderExtensions`

Provides NLog extension methods for `LogConsumerBuilder<TLogConsumer>`.

```cs
public static class NLogConsumerBuilderExtensions
{
    // Sets the file name template of the log file.
    // The default value is "Atata.log".
    public static LogConsumerBuilder<NLogFileConsumer> WithFileNameTemplate(
        this LogConsumerBuilder<NLogFileConsumer> builder,
        string fileNameTemplate);

    // Specifies the layout of log event.
    // The default value is @"${event-property:time-elapsed:format=hh\\\:mm\\\:ss\\.fff} ${event-property:execution-unit-id} ${uppercase:${level}:padding=5} ${event-property:log-nesting-text}${when:when='${event-property:log-source}'!='':inner={${event-property:log-source}\} }${when:when='${event-property:log-category}'!='':inner=[${event-property:log-category}] }${when:when='${message}'!='':inner=${message}${onexception:inner= }${exception:format=ToString:flattenException=false}:else=${exception:format=ToString:flattenException=false}".
    // If you want to replace "time elapsed" column in layout with "timestamp", you can replace the value
    // "{event-property:time-elapsed:format=hh\\\:mm\\\:ss\\.fff}" with
    // "{date:format=yyyy-MM-dd HH\:mm\:ss.fff}".
    public static LogConsumerBuilder<NLogFileConsumer> WithLayout(
        this LogConsumerBuilder<NLogFileConsumer> builder,
        string layout);

    // Specifies to use separate log files for log sources.
    // The main log file will be "Atata.log".
    // Source log file will be "{source}.log", e.g., "Browser.log", "App.log".
    // Sets "${{event-property:log-source:whenEmpty=Atata}}.log" to NLogFileConsumer.FileNameTemplate.
    // Sets @"${event-property:time-elapsed:format=hh\\\:mm\\\:ss\\.fff} ${event-property:execution-unit-id} ${uppercase:${level}:padding=5} ${event-property:log-nesting-text}${when:when='${event-property:log-category}'!='':inner=[${event-property:log-category}] }${when:when='${message}'!='':inner=${message}${onexception:inner= }${exception:format=ToString:flattenException=false}:else=${exception:format=ToString:flattenException=false}" to NLogFileConsumer.Layout.
    public static LogConsumerBuilder<NLogFileConsumer> WithSeparateSourceLogFiles(
        this LogConsumerBuilder<NLogFileConsumer> builder);
}
```

## Community

- Slack: [https://atata-framework.slack.com](https://join.slack.com/t/atata-framework/shared_invite/zt-5j3lyln7-WD1ZtMDzXBhPm0yXLDBzbA)
- X: https://x.com/AtataFramework
- Stack Overflow: https://stackoverflow.com/questions/tagged/atata

## Feedback

Any feedback, issues and feature requests are welcome.

If you faced an issue please report it to [Atata.NLog Issues](https://github.com/atata-framework/atata-nlog/issues),
[ask a question on Stack Overflow](https://stackoverflow.com/questions/ask?tags=atata+csharp) using [atata](https://stackoverflow.com/questions/tagged/atata) tag
or use another [Atata Contact](https://atata.io/contact/) way.

## Contact author

Contact me if you need a help in test automation using Atata Framework, or if you are looking for a quality test automation implementation for your project.

- LinkedIn: https://www.linkedin.com/in/yevgeniy-shunevych
- Email: yevgeniy.shunevych@gmail.com
- Consulting: https://atata.io/consulting/

## Contributing

Check out [Contributing Guidelines](CONTRIBUTING.md) for details.

## SemVer

Atata Framework tries to follow [Semantic Versioning 2.0](https://semver.org/) when possible.
Sometimes Selenium.WebDriver dependency package can contain breaking changes in minor version releases,
so those changes can break Atata as well.
But Atata manages its sources according to SemVer.
Thus backward compatibility is mostly followed and updates within the same major version
(e.g. from 2.1 to 2.2) should not require code changes.

## License

Atata is an open source software, licensed under the Apache License 2.0.
See [LICENSE](LICENSE) for details.
