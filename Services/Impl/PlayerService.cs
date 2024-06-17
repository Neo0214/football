using football.Entities;
using football.ExLibs;
using football.Models;
using football.Repos;
using football.Services;
namespace football.Services.Impl
{
    public class PlayerService : IPlayer
    {
        public readonly PlayerRepo _repo;
        public readonly ClubRepo _clubRepo;
        public PlayerService(PlayerRepo repo, ClubRepo clubRepo)
        {
            _repo = repo;
            _clubRepo = clubRepo;
        }
        public TransferDTO getAllPlayerTransfer()
        {
            List<Player> players = _repo.getAll();
            TransferDTO output = new TransferDTO();
            foreach (Player player in players)
            {
                string expireTime = calcExpireTime(player.ExpireDate);
                output.add(new TransferInfo(player.PlayerId, player.Name, player.Position, player.Value, player.Clubs.First().ClubName, player.ExpireDate, expireTime));

            }
            return output;
        }
        private string calcExpireTime(DateOnly? end)
        {
            if (end == null)
            {
                return "无有效期";
            }

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            if (end.Value < today)
            {
                return "已过期";
            }

            int totalMonths = ((end.Value.Year - today.Year) * 12) + end.Value.Month - today.Month;
            int years = totalMonths / 12;
            int months = totalMonths % 12;

            if (years == 0)
            {
                return $"{months}个月";
            }
            else
            {
                return $"{years}年{months}个月";
            }
        }

        public DataDTO getAllPlayerData()
        {
            List<Player> players = _repo.getAll();
            DataDTO output = new DataDTO();
            foreach (Player player in players)
            {
                output.add(new Data(player.PlayerId, player.Name, player.Position, generateClubScoreString(player), generateNationScoreString(player.Nations.First()), player.Score, calcPotentionScore(player)));
            }
            return output;
        }
        private string generateClubScoreString(Player player)
        {
            string output = "";
            output += player.Clubs.First().ClubName;
            output += "-";
            output += player.Clubs.First().Score;
            return output;
        }
        private string generateNationScoreString(Nation nation)
        {
            string output = "";
            output += nation.NationName;
            output += "-";
            output += nation.Score;
            return output;
        }
        private int calcPotentionScore(Player player)
        {
            int? nationScore = player.Nations.First().Score;
            int? clubScore = player.Clubs.First().Score;
            return CalcPotential(player.Shot, player.Pass, player.Dribble, player.Defence, player.Energy, player.Vel, nationScore, clubScore);
        }
        public int CalcPotential(int? shot, int? pass, int? dribble, int? defence, int? energy, int? vel, int? nationScore, int? clubScore)
        {
            // 权重
            double w_pass = 0.10;
            double w_shot = 0.10;
            double w_dribble = 0.10;
            double w_defence = 0.10;
            double w_energy = 0.10;
            double w_vel = 0.10;
            double w_nationScore = 0.15;
            double w_clubScore = 0.25;

            // 计算非线性得分
            double nonlinearScore = Math.Pow(pass ?? default, w_pass) *
                                    Math.Pow(shot ?? default, w_shot) *
                                    Math.Pow(dribble ?? default, w_dribble) *
                                    Math.Pow(defence ?? default, w_defence) *
                                    Math.Pow(energy ?? default, w_energy) *
                                    Math.Pow(vel ?? default, w_vel) *
                                    Math.Pow(nationScore ?? default, w_nationScore) *
                                    Math.Pow(clubScore ?? default, w_clubScore);

            // Sigmoid函数参数
            double k = 0.05;
            double threshold = 100;

            // 使用Sigmoid函数进行标准化处理
            double potentialScore = 100 / (1 + Math.Exp(-k * (nonlinearScore - threshold)));

            // 将潜力分数限制在0到100之间，并转换为整数
            return (int)Math.Round(potentialScore);
        }
        public string getPlayerPic(int playerId)
        {
            FileHandler fileHandler = new FileHandler();
            return fileHandler.getPlayerPicById(playerId);
        }
    }
}
