namespace football.Models
{
    public class Data
    {
        public int id { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public string clubScore { get; set; }
        public string nationScore { get; set; }
        public int? grade { get; set; }
        public int potentialGrade { get; set; }
        public Data(int id, string name, string position, string clubScore, string nationScore, int? grade, int potentialGrade)
        {
            this.id = id;
            this.name = name;
            this.position = position;
            this.clubScore = clubScore;
            this.nationScore = nationScore;
            this.grade = grade;
            this.potentialGrade = potentialGrade;
        }
    }
    public class DataDTO
    {
        public List<Data> tableData { get; set; }
        public DataDTO()
        {
            tableData = new List<Data>();
        }
        public void add(Data data)
        {
            tableData.Add(data);
        }
    }
}
