using Microsoft.EntityFrameworkCore;
using QuestionSysTB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionSysTB.Services
{
    public class ReactionService
    {
        ApplicationDbContext _dbContext;
        public ReactionService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SetReaction(string from, int messageId, long chatId, int reaction)
        {
            var react = _dbContext.Reactions.FirstOrDefault(m => m.ChatId == chatId && m.Username == from && m.MessageId == messageId);
            if (react == null)
            {
                react = new Models.Reaction() { ChatId = chatId, MessageId = messageId, ReactionType = reaction, Username = from };
                _dbContext.Reactions.Add(react);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                if (react.ReactionType != reaction)
                {
                    react.ReactionType = reaction;
                    _dbContext.Reactions.Update(react);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        public async Task<int[]> GetReactions(int messageId, long chatId)
        {
            var react = await _dbContext.Reactions.Where(m => m.ChatId == chatId && m.MessageId == messageId).ToListAsync();
            var amount = new int[2] { 0,0};
            foreach (var item in react)
            {
                amount[item.ReactionType] += 1;
            }
            return amount;
        }


    }
}
