using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionSysTB.Models
{
    public class ApplicationBill
    {
        [Key]
        public int Id { get; set; }
        public int MessagesId { get; set; }
        public string Username { get; set; }
        public string BillId { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
