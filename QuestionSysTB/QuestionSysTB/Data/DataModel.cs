using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionSysTB.Data
{
    public class DataModel
    {
        public long ModerationId { get; set; } = -1;
        public HashSet<long> DiscussionList { get; set; } = new HashSet<long>();
        public HashSet<long> PublishChannelList { get; set; } = new HashSet<long>();
        public HashSet<string> Admins { get; set; } = new HashSet<string>() { "Invectys","smokeadm" };


    }
}
