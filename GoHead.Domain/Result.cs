using System;

namespace GoHead.Domain
{
    using static Helpers;

    /// <summary>
    ///
    /// Essa classe tem o objetivo de fornecer um retorno mais expressivo dos resultados de um fun��o
    /// Ao realizar uma chamada para um Object podemos ter como resultado: Exception, null ou Object
    ///
    /// Ex 1: Result<Exception, Customer> t;
    ///     Se IsFailure � True, temos uma instancia de uma Exception
    ///     Se IsSucess � True, temos uma instancia de um Customer
    ///
    /// Ex 2: Result<Exception, Success>
    ///     Para Success � necess�rio fazer o retorno de Unit.Successful
    ///
    /// </summary>
    /// <typeparam name="TFailure"></typeparam>
    /// <typeparam name="TSuccess"></typeparam>
    public struct Result<TFailure, TSuccess>
    {
        public TFailure Failure { get; internal set; }
        public TSuccess Success { get; internal set; }

        public bool IsFailure { get; }
        public bool IsSuccess => !IsFailure;

        public Option<TFailure> OptionalFailure => IsFailure ? Some(Failure) : None;

        public Option<TSuccess> OptionalSuccess => IsSuccess ? Some(Success) : None;

        internal Result(TFailure failure)
        {
            IsFailure = true;
            Failure = failure;
            Success = default(TSuccess);
        }

        internal Result(TSuccess success)
        {
            IsFailure = false;
            Failure = default(TFailure);
            Success = success;
        }

        public TResult Match<TResult>(
                Func<TFailure, TResult> failure,
                Func<TSuccess, TResult> success
            )
            => IsFailure ? failure(Failure) : success(Success);

        public Unity Match(
                Action<TFailure> failure,
                Action<TSuccess> success
            )
            => Match(ToFunc(failure), ToFunc(success));

        public static implicit operator Result<TFailure, TSuccess>(TFailure failure)
            => new Result<TFailure, TSuccess>(failure);

        public static implicit operator Result<TFailure, TSuccess>(TSuccess success)
            => new Result<TFailure, TSuccess>(success);

        public static Result<TFailure, TSuccess> Of(TSuccess obj) => obj;

        public static Result<TFailure, TSuccess> Of(TFailure obj) => obj;
    }

    /// <summary>
    /// Struct utilizado quando n�o existir um objeto de retorno.
    /// </summary>
    public struct Unity
    {
        public static Unity Successful { get { return new Unity(); } }
    }

    /// <summary>
    /// Helper para essa classe que auxilia na manipula��o das chamadas
    /// </summary>
    public static partial class Helpers
    {
        private static readonly Unity unit = new Unity();

        public static Unity Unit() => unit;

        public static Func<T, Unity> ToFunc<T>(Action<T> action) => o =>
            {
                action(o);
                return Unit();
            };

        public static Func<Unity> ToFunc(Action action) => () =>
            {
                action();
                return Unit();
            };
    }
}