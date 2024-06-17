namespace football.Models
{
    public class TransferInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public int? value { get; set; }
        public string club { get; set; }
        public DateOnly? contract { get; set; }
        public string expireTime { get; set; }
        public TransferInfo(int id, string name, string position, int? value, string club, DateOnly? contract, string expireTime)
        {
            this.id = id;
            this.name = name;
            this.position = position;
            this.value = value;
            this.club = club;
            this.contract = contract;
            this.expireTime = expireTime;
        }
    }
    public class TransferDTO
    {
        public List<TransferInfo> transferInfos { get; set; }
        public TransferDTO()
        {
            transferInfos = new List<TransferInfo>();
        }
        public void add(TransferInfo transferInfo)
        {
            transferInfos.Add(transferInfo);
        }

    }
}
