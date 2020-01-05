using System;
using System.Xml;
using Telegram.Bot;
using Telegram.Bot.Args;
using AIMLbot;

namespace ChatBotProject
{
    static class Program
    {
        const string aimlPath = "..//..//dialog.aiml";
        const string token = "";    // there was my Telegram bot token.
        static TelegramBotClient bot = new TelegramBotClient(token) { Timeout = TimeSpan.FromSeconds(10) };
        static Bot aiml = new Bot();
        static User user = new User("Dmitry", new Bot());

        static void Main(string[] args)
        {
            aiml.loadSettings();
            aiml.isAcceptingUserInput = false;
            XmlDocument source = new XmlDocument();
            source.Load(aimlPath);
            aiml.loadAIMLFromXML(source, aimlPath);
            aiml.isAcceptingUserInput = true;
            bot.OnMessage += Bot_OnMessage;
            bot.StartReceiving();
            Console.ReadLine();
        }

        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            string received = e.Message.Text;
            
            if (received == null)
                return;
            Console.WriteLine(received);

            string response = getOutput(received, aiml); ;
            if (response.Length == 0)
                response = "Не понял. У этой штуки точно два колеса?";

            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: response
                ).ConfigureAwait(false);
        }

        private static String getOutput(String input, Bot aiml)
        {
            Request r = new Request(input, user, aiml);
            Result res = aiml.Chat(r);
            return (res.Output);
        }
    }
}