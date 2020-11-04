using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionSysTB.Models
{
    public class Reaction
    {
        [Key]
        public int Key { get; set; }
        public int ReactionType { get; set; }
        public string Username { get; set; }
        public int MessageId { get; set; }
        public long ChatId { get; set; }
    }
}
