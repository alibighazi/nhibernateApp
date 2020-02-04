namespace Alibi.Framework.Exception
{
    public class CustomValidationException : System.Exception
    {
        public CustomValidationException(string result)
            : base(result)
        {
        }
    }
}