using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace ImageboardBackuper
{
    public static class Download
    {
        public static async Task DownloadFileAsync(string url, string filePath)
        {
            using (HttpClient client = new HttpClient()) // Создаем HttpClient
            {
                try
                {
                    // Отправляем GET-запрос и получаем ответ
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Убеждаемся, что запрос был успешным (код 200 OK)
                    response.EnsureSuccessStatusCode();

                    // Читаем содержимое ответа как поток
                    using (Stream stream = await response.Content.ReadAsStreamAsync())
                    {
                        // Создаем файл и записываем в него содержимое потока
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            await stream.CopyToAsync(fileStream);
                        }
                    }

                    Console.WriteLine($"Файл успешно загружен в: {filePath}");

                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Ошибка при загрузке файла: {e.Message}");
                    throw e;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Произошла ошибка: {e.Message}");
                    throw e;
                }
            }
        }

        
    }
}

