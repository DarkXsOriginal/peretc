using System;
using System.Net.Http;
using System.Net.Http.Headers;

public static class BulkHeaderParser
{
    /// <summary>
    /// Парсит текст с HTTP-заголовками и добавляет их в HttpClient
    /// </summary>
    /// <param name="client">HttpClient для добавления заголовков</param>
    /// <param name="headersText">Текст с заголовками (формат "Name: Value")</param>
    /// <param name="replaceExisting">Заменить существующие заголовки</param>
    public static void AddHeadersFromText(this HttpClient client, string headersText, bool replaceExisting = false)
    {
        if (client == null)
            throw new ArgumentNullException(nameof(client));

        if (string.IsNullOrWhiteSpace(headersText))
            return;

        // Удаляем BOM (Byte Order Mark) если он есть
        headersText = headersText.TrimStart('\uFEFF');

        var lines = headersText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Split(new[] { ':' }, 2);
            if (parts.Length != 2) continue;

            var name = parts[0].Trim();
            var value = parts[1].Trim();

            // Удаляем BOM из имени заголовка (на случай, если он был в середине текста)
            name = name.Replace("\uFEFF", "");

            if (replaceExisting && client.DefaultRequestHeaders.Contains(name))
            {
                client.DefaultRequestHeaders.Remove(name);
            }

            if (!client.DefaultRequestHeaders.Contains(name))
            {
                try
                {
                    // Особые случаи для стандартных заголовков
                    if (name.Equals("Accept", StringComparison.OrdinalIgnoreCase))
                    {
                        client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(value));
                    }
                    else if (name.Equals("User-Agent", StringComparison.OrdinalIgnoreCase))
                    {
                        client.DefaultRequestHeaders.UserAgent.ParseAdd(value);
                    }
                    else if (name.Equals("Cookie", StringComparison.OrdinalIgnoreCase))
                    {
                        // Для Cookie используем CookieContainer
                        if (client.DefaultRequestHeaders.TryAddWithoutValidation(name, value))
                        {
                            // Альтернатива: добавление в CookieContainer
                            // handler.CookieContainer.SetCookies(new Uri("http://example.com"), value);
                        }
                    }
                    else
                    {
                        client.DefaultRequestHeaders.Add(name, value);
                    }
                }
                catch (FormatException ex)
                {
                    // Пробуем добавить заголовок без валидации
                    if (!client.DefaultRequestHeaders.TryAddWithoutValidation(name, value))
                    {
                        Console.WriteLine($"Failed to add header '{name}': {ex.Message}");
                    }
                }
            }
        }
    }

}