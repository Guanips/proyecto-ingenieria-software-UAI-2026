using BE;
using System.Text.RegularExpressions;

namespace servicios
{
    public static class FormFieldValidationService
    {
        private static readonly Regex UsernameRegex = new Regex(@"^[a-zA-Z0-9_-]{3,16}$", RegexOptions.Compiled);
        private static readonly Regex EmailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled);
        private static readonly Regex PhoneRegex = new Regex(@"^\+?[0-9]{7,15}$", RegexOptions.Compiled);

        private static readonly Regex OnlyLettersRegex = new Regex(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ ]+$", RegexOptions.Compiled);
        private static readonly Regex AlphaNumericStrictRegex = new Regex(@"^[a-zA-Z0-9]+$", RegexOptions.Compiled);
        private static readonly Regex AlphaNumericWithSpacesRegex = new Regex(@"^[a-zA-Z0-9ñÑáéíóúÁÉÍÓÚüÜ ]+$", RegexOptions.Compiled);
        private static readonly Regex ProfileNameRegex = new Regex(@"^PERF-[^\s]+$", RegexOptions.Compiled);

        public static ValidationResult ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return new ValidationResult(false, "err_UsernameVacio");

            bool isValid = UsernameRegex.IsMatch(username.Trim());
            if (isValid) return new ValidationResult(true);

            return new ValidationResult(false, "err_UsernameFormato");
        }

        public static ValidationResult ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return new ValidationResult(false, "err_EmailVacio");

            bool isValid = EmailRegex.IsMatch(email.Trim());
            if (isValid) return new ValidationResult(true);

            return new ValidationResult(false, "err_EmailFormato");
        }

        public static ValidationResult ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return new ValidationResult(false, "err_PhoneVacio");

            bool isValid = PhoneRegex.IsMatch(phone.Trim());
            if (isValid) return new ValidationResult(true);

            return new ValidationResult(false, "err_PhoneFormato");
        }

        public static ValidationResult ValidateOnlyLetters(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new ValidationResult(false, "err_OnlyLettersVacio");

            bool isValid = OnlyLettersRegex.IsMatch(text.Trim());
            if (isValid) return new ValidationResult(true);

            return new ValidationResult(false, "err_OnlyLettersFormato");
        }

        public static ValidationResult ValidateAlphaNumericStrict(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new ValidationResult(false, "err_AlphaNumStrictVacio");

            bool isValid = AlphaNumericStrictRegex.IsMatch(text.Trim());
            if (isValid) return new ValidationResult(true);

            return new ValidationResult(false, "err_AlphaNumStrictFormato");
        }

        public static ValidationResult ValidateAlphaNumericWithSpaces(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new ValidationResult(false, "err_AlphaNumSpacesVacio");

            bool isValid = AlphaNumericWithSpacesRegex.IsMatch(text.Trim());
            if (isValid) return new ValidationResult(true);

            return new ValidationResult(false, "err_AlphaNumSpacesFormato");
        }

        public static ValidationResult ValidateProfileName(string profileName)
        {
            if (string.IsNullOrWhiteSpace(profileName)) return new ValidationResult(false, "El nombre del perfil no puede estar vacío");

            bool isValid = ProfileNameRegex.IsMatch(profileName.Trim());
            if (isValid) return new ValidationResult(true);
            return new ValidationResult(false, "El nombre del perfil debe comenzar con 'PERF-' seguido de caracteres (sin espacios)");
        }
    }
}