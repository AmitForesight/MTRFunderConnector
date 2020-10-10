using DataModel.BaseModel;

namespace MTRFunderConnector.Model.ApiModel
{
    public class FunderApiModel :ApiBase
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Uid { get; set; }
    }
}
