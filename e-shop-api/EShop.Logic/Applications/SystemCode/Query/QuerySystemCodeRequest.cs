using MediatR;

namespace EShop.Logic.Applications.SystemCode.Query
{
        public class QuerySystemCodeRequest : IRequest<QuerySystemCodeResponse>
    {
        public string Type { get; set; }
    }
}