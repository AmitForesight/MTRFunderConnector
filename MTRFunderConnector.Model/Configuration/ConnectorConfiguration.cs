using Connector.SharedModel.Shared;
using DataModel.BaseModel;

namespace MTRFunderConnector.Model.Configuration
{
    public class ConnectorConfiguration : ConfigurationBase
    {
        public override InvocationMode InvocationMode { get; set; } = InvocationMode.OneTime;
    }
}
