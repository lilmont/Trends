namespace Twitter.Clone.Trends.Models.Validators;

public class HashtagEventValidator : AbstractValidator<HashtagsEvent>
{
    public HashtagEventValidator()
    {
        RuleFor(p => p.IPAddress)
            .NotEmpty().WithMessage("{IP Address} cannot be empty.")
            .NotNull().WithMessage("IP Address cannot be null.")
            .Must(ip => IPAddress.TryParse(ip, out _)).WithMessage("IP Address is invalid.");

        RuleFor(p => p.Hashtags)
            .NotNull().WithMessage("Hashtags list cannot be null")
            .NotEmpty().WithMessage("Hashtags list cannot be empty.")
            .Must(p => p.Count > 0).WithMessage("Hashtags list must contain at least one hashtag.")
            .ForEach(p => p.NotEmpty().WithMessage("Hashtag cannot be empty."));
    }
}
