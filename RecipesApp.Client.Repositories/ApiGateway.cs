using RecipesApp.Client.Core;
using Refit;

namespace RecipesApp.Client.Repositories;

public abstract class ApiGateway
{
    protected Task<Result<TType>>
        InvokeAndMap<TType>(
            Task<ApiResponse<TType>> call)
    {
        return InvokeAndMap(call, e => e);
    }


    protected async Task<Result<TResult>>
        InvokeAndMap<TResult, TDtoResult>(
            Task<ApiResponse<TDtoResult>> call,
            Func<TDtoResult, TResult> mapper)
    {
        try
        {
            var response = await call;

            if (response.IsSuccessStatusCode)
                return Result<TResult>
                    .Success(mapper(response.Content));

            return Result<TResult>
                .Fail("FAILED_REQUEST", response.Error.StatusCode.ToString());
        }
        catch (ApiException aex)
        {
            return Result<TResult>
                .Fail("ApiException", aex.StatusCode.ToString(), aex);
        }
        catch (Exception ex) { return Result<TResult>.Fail(ex); }
    }
}