using System;
using System.Linq;
using FluentValidation;
using Sports_Playlist_Server.DTOs;
using Sports_Playlist_Server.Enums;

namespace Sports_Playlist_Server.Validators
{
    public class MatchCreateDtoValidator : AbstractValidator<CreateMatchDto>
    {
        public MatchCreateDtoValidator()
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