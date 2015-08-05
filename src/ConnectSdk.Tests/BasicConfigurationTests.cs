using ConnectSdk.Config;
using Xunit;

namespace ConnectSdk.Tests
{
    public class BasicConfigurationTests
    {
        public class WhenConstructedWithInvalidConfig
        {
            [Fact]
            public void It_should_throw_exception_for_empty_api_key()
            {
                Assert.Throws<ConnectInitializationException>(() => new BasicConfiguration("", "proj"));
            }

            [Fact]
            public void It_should_throw_exception_for_empty_proj_id()
            {
                Assert.Throws<ConnectInitializationException>(() => new BasicConfiguration("apiKey", ""));
            }
        }
    }
}
