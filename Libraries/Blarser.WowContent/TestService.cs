using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blarser.WowContent
{
    public class TestModel
    {
        public string Name { get; set; }
        public bool Flag { get; set; }
    }

    public class TestService : ITestService
    {
        public void DoStuff(TestModel t)
        {
            t.Flag = true;
        }
    }

    public interface ITestService
    {
        void DoStuff(TestModel t);
    }
}
