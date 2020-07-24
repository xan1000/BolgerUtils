using System;

namespace BolgerUtils
{
    public class Result<T>
    {
        private readonly T _returnValue;

        public Result(T returnValue) => _returnValue = returnValue;
        public Result(Exception exception) => Exception = exception;

        public bool HasReturnValue => Exception == null;
        public T ReturnValue
        {
            get
            {
                if(Exception != null)
                    throw new InvalidOperationException($"{nameof(Exception)} is not null");

                return _returnValue;
            }
        }

        public Exception Exception { get; private set; }
    }
}
