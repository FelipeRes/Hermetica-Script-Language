
namespace HermeticaInterpreter{

    public enum TOKEN{

        //Define blocks 
        LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE,SEMICOLON,LEFT_BRACKET,RIGHT_BRACKET,
        //Separte instructions
        COMMA,
        //Operators
        BANG_EQUAL,BANG,EQUAL_EQUAL,                            
        EQUAL,IS,                          
        GREATER, GREATER_EQUAL,                          
        LESS, LESS_EQUAL,
        AND, JOIN,OR,
        //Literals.                                     
        STRING, NUMBER,IDENTIFIER,LIST,ENTITY,
        //filters

        //Flow
        IF,ELSE,WHILE,FOR,FOREACH,IN,ON,FROM,TO,DO,SIZE,OF,COLON,DEFINE,HASHTAG,THEN,

        //List operators
        ADD,REMOVE,FIRST,LAST,POP,AT,THE,TOP,END,AS,

        //Functions
        SHOW,CAST,USING,
        //Math
        MINUS, PLUS,SLASH, STAR,
        //Literals
        FALSE,TRUE,NIL,
        EOF 

    }
    public class HToken{
        
        public TOKEN type;
        public string lexeme;
        public object literal;
        public int line;
        public HToken(TOKEN type, string lexeme, object literal, int line) {
            this.type = type;                                             
            this.lexeme = lexeme;                                         
            this.literal = literal;                                       
            this.line = line;                                             
        } 
        public string toString() {                                      
            return type + " " + lexeme + " " + literal;                   
        }
    }
}
