using Microsoft.AspNetCore.Mvc.TagHelpers;
using QuestionSysTB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace QuestionSysTB.Markup
{

    public static class Keyboards
    {
        public static ReplyKeyboardMarkup QuestionTypekeyboard()
        {
            ReplyKeyboardMarkup m = new ReplyKeyboardMarkup()
            {
                ResizeKeyboard = true,
                Keyboard = new[]
                {
                    new KeyboardButton[]
                    {
                        new KeyboardButton()
                        {
                            Text = DefaultMessages.PayQuestion,
                        }
                    }
                }
            };
            return m;
        }
    }

    public static class InlineKeyboards
    {
        public static InlineKeyboardMarkup GetModerationMarkup(string from,long chatId)
        {
            var markup = new InlineKeyboardMarkup(new[]
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardButton()
                    {
                        Text = "Одобрить",
                        CallbackData = "approve|" + chatId
                    },
                    new InlineKeyboardButton()
                    {
                        Text = "Отклонить",
                        CallbackData = "deny|" + chatId,
                    }
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardButton()
                    {
                        Text = "Вопрос от " + from,
                        Url = "t.me/"+ from
                    },
                    
                },

            });
            return markup;
        }
        public static InlineKeyboardMarkup GetModerationMarkupPayQuestion(string from, long chatId)
        {
            var markup = new InlineKeyboardMarkup(new[]
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardButton()
                    {
                        Text = "Одобрить 💰",
                        CallbackData = "approvepay|" + chatId + "|" + from
                    },
                    new InlineKeyboardButton()
                    {
                        Text = "Отклонить",
                        CallbackData = "denypay|" + chatId,
                    }
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardButton()
                    {
                        Text = "Вопрос от " + from,
                        Url = "t.me/" + from
                    },

                },

            });
            return markup;
        }

        public static InlineKeyboardMarkup GetWaitPayment(string from, long chatId)
        {
            var markup = new InlineKeyboardMarkup(new[]
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardButton()
                    {
                        Text = "💰 ...Ждем... 💰",
                        CallbackData = "waitmoney|" + chatId,
                    }
                    
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardButton()
                    {
                        Text = "Вопрос от " + from,
                        Url = "t.me/" + from
                    },

                },

            });
            return markup;
        }

        public static InlineKeyboardMarkup LinkToChannel(string to)
        {
            var m = new InlineKeyboardMarkup(new InlineKeyboardButton[]
            {
               new InlineKeyboardButton()
               {
                   Text = "Перейти в канал",
                   Url = "t.me/" + to
               }
            });
            return m;
        }


        public static InlineKeyboardMarkup GetReactionKeyboard(int likes,int dislikes)
        {
            var markup = new InlineKeyboardMarkup(new[]
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardButton()
                    {
                        Text = likes +" 👍",
                        CallbackData = "like"
                    },
                    new InlineKeyboardButton()
                    {
                        Text = dislikes + " 👎",
                        CallbackData = "dislike"
                    }
                }
            });
            return markup;
        }
    }
}
