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
                Status = matchDto.Status
            };
        }

        public static IEnumerable<MatchDto> FromMatchesToMatchesDto(this IEnumerable<Match> matches)
        {
            return matches.Select(match => new MatchDto
            {
                Id = match.Id,
                Title = match.Title,
                Competition = match.Competition,
                MatchDate = match.MatchDate,
                Status = match.Status
            });
        }

        public static MatchDto FromMatchToMatchDto(this Match match)
        {
            return new MatchDto
            {
                Id = match.Id,
                Title = match.Title,
                Competition = match.Competition,
                MatchDate = match.MatchDate,
                Status = match.Status
            };
        }

    }
}