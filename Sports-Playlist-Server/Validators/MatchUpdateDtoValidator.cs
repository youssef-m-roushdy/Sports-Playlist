using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Sports_Playlist_Server.DTOs;

namespace Sports_Playlist_Server.Validators
{
    public class MatchUpdateDtoValidator : AbstractValidator<UpdateMatchDto>
    {
        public MatchUpdateDtoValidator()
        {
            RuleFor(m => m.Title)
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(m => m.Competition)
                .MaximumLength(100).WithMessage("Competition cannot exceed 100 characters.");

            RuleFor(m => m.MatchDate)
                .NotEmpty().WithMessage("Match date is required.");

            RuleFor(m => m.MatchUrl)
                .NotEmpty().WithMessage("Match Url is required.")
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("Match Url must be a valid URL.");
        }
    }
}