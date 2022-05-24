using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blarser.WowContent.Tests;

[TestFixture]
public class TempTest
{
    [Test]
    public void Things_were_done()
    {
        var t = new TestModel();

        var service = new TestService();

        service.DoStuff(t);

        Assert.True(t.Flag);
    }
}
