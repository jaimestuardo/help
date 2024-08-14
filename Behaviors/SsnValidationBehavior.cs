using CommunityToolkit.Maui.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TimeApp.Behaviors
{
    public partial class SsnValidationBehavior : TextValidationBehavior
    {
        public static readonly BindableProperty Iso2CountryProperty = BindableProperty.Create(nameof(Iso2Country), typeof(string), typeof(SsnValidationBehavior), "cl");

        public string Iso2Country
        {
            get => (string)GetValue(Iso2CountryProperty);
            set => SetValue(Iso2CountryProperty, value);
        }

        public static bool ChileValidate(string docNumber)
        {
            string rut = CleanRegex().Replace(docNumber, string.Empty).ToUpper();

            if (rut.Length >= 2)
            {
                string dv = rut[^1].ToString();
                if (MatchRegex().IsMatch(dv))
                {
                    int largo = rut.Length;

                    if (largo > 10 || largo < 2) return false;

                    // La parte del RUT debe ser un número entero
                    if (int.TryParse(rut.AsSpan(0, largo - 1), out _))
                    {
                        // El dígito verificador debe ser un número o la K
                        if ((rut[largo - 1].CompareTo('0') < 0 || rut[largo - 1].CompareTo('9') > 0) && rut[largo - 1] != 'K')
                            return false;

                        // Realiza la operación módulo
                        int suma = 0;
                        int mul = 2;

                        // -2 porque rut contiene el dígito verificador, el cual no hay que considerar
                        for (int i = rut.Length - 2; i >= 0; i--)
                        {
                            suma += int.Parse(rut[i].ToString()) * mul;
                            if (mul == 7) mul = 2; else mul++;
                        }

                        int residuo = suma % 11;

                        char dvr;
                        if (residuo == 1)
                            dvr = 'K';
                        else if (residuo == 0)
                            dvr = '0';
                        else
                            dvr = (11 - residuo).ToString()[0];

                        return dvr.Equals(rut[^1]);
                    }
                }
            }

            return false;
        }

        protected override async ValueTask<bool> ValidateAsync(string value, CancellationToken token)
        {
            bool result = true;
            string iso2 = Iso2Country ?? string.Empty;
            switch (iso2)
            {
                case "cl":
                    result = ChileValidate(value);
                    break;
            }

            return result && await base.ValidateAsync(value, token);
        }

        [GeneratedRegex("[ .-]")]
        private static partial Regex CleanRegex();
        [GeneratedRegex("[0-9K]")]
        private static partial Regex MatchRegex();
    }
}
