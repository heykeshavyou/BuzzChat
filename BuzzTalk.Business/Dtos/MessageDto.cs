using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuzzTalk.Business.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }

        public int? FromId { get; set; }

        public int? ToId { get; set; }

        public string Content { get; set; } = null!;

        public DateTime? SentOn { get; set; }
        public bool IsRead { get; set; }
        public int? GroupId { get; set; }



        public virtual UserDto? From { get; set; }

        public virtual UserDto? To { get; set; }
    }
}
