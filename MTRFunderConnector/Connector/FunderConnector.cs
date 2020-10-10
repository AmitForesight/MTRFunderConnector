using Connector.SharedModel.BaseModel;
using DataModel.ApiModel;
using DataModel.BaseModel;
using KC.DataAcquisition;
using KC.DataModel;
using MTRFunderConnector.Model.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Authentication;
using Utilities.CollectionService;
using Utilities.LoggerService;

namespace MTRFunderConnector.Connector
{
    public class FunderConnector : ConnectorBase
    {
        #region Class Properties
        private ConnectorConfiguration _classConfiguration { get; set; }
        private Customer _customer { get; set; }

        private Extractor _extractor { get; set; }
        #endregion

        public FunderConnector(AuthenticationHelper authenticationHelper, ConfigurationBase classConfiguration) : base(authenticationHelper)
        {
            _classConfiguration = (ConnectorConfiguration)classConfiguration;

        }
        public override async Task StartProcess(Customer customer)
        {
            _customer = customer;
            _extractor = new Extractor(customer);
            try
            {
                if (!Extractor.Funders.IsNullOrEmpty())
                {
                    List<FunderApiModel> funderApiModels = MapFundersToApiModel(Extractor.Funders);
                    if (!funderApiModels.IsNullOrEmpty())
                    {
                        var response = await PostToApi(funderApiModels, _classConfiguration.ApiUrl);
                        if (response.Item2 > 250)
                        {
                            response.Item1.Error();
                            await ExcecuteFullBack(funderApiModels);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Error();
            }
        }
        private async Task ExcecuteFullBack(List<FunderApiModel> funders)
        {
            foreach (var funder in funders)
            {
                var response = await PostToApi(funder, _classConfiguration.ApiUrl);
                if (response.Item2 > 250)
                {
                    response.Item1.Error();
                }
            }
        }
        private List<FunderApiModel> MapFundersToApiModel(List<AllocationFunder> funders)
        {
            List<FunderApiModel> funderApiModels = new List<FunderApiModel>();
            try
            {
                var reportDate = DateTime.Now.ToString(_classConfiguration.DateFormat);
                foreach (var funder in funders)
                {
                    if (!string.IsNullOrEmpty(funder.FunderName))
                    {
                        FunderApiModel funderApi = MapFunderApi(reportDate, funder);
                        if (funderApi != null)
                        {
                            funderApiModels.Add(funderApi);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Error();
            }
            return funderApiModels;
        }

        private FunderApiModel MapFunderApi(string reportDate, AllocationFunder funder)
        {
            FunderApiModel funderApi = null;
            try
            {
                funderApi = new FunderApiModel
                {
                    CustomerId = _classConfiguration.CustomerId,
                    ExternalUploadDate = reportDate,
                    Name = funder.FunderName,
                    ReportDate = reportDate,
                    Type = funder.FunderType.ToString(),
                    Uid = funder.FunderName
                };
            }
            catch (Exception ex)
            {

                throw;
            }
            return funderApi;
        }
    }
}
