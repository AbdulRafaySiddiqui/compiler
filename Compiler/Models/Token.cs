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

        public string ToCSVString()
        {
            return $"{LineNumber},{ClassPart},{Value}\r\n";
        }
        public override string ToString()
        {
            return $"LINE NUMBER: {LineNumber}\t\tCLASS PART: {ClassPart}\t\t\t\t\t\tVALUE: {Value}\r\n";
        }
    }
}