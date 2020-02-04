using Alibi.Framework.Models;
using Alibi.Framework.Validation;
using FluentValidation;

namespace Server.Modules.Core.Common.Models
{
    public class Book : Entity
    {
        public virtual string Title { get; set; }
        public virtual SignInModelValidator Validate { get; set; }
    }


    public sealed class SignInModelValidator : Validator<Book>
    {
        public SignInModelValidator()
        {
            WithMessage("Error" );
            RuleFor(x => x.Title)
                .NotEmpty()
                .Matches(Regexes.Email);
        }
    }


    public sealed class BookValidator : Validator<Book>
    {
        public BookValidator()
        {
            WithMessage("Error");
            RuleFor(x => x.Title)
                .NotEmpty()
                .Matches(Regexes.Email);
        }
    }
}