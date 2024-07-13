using football.Entities;
using football.Models;
using football.Repos;
using System.Drawing.Imaging;
using football.Services;
using System.Drawing;
using System.Runtime.InteropServices;
namespace football.Services.Impl
{
    public class PlayerService : IPlayer
    {
        [DllImport("JPGEncoder.dll")]
        extern static void convertJPG(byte[] data, int x, int y, string filePath);

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
        public UploadDTO addPlayer(AddPlayerDTO playerDTO)
        {


            int newId = _repo.addPlayer(playerDTO);
            if (newId != -1)
            {
                // 添加成功
                return new UploadDTO
                {
                    playerId = newId,
                    success = true
                };
            }
            return new UploadDTO
            {
                playerId = -1,
                success = false
            };

        }
        public PicOkDTO uploadPlayerPic(IFormFile file, int playerId)
        {
            var memoryStream = new MemoryStream();
            file.CopyToAsync(memoryStream);
            byte[] fileBytes = memoryStream.ToArray();
            // to-do
            int width = 0, height = 0;
            byte[] picContent = readContentWithRGBA(file, ref width, ref height);
            convertJPG(picContent, width, height, "tmp.jpg");
            // next
            byte[] writeIn = File.ReadAllBytes("tmp.jpg");
            if (_repo.addPlayerPic(writeIn, playerId))
            {
                return new PicOkDTO(true);
            }
            return new PicOkDTO(false);

        }

        private byte[] readContentWithRGBA(IFormFile file, ref int cutWidth, ref int cutHeight)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Copy the uploaded file stream to a memory stream
                file.OpenReadStream().CopyTo(memoryStream);

                // Reset the memory stream position to the beginning
                memoryStream.Seek(0, SeekOrigin.Begin);

                // Load the image from the memory stream
                using (Bitmap bitmap = new Bitmap(memoryStream))
                {
                    int width = bitmap.Width;
                    int height = bitmap.Height;

                    // Calculate new width and height that are multiples of 8
                    int newWidth = (width / 8) * 8;
                    int newHeight = (height / 8) * 8;
                    cutWidth = newWidth;
                    cutHeight = newHeight;

                    // Create a new Bitmap with the adjusted dimensions
                    Bitmap adjustedBitmap = new Bitmap(bitmap, newWidth, newHeight);

                    // Lock the bits of the bitmap
                    BitmapData bitmapData = adjustedBitmap.LockBits(
                        new Rectangle(0, 0, adjustedBitmap.Width, adjustedBitmap.Height),
                        ImageLockMode.ReadOnly,
                        PixelFormat.Format32bppArgb);

                    // Create byte array for RGBA data
                    int bytesPerPixel = 4; // RGBA = 4 bytes
                    int byteCount = bitmapData.Stride * bitmapData.Height;
                    byte[] rgbaValues = new byte[byteCount];

                    // Copy the locked bytes from memory
                    Marshal.Copy(bitmapData.Scan0, rgbaValues, 0, byteCount);

                    // Unlock the bits
                    adjustedBitmap.UnlockBits(bitmapData);

                    // Convert ARGB to RGBA order
                    byte[] rgbaOrderedValues = new byte[byteCount];
                    for (int i = 0; i < byteCount; i += 4)
                    {
                        rgbaOrderedValues[i] = rgbaValues[i + 2];     // R
                        rgbaOrderedValues[i + 1] = rgbaValues[i + 1]; // G
                        rgbaOrderedValues[i + 2] = rgbaValues[i];     // B
                        rgbaOrderedValues[i + 3] = rgbaValues[i + 3]; // A
                    }

                    return rgbaOrderedValues;
                }
            }
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
            return (int)Math.Round(potentialScore * 3);
        }
        public byte[] getPlayerPic(int playerId)
        {
            return _repo.getPlayerPic(playerId);
        }

        public PlayerDetailDTO getPlayerDetail(int playerId)
        {
            PlayerDetailDTO output = new PlayerDetailDTO();
            Player player = _repo.getPlayer(playerId);
            output.position = player.Position;
            output.nation = player.Nations.First().NationId;
            output.name = player.Name;
            output.valueData.Add(player.Vel);
            output.valueData.Add(player.Shot);
            output.valueData.Add(player.Pass);
            output.valueData.Add(player.Dribble);
            output.valueData.Add(player.Defence);
            output.valueData.Add(player.Energy);
            return output;
        }
        public MyPlayerDTO getMyPlayer(int clubId)
        {
            MyPlayerDTO myPlayer = new MyPlayerDTO();
            List<Player> players = _repo.getPlayerWithClub(clubId);
            foreach (Player player in players)
            {
                myPlayer.options.Add(new MyPlayer() { value = player.PlayerId, label = player.Name });
            }
            return myPlayer;
        }
    }
}
