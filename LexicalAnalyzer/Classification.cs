using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace LexicalAnalyzer
{
    class Classification
    {
        public static List<Tokens> tokenList = new List<Tokens>();
        public static string[,] keywords =
         {
            {"DT","int" },
            {"DT","char" },
            {"DT","float" },
            {"DT","string" },
            {"if","if" },
            {"else","else" },
            {"for","for" },
            {"while","while" },
            {"switch","switch" },
            {"case","case" },
            {"default","default" },
            {"break","break" },
            { "continue", "continue" },
            {"static","static" },
            { "return", "return" },
            {"void","void" },
            {"main","main" },
            {"access","public" },
            {"access","private" },
            {"protected","protected" },
            { "new", "new" },
            { "this", "this" },
            { "extend", "extend" },
            { "sealed", "sealed" },
            { "base", "base" },
            {"virtual","virtual" },
            {"override","override" },
            {"class","class" }
        };

        public string[,] puntuators =
       {
            { ";", ";" },
            {",", "," },
            {".", "." },
            {"(", "(" },
            {")", ")" },
            {"{", "{" },
            {"}", "}" },
            {"[", "[" },
            {"]", "]" },
            {":", ":" },
            {"\\","invalid" }
        };
        string[,] operators = {
            { "MDM", "*"},
            { "MDM", "/" },
            { "MDM", "%" },
            { "PM", "+" },
            { "PM", "-" },
            { "RO", "<" },
            { "RO", ">" },
            { "RO", "<=" },
            { "RO", ">=" },
            { "RO", "!=" },
            { "RO", "==" },
            { "AO", "&&" },
            { "AO", "||" },
            { "NOT", "!"},
            { "assign", "=" },
            { "compAss", "-="},
            { "compAss", "+="},
            { "compAss", "*="},
            { "compAss", "/="},
            { "compAss", "%="},
            { "inc", "++"},
            {"dec", "--" },
        };
        public static List<string> compoundOperators = new List<string>(new string[] { "<=", ">=", "!=", "==", "&&", "||", "-=", "+=", "*=", "/=", "%=", "++", "--" });

        public string isKeyword(string input)
        {
            string CP = "";

            for (int i = 0; i < keywords.Length / 2; i++)
            {
                if (keywords[i, 1] == input)
                {
                    CP = keywords[i, 0]; break;
                }
            }


            return CP;

        }
        public string isOperator(string input)
        {
            string CP = "";

            for (int i = 0; i < operators.Length / 2; i++)
            {
                if (operators[i, 1] == input)
                {
                    CP = operators[i, 0]; break;
                }
            }


            return CP;

        }
        bool isIdentifier(string word)
        {
            bool status = false;
            Regex reg = new Regex("^[A-Za-z_][A-Za-z0-9_]{0,30}$");
            //Regex reg1 = new Regex("[A-Za-z0-9]$");
            if (reg.IsMatch(word))
            {
                status = true;
            }
            return status;
        }

        public string isPunc(string input)
        {
            string CP = "";

            for (int i = 0; i < puntuators.Length / 2; i++)
            {
                if (puntuators[i, 1] == input)
                {
                    CP = puntuators[i, 0]; break;
                }
            }


            return CP;

        }


        bool isInt(string input)
        {
            Regex reg = new Regex("^[+|-]?[0-9]{1,7}$");
            return reg.IsMatch(input);
        }

        bool isFloat(string input)
        {
            Regex reg = new Regex("^[+-]?[0-9]{0,7}.[0-9]{1,7}$");
            return reg.IsMatch(input);
        }
        public bool isChar(string input)
        {
            bool status = false;
            Regex reg = new Regex("^\'[\\\\][\\\\\'\"ntr]\'$");
            Regex reg1 = new Regex("^\'[^\\\\\"\']\'$");
            if (reg.IsMatch(input) || reg1.IsMatch(input))
            {
                status = true;
            }
            return status;
        }
        string getClass(string tokenValue)
        {
            string classPart = "";
            if (isIdentifier(tokenValue))
            {
                classPart = isKeyword(tokenValue);
                if (classPart == "")
                    classPart = "ID";
            }
            else
            {
                classPart = isPunc(tokenValue);
                if (classPart == "")
                {
                    classPart = isOperator(tokenValue);
                    if (classPart == "")
                    {

                        classPart = "invalid_token";
                    }
                }
            }
            return classPart;
        }

        public bool isString(string input)
        {
            bool status = true;
            Regex reg = new Regex("^[\\\\][\"\\\\ntr]$");
            Regex reg1 = new Regex("^[^\"\\\\]$");

            if (input[0] != '\"' && input[input.Last()] != '\"')
            {
                return false;
            }
            if (input.Length >= 2)
            {


                input = input.Substring(1, input.Length - 2); ;
                for (int i = 0; i < input.Length; i++)
                {
                    char a = input[i];
                    if (reg1.IsMatch(a.ToString()))
                    {
                        status = true;
                        continue;
                    }
                    else
                    {
                        if (a == '\\' && i + 1 < input.Length)
                        {
                            string temp = a.ToString() + input[++i].ToString();
                            if (reg.IsMatch(temp))
                            {
                                status = true;
                                continue;
                            }
                        }
                        else
                        {
                            status = false;
                        }
                    }
                }
            }
            else
            {
                status = false;
                
            }
            return status;

        }

        public void switch_case()
        {
            string test = "";
            string firstWord = "";
            Classification c1 = new Classification();

            foreach (var token in Classification.tokenList)
            {

                firstWord = token.value[0].ToString();

                if (char.IsDigit(token.value[0]))
                {
                    firstWord = "num";
                }
                switch (firstWord)
                {
                    case "+":
                    case "-":
                    case ".":
                    case "num":

                        if (c1.isInt(token.value))
                        {
                            token.classKeyword = "int_const";
                        }
                        else if (c1.isFloat(token.value))
                        {
                            token.classKeyword = "float_const";
                        }
                        else {
                            token.classKeyword = getClass(token.value);

                        }
                        break;
                    case "\"":
                        if (c1.isString(token.value))
                        {
                            token.classKeyword = "string_const";
                        }
                        else
                        {
                            token.classKeyword = "Invalid";
                        }
                        break;
                    case "\'":
                        if (c1.isChar(token.value))
                        {
                            token.classKeyword = "char_const";
                        }
                        else
                        {
                            token.classKeyword = "Invalid";
                        }
                        break;
                    case "_":
                        if (c1.isIdentifier(token.value))
                        {
                            token.classKeyword = "ID";
                        }
                        else
                        {
                            token.classKeyword = "Invalid";
                        }
                        break;

                    default:
                        token.classKeyword = c1.getClass(token.value);
                        break;
                }
            }


        }




    }
}
