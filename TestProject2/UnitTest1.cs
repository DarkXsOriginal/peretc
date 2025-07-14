using System.Windows;
using ImageboardBackuper;

namespace TestProject2
{
    public class BackuperTest
    {
        [Fact]
        public void Test1()
        {
            var userData = new UserData("pasha");
            var backuper = new Backuper(userData);
        }
    }
}