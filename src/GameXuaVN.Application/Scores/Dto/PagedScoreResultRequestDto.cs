using Abp.Application.Services.Dto;

namespace GameXuaVN.Scores.Dto
{
    public class PagedScoreResultRequestDto : PagedResultRequestDto
    {
        public int GameId { get; set; }
        public ScoreType? Type { get; set; }
        public string Keyword { get; set; }
    }

}
