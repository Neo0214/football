using football.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using football.Models;
using Microsoft.EntityFrameworkCore;

namespace football.Repos
{
    public class TrainRepo
    {
        private readonly FootballContext _context;
        public TrainRepo(FootballContext context)
        {
            _context = context;
        }

        public bool makeTrain(TrainDTO trainDTO, DateOnly date)
        {
            Player player = _context.Players.Find(trainDTO.playerId);
            Train train = new Train
            {
                Score = trainDTO.score,
                TrainDate = date,


            };
            _context.Trains.Add(train);

            train.Players.Add(player);

            return _context.SaveChanges() > 0;

        }

        public List<Train> getTrain(int playerId)
        {
            return _context.Trains.Include(t => t.Players).Where(t => t.Players.Any(p => p.PlayerId == playerId)).ToList();
        }

        public void addTrain(int playerId, int newTotalScore)
        {
            Player player = _context.Players.Find(playerId);
            player.Score = newTotalScore;
            _context.Update(player);
            _context.SaveChanges();
            return;
        }
    }
}
