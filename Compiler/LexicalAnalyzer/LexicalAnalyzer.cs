using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Compiler
{
    /// <summary>
    /// This class only deals with the lexical logic all grammar is separated in another class
    /// </summary>
    public class LexicalAnalyzer
    {
        public LexicalAnalyzer()
        {
            Grammar = new Grammar();
        }

        public Grammar Grammar { get; set; }

        /// <summary>
        /// Tokenize the code according to the available grammar
        /// </summary>
        /// <param name="rawText"></param>
        /// <returns></returns>
        public List<Token> Analyze(string rawText)
        {
            var tokens = new List<Token>();

            //main index to track the current position
            int index = 0;
            int line = 1;
            string text = rawText;
            int lenth = rawText.Length;
            while (index < lenth)
            {
                //Remove spaces,tabs and new line from front of string and increase the current index
                var items = TrimStart(text);
                line += items.Item3;
                var count = items.Item1 + items.Item2 + items.Item3;
                index += count;
                text = text.Substring(count);

                //if we are at the end of text
                if (string.IsNullOrEmpty(text))
                    break;

                Token token;

                //check keyword
                token = IsKeyword(text, line);

                //check punctuators
                if (token == null)
                    token = IsPunctuator(text, line);

                //Check string
                if (token == null)
                    token = IsString(text, line);

                //Check comment
                if (token == null)
                {
                    var val = IsComment(text, line);
                    token = val.Item1;
                    line += val.Item2;
                }

                //check operator
                if (token == null)
                    token = IsOperator(text, line);

                if (token == null)
                {
                    //now we have to break the word in order to validate it as int,string,double, bool or identifier
                    var word = BreakWord(text);
                    token = IsInt(word, line);
                    if (token == null)
                        token = IsDouble(word, line);
                    if (token == null)
                        token = IsBool(word, line);
                    if (token == null)
                        token = IsIdentifier(word, line);
                    if (token == null)
                        token = new Token(ClassPart.INVALID, word, line);

                }


                //increase current index
                index += token.Value.Length;
                //remove the token from text
                text = text.Substring(token.Value.Length);
                //add token
                tokens.Add(token);
            }

            return tokens;
        }

        private (int, int, int) TrimStart(string text)
        {
            int spaceCount = 0;
            int lineCount = 0;
            int tabCount = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ' ')
                {
                    spaceCount++;
                }
                else if (text[i] == '\n')
                {
                    lineCount++;
                }
                else if (text[i] == '\t' || text[i] == '\r')
                {
                    tabCount++;
                }
                else
                    break;
            }
            return (spaceCount, tabCount, lineCount);
        }

        private string BreakWord(string text)
        {
            //check for work breakers and punctuators
            //start from index 1 cause we have already checked the punctuator and operator before, so they won't be present at start at least
            for (int i = 1; i < text.Length; i++)
            {
                if (Grammar.WordBreakers.Contains(text[i]) || Grammar.Punctuators.Keys.Contains(text[i].ToString()))
                {
                    return text.Substring(0, i);
                }
                else
                {
                    //check for operators
                    foreach (var op in Grammar.Operators.Keys)
                    {
                        //match the operators
                        if (text.Substring(i).StartsWith(op))
                        {
                            return text.Substring(0, i);
                        }
                    }
                }
            }
            return text;
        }

        private Token IsKeyword(string text, int lineNumber)
        {
            Token token = null;
            foreach (var keyword in Grammar.Keywords.Keys)
            {
                if (text.StartsWith(keyword))
                {
                    var classPart = Grammar.Keywords[keyword];
                    token = new Token(classPart, keyword, lineNumber);
                }
            }
            return token;
        }

        private Token IsPunctuator(string text, int lineNumber)
        {
            Token token = null;
            foreach (var punctuator in Grammar.Punctuators.Keys)
            {
                if (text.StartsWith(punctuator))
                {
                    var classPart = Grammar.Punctuators[punctuator];
                    token = new Token(classPart, punctuator, lineNumber);
                }
            }
            return token;
        }

        private Token IsOperator(string text, int lineNumber)
        {
            Token token = null;
            foreach (var op in Grammar.Operators.Keys)
            {
                if (text.StartsWith(op))
                {
                    //we only except plus and minus operator which don't have a dot or number after them, cause we have to differentiate between +/- sign and operator
                    if (text[0] == '+' || text[0] == '-')
                    {
                        if (text[1] != '.' && !Regex.Match(text[1].ToString(), "^[0-9]$").Success)
                        {
                            var classPart = Grammar.Operators[op];
                            token = new Token(classPart, op, lineNumber);
                        }
                    }
                    else
                    {
                        var classPart = Grammar.Operators[op];
                        token = new Token(classPart, op, lineNumber);
                    }
                }
            }
            return token;
        }

        private Token IsString(string text, int lineNumber)
        {
            Token token = null;

            //if it starts with double quote, search for the ending quote 
            if (text.StartsWith("\"") && text.Length > 1)
            {
                for (int i = 1; i < text.Length; i++)
                {
                    //line break
                    if (text[i] == '\n')
                    {
                        //don't include the new line character in the value part
                        token = new Token(ClassPart.INVALID, text.Substring(0, i - 1), lineNumber);
                        break;
                    }
                    //find the end of string and also deal the escape condition
                    if (text[i] == '"' && text[i - 1] != '\\')
                    {
                        token = new Token(ClassPart.STRING_VALUE, text.Substring(0, i + 1), lineNumber);
                        break;
                    }
                    //if text ends and we don't find the ending quote for string, means string is not closed
                    if (i == text.Length - 1)
                        token = new Token(ClassPart.INVALID, text, lineNumber);
                }
            }
            //if string is too short and incomplete
            else if (text.StartsWith("\"") && text.Length < 2)
            {
                token = new Token(ClassPart.INVALID, text, lineNumber);
            }

            return token;
        }

        private (Token, int) IsComment(string text, int lineNumber)
        {
            Token token = null;
            int lineCount = 0;

            //for single line comment
            if (text.StartsWith("//") && text.Length > 1)
            {
                for (int i = 1; i < text.Length; i++)
                {
                    if (text[i] == '\n')
                    {
                        token = new Token(ClassPart.SINGLE_LINE_COMMENT, text.Substring(0, i + 1), lineNumber);
                        lineCount++;
                        break;
                    }
                }
            }
            //for multi line comment
            else if (text.StartsWith("/**") && text.Length > 5)
            {
                for (int i = 3; i < text.Length - 2; i++)
                {
                    if (text[i] == '\n')
                    {
                        lineCount++;
                        continue;
                    }
                    //find the end of multi line comment
                    if (text[i] == '*' && text[i + 1] == '*' && text[i + 2] == '/')
                    {
                        token = new Token(ClassPart.MULTI_LINE_COMMENT, text.Substring(0, i + 1), lineNumber + lineCount);
                        break;
                    }
                    //if text ends and we don't find the closing comment tag
                    if (i == text.Length - 3)
                        token = new Token(ClassPart.INVALID, text, lineNumber);
                }
            }
            //if text is too short and comment is incomplete i.e not closed
            else if (text.StartsWith("/**") && text.Length < 6)
            {
                token = new Token(ClassPart.INVALID, text, lineNumber);
            }

            return (token, lineCount);
        }

        private Token IsInt(string word, int lineNumber)
        {
            if (Regex.Match(word, "^[+-]?[0-9]+$").Success)
            {
                return new Token(ClassPart.INT_VALUE, word, lineNumber);
            }
            return null;
        }

        private Token IsDouble(string word, int lineNumber)
        {
            if (Regex.Match(word, "^[+-]?[0-9]*.[0-9]+$").Success)
            {
                return new Token(ClassPart.DOUBLE_VALUE, word, lineNumber);
            }
            return null;
        }

        private Token IsBool(string word, int lineNumber)
        {
            if (word == "true" || word == "false")
            {
                return new Token(ClassPart.BOOL, word, lineNumber);
            }
            return null;
        }

        private Token IsIdentifier(string word, int lineNumber)
        {
            if (Regex.Match(word, "^[_]?[A-Za-z]+[A-Za-z0-9]*$").Success)
            {
                return new Token(ClassPart.IDENTIFIER, word, lineNumber);
            }
            return null;
        }
    }
}