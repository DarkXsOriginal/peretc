using ImageboardBackuper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject2
{
    public class DownloadTest
    {
        [Fact]
        public void testDownload()
        {
            Task task = testDownloadAsync();
            task.Wait();
        }
        async Task testDownloadAsync()
        {
            string url = "https://i.pinimg.com/736x/92/52/26/92522636e62ff8685a72da760b871f51.jpg";
            string filePath =  Path.GetTempFileName().Replace("tmp", "jpg");

            await Download.DownloadFileAsync(url, filePath);
            File.Delete(filePath);
        }
    }
}
