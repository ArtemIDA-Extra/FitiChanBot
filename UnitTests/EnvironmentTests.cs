using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentTests
{
    [TestClass]
    public class EnvironmentVariables
    {
        [TestMethod("Discord Token access")]
        public void GetApiToken()
        {
            var token = Environment.GetEnvironmentVariable("FitiToken");
            //Assert
            Assert.IsNotNull(token);
        }
    }
}
