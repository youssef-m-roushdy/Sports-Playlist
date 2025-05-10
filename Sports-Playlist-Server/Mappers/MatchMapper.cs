using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sports_Playlist_Server.DTOs;
using Sports_Playlist_Server.Models;

namespace Sports_Playlist_Server.Mappers
{
    public static class MatchMapper
    {
        public static Match FromCreateMatchDtoToMatch(this CreateMatchDto matchDto)
        {
            return new Match
            {
                Title = matchDto.Title,
                Competition = matchDto.Competition,
                MatchDate = matchDto.MatchDate,
                MatchUrl = matchDto.MatchUrl,
                Status = matchDto.Status
            };
        }
    }
}