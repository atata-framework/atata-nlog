namespace Atata.NLog.IntegrationTests;

[SetUpFixture]
public class GlobalFixture
{
    [OneTimeSetUp]
    public void SetUpAtataContextGlobalProperties() =>
        AtataContext.GlobalProperties.RootNamespace = GetType().Namespace;
}
