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
        [Theory(DisplayName ="UserBoardsParse")]
        [InlineData("marcykatya", "маг-заери и т.д.")]
        public void testBoardListInvoke(string username, string boardName)
        {
            var ap = new PinterestAccountScraper(username);
            var boards = ap.GetBoardList().Result;
            Assert.True(boards.Any(b=>b.Name == boardName));
        }
        [Theory(DisplayName = "UserBoardPinsParse")]
        [InlineData("marcykatya", "маг-заери и т.д.", "853854410601705101")] //один из ранних
        [InlineData("marcykatya", "маг-заери и т.д.", "853854410601705487")]//один из новых

        public void testBoardPinsInvoke(string username, string boardName, string pinId)
        {
            var ap = new PinterestAccountScraper(username);
            var boards = ap.GetBoardList().Result;
            var boardId = boards.First(b => b.Name == boardName).Id;
            var boardData = new BoardData()
            {
                Name = boardName,
                Id = boardId
            };


            var pins = ap.GetBoardPins(boardData).Result;
            Console.WriteLine($"Total pins: {pins.Length}");
            Assert.True(pins.Any(p => p.Id == pinId));
        }
    }
    public class AccountParserCacheTest
    {
       static bool cached;
        static PinterestAccountScraper ap;
        [Theory(DisplayName = "UserBoardPinsParseCache")]
        [InlineData("marcykatya", "маг-заери и т.д.", "853854410601705101")] //один из ранних
        [InlineData("marcykatya", "маг-заери и т.д.", "853854410601705487")]//один из новых

        public async Task testBoardPinsInvoke(string username, string boardName, string pinId)
        {
            if (!cached)
            {
                ap = new PinterestAccountScraper(username);
                await ap.Cache();
                cached = true;
            }

            var boards = ap.GetBoardList().Result;
            var boardId = boards.First(b => b.Name == boardName).Id;
            var boardData = new BoardData()
            {
                Name = boardName,
                Id = boardId
            };


            var pins = ap.GetBoardPins(boardData).Result;
            Console.WriteLine($"Total pins: {pins.Length}");
            Assert.True(pins.Any(p => p.Id == pinId));
        }
    }
}
