using football.Models;
namespace football.Services
{
    public interface ITrain
    {
        string makeTrain(TrainDTO train);
        TrainDataDTO getTrain(int playerId);
    }
}
