using System;

namespace BolgerUtils
{
    public class Result<T>
    {
        // ReSharper disable once ReplaceWithFieldKeyword
        private readonly T _returnValue;

        public Result(T returnValue)
        {
            _returnValue = returnValue;
        }

        public Result(Exception exception)
        {
            _returnValue = default!;
            Exception = exception;
        }

        public bool HasReturnValue => Exception == null;
        public T ReturnValue
        {
            get
            {
                // ReSharper disable once ConvertIfStatementToReturnStatement
                if(Exception != null)
                    throw new InvalidOperationException($"{nameof(Exception)} is not null");

                return _returnValue;
            }
        }

        public Exception? Exception { get; }
    }
}
