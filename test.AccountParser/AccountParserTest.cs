using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using peretc.AccountParser;

namespace test.AccountParser
{
    public class AccountParserTest
    {
        [Fact]
        public void testBoardListInvoke()
        {
            var ap = new peretc.AccountParser.AccountParser("sashkopodletc");
            var boards = ap.GetBoardList().Result;
            Assert.True(boards.Contains("кулебяка"));
        }
    }
}
