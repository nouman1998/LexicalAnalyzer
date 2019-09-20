namespace LexicalAnalyzer
{
    public class Tokens
    {
        public string classKeyword;
        public string value;
        //public int wordNumber;
        public int lineNumber;

        public Tokens(string _classKeyword, string _value, int _lineNumber)
        {
            classKeyword = _classKeyword;
            value = _value;
            //wordNumber = _wordNumber;
            lineNumber = _lineNumber;
        }
    }
}