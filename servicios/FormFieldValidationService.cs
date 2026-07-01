using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using BE;

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

        public static ValidationResult ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return new ValidationResult(false, "El nombre de usuario no puede estar vacio");

            bool isValid = UsernameRegex.IsMatch(username.Trim());
            if (isValid) return new ValidationResult(true);
            return new ValidationResult(false, "El nombre de usuario debe tener entre 3 y 16 caracteres y solo puede contener letras, números, guiones bajos y guiones");
        }

        public static ValidationResult ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return new ValidationResult(false, "El correo electrónico no puede estar vacío");

            bool isValid = EmailRegex.IsMatch(email.Trim());
            if (isValid) return new ValidationResult(true);
            return new ValidationResult(false, "El formato del correo electrónico no es válido");
        }

        public static ValidationResult ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return new ValidationResult(false, "El número de teléfono no puede estar vacío");

            bool isValid = PhoneRegex.IsMatch(phone.Trim());
            if (isValid) return new ValidationResult(true);
            return new ValidationResult(false, "El formato del número de teléfono no es válido");
        }

        public static ValidationResult ValidateOnlyLetters(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new ValidationResult(false, "El campo de texto no puede estar vacío");

            bool isValid = OnlyLettersRegex.IsMatch(text.Trim());
            if (isValid) return new ValidationResult(true);
            return new ValidationResult(false, "El campo solo puede contener letras y espacios (se permiten acentos y eñes)");
        }

        public static ValidationResult ValidateAlphaNumericStrict(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new ValidationResult(false, "El código o ID no puede estar vacío");

            bool isValid = AlphaNumericStrictRegex.IsMatch(text.Trim());
            if (isValid) return new ValidationResult(true);
            return new ValidationResult(false, "El campo solo puede contener letras (sin acentos) y números, sin espacios");
        }

        public static ValidationResult ValidateAlphaNumericWithSpaces(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new ValidationResult(false, "El texto no puede estar vacío");

            bool isValid = AlphaNumericWithSpacesRegex.IsMatch(text.Trim());
            if (isValid) return new ValidationResult(true);
            return new ValidationResult(false, "El campo solo puede contener letras, números y espacios (sin caracteres especiales)");
        }
    }
}