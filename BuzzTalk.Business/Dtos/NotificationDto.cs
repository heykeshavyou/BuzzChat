using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuzzTalk.Business.Dtos
{
    public class NotificationDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int GroupId {  get; set; }
        public int UserId { get ; set; }
    }
}
