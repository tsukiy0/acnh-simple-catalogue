using System;

namespace Core.Shared
{
    public static class NullableExtensions
    {
        public static Nullable<U> Select<T, U>(this Nullable<T> target, Func<T, U> op)
            where T : struct
            where U : struct
        {
            if (!target.HasValue)
            {
                return null;
            }

            return op(target.Value);
        }
    }
}