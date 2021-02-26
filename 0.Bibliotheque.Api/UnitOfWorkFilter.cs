using System.Threading.Tasks;

using Bibliotheque.Commands.Domains.Contracts;

using Microsoft.AspNetCore.Mvc.Filters;

namespace Bibliotheque.Api
{
    public class UnitOfWorkFilter : IAsyncActionFilter
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkFilter(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var executedContext = await next.Invoke();
            if (executedContext.Exception == null && executedContext.Result is Microsoft.AspNetCore.Mvc.OkResult)
            {
                await _unitOfWork.CommitAsync();
            }
            else
            {
                await _unitOfWork.RollBackAsync();
            }
        }
    }
}
