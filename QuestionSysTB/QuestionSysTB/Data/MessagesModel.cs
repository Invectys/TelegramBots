using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionSysTB.Data
{
    public static class DefaultMessages
    {
        //Buttons
        public const string PayQuestion = "Отправить вопрос с закрепом";


        //
        public const string AllPublRemoved = "Все публикационный каналы удалены";
        public const string AllDiscRemoved = "Все дискуссионные чаты удалены";
        public const string AdminRemoved = "Админ удален";
        public const string AllRemoved = "Все удалено";
        public const string PublRemoved = "Публикационный канал удален";
        public const string DiscRemoved = "Дискуссионный чат удален";
        public const string MessageEdited = "Сообщение изменино";
        public const string WrongFormat = "Неверный формат";
        public const string DiscAdded = "Дискуссионный чат добавлен";
        public const string AlreadyDiscAdded = "Этот дискуссионный чат уже в списке";
        public const string PublishAdded = "Публикационный канал добавлен";
        public const string AlreadyPublishAdded = "Этот публикационный канал уже в списке";
        public const string AdminAdded = "Админ добавлен";
        public const string AdminAlreadyAdded = "Админ уже есть в списке";
        public const string ModerationRemoved = "Модерационный канал удален";
        public static readonly string[] AllMessages =
        {
            "Привет. Теперь можно писать",
            "В канале @yasprosil задан новый вопрос, помогите с ответом 👌",
            "Вопрос отклонен по правилам модерации",
            "Я запомнил твой вопрос Он передан на модерацию Ожидайте",
            "Теперь можешь написать крайне важный вопрос",
            "Мы приняли платный вопрос Теперь он на модерации",
            "Извините, ваш вопрос не одобрен к закреплению, но вы можете подать его в обычно виде и возможно он будет одобрен"
        };
    }

    public static class DefaultMessagesKeys
    {
        public static readonly string[] AllKeys = 
        {
            "start",
            "discpublish",
            "questiondeny",
            "newquest",
            "choosepayq1",//4
            "newpayquest" ,
            "denypayquest" //6

        };

    }

    public class MessagesModel
    {
        public Dictionary<string,string> Messages { get; set; }

        

        public MessagesModel()
        {
            //Init default messages
            Messages = new Dictionary<string, string>();
            for(int i = 0; i < DefaultMessagesKeys.AllKeys.Length;i++)
            {
                Messages.Add(DefaultMessagesKeys.AllKeys[i], DefaultMessages.AllMessages[i]);
            }
        }
    }
}
