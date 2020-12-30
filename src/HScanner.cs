using System.Collections.Generic;

namespace HermeticaInterpreter{
    public class HScanner{
        private string source;                                            
        private List<HToken> tokens = new List<HToken>();                   
        private int start = 0;                               
        private int current = 0;                             
        private int line = 1;   
        private static Dictionary<string, TOKEN> keywords = new Dictionary<string, TOKEN>();
        
        public void LoadKeywords(){
            keywords = new Dictionary<string, TOKEN>();
            keywords["show"]  = TOKEN.SHOW;
            keywords["for"]  = TOKEN.FOR;
            keywords["if"]  = TOKEN.IF;
            keywords["while"]  = TOKEN.WHILE;
            keywords["else"]  = TOKEN.ELSE;
            keywords["or"]  = TOKEN.OR;
            keywords["and"]  = TOKEN.AND;
            keywords["foreach"]  = TOKEN.FOREACH;
            keywords["in"]  = TOKEN.IN;
            keywords["on"]  = TOKEN.ON;
            keywords["from"]  = TOKEN.FROM;
            keywords["to"]  = TOKEN.TO;
            keywords["do"]  = TOKEN.DO;
            keywords["size"]  = TOKEN.SIZE;
            keywords["of"]  = TOKEN.OF;
            keywords["define"]  = TOKEN.DEFINE;
            keywords["is"]  = TOKEN.IS;
            keywords["true"]  = TOKEN.TRUE;
            keywords["false"]  = TOKEN.FALSE;
            keywords["nil"]  = TOKEN.FALSE;
            keywords["add"]  = TOKEN.ADD;
            keywords["remove"]  = TOKEN.REMOVE;
            keywords["first"]  = TOKEN.FIRST;
            keywords["last"]  = TOKEN.LAST;
            keywords["pop"]  = TOKEN.POP;
            keywords["at"]  = TOKEN.AT;
            keywords["the"]  = TOKEN.THE;
            keywords["end"]  = TOKEN.END;
            keywords["top"]  = TOKEN.TOP;
            keywords["then"]  = TOKEN.THEN;
            keywords["entity"]  = TOKEN.ENTITY;
            keywords["as"]  = TOKEN.AS;
            keywords["cast"]  = TOKEN.CAST;
            keywords["using"]  = TOKEN.USING;
        }
        public HScanner(){
            LoadKeywords();
        }
        public HScanner(string source){
            LoadKeywords();
            tokens.Clear();
            this.source = source;
        }
        private void scanToken() {             
            char c = advance();                          
            switch (c) {                                 
            case '(': addToken(TOKEN.LEFT_PAREN); break;     
            case ')': addToken(TOKEN.RIGHT_PAREN); break;  
            case '[': addToken(TOKEN.LEFT_BRACKET); break;     
            case ']': addToken(TOKEN.RIGHT_BRACKET); break;    
            case '{': addToken(TOKEN.LEFT_BRACE); break;     
            case '}': addToken(TOKEN.RIGHT_BRACE); break; 
            case ':': addToken(TOKEN.COLON); break;  
            case '#': addToken(TOKEN.HASHTAG); break;   
            case ',': addToken(TOKEN.COMMA); break;
            case '+': addToken(TOKEN.PLUS); break;
            case '-': addToken(TOKEN.MINUS); break;
            case '*': addToken(TOKEN.STAR); break;
            case '/': addToken(TOKEN.SLASH); break;
            case ';': addToken(TOKEN.SEMICOLON); break;
            case '=': addToken(match('=') ? TOKEN.EQUAL_EQUAL : TOKEN.EQUAL); break;     
            case '!': addToken(match('=') ? TOKEN.BANG_EQUAL : TOKEN.BANG); break;      
            case '<': addToken(match('=') ? TOKEN.LESS_EQUAL : TOKEN.LESS); break;      
            case '>': addToken(match('=') ? TOKEN.GREATER_EQUAL : TOKEN.GREATER); break;
            case ' ':                                    
            case '\r':                                   
            case '\t': break;
            case '\n': line++; break;
            case '"': String(); break;
            default:
                if (isDigit(c)) {                          
                    number();                                
                }  else if (isAlpha(c)) {                   
                    identifier();                            
                }else {                                   
                    throw new System.Exception("Line:"+line.ToString() + ", Unexpected character: '" + source[current]+"'");  
                }  
                break;
            }                                            
        } 
        public List<HToken> scanTokens(string source){
            tokens.Clear();                        
            this.source = source;
            return scanTokens();                                  
        } 
        public List<HToken> scanTokens(){
            start = 0;                               
            current = 0;                             
            line = 1;
            tokens.Clear();                        
            while (!isAtEnd()) {                            
                start = current;                              
                scanToken();                                  
            }
            tokens.Add(new HToken(TOKEN.EOF, "", null, line));    
            return tokens;                                  
        } 
        bool isAtEnd() {         
            return current >= source.Length;
        }
        private char advance() {                               
            current++;                                           
            return source[current - 1];                   
        }

        private void addToken(TOKEN type) {                
            addToken(type, null);                                
        }                                                      

        private void addToken(TOKEN type, object literal) {
            string text = source.Substring(start, current-start);      
            tokens.Add(new HToken(type, text, literal, line));    
        }
        private bool match(char expected) {                 
            if (isAtEnd()) return false;                         
            if (source[current] != expected) return false;
            current++;                                           
            return true;                                         
        } 
        private char peek() {           
            if (isAtEnd()) return '\0';   
            return source[current];
        }
        private void String() {                                   
            while (peek() != '"' && !isAtEnd()) {                   
                if (peek() == '\n') line++;                           
                advance();                                            
            }
            // Unterminated string.                                 
            if (isAtEnd()) {                                        
                throw new System.Exception("Line:"+line.ToString() + ", Unterminated string.");                                         
            }  
            // The closing ".                                       
            advance();                                              
            // Trim the surrounding quotes.                         
            string value = source.Substring(start + 1, current - 2 - start);
            addToken(TOKEN.STRING, value);                                
        } 
        private bool isDigit(char c) {
            return c >= '0' && c <= '9';   
        }
        private bool isAlpha(char c) {       
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') ||  c == '_';                     
        }
        private bool isAlphaNumeric(char c) {
            return isAlpha(c) || isDigit(c);      
        }
        private void identifier() {                
            while (isAlphaNumeric(peek())) advance();
            string text = source.Substring(start, current - start);
                      
            if (keywords.ContainsKey(text)) {
                addToken(keywords[text]);
            }
            else addToken(TOKEN.IDENTIFIER);          
        }  
        private char peekNext() {                         
            if (current + 1 >= source.Length) return '\0';
            return source[current + 1];              
        }
        private void number() {                                     
            while (isDigit(peek())) advance();                                               
            addToken(TOKEN.NUMBER,System.Convert.ToInt32(source.Substring(start, current - start)));
        } 
    }
}
