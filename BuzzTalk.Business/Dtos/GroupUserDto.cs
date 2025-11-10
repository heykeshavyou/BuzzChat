using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuzzTalk.Business.Dtos
{
    public class GroupUserDto
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public int UserId { get; set; }

        public bool? Isjoined { get; set; }

    }
}
