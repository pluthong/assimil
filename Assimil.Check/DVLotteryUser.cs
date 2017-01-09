using System;
namespace Assimil
{
    public class DVLotteryUser
    {
        public string REG { get; set; }
        public int CN { get; set; }
        public string FCN { get; set; }
        public string CON { get; set; }
        public string Status { get; set; }
        public DateTime SubmitDate { get; set; }
        public DateTime StatusDate { get; set; }
        public int FamilyMembers { get; set; }
        public int ISSUED { get; set; }
        public int REFUSED { get; set; }
        public int AP { get; set; }
        public int READY { get; set; }
        public int TRANSFER { get; set; }
        public DateTime UpdateDt { get; set; }
    }
}
