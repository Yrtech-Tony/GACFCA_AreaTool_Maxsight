using FIAT.API.Models.CalenderMngDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIAT.API.Models.HomeDto
{
    public class ItemResultDto
    {
        public ItemResultDto()
        {
            FirstItemList = new List<EachDayTypeDto>();
            SecondItemList = new List<DoItemDto>();
            ThirdItemList = new List<DoItemDto>();
        } 
        public string  Id { get; set; }
        public List<EachDayTypeDto> FirstItemList { get; set; }
        public List<DoItemDto> SecondItemList { get; set; }
        public List<DoItemDto> ThirdItemList { get; set; }
    }
}
