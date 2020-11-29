namespace Compiler
{
    public class Token
    {
        /// <summary>
        /// Model class for token
        /// </summary>
        /// <param name="classPart"></param>
        /// <param name="value"></param>
        /// <param name="lineNumber"></param>
        public Token(ClassPart classPart, string value, int lineNumber)
        {
            ClassPart = classPart;
            Value = value;
            LineNumber = lineNumber;
        }

        public ClassPart ClassPart { get; set; }
        public string Value { get; set; }
        public int LineNumber { get; set; }

        public override string ToString()
        {
            return $"CLASS PART: {ClassPart.ToString()}\r\nVALUE: {Value}\r\nLINE NUMBER: {LineNumber}\r\n";
        }
    }
}