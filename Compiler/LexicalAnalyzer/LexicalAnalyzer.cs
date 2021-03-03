using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System;

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
            int indexLength = 1;
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
                    //now we have to break the word in order to validate it as arithmetic operators or identifier
                    var word = BreakWord(text);
                    token = IsDouble(word, line);
                    if (token == null)
                        token = IsInt(word, line);
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
            if (Regex.Match(text.First().ToString(), "^[0-9]$").Success
                || (text.Length > 1 && text[0] == '.'
                && Regex.Match(text[1].ToString(), "^[0-9]$").Success))
            {
                //only accepts 1 dot (for double only)
                int dotCount = 0;
                for (int i = 1; i < text.Length; i++)
                {
                    if (text[i] == '.') dotCount++;
                    if (!Regex.Match(text[i].ToString(), "^[0-9]$").Success || (text[i] == '.' && dotCount == 2))
                    {
                        return text.Substring(0, i);
                    }
                }
            }
            else
            {
                for (int i = 1; i < text.Length; i++)
                {
                    if (Grammar.WordBreakers.Contains(text[i]) || Grammar.Punctuators.Keys.Contains(text[i].ToString()))
                    {
                        //if word breaker is a dot make sure the word is not a double
                        //if (text[i] != '.' || (text[i] == '.' && !Regex.Match(text[i + 1].ToString(), "^[0-9]$").Success))
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
            }
            return text;
        }

        private Token IsKeyword(string text, int lineNumber)
        {
            Token token = null;
            foreach (var keyword in Grammar.Keywords.Keys)
            {
                //check if text starts with keyword and the keyword not end with any alphabet or digit
                if (text.StartsWith(keyword) && (text.Length == keyword.Length || !Regex.Match(text.Substring(keyword.Length, 1), "^[a-zA-Z][a-zA-Z0-9]*$").Success)) //!Regex.Match(text.Substring(keyword.Length, 1), "^[A-Za-z0-9]+$").Success)
                {
                    var classPart = Grammar.Keywords[keyword];
                    token = new Token(classPart, keyword, lineNumber);
                    break;
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
                    //don't take DOT as a punctuator if it has preceding digits, cause it will be a double constant in that case
                    if (text[0] == '.')
                    {
                        if (text.Length > 1 && !Regex.Match(text[1].ToString(), "^[0-9]$").Success)
                        {
                            token = new Token(Grammar.Punctuators[punctuator], punctuator, lineNumber);
                            break;
                        }
                    }
                    else
                    {
                        var classPart = Grammar.Punctuators[punctuator];
                        token = new Token(classPart, punctuator, lineNumber);
                        break;
                    }
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
                    if (op == "+" || op == "-")
                    {
                        if (text[1] != '.' && !Regex.Match(text[1].ToString(), "^[0-9]$").Success)
                        {
                            var classPart = Grammar.Operators[op];
                            token = new Token(classPart, op, lineNumber);
                            break;
                        }
                    }
                    else
                    {
                        var classPart = Grammar.Operators[op];
                        token = new Token(classPart, op, lineNumber);
                        break;
                    }
                }
            }
            return token;
        }

        private Token IsString(string text, int lineNumber)
        {

            Token token = null;
            //true when we find an invalid escape sequence \
            bool isInvalid = false;
            //if it starts with double quote, search for the ending quote 
            if (text.StartsWith("\"") && text.Length > 1)
            {
                for (int i = 1; i < text.Length; i++)
                {
                    if (text[i] == '\\')
                    {
                        //isInvalid = true;
                    }
                    //line break
                    else if (text[i] == '\r' || text[i] == '\n')
                    {
                        //don't include the new line character in the value part
                        token = new Token(ClassPart.INVALID, text.Substring(0, i), lineNumber);
                        break;
                    }
                    //find the end of string and also deal the escape condition
                    if (text[i] == '\"' && text[i - 1] != '\\')
                    {
                        if (isInvalid)
                            return new Token(ClassPart.INVALID, text.Substring(0, i + 1), lineNumber);
                        return new Token(ClassPart.STRING_CONSTANT, text.Substring(0, i + 1), lineNumber);
                    }
                    //if text ends and we don't find the ending quote for string, means string is not closed
                    if (i == text.Length - 1)
                        token = new Token(ClassPart.INVALID, text, lineNumber);
                }
            }
            //if it starts with single quote, search for the ending single 
            else if (text.StartsWith("\'") && text.Length > 1)
            {
                for (int i = 1; i < text.Length; i++)
                {
                    if (text[i] == '\\')
                    {
                        isInvalid = true;
                    }
                    //line break
                    else if (text[i] == '\r' || text[i] == '\n')
                    {
                        //don't include the new line character in the value part
                        token = new Token(ClassPart.INVALID, text.Substring(0, i), lineNumber);
                        break;
                    }
                    //find the end of string and also deal the escape condition
                    if (text[i] == '\'' && text[i - 1] != '\\')
                    {
                        if (isInvalid)
                            return new Token(ClassPart.INVALID, text.Substring(0, i + 1), lineNumber);
                        return new Token(ClassPart.STRING_CONSTANT, text.Substring(0, i + 1), lineNumber);
                    }
                    //if text ends and we don't find the ending quote for string, means string is not closed
                    if (i == text.Length - 1)
                        token = new Token(ClassPart.INVALID, text, lineNumber);
                }
            }
            //if string is too short and incomplete
            else if ((text.StartsWith("\"") || text.StartsWith("'")) && text.Length < 2)
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
                        token = new Token(ClassPart.SINGLE_LINE_COMMENT, text.Substring(0, i), lineNumber);
                        lineCount++;
                        break;
                    }
                    //if we couldn't find a line break means its the end of string
                    if (i == text.Length - 1)
                    {
                        token = new Token(ClassPart.SINGLE_LINE_COMMENT, text, lineNumber);
                    }
                }
            }
            //for multi line comment
            else if (text.StartsWith("/*") && text.Length > 4)
            {
                for (int i = 2; i < text.Length - 1; i++)
                {
                    if (text[i] == '\n')
                    {
                        lineCount++;
                    }
                    //find the end of multi line comment
                    if (text[i] == '*' && text[i + 1] == '/')
                    {
                        token = new Token(ClassPart.MULTI_LINE_COMMENT, text.Substring(0, i + 2), lineNumber + lineCount);
                        break;
                    }
                    //if text ends and we don't find the closing comment tag
                    if (i == text.Length - 2)
                        token = new Token(ClassPart.INVALID, text, lineNumber);
                }
            }
            //if text is too short and comment is incomplete i.e not closed
            else if (text.StartsWith("/*") && text.Length < 5)
            {
                token = new Token(ClassPart.INVALID, text, lineNumber);
            }

            return (token, lineCount);
        }

        private Token IsInt(string word, int lineNumber)
        {
            if (Regex.Match(word, "^[+-]?[0-9]+$").Success)
            {
                return new Token(ClassPart.INT_CONSTANT, word, lineNumber);
            }
            return null;
        }

        private Token IsDouble(string word, int lineNumber)
        {
            if (Regex.Match(word, "^[+-]?[0-9]*[.][0-9]+[d]?$").Success)
            {
                return new Token(ClassPart.DOUBLE_CONSTANT, word, lineNumber);
            }
            return null;
        }

        private Token IsBool(string word, int lineNumber)
        {
            if (word == "true" || word == "false")
            {
                return new Token(ClassPart.BOOL_CONSTANT, word, lineNumber);
            }
            return null;
        }

        private Token IsIdentifier(string word, int lineNumber)
        {
            if (Regex.Match(word, "^[_]?[A-Za-z]+([_]*[A-Za-z0-9]*)*[_]*$").Success)
            {
                return new Token(ClassPart.IDENTIFIER, word, lineNumber);
            }
            return null;
        }
    }
}