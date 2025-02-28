using AutoMapper;
using ReactApp.Server.Services.Interface;

namespace ReactApp.Server.Services
{
    public class BaseApplicationService : IBaseApplicationService
    {
        protected readonly ILogger<IBaseApplicationService> logger;
        protected readonly string ServiceName;
        protected readonly IMapper Mapper;
        protected BaseApplicationService(ILogger<IBaseApplicationService> logger, IMapper mapper)
        {
            this.logger = logger;
            Mapper = mapper;
            ServiceName = GetType().Name;
        }
    }
}
