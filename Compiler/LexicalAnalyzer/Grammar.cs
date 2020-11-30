using System.Collections.Generic;

namespace Compiler
{
    /// <summary>
    /// Grammar of the Compiler
    /// Mostly uses dictionaries to store the Class part with keywords, punctuators and operators
    /// </summary>
    public class Grammar
    {
        public char[] WordBreakers { get; set; } = new char[]
        {
            ' ',
            '\n',
            '\t',
            '\r'
        };

        public Dictionary<string, ClassPart> Keywords { get; set; } = new Dictionary<string, ClassPart>()
        {
            {"break", ClassPart.BREAK},
            {"continue", ClassPart.CONTINUE},
            {"do",  ClassPart.DO },
            {"class", ClassPart.CLASS},
            {"catch", ClassPart.CATCH},
            {"else", ClassPart.ELSE },
            //{"false", ClassPart.FALSE},
            {"default", ClassPart.DEFAULT},
            //{"true", ClassPart.TRUE},
            {"if",   ClassPart.IF},
            {"for",  ClassPart.FOR},
            {"finally", ClassPart.FINALLY},
            {"using", ClassPart.USING},
            {"namespace" , ClassPart.NAMESPACE},
            {"new",  ClassPart.NEW},
            {"foreach",ClassPart.FOREACH },
            {"void", ClassPart.VOID},
            {"try",  ClassPart.TRY},
            {"return",ClassPart.RETURN},
            {"in",   ClassPart.IN},
            {"get",  ClassPart.GET},
            {"static",ClassPart.STATIC},
            {"null", ClassPart.NULL},
            {"this", ClassPart.THIS},
            {"while",ClassPart.WHILE},
            {"set", ClassPart.SET},

            //Data Types
            { "int",ClassPart.DATA_TYPE },
            { "string",ClassPart.DATA_TYPE },
            { "double",ClassPart.DATA_TYPE },
            { "bool", ClassPart.DATA_TYPE },
        };

        public Dictionary<string, ClassPart> Punctuators { get; set; } = new Dictionary<string, ClassPart>()
        {
            { ";", ClassPart.SEMI_COLON},
            { ":", ClassPart.COLON },
            { ",", ClassPart.COMMA },
            { ".", ClassPart.DOT },
            { "(", ClassPart.OPENING_PARANTHESES },
            { ")", ClassPart.CLOSING_PARANTHESES },
            { "{", ClassPart.OPENING_CURLY_BRACKET },
            { "}", ClassPart.CLOSING_CURLY_BRACKET },
            { "[", ClassPart.OPENING_SQUARE_BRACKET },
            { "]", ClassPart.CLOSING_SQUARE_BRACKET },
        };

        /// <summary>
        /// NOTE: The order of operators is IMPORTANT, it is used for checking
        /// </summary>
        public Dictionary<string, ClassPart> Operators { get; set; } = new Dictionary<string, ClassPart>()
        {
            //Assignment Operators
            {"+=",ClassPart.COMPOUND_EQUAL},
            {"-=",ClassPart.COMPOUND_EQUAL},
            {"/=",ClassPart.COMPOUND_EQUAL},
            {"*=",ClassPart.COMPOUND_EQUAL},
            {"%=",ClassPart.COMPOUND_EQUAL},

            //Increment/Decrement Operators
            {"++",ClassPart.INCREMENT_DECREMENT},
            {"--",ClassPart.INCREMENT_DECREMENT},

            //Arithmetic Operators
            {"+",ClassPart.PLUS_MINUS},
            {"-",ClassPart.PLUS_MINUS},
            {"*",ClassPart.MULTIPLY_DIVIDE_MODULUS},
            {"/",ClassPart.MULTIPLY_DIVIDE_MODULUS},
            {"%",ClassPart.MULTIPLY_DIVIDE_MODULUS},

            //Logical Operators
            {"&&",ClassPart.AND},
            {"||",ClassPart.OR},
            {"<=",ClassPart.RELATIONAL_OPERATOR},
            {">=",ClassPart.RELATIONAL_OPERATOR},
            {"!=",ClassPart.RELATIONAL_OPERATOR},
            {"==",ClassPart.RELATIONAL_OPERATOR},
            {"!",ClassPart.NOT},
            {">",ClassPart.RELATIONAL_OPERATOR},
            {"<",ClassPart.RELATIONAL_OPERATOR},


            //Equal
            {"=",ClassPart.EQUAL},
        };
    }
}