using System;

namespace Core.Shared
{
    public interface IConfigService
    {
        string Get(string key);
    }

    public class EnvironmentConfigService : IConfigService
    {
        public class KeyNotFoundException : BaseException { }

        public string Get(string key)
        {
            var value = Environment.GetEnvironmentVariable(key);

            if (value == null)
            {
                throw new KeyNotFoundException();
            }

            return value;
        }
    }

}