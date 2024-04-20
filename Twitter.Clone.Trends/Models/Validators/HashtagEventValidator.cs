namespace Twitter.Clone.Trends.Models.Validators;

public class HashtagEventValidator : AbstractValidator<HashtagsEvent>
{
    public HashtagEventValidator()
    {
        RuleFor(p => p.IPAddress)
            .NotEmpty().WithMessage("IP Address cannot be empty.")
            .NotNull().WithMessage("IP Address cannot be null.")
            .Must(BeAValidIPAddress).WithMessage("IP Address is invalid.");

        RuleFor(p => p.Hashtags)
            .NotNull().WithMessage("Hashtags list cannot be null")
            .NotEmpty().WithMessage("Hashtags list cannot be empty.")
            .Must(p => p.Count > 0).WithMessage("Hashtags list must contain at least one hashtag.")
            .ForEach(p => p.NotEmpty().WithMessage("Hashtag cannot be empty."));
    }

    private bool BeAValidIPAddress(string ipAddress)
    {
        string ipAddressPattern = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$|^([0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4}$";
        return Regex.IsMatch(ipAddress, ipAddressPattern);
    }
}
