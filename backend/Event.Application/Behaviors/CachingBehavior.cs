using Event.Application.Interfaces;
using MediatR;

namespace Event.Application.Behaviors
{
    public class CachingBehavior<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICachQuery
    {
        private readonly ICachService cachService;

        public CachingBehavior(
            ICachService cachService)
        {
            this.cachService = cachService;
        }

        public async Task<TResponse> Handle(
            TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            var cachValue = await cachService.GetData<TResponse>(request.Key);

            if(cachValue.IsSuccess)
            {
                return cachValue.Value;
            }

            var response = await next();

            if(request.ExpirationSecond.HasValue)
            {
                await cachService.SetData(
                    request.Key, 
                    response, 
                    request.ExpirationSecond.Value);
            }

            return response;
        }
    }
}
