using MediatR;

namespace e_shop_api.Applications.SystemCode.Query
{
        public class QuerySystemCodeRequest : IRequest<QuerySystemCodeResponse>
    {
        public string Type { get; set; }
    }
}