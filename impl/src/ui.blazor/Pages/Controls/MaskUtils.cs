using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Works.Pages.Controls
{
    /* 
* Formatar CNPJ, CPF e Retirar a Formatação
* 
* Visite nossa página http://www.codigoexpresso.com.br
* 
* by Antonio Azevedo
*  
* public static string FormatCNPJ(string CNPJ)
*        Formata uma string CNPJ 
*        Informar uma string sem formatacao com o codigo do CNPJ 
*        Exemplo '11222333000181' 
*
* public static string FormatCPF(string CPF)
*        Formata uma string CPF 
*        Informar uma string sem formatacao com o codigo do CPF 
*        Exemplo '01122233344'
*        
* public static string SemFormatacao(string Codigo)
*        Retira formatacao de uma string CNPJ ou CPF 
*        Informar uma string formatada com o codigo do CNPJ ou CPF 
*        Exemplo '99.999.999/9999-99' ou '111.222.333-44'
*        
*/
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;

    public static class FormatCnpjCpf
    {
        /// <summary>
        /// Formatar uma string CNPJ
        /// </summary>
        /// <param name="CNPJ">string CNPJ sem formatacao</param>
        /// <returns>string CNPJ formatada</returns>
        /// <example>Recebe '99999999999999' Devolve '99.999.999/9999-99'</example>

        public static string FormatCNPJ(string CNPJ)
        {
            return Convert.ToUInt64(CNPJ).ToString(@"00\.000\.000\/0000\-00");
        }

        /// <summary>
        /// Formatar uma string CPF
        /// </summary>
        /// <param name="CPF">string CPF sem formatacao</param>
        /// <returns>string CPF formatada</returns>
        /// <example>Recebe '99999999999' Devolve '999.999.999-99'</example>

        public static string FormatCPF(string CPF)
        {
            if (CPF.IsNullOrWhiteSpace()) return string.Empty;
            if (!IsCpfCnpj(CPF)) return string.Empty;
            var value = SemFormatacao(CPF);
            if (value.IsNullOrWhiteSpace()) return string.Empty;
            return Convert.ToUInt64(value).ToString(@"000\.000\.000\-00");
        }
        /// <summary>
        /// Retira a Formatacao de uma string CNPJ/CPF
        /// </summary>
        /// <param name="Codigo">string Codigo Formatada</param>
        /// <returns>string sem formatacao</returns>
        /// <example>Recebe '99.999.999/9999-99' Devolve '99999999999999'</example>

        public static string SemFormatacao(string Codigo)
        {
            if (Codigo.IsNullOrEmpty()) return string.Empty;
            return Codigo.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
        }
        // o metodo IsCpfcnpj recebe dois parâmetros:
        // uma string contendo o cpf ou cnpj a ser validado
        // e um valor do tipo boolean, indicando se o método irá
        // considerar como válido um cpf ou cnpj em branco.
        // o retorno do método também é do tipo boolean:
        // (true = cpf ou cnpj válido; false = cpf ou cnpj inválido)
        public static bool IsCpfCnpj(string cpfcnpj)
        {
            if (string.IsNullOrEmpty(cpfcnpj))
                return false;
            else
            {
                int[] d = new int[14];
                int[] v = new int[2];
                int j, i, soma;
                string Sequencia, SoNumero;

                SoNumero = Regex.Replace(cpfcnpj, "[^0-9]", string.Empty);

                //verificando se todos os numeros são iguais
                if (new string(SoNumero[0], SoNumero.Length) == SoNumero) return false;

                // se a quantidade de dígitos numérios for igual a 11
                // iremos verificar como CPF
                if (SoNumero.Length == 11)
                {
                    for (i = 0; i <= 10; i++) d[i] = Convert.ToInt32(SoNumero.Substring(i, 1));
                    for (i = 0; i <= 1; i++)
                    {
                        soma = 0;
                        for (j = 0; j <= 8 + i; j++) soma += d[j] * (10 + i - j);

                        v[i] = (soma * 10) % 11;
                        if (v[i] == 10) v[i] = 0;
                    }
                    return (v[0] == d[9] & v[1] == d[10]);
                }
                // se a quantidade de dígitos numérios for igual a 14
                // iremos verificar como CNPJ
                else if (SoNumero.Length == 14)
                {
                    Sequencia = "6543298765432";
                    for (i = 0; i <= 13; i++) d[i] = Convert.ToInt32(SoNumero.Substring(i, 1));
                    for (i = 0; i <= 1; i++)
                    {
                        soma = 0;
                        for (j = 0; j <= 11 + i; j++)
                            soma += d[j] * Convert.ToInt32(Sequencia.Substring(j + 1 - i, 1));

                        v[i] = (soma * 10) % 11;
                        if (v[i] == 10) v[i] = 0;
                    }
                    return (v[0] == d[12] & v[1] == d[13]);
                }
                // CPF ou CNPJ inválido se
                // a quantidade de dígitos numérios for diferente de 11 e 14
                else return false;
            }
        }
    }
}
