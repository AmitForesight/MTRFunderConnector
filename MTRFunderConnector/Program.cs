using Connectors.SharedModel.ProjectInitializer;
using MTRFunderConnector.Connector;
using MTRFunderConnector.Model.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.LoggerService;

namespace MTRFunderConnector
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                InitializerService<ConnectorConfiguration, FunderConnector> initializerService =
                               new InitializerService<ConnectorConfiguration, FunderConnector>();
                initializerService.InitializedConnector();
            }
            catch (Exception ex)
            {
                ex.Error();
            }
        }
    }
}
