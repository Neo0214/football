namespace football.Models
{
    public class UserInfoDTO
    {
        public string name { get; set; }
        public string tele { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public int clubId { get; set; }
        public UserInfoDTO(string name, string tele, string email, string address, int clubId)
        {
            this.name = name;
            this.tele = tele;
            this.email = email;
            this.address = address;
            this.clubId = clubId;
        }
    }
    public class History
    {
        public string title { get; set; }
        public string content { get; set; }
        public DateOnly? startYear { get; set; }
        public DateOnly? endYear { get; set; }
        public History(string title, string content, DateOnly? startYear, DateOnly? endYear)
        {
            this.title = title;
            this.content = content;
            this.startYear = startYear;
            this.endYear = endYear;
        }
    }
    public class UserHistoryListDTO
    {
        public List<History> histories { get; set; }
        public UserHistoryListDTO()
        {
            histories = new List<History>();
        }
        public void add(History history)
        {

            histories.Add(history);
        }
    }

}
