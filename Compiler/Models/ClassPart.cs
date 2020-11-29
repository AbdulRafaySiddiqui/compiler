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
        FALSE,
        DEFAULT,
        TRUE,
        IF,
        FOR,
        FINALLY,
        USING,
        NAMESPACE,
        NEW,
        FOREACH,
        VOID,
        TRY,
        RETURN,
        IN,
        GET,
        STATIC,
        NULL,
        THIS,
        WHILE,
        SET,

        //DATA TYPES
        INT,
        STRING,
        DOUBLE,
        BOOL,

        //DATA TYPE VALUES
        INT_VALUE,
        DOUBLE_VALUE,
        STRING_VALUE,
        BOOL_VALUE,

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

        //ARITHMETIC OPERATORS
        PLUS,
        MINUS,
        MULTIPLY,
        DIVIDE,
        MODULUS,

        //ASSIGNMENT OPERATORS
        PLUS_EQUAL,
        MINUS_EQUAL,
        DIVIDE_EQUAL,
        MULTIPLY_EQUAL,
        MODULUS_EQUAL,
        EQUAL,

        //INCREMENT/DECREMENT OPERATOR
        INCREMENT,
        DECREMENT,

        //LOGICAL OPERATOS
        AND,
        OR,
        NOT,
        LESS_EQUAL,
        GREATER_EQUAL,
        NOT_EQUAL,
        EQUAL_EQUAL,
        LESS_THAN,
        GREATER_THAN,

        //COMMENTS
        SINGLE_LINE_COMMENT,
        MULTI_LINE_COMMENT,
    }
}