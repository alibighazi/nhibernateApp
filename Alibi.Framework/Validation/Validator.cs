using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alibi.Framework.Validation
{

    public interface IResult
    {
        string Message { get; }

        bool Succeeded { get; }

        IList<ValidationFailure> Errors { get; }
    }


    public class Result : IResult
    {
        protected Result() { }

        public string Message { get; set; }

        public bool Succeeded { get; set; }

        public IList<ValidationFailure> Errors { get; set; }

        public static IResult Fail()
        {
            return new Result { Succeeded = false };
        }

        public static IResult Fail(string message)
        {
            return new Result { Succeeded = false, Message = message };
        }


        public static IResult Fail(string message, IList<ValidationFailure> errors)
        {
            return new Result { Succeeded = false, Message = message, Errors = errors };
        }


        public static IResult Success()
        {
            return new Result { Succeeded = true };
        }

        public static IResult Success(string message)
        {
            return new Result { Succeeded = true, Message = message };
        }

    }



    public abstract class Validator<T> : AbstractValidator<T>
    {
        private string Message { get; set; }

        public new IResult Validate(T instance)
        {
            if (instance == null)
            {
                return Result.Fail(Message ?? string.Empty);
            }

            var result = base.Validate(instance);

            if (result.IsValid)
            {
                return Result.Success();
            }

            return Result.Fail(Message ?? result.ToString(), result.Errors);
        }

        public Task<IResult> ValidateAsync(T instance)
        {
            return Task.FromResult(Validate(instance));
        }

        public void WithMessage(string message)
        {
            Message = message;
        }
    }
}
