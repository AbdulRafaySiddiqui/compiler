using System.Collections.Generic;

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
        public Token Analyze(List<Token> _tokens)
        {
            index = 0;
            tokens = _tokens;

            if (S())
            {
                if (defst())
                {
                    index++;
                }
            }
            return tokens[index];
        }

        bool S()
        {
            if (tokens[index].ClassPart == ClassPart.PUBLIC ||
                tokens[index].ClassPart == ClassPart.STATIC ||
                tokens[index].ClassPart == ClassPart.SEALED ||
                tokens[index].ClassPart == ClassPart.ABSTRACT ||
                tokens[index].ClassPart == ClassPart.CLASS)
            {
                if (defst())
                {
                    if (tokens[index].ClassPart == ClassPart.OPENING_CURLY_BRACKET)
                    {
                        index++;
                        if (class_body_main())
                        {
                            if (class_body())
                            {
                                if (tokens[index].ClassPart == ClassPart.CLOSING_CURLY_BRACKET)
                                {
                                    if (defen())
                                    {
                                        return true;
                                    }
                                }
                            }

                        }
                    }
                }

            }
            return false;
        }

        bool static_abstract()
        {
            if (tokens[index].ClassPart == ClassPart.STATIC)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.ABSTRACT)
            {
                index++;
                return true;
            }
            return false;
        }

        bool defst()
        {
            if (tokens[index].ClassPart == ClassPart.PUBLIC)
            {
                index++;
                if (defst1())
                {
                    if (defst())
                    {
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.SEALED ||
                    tokens[index].ClassPart == ClassPart.STATIC ||
                    tokens[index].ClassPart == ClassPart.ABSTRACT)
            {
                if (defst1())
                {
                    if (defst())
                    {
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.CLASS)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                {
                    index++;
                    return true;
                }
            }
            return false;
        }

        bool defst1()
        {
            if (tokens[index].ClassPart == ClassPart.SEALED)
            {
                if (tokens[index].ClassPart == ClassPart.SEALED)
                {
                    index++;
                    if (tokens[index].ClassPart == ClassPart.CLASS)
                    {
                        index++;
                        if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                        {
                            index++;
                            if (inheritance())
                            {
                                if (tokens[index].ClassPart == ClassPart.OPENING_CURLY_BRACKET)
                                {
                                    index++;
                                    if (class_body())
                                    {
                                        if (tokens[index].ClassPart == ClassPart.CLOSING_CURLY_BRACKET)
                                        {
                                            index++;
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (tokens[index].ClassPart == ClassPart.STATIC ||
                        tokens[index].ClassPart == ClassPart.ABSTRACT)
                {
                    if (static_abstract())
                    {
                        if (defstCC())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        bool defst1C()
        {
            if (tokens[index].ClassPart == ClassPart.COLON ||
                tokens[index].ClassPart == ClassPart.OPENING_CURLY_BRACKET)
            {
                if (inheritance())
                {
                    if (tokens[index].ClassPart == ClassPart.OPENING_CURLY_BRACKET)
                    {
                        index++;
                        if (class_body())
                        {
                            if (tokens[index].ClassPart == ClassPart.CLOSING_CURLY_BRACKET)
                            {
                                index++;
                                return true;
                            }
                        }
                    }
                }

            }
            else if (tokens[index].ClassPart == ClassPart.PUBLIC ||
                    tokens[index].ClassPart == ClassPart.SEALED ||
                    tokens[index].ClassPart == ClassPart.STATIC ||
                    tokens[index].ClassPart == ClassPart.ABSTRACT ||
                    tokens[index].ClassPart == ClassPart.CLASS)
            {
                return true;
            }
            return false;
        }

        bool defstCC()
        {
            if (tokens[index].ClassPart == ClassPart.CLASS)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                {
                    index++;
                    if (defst1C())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        bool defen()
        {
            if (tokens[index].ClassPart == ClassPart.PUBLIC)
            {
                index++;
                if (defen1())
                {
                    if (defen())
                    {
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.STATIC ||
                    tokens[index].ClassPart == ClassPart.ABSTRACT ||
                    tokens[index].ClassPart == ClassPart.SEALED ||
                    tokens[index].ClassPart == ClassPart.CLASS)
            {
                if (defen1())
                {
                    if (defen())
                    {
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.END_MARKER)
            {
                return true;
            }
            return false;
        }

        bool defen1()
        {
            if (tokens[index].ClassPart == ClassPart.STATIC ||
                tokens[index].ClassPart == ClassPart.ABSTRACT ||
                tokens[index].ClassPart == ClassPart.SEALED ||
                tokens[index].ClassPart == ClassPart.CLASS)
            {
                if (c_modifier())
                {
                    if (tokens[index].ClassPart == ClassPart.CLASS)
                    {
                        index++;
                        if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                        {
                            index++;
                            if (inheritance())
                            {
                                if (tokens[index].ClassPart == ClassPart.OPENING_CURLY_BRACKET)
                                {
                                    index++;
                                    if (class_body())
                                    {
                                        if (tokens[index].ClassPart == ClassPart.CLOSING_CURLY_BRACKET)
                                        {
                                            index++;
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        bool class_body_main()
        {
            if (tokens[index].ClassPart == ClassPart.ABSTRACT ||
                tokens[index].ClassPart == ClassPart.SEALED ||
                tokens[index].ClassPart == ClassPart.VOID ||
                tokens[index].ClassPart == ClassPart.CLASS)
            {
                if (c_modifier())
                {
                    if (class_body2())
                    {
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.STATIC)
            {
                index++;
                if (main1())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.PUBLIC ||
                tokens[index].ClassPart == ClassPart.PRIVATE ||
                tokens[index].ClassPart == ClassPart.PROTECTED ||
                tokens[index].ClassPart == ClassPart.INTERNAL)
            {
                if (acc_modifier())
                {
                    if (c_modifier())
                    {
                        if (tokens[index].ClassPart == ClassPart.CLASS)
                        {
                            index++;
                            if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                            {
                                index++;
                                if (inheritance())
                                {
                                    if (tokens[index].ClassPart == ClassPart.OPENING_CURLY_BRACKET)
                                    {
                                        index++;
                                        if (class_body())
                                        {
                                            if (tokens[index].ClassPart == ClassPart.CLOSING_CURLY_BRACKET)
                                            {
                                                index++;
                                                return true;
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        bool class_body()
        {
            if (tokens[index].ClassPart == ClassPart.INTEGER ||
                tokens[index].ClassPart == ClassPart.STRING ||
                tokens[index].ClassPart == ClassPart.DOUBLE ||
                tokens[index].ClassPart == ClassPart.BOOL)
            {
                if (data_type())
                {
                    if (class_body1())
                    {
                        if (class_body())
                        {
                            return true;
                        }
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.STATIC ||
                tokens[index].ClassPart == ClassPart.ABSTRACT ||
                tokens[index].ClassPart == ClassPart.SEALED ||
                tokens[index].ClassPart == ClassPart.VOID ||
                tokens[index].ClassPart == ClassPart.CLASS)
            {
                if (c_modifier())
                {
                    if (class_body2())
                    {
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.PUBLIC ||
                tokens[index].ClassPart == ClassPart.PRIVATE ||
                tokens[index].ClassPart == ClassPart.PROTECTED ||
                tokens[index].ClassPart == ClassPart.INTERNAL)
            {
                if (acc_modifier())
                {
                    if (c_modifier())
                    {
                        if (tokens[index].ClassPart == ClassPart.CLASS)
                        {
                            index++;
                            if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                            {
                                index++;
                                if (inheritance())
                                {
                                    if (tokens[index].ClassPart == ClassPart.OPENING_CURLY_BRACKET)
                                    {
                                        index++;
                                        if (class_body())
                                        {
                                            if (tokens[index].ClassPart == ClassPart.CLOSING_CURLY_BRACKET)
                                            {
                                                index++;
                                                return true;
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
            {
                index++;
                if (class_body1())
                {
                    if (class_body())
                    {
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.CLOSING_CURLY_BRACKET)
            {
                return true;
            }
            return false;
        }

        bool class_body1()
        {
            if (tokens[index].ClassPart == ClassPart.EQUAL ||
                tokens[index].ClassPart == ClassPart.SEMI_COLON)
            {
                if (initial())
                {
                    if (listt())
                    {
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES)
                {
                    index++;
                    if (PL())
                    {
                        if (tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES)
                        {
                            index++;
                            if (body())
                            {
                                if (tokens[index].ClassPart == ClassPart.CLOSING_CURLY_BRACKET)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        bool class_body2()
        {
            if (tokens[index].ClassPart == ClassPart.VOID)
            {
                index++;
                if (main2())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.CLASS)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                {
                    index++;
                    if (inheritance())
                    {
                        if (tokens[index].ClassPart == ClassPart.OPENING_CURLY_BRACKET)
                        {
                            index++;
                            if (class_body())
                            {
                                if (tokens[index].ClassPart == ClassPart.CLOSING_CURLY_BRACKET)
                                {
                                    index++;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        bool main1()
        {
            if (tokens[index].ClassPart == ClassPart.VOID)
            {
                index++;
                if (main2())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.CLASS)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                {
                    index++;
                    if (inheritance())
                    {
                        if (tokens[index].ClassPart == ClassPart.OPENING_CURLY_BRACKET)
                        {
                            index++;
                            if (class_body())
                            {
                                if (tokens[index].ClassPart == ClassPart.CLOSING_CURLY_BRACKET)
                                {
                                    index++;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        bool main2()
        {
            if (tokens[index].ClassPart == ClassPart.MAIN)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.OPENING_CURLY_BRACKET)
                {
                    index++;
                    if (body())
                    {
                        if (tokens[index].ClassPart == ClassPart.CLOSING_CURLY_BRACKET)
                        {
                            index++;
                            return true;
                        }
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.EQUAL ||
                tokens[index].ClassPart == ClassPart.SEMI_COLON ||
                tokens[index].ClassPart == ClassPart.COMMA ||
                tokens[index].ClassPart == ClassPart.IDENTIFIER)
            {
                if (class_body1())
                {
                    if (main1())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        bool body()
        {
            if (tokens[index].ClassPart == ClassPart.SEMI_COLON)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.OPENING_CURLY_BRACKET)
            {
                index++;
                if (MSt())
                {
                    return true;
                }
            }
            return false;
        }

        bool body1()
        {
            if (tokens[index].ClassPart == ClassPart.WHILE ||
                tokens[index].ClassPart == ClassPart.FOR ||
                tokens[index].ClassPart == ClassPart.IF ||
                tokens[index].ClassPart == ClassPart.DO ||
                tokens[index].ClassPart == ClassPart.CONTINUE ||
                tokens[index].ClassPart == ClassPart.BREAK ||
                tokens[index].ClassPart == ClassPart.RETURN ||
                tokens[index].ClassPart == ClassPart.STATIC ||
                tokens[index].ClassPart == ClassPart.IDENTIFIER ||
                tokens[index].ClassPart == ClassPart.THIS ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_PLUS ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_MINUS ||
                tokens[index].ClassPart == ClassPart.INTEGER ||
                tokens[index].ClassPart == ClassPart.STRING ||
                tokens[index].ClassPart == ClassPart.DOUBLE ||
                tokens[index].ClassPart == ClassPart.BOOL)
            {
                if (SSt())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.SEMI_COLON ||
                tokens[index].ClassPart == ClassPart.OPENING_CURLY_BRACKET)
            {
                if (body())
                {
                    if (tokens[index].ClassPart == ClassPart.CLOSING_CURLY_BRACKET)
                    {
                        index++;
                        return true;
                    }
                }
            }
            return false;
        }

        bool MSt()
        {
            if (tokens[index].ClassPart == ClassPart.CLOSING_CURLY_BRACKET ||
                tokens[index].ClassPart == ClassPart.SEMI_COLON)
            {
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.WHILE ||
                tokens[index].ClassPart == ClassPart.FOR ||
                tokens[index].ClassPart == ClassPart.IF ||
                tokens[index].ClassPart == ClassPart.DO ||
                tokens[index].ClassPart == ClassPart.CONTINUE ||
                tokens[index].ClassPart == ClassPart.BREAK ||
                tokens[index].ClassPart == ClassPart.RETURN ||
                tokens[index].ClassPart == ClassPart.STATIC ||
                tokens[index].ClassPart == ClassPart.IDENTIFIER ||
                tokens[index].ClassPart == ClassPart.THIS ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_PLUS ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_MINUS ||
                tokens[index].ClassPart == ClassPart.INTEGER ||
                tokens[index].ClassPart == ClassPart.STRING ||
                tokens[index].ClassPart == ClassPart.DOUBLE ||
                tokens[index].ClassPart == ClassPart.BOOL)
            {
                if (SSt())
                {
                    if (MSt())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        bool acc_modifier()
        {
            if (tokens[index].ClassPart == ClassPart.PUBLIC)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.PRIVATE)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.PROTECTED)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.INTERNAL)
            {
                index++;
                return true;
            }
            return false;
        }

        bool c_modifier()
        {
            if (tokens[index].ClassPart == ClassPart.STATIC)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.ABSTRACT)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.SEALED)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.CLASS ||
                tokens[index].ClassPart == ClassPart.VOID)
            {
                return true;
            }
            return false;
        }

        bool c_modifier1()
        {
            if (tokens[index].ClassPart == ClassPart.ABSTRACT)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.SEALED)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.CLASS ||
                tokens[index].ClassPart == ClassPart.VOID)
            {
                return true;
            }
            return false;
        }

        bool ret_type()
        {
            if (tokens[index].ClassPart == ClassPart.VOID)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.INTEGER ||
                tokens[index].ClassPart == ClassPart.STRING ||
                tokens[index].ClassPart == ClassPart.DOUBLE ||
                tokens[index].ClassPart == ClassPart.BOOL)
            {
                if (data_type())
                {
                    return true;
                }
            }
            return false;
        }
        bool data_type()
        {
            if (tokens[index].ClassPart == ClassPart.STRING)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.INTEGER)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.DOUBLE)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.BOOL)
            {
                index++;
                return true;
            }
            return false;
        }

        bool inheritance()
        {
            if (tokens[index].ClassPart == ClassPart.COLON)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                {
                    index++;
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.OPENING_CURLY_BRACKET)
            {
                return true;
            }
            return false;
        }

        bool function()
        {
            if (tokens[index].ClassPart == ClassPart.STATIC)
            {
                index++;
                if (ret_type())
                {
                    if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                    {
                        index++;
                        if (tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES)
                        {
                            index++;
                            if (PL())
                            {
                                if (tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES)
                                {
                                    index++;
                                    if (body())
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }

            }
            return false;
        }

        bool PL()
        {
            if (tokens[index].ClassPart == ClassPart.INTEGER ||
                tokens[index].ClassPart == ClassPart.DOUBLE ||
                tokens[index].ClassPart == ClassPart.BOOL ||
                tokens[index].ClassPart == ClassPart.STRING)
            {
                if (data_type())
                {
                    if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                    {
                        index++;
                        if (PL1())
                        {
                            return true;
                        }
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES)
            {
                return true;
            }
            return false;
        }

        bool PL1()
        {
            if (tokens[index].ClassPart == ClassPart.EQUAL)
            {
                index++;
                if (OE())
                {
                    if (PL2())
                    {
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.COMMA)
            {
                if (PL2())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES)
            {
                return true;
            }
            return false;
        }

        bool PL2()
        {
            if (tokens[index].ClassPart == ClassPart.COMMA)
            {
                index++;
                if (data_type())
                {
                    if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                    {
                        index++;
                        if (PL1())
                        {
                            return true;
                        }
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES)
            {
                return true;
            }
            return false;
        }

        bool thiss()
        {
            if (tokens[index].ClassPart == ClassPart.THIS)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.DOT)
                {
                    index++;
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
            {
                return true;
            }
            return false;
        }

        ///////////////////////////////////////////////////////////////////////////
        bool OE()
        {
            if (tokens[index].ClassPart == ClassPart.IDENTIFIER ||
                tokens[index].ClassPart == ClassPart.INT_CONSTANT ||
                tokens[index].ClassPart == ClassPart.STRING_CONSTANT ||
                tokens[index].ClassPart == ClassPart.DOUBLE_CONSTANT ||
                tokens[index].ClassPart == ClassPart.BOOL_CONSTANT ||
                tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES ||
                tokens[index].ClassPart == ClassPart.NOT ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_PLUS ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_MINUS ||
                tokens[index].ClassPart == ClassPart.THIS)
            {
                if (AE())
                {
                    if (OEC())
                        return true;
                }
            }
            return false;
        }
        bool OEC()
        {
            if (tokens[index].ClassPart == ClassPart.OR)
            {
                index++;
                if (AE())
                {
                    if (OEC())
                    {
                        return true;
                    }

                }

            }


            else if (tokens[index].ClassPart == ClassPart.COMMA ||
                    tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES ||
                    tokens[index].ClassPart == ClassPart.SEMI_COLON)
            {
                return true;
            }
            return false;
        }

        bool AE()
        {
            if (tokens[index].ClassPart == ClassPart.IDENTIFIER ||
                tokens[index].ClassPart == ClassPart.INT_CONSTANT ||
                tokens[index].ClassPart == ClassPart.STRING_CONSTANT ||
                tokens[index].ClassPart == ClassPart.DOUBLE_CONSTANT ||
                tokens[index].ClassPart == ClassPart.BOOL_CONSTANT ||
                tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES ||
                tokens[index].ClassPart == ClassPart.NOT ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_PLUS ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_MINUS ||
                tokens[index].ClassPart == ClassPart.THIS)
            {
                if (RE())
                    if (AEC())
                    {
                        return true;
                    }
            }
            return false;
        }

        bool AEC()
        {
            if (tokens[index].ClassPart == ClassPart.AND)
            {
                index++;
                if (RE())
                {
                    if (AEC())
                    {
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.OR ||
                    tokens[index].ClassPart == ClassPart.COMMA ||
                    tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES ||
                    tokens[index].ClassPart == ClassPart.SEMI_COLON)
            {
                return true;
            }
            return false;
        }

        bool RE()
        {
            if (tokens[index].ClassPart == ClassPart.IDENTIFIER ||
                tokens[index].ClassPart == ClassPart.INT_CONSTANT ||
                tokens[index].ClassPart == ClassPart.STRING_CONSTANT ||
                tokens[index].ClassPart == ClassPart.DOUBLE_CONSTANT ||
                tokens[index].ClassPart == ClassPart.BOOL_CONSTANT ||
                tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES ||
                tokens[index].ClassPart == ClassPart.NOT ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_PLUS ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_MINUS ||
                tokens[index].ClassPart == ClassPart.THIS)
            {
                if (E())
                {
                    if (REC())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        bool REC()
        {
            if (tokens[index].ClassPart == ClassPart.RELATIONAL_OPERATOR)
            {
                index++;
                if (E())
                {
                    if (REC())
                    {
                        return true;
                    }
                }
            }


            else if (tokens[index].ClassPart == ClassPart.AND ||
                    tokens[index].ClassPart == ClassPart.OR ||
                    tokens[index].ClassPart == ClassPart.COMMA ||
                    tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES ||
                    tokens[index].ClassPart == ClassPart.SEMI_COLON)
            {
                return true;
            }
            return false;
        }

        bool E()
        {
            if (tokens[index].ClassPart == ClassPart.IDENTIFIER ||
                tokens[index].ClassPart == ClassPart.INT_CONSTANT ||
                tokens[index].ClassPart == ClassPart.STRING_CONSTANT ||
                tokens[index].ClassPart == ClassPart.DOUBLE_CONSTANT ||
                tokens[index].ClassPart == ClassPart.BOOL_CONSTANT ||
                tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES ||
                tokens[index].ClassPart == ClassPart.NOT ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_PLUS ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_MINUS ||
                tokens[index].ClassPart == ClassPart.THIS)
            {
                if (T())
                {
                    if (EC())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        bool EC()
        {
            if (tokens[index].ClassPart == ClassPart.PLUS_MINUS)
            {
                index++;
                if (T())
                {
                    if (EC())
                    {
                        return true;
                    }
                }
            }


            else if (tokens[index].ClassPart == ClassPart.RELATIONAL_OPERATOR ||
                    tokens[index].ClassPart == ClassPart.AND ||
                    tokens[index].ClassPart == ClassPart.OR ||
                    tokens[index].ClassPart == ClassPart.COMMA ||
                    tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES ||
                    tokens[index].ClassPart == ClassPart.SEMI_COLON)
            {
                return true;
            }
            return false;
        }

        bool T()
        {
            if (tokens[index].ClassPart == ClassPart.IDENTIFIER ||
                tokens[index].ClassPart == ClassPart.INT_CONSTANT ||
                tokens[index].ClassPart == ClassPart.STRING_CONSTANT ||
                tokens[index].ClassPart == ClassPart.DOUBLE_CONSTANT ||
                tokens[index].ClassPart == ClassPart.BOOL_CONSTANT ||
                tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES ||
                tokens[index].ClassPart == ClassPart.NOT ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_PLUS ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_MINUS ||
                tokens[index].ClassPart == ClassPart.THIS)
            {
                if (F())
                {
                    if (TC())
                    {
                        return true;
                    }
                }

            }
            return false;
        }
        bool TC()
        {
            if (tokens[index].ClassPart == ClassPart.MULTIPLY_DIVIDE_MODULUS)
            {
                index++;
                if (F())
                {
                    if (TC())
                    {
                        return true;
                    }
                }
            }


            else if (tokens[index].ClassPart == ClassPart.PLUS_MINUS ||
                    tokens[index].ClassPart == ClassPart.RELATIONAL_OPERATOR ||
                    tokens[index].ClassPart == ClassPart.AND ||
                    tokens[index].ClassPart == ClassPart.OR ||
                    tokens[index].ClassPart == ClassPart.COMMA ||
                    tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES ||
                    tokens[index].ClassPart == ClassPart.SEMI_COLON)
            {
                return true;
            }
            return false;
        }
        bool F()
        {
            if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
            {
                index++;
                if (FC())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.INT_CONSTANT ||
                    tokens[index].ClassPart == ClassPart.STRING_CONSTANT ||
                    tokens[index].ClassPart == ClassPart.DOUBLE_CONSTANT ||
                    tokens[index].ClassPart == ClassPart.BOOL_CONSTANT)
            {
                if (constt())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES)
            {
                index++;
                if (OE())
                {
                    if (tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES)
                    {
                        index++;
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.NOT)
            {
                index++;
                if (F())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_PLUS ||
                    tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_MINUS)
            {
                if (inc_dec())
                {
                    if (thiss())
                    {
                        if ((tokens[index].ClassPart == ClassPart.IDENTIFIER))
                        {

                            index++;
                            if (Z2())
                            {

                                return true;
                            }

                        }

                    }

                }
            }
            else if (tokens[index].ClassPart == ClassPart.THIS)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.DOT)
                {
                    index++;
                    if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                    {
                        index++;
                        if (Z())
                        {
                            return true;
                        }


                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.DOT ||
                    tokens[index].ClassPart == ClassPart.EQUAL ||
                    tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_PLUS ||
                    tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_MINUS ||
                    tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_MULTIPLY ||
                    tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_DIVIDE ||
                    tokens[index].ClassPart == ClassPart.OPENING_SQUARE_BRACKET ||
                    tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES ||
                    tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_PLUS ||
                    tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_MINUS)
            {
                if (Z())
                    if (tokens[index].ClassPart == ClassPart.SEMI_COLON)
                    {
                        return true;
                    }
            }
            else if (tokens[index].ClassPart == ClassPart.MULTIPLY_DIVIDE_MODULUS ||
                    tokens[index].ClassPart == ClassPart.PLUS_MINUS ||
                    tokens[index].ClassPart == ClassPart.RELATIONAL_OPERATOR ||
                    tokens[index].ClassPart == ClassPart.AND ||
                    tokens[index].ClassPart == ClassPart.OR ||
                    tokens[index].ClassPart == ClassPart.COMMA ||
                    tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES ||
                    tokens[index].ClassPart == ClassPart.SEMI_COLON)
            {
                return true;
            }
            return false;
        }
        bool FC()
        {
            if (tokens[index].ClassPart == ClassPart.DOT ||
                tokens[index].ClassPart == ClassPart.EQUAL ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_PLUS ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_MINUS ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_MULTIPLY ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_DIVIDE ||
                tokens[index].ClassPart == ClassPart.OPENING_SQUARE_BRACKET ||
                tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_PLUS ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_MINUS)
            {
                if (Z())
                {
                    if (tokens[index].ClassPart == ClassPart.SEMI_COLON)
                    {
                        index++;
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.MULTIPLY_DIVIDE_MODULUS ||
                tokens[index].ClassPart == ClassPart.PLUS_MINUS ||
                tokens[index].ClassPart == ClassPart.RELATIONAL_OPERATOR ||
                tokens[index].ClassPart == ClassPart.AND ||
                tokens[index].ClassPart == ClassPart.OR ||
                tokens[index].ClassPart == ClassPart.COMMA ||
                tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES ||
                tokens[index].ClassPart == ClassPart.SEMI_COLON)
            {
                return true;
            }
            return false;
        }

        bool whilee()
        {
            if (tokens[index].ClassPart == ClassPart.WHILE)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES)
                {
                    index++;
                    if (OE())
                    {
                        if (tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES)
                        {
                            if (body1())
                            {
                                return true;
                            }
                        }
                    }
                }

            }
            return false;
        }
        bool do_while()
        {
            if (tokens[index].ClassPart == ClassPart.DO)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.OPENING_CURLY_BRACKET)
                {

                    index++;
                    if (semicolon())
                    {
                        if (MSt())
                        {

                            if (tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES)
                            {
                                index++;
                                if (tokens[index].ClassPart == ClassPart.WHILE)
                                {
                                    index++;
                                    if (tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES)
                                    {
                                        index++;
                                        if (OE())
                                        {
                                            if (tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES)
                                            {
                                                index++;
                                                return true;

                                            }
                                        }

                                    }

                                }

                            }

                        }
                    }
                }
            }
            return false;
        }
        bool semicolon()
        {
            if (tokens[index].ClassPart == ClassPart.SEMI_COLON)
            {
                index++;
                if (semicolon())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.WHILE ||
                    tokens[index].ClassPart == ClassPart.FOR ||
                    tokens[index].ClassPart == ClassPart.IF ||
                    tokens[index].ClassPart == ClassPart.DO ||
                    tokens[index].ClassPart == ClassPart.CONTINUE ||
                    tokens[index].ClassPart == ClassPart.BREAK ||
                    tokens[index].ClassPart == ClassPart.RETURN ||
                    tokens[index].ClassPart == ClassPart.STATIC ||
                    tokens[index].ClassPart == ClassPart.IDENTIFIER ||
                    tokens[index].ClassPart == ClassPart.THIS ||
                    tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_PLUS ||
                    tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_MINUS ||
                    tokens[index].ClassPart == ClassPart.INTEGER ||
                    tokens[index].ClassPart == ClassPart.STRING ||
                    tokens[index].ClassPart == ClassPart.BOOL ||
                    tokens[index].ClassPart == ClassPart.DOUBLE ||
                    tokens[index].ClassPart == ClassPart.CLOSING_CURLY_BRACKET)
            {
                return true;
            }
            return false;
        }
        bool if_else()
        {
            if (tokens[index].ClassPart == ClassPart.IF)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES)
                {
                    index++;
                    if (OE())
                    {
                        if (tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES)
                        {
                            if (body1())
                            {
                                if (elsee())
                                    return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        bool elsee()
        {
            if (tokens[index].ClassPart == ClassPart.ELSE)
            {
                index++;
                if (body1())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.WHILE ||
                    tokens[index].ClassPart == ClassPart.FOR ||
                    tokens[index].ClassPart == ClassPart.IF ||
                    tokens[index].ClassPart == ClassPart.DO ||
                    tokens[index].ClassPart == ClassPart.CONTINUE ||
                    tokens[index].ClassPart == ClassPart.BREAK ||
                    tokens[index].ClassPart == ClassPart.RETURN ||
                    tokens[index].ClassPart == ClassPart.STATIC ||
                    tokens[index].ClassPart == ClassPart.IDENTIFIER ||
                    tokens[index].ClassPart == ClassPart.THIS ||
                    tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_PLUS ||
                    tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_MINUS ||
                    tokens[index].ClassPart == ClassPart.INTEGER ||
                    tokens[index].ClassPart == ClassPart.STRING ||
                    tokens[index].ClassPart == ClassPart.DOUBLE ||
                    tokens[index].ClassPart == ClassPart.BOOL)
            {
                return true;
            }
            return false;
        }
        bool forr()
        {
            if (tokens[index].ClassPart == ClassPart.FOR)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES)
                {
                    if (c1())
                    {
                        if (c2())
                        {
                            if (tokens[index].ClassPart == ClassPart.SEMI_COLON)
                            {
                                if (c3())
                                {
                                    if (tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES)
                                    {
                                        if (body1())
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        bool c1()
        {
            if (tokens[index].ClassPart == ClassPart.SEMI_COLON)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.INTEGER ||
                    tokens[index].ClassPart == ClassPart.STRING ||
                    tokens[index].ClassPart == ClassPart.BOOL ||
                    tokens[index].ClassPart == ClassPart.DOUBLE)
            {
                if (declare())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.THIS ||
                    tokens[index].ClassPart == ClassPart.IDENTIFIER)
            {
                if (assign_st())
                {
                    return true;
                }
            }
            return false;
        }
        bool c2()
        {
            if (tokens[index].ClassPart == ClassPart.IDENTIFIER ||
                tokens[index].ClassPart == ClassPart.INT_CONSTANT ||
                tokens[index].ClassPart == ClassPart.STRING_CONSTANT ||
                tokens[index].ClassPart == ClassPart.DOUBLE_CONSTANT ||
                tokens[index].ClassPart == ClassPart.BOOL_CONSTANT ||
                tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES ||
                tokens[index].ClassPart == ClassPart.NOT ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_PLUS ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_MINUS ||
                tokens[index].ClassPart == ClassPart.THIS)
            {
                if (OE())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.SEMI_COLON)
            {
                return true;
            }
            return false;
        }
        bool c3()
        {
            if (tokens[index].ClassPart == ClassPart.THIS ||
                tokens[index].ClassPart == ClassPart.IDENTIFIER)
            {
                if (thiss())
                {
                    if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                    {
                        index++;
                        if (X())
                        {
                            return true;
                        }
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_PLUS ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_MINUS)
            {
                if (inc_dec())
                {
                    if (thiss())
                    {
                        if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                        {
                            index++;
                            if (X1())
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES)
            {
                return true;
            }
            return false;
        }
        bool X1()
        {
            if (tokens[index].ClassPart == ClassPart.DOT)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                {
                    index++;
                    if (X1())
                    {
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.OPENING_SQUARE_BRACKET)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.INT_CONSTANT)
                {
                    index++;
                    if (tokens[index].ClassPart == ClassPart.CLOSING_SQUARE_BRACKET)
                    {
                        index++;
                        if (tokens[index].ClassPart == ClassPart.DOT)
                        {
                            index++;
                            if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                            {
                                index++;
                                if (X())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES)
            {
                index++;
                if (PL())
                {
                    if (tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES)
                    {
                        index++;
                        if (tokens[index].ClassPart == ClassPart.DOT)
                        {
                            index++;
                            if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                            {
                                index++;
                                if (X1())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        bool X()
        {
            if (tokens[index].ClassPart == ClassPart.DOT)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                {
                    index++;
                    if (X())
                    {
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.EQUAL ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_PLUS ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_MINUS ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_MULTIPLY ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_DIVIDE)
            {
                if (assign_st())
                {
                    if (OE())
                    {
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.OPENING_SQUARE_BRACKET)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.INT_CONSTANT)
                {
                    index++;
                    if (tokens[index].ClassPart == ClassPart.CLOSING_SQUARE_BRACKET)
                    {
                        index++;
                        if (tokens[index].ClassPart == ClassPart.DOT)
                        {
                            index++;
                            if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                            {
                                index++;
                                if (X())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES)
            {
                index++;
                if (PL())
                {
                    if (tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES)
                    {
                        index++;
                        if (tokens[index].ClassPart == ClassPart.DOT)
                        {
                            index++;
                            if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                            {
                                index++;
                                if (X())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_PLUS ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_MINUS)
            {
                if (inc_dec())
                {
                    return true;
                }
            }
            return false;
        }
        bool constt()
        {
            if (tokens[index].ClassPart == ClassPart.INT_CONSTANT)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.STRING_CONSTANT)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.DOUBLE_CONSTANT)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.BOOL_CONSTANT)
            {
                index++;
                return true;
            }
            return false;
        }
        bool declare()
        {
            if (tokens[index].ClassPart == ClassPart.INTEGER || tokens[index].ClassPart == ClassPart.STRING ||
                tokens[index].ClassPart == ClassPart.BOOL || tokens[index].ClassPart == ClassPart.DOUBLE)
            {
                if (data_type())
                {
                    if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                    {
                        index++;
                        if (initial())
                        {
                            if (listt())
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        bool initial()
        {
            if (tokens[index].ClassPart == ClassPart.EQUAL)
            {
                index++;
                if (OE())
                {
                    return true;

                }
            }
            else if (tokens[index].ClassPart == ClassPart.SEMI_COLON ||
                    tokens[index].ClassPart == ClassPart.COMMA)
            {
                return true;
            }
            return false;
        }
        bool listt()
        {
            if (tokens[index].ClassPart == ClassPart.SEMI_COLON)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.COMMA)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                {
                    index++;
                    if (initial())
                    {
                        if (listt())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        bool SSt()
        {
            if (tokens[index].ClassPart == ClassPart.WHILE)
            {
                if (whilee())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.FOR)
            {
                if (forr())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.IF)
            {
                if (if_else())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.DO)
            {
                if (do_while())
                {
                    if (tokens[index].ClassPart == ClassPart.SEMI_COLON)
                    {
                        index++;
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.CONTINUE)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.SEMI_COLON)
                {
                    index++;
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.BREAK)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.SEMI_COLON)
                {
                    index++;
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.RETURN)
            {
                index++;
                if (OE())
                {
                    if (tokens[index].ClassPart == ClassPart.SEMI_COLON)
                    {
                        index++;
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.STATIC)
            {
                if (function())
                {
                    if (tokens[index].ClassPart == ClassPart.CLOSING_CURLY_BRACKET)
                    {
                        index++;
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
            {
                index++;
                if (obj())
                {
                    if (tokens[index].ClassPart == ClassPart.SEMI_COLON)
                    {
                        index++;
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.THIS)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.DOT)
                {
                    index++;
                    if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                    {
                        index++;
                        if (K())
                        {
                            if (tokens[index].ClassPart == ClassPart.SEMI_COLON)
                            {
                                index++;
                                return true;
                            }
                        }
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_PLUS ||
                    tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_MINUS)
            {
                if (inc_dec())
                {
                    if (thiss())
                    {
                        if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                        {
                            index++;
                            if (Z2())
                            {
                                if (tokens[index].ClassPart == ClassPart.SEMI_COLON)
                                {
                                    index++;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.INTEGER || tokens[index].ClassPart == ClassPart.STRING ||
                tokens[index].ClassPart == ClassPart.BOOL || tokens[index].ClassPart == ClassPart.DOUBLE)
            {
                if (data_type())
                {
                    if (nArrayy())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        bool nArrayy()
        {
            if (tokens[index].ClassPart == ClassPart.OPENING_SQUARE_BRACKET)
            {
                index++;
                if (I())
                {
                    if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                    {
                        index++;
                        if (A())
                        {
                            if (tokens[index].ClassPart == ClassPart.SEMI_COLON)
                            {
                                index++;
                                return true;
                            }
                        }
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
            {
                index++;
                if (initial())
                {
                    if (listt())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        bool obj()
        {
            if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.EQUAL)
                {
                    index++;
                    if (tokens[index].ClassPart == ClassPart.NEW)
                    {
                        index++;
                        if (c_name())
                        {
                            if (tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES)
                            {
                                index++;
                                if (tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES)
                                {
                                    index++;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.DOT ||
                tokens[index].ClassPart == ClassPart.EQUAL ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_PLUS ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_MINUS ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_MULTIPLY ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_DIVIDE ||
                tokens[index].ClassPart == ClassPart.OPENING_SQUARE_BRACKET ||
                tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_PLUS ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_MINUS)
            {
                if (Z())
                {
                    return true;
                }
            }
            return false;
        }

        bool K()
        {
            if (tokens[index].ClassPart == ClassPart.DOT ||
                tokens[index].ClassPart == ClassPart.OPENING_SQUARE_BRACKET ||
                tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES ||
                tokens[index].ClassPart == ClassPart.SEMI_COLON)
            {
                if (Z2())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.EQUAL)
            {
                index++;
                if (OE())
                {
                    return true;
                }
            }
            return false;
        }
        bool Z()
        {
            if (tokens[index].ClassPart == ClassPart.DOT)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                {
                    index++;
                    if (R1())
                    {
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.OPENING_SQUARE_BRACKET)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.INT_CONSTANT)
                {
                    index++;
                    if (tokens[index].ClassPart == ClassPart.CLOSING_SQUARE_BRACKET)
                    {
                        index++;
                        if (tokens[index].ClassPart == ClassPart.DOT)
                        {
                            index++;
                            if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                            {
                                index++;
                                if (R1())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES)
            {
                index++;
                if (PL())
                {
                    if (tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES)
                    {
                        index++;
                        if (R2())
                        {
                            return true;
                        }
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.EQUAL ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_PLUS ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_MINUS ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_MULTIPLY ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_DIVIDE ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_PLUS ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_MINUS)
            {
                if (X())
                {
                    return true;
                }
            }
            return false;
        }
        bool R1()
        {
            if (tokens[index].ClassPart == ClassPart.DOT)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                {
                    index++;
                    if (Z())
                    {
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.OPENING_SQUARE_BRACKET)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.INT_CONSTANT)
                {
                    index++;
                    if (tokens[index].ClassPart == ClassPart.CLOSING_SQUARE_BRACKET)
                    {
                        index++;
                        if (tokens[index].ClassPart == ClassPart.DOT)
                        {
                            index++;
                            if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                            {
                                index++;
                                if (Z())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES)
            {
                index++;
                if (PL())
                {
                    if (tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES)
                    {
                        index++;
                        if (tokens[index].ClassPart == ClassPart.DOT)
                        {
                            index++;
                            if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                            {
                                index++;
                                if (Z())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.SEMI_COLON)
            {
                return true;
            }
            return false;
        }
        bool R2()
        {
            if (tokens[index].ClassPart == ClassPart.DOT)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                {
                    index++;
                    if (B())
                    {
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.SEMI_COLON)
            {
                return true;
            }
            return false;
        }

        bool B()
        {
            if (tokens[index].ClassPart == ClassPart.DOT ||
                tokens[index].ClassPart == ClassPart.OPENING_SQUARE_BRACKET ||
                tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES ||
                tokens[index].ClassPart == ClassPart.SEMI_COLON)
            {
                if (Z2())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.EQUAL ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_PLUS ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_MINUS ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_MULTIPLY ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_DIVIDE ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_PLUS ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_MINUS)
            {
                if (XC())
                {
                    return true;
                }
            }
            return false;
        }

        bool XC()
        {
            if (tokens[index].ClassPart == ClassPart.EQUAL ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_PLUS ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_MINUS ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_MULTIPLY ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_DIVIDE)
            {
                if (assign_op())
                {
                    if (OE())
                    {
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_PLUS ||
                tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_MINUS)
            {
                if (inc_dec())
                {
                    return true;
                }
            }
            return false;
        }

        bool Z2()
        {
            if (tokens[index].ClassPart == ClassPart.DOT)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                {
                    index++;
                    if (R1C())
                    {
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.OPENING_SQUARE_BRACKET)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.INT_CONSTANT)
                {
                    index++;
                    if (tokens[index].ClassPart == ClassPart.CLOSING_SQUARE_BRACKET)
                    {
                        index++;
                        if (tokens[index].ClassPart == ClassPart.DOT)
                        {
                            index++;
                            if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                            {
                                index++;
                                if (R1C())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES)
            {
                index++;
                if (PL())
                {
                    if (tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES)
                    {
                        if (tokens[index].ClassPart == ClassPart.DOT)
                        {
                            index++;
                            if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                            {
                                index++;
                                if (R1C())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.SEMI_COLON)
            {
                return true;
            }
            return false;
        }
        bool R1C()
        {
            if (tokens[index].ClassPart == ClassPart.DOT)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                {
                    index++;
                    if (Z2())
                    {
                        return true;
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.OPENING_SQUARE_BRACKET)
            {
                index++;
                if (tokens[index].ClassPart == ClassPart.INT_CONSTANT)
                {
                    index++;
                    if (tokens[index].ClassPart == ClassPart.CLOSING_SQUARE_BRACKET)
                    {
                        index++;
                        if (tokens[index].ClassPart == ClassPart.DOT)
                        {
                            index++;
                            if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                            {
                                index++;
                                if (Z2())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.OPENING_PARANTHESES)
            {
                index++;
                if (PL())
                {
                    if (tokens[index].ClassPart == ClassPart.CLOSING_PARANTHESES)
                    {
                        index++;
                        if (tokens[index].ClassPart == ClassPart.DOT)
                        {
                            index++;
                            if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                            {
                                index++;
                                if (Z2())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            else if (tokens[index].ClassPart == ClassPart.SEMI_COLON)
            {
                return true;
            }
            return false;
        }

        bool c_name()
        {
            if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
            {
                index++;
                return true;
            }
            return false;
        }

        bool assign_st()
        {
            if (tokens[index].ClassPart == ClassPart.THIS ||
                tokens[index].ClassPart == ClassPart.IDENTIFIER)
            {
                if (thiss())
                {
                    if (tokens[index].ClassPart == ClassPart.IDENTIFIER)
                    {
                        index++;
                        if (X1())
                        {
                            if (initial())
                            {
                                if (listt())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        bool assign_op()
        {
            if (tokens[index].ClassPart == ClassPart.EQUAL)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_PLUS ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_MINUS ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_MULTIPLY ||
                tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_DIVIDE)
            {
                if (c_assign())
                {
                    return true;
                }
            }
            return false;
        }

        bool c_assign()
        {
            if (tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_PLUS)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_MINUS)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_MULTIPLY)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.COMPOUND_EQUAL_DIVIDE)
            {
                index++;
                return true;
            }
            return false;
        }

        bool inc_dec()
        {
            if (tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_PLUS)
            {
                index++;
                return true;
            }
            else if (tokens[index].ClassPart == ClassPart.INCREMENT_DECREMENT_MINUS)
            {
                index++;
                return true;
            }
            return false;
        }

        //MULTI-DIMENSIONAL ARRAY
        bool A()
        {
            if (tokens[index].ClassPart == ClassPart.EQUAL)
            {
                index++;
                if (AC())
                {
                    return true;
                }
            }
            return false;
        }

        bool AC()
        {
            if (tokens[index].ClassPart == ClassPart.CLOSING_CURLY_BRACKET)
            {
                if (C())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.NEW)
            {
                index++;
                if (data_type())
                {
                    if (tokens[index].ClassPart == ClassPart.OPENING_SQUARE_BRACKET)
                    {
                        index++;
                        if (N())
                        {
                            if (C1())
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        bool C()
        {
            if (tokens[index].ClassPart == ClassPart.OPENING_CURLY_BRACKET)
            {
                index++;
                if (D())
                {
                    return true;
                }
            }
            return false;
        }

        bool C1()
        {
            if (tokens[index].ClassPart == ClassPart.OPENING_CURLY_BRACKET)
            {
                index++;
                if (D())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.SEMI_COLON)
            {
                return true;
            }
            return false;
        }

        bool N()
        {
            if (tokens[index].ClassPart == ClassPart.INT_CONSTANT)
            {
                index++;
                if (IC())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.COMMA)
            {
                index++;
                if (I2())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.CLOSING_SQUARE_BRACKET)
            {
                index++;
                return true;
            }
            return false;
        }

        bool D()
        {
            if (tokens[index].ClassPart == ClassPart.OPENING_CURLY_BRACKET)
            {
                index++;
                if (M())
                {
                    if (DC())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        bool DC()
        {
            if (tokens[index].ClassPart == ClassPart.COMMA)
            {
                index++;
                if (D())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.CLOSING_CURLY_BRACKET)
            {
                index++;
                return true;
            }
            return false;
        }

        bool M()
        {
            if (tokens[index].ClassPart == ClassPart.INT_CONSTANT ||
               tokens[index].ClassPart == ClassPart.STRING_CONSTANT ||
               tokens[index].ClassPart == ClassPart.DOUBLE_CONSTANT ||
               tokens[index].ClassPart == ClassPart.BOOL_CONSTANT)
            {
                if (constt())
                {
                    if (MC())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        bool MC()
        {
            if (tokens[index].ClassPart == ClassPart.COMMA)
            {
                if (M())
                {
                    return true;
                }
            }
            return false;
        }

        bool BC()
        {
            if (tokens[index].ClassPart == ClassPart.CLOSING_CURLY_BRACKET)
            {
                index++;
                return true;
            }
            return false;
        }

        bool I2()
        {
            if (tokens[index].ClassPart == ClassPart.COMMA)
            {
                index++;
                if (I2())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.CLOSING_SQUARE_BRACKET)
            {
                index++;
                return true;
            }
            return false;
        }

        bool I()
        {
            if (tokens[index].ClassPart == ClassPart.INT_CONSTANT)
            {
                index++;
                if (IC())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.COMMA)
            {
                index++;
                if (I2())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.CLOSING_SQUARE_BRACKET)
            {
                index++;
                return true;
            }
            return false;
        }

        bool IC()
        {
            if (tokens[index].ClassPart == ClassPart.COMMA)
            {
                index++;
                if (I())
                {
                    return true;
                }
            }
            else if (tokens[index].ClassPart == ClassPart.CLOSING_SQUARE_BRACKET)
            {
                index++;
                return true;
            }
            return false;

        }
    }
}
