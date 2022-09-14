﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace Telegram2;

class Program
{
    static void Main(string[] args)
    {
        var client = new TelegramBotClient("5784229032:AAE1pOIizT1S13GJOP3_aGAftkKdAy8BBwA");
        client.StartReceiving(Update, Error);


        Console.ReadLine();
    }
    async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
        var message = update.Message;
        if (message.Text != null)
        {
            Console.WriteLine($"{message.Chat.FirstName}   |   {message.Text}");
            if (message.Text.ToLower().Contains("здорова"))
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Здоровей видали");
                return;
            }
            if (message.Text.ToLower().Contains("привет"))
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "приветик");
                return;
            }
        }
            if (message.Photo != null)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Хорошее фото, но лучше документом.");
                return;
            }
        if (message.Document != null)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Секунду, сделаю лучше...");

            var fileId = update.Message.Document.FileId;
            var fileInfo = await botClient.GetFileAsync(fileId);
            var filePath = fileInfo.FilePath;

            string destinationFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\{message.Document.FileName}";
            await using FileStream fileStream = System.IO.File.OpenWrite(destinationFilePath);
            await botClient.DownloadFileAsync(filePath, fileStream);
            fileStream.Close();

            Process.Start(@"C:\Users\Babetta\Desktop\Безымянный.exe", $@"""{destinationFilePath}""");
            await Task.Delay(1500);

            await using Stream stream = System.IO.File.OpenRead(destinationFilePath);
            await botClient.SendDocumentAsync(message.Chat.Id, new 
            (stream,message.Document.FileName.Replace("1.jpg", " (edited).jpg")));

            return;
        }
    }
    private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
    { 
        throw new NotImplementedException(); 
    }
}

    





