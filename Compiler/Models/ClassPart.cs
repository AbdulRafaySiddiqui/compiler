namespace Compiler
{
    /// <summary>
    /// All class parts are available as Enum to avoid spelling mistakes and easy handling
    /// </summary>
    public enum ClassPart
    {
        INVALID,
        IDENTIFIER,

        //KEYWORDS
        BREAK,
        CONTINUE,
        DO,
        CLASS,
        CATCH,
        ELSE,
        DEFAULT,
        IF,
        FOR,
        USING,
        NEW,
        VOID,
        TRY,
        RETURN,
        IN,
        STATIC,
        THIS,
        WHILE,
        PUBLIC,
        PRIVATE,

        //DATA TYPES
        DATA_TYPE,
       
        //CONSTANTS
        INT_CONSTANT,
        DOUBLE_CONSTANT,
        STRING_CONSTANT,
        BOOL_CONSTANT,

        //PUNCTUATORS
        OPENING_PARANTHESES,
        CLOSING_PARANTHESES,

        OPENING_CURLY_BRACKET,
        CLOSING_CURLY_BRACKET,

        CLOSING_SQUARE_BRACKET,
        OPENING_SQUARE_BRACKET,

        COLON,
        SEMI_COLON,
        COMMA,
        DOT,

        //ARITHMETIC OPERATORS
        PLUS_MINUS,
        MULTIPLY_DIVIDE_MODULUS,

        //ASSIGNMENT OPERATORS
        COMPOUND_EQUAL,
        EQUAL,

        //INCREMENT/DECREMENT OPERATOR
        INCREMENT_DECREMENT,

        //LOGICAL OPERATOS
        AND,
        OR,
        NOT,
        RELATIONAL_OPERATOR,

        //COMMENTS
        SINGLE_LINE_COMMENT,
        MULTI_LINE_COMMENT,
    }
}