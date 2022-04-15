using System;

namespace GoHead.Domain
{
    using static Helpers;

    public static class Result
    {
        #region Bind
        public static Result<TFailure, NewTSuccess> Bind<TFailure, TSuccess, NewTSuccess>(this Result<TFailure, TSuccess> @Result, Func<TSuccess, Result<TFailure, NewTSuccess>> func) =>
            @Result.IsSuccess ? func(@Result.Success) : Result<TFailure, NewTSuccess>.Of(@Result.Failure);
        #endregion

        public static Result<Exception, TSuccess> Run<TSuccess>(this Func<TSuccess> func)
        {
            try
            {
                return func();
            }
            catch (Exception e)
            {
                return e;
            }
        }

        public static Result<Exception, Unity> Run(this Action action) => Run(ToFunc(action));

        public static Result<Exception, TSuccess> Run<TSuccess>(this Exception ex) => ex;

    }
}