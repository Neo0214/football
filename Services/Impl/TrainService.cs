using football.Repos;
using football.Services;
using football.Models;
using football.Entities;
using Scorelib;
namespace football.Services.Impl
{
    public class TrainService : ITrain
    {
        private readonly TrainRepo _trainRepo;
        public TrainService(TrainRepo trainRepo)
        {
            _trainRepo = trainRepo;
        }

        public string makeTrain(TrainDTO train)
        {
            // 获取当前时间
            DateTime dateTime = DateTime.Now;
            DateOnly date = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
            //ScoreHandler handler = new ScoreHandler();
            //int newTotalScore = handler.getScore(train.score, 50, 1);
            int newTotalScore = train.score + 1;
            _trainRepo.addTrain(train.playerId, newTotalScore);
            return _trainRepo.makeTrain(train, date) == true ? "success" : "fail";
        }

        public TrainDataDTO getTrain(int playerId)
        {
            TrainDataDTO trainData = new TrainDataDTO();
            List<Train> trains = _trainRepo.getTrain(playerId);
            foreach (Train train in trains)
            {
                trainData.addOne(train.TrainDate, train.Score);
            }
            return trainData;
        }
    }
}
