using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public class SyntaxAnalyzer
    {
        static int index = 0;
        static List<Token> tokens;

        /// <summary>
        /// Analyzes the tokens, if correct returns null else return the line number of error
        /// </summary>
        /// <param name="_tokens"></param>
        /// <returns></returns>
        public string Analyze(List<Token> _tokens)
        {
            index = 0;
            tokens = _tokens;

            if (S())
            {
                if (Defst())
                {
                    index++;
                }
            }
            return null;
        }

        bool S()
        {
            if (tokens[index].ClassPart == ClassPart.PUBLIC || tokens[index].ClassPart == ClassPart.STATIC ||
                tokens[index].ClassPart == ClassPart.ABSTRACT || tokens[index].ClassPart == ClassPart.CLASS)
                return true;
            return false;
        }

        bool Defst()
        {
            if (tokens[index].ClassPart == ClassPart.PUBLIC)
            {
                index++;
                if (Defst1())
                    return true;
            }
            return false;
        }

        bool Defst1()
        {
            if (tokens[index].ClassPart == ClassPart.STATIC || tokens[index].ClassPart == ClassPart.ABSTRACT)
            {

            }
            return false;
        }
    }
}
