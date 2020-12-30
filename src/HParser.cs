using System.Collections.Generic;

namespace HermeticaInterpreter{
    public partial class HParser {
        private List<HToken> tokens = new List<HToken>();      
        List<HExpression> expressions = new List<HExpression>();              
        private int current = 0; 

        //The constructor of class
        public HParser() {     
            tokens.Clear();
            current = 0;                      
        }
        
        public HParser(List<HToken> tokens) {                         
            current = 0;
            this.tokens = tokens;                              
        }
        
        public HExpression parseExpresstion() {
            try {
                return expression();
            } catch (ParseError error) {
                throw error;
            }
        }
        public List<HStatement> parse() {
            List<HStatement> statements = new List<HStatement>();
            while (!isAtEnd()) {
                statements.Add(declaration());
            }
            return statements; 
        }
        //Parse Statements
        private HStatement declaration(){
            try {
                return statement();
            } catch (ParseError error) {
                synchronize();
                throw error;
            }
        }

        private HStatement statement(){
            if (match(TOKEN.DEFINE)) return varDeclaration();
            if (match(TOKEN.FOREACH)) return forEachStatement();
            if (match(TOKEN.WHILE)) return whileStatement();
            if (match(TOKEN.IF)) return ifStatement();
            if (match(TOKEN.SHOW)) return printStatement();
            if (match(TOKEN.ADD)) return add();
            if (match(TOKEN.REMOVE)) return remove();
            if (match(TOKEN.CAST)) return cast();
            return expressionStatement();
        }
        private HStatement varDeclaration(){
            HToken name = consume(TOKEN.IDENTIFIER, "Expect variable name.");
            HExpression initializer = null;
            if (match(TOKEN.TO,TOKEN.EQUAL)) {
                initializer = expression();
            }
            if(previous().type == TOKEN.END){
                return new HStatement.Variable(name, initializer);
            }
            consume(TOKEN.SEMICOLON, "Expect ';' after variable declaration.");
            return new HStatement.Variable(name, initializer);
        }
        private HStatement whileStatement() {
            HExpression condition = expression();
            consume(TOKEN.THEN, "Expect 'then' after condition.");
            if(match(TOKEN.COLON)){
                List<HStatement> body = block();
                HStatement blockList = new HStatement.Block(body);
                return new HStatement.While(condition,blockList);
            }else{
                HStatement body = statement();
                return new HStatement.While(condition,body);
            }
        }
        private HStatement forEachStatement() {
            HToken variable = consume(TOKEN.IDENTIFIER,"Must be a variable name");
            consume(TOKEN.IN, "Expect 'in' after condition.");
            HExpression list = expression();
            consume(TOKEN.THEN, "Expect 'then' after card list.");
            if(match(TOKEN.COLON)){
                List<HStatement> body = block();
                HStatement blockList = new HStatement.Block(body);
                return new HStatement.Foreach(variable,list, blockList);
            }else{
                HStatement body = statement();
                return new HStatement.Foreach(variable,list, body);
            }
        }

        private HStatement ifStatement(){
            HExpression condition = expression();
            consume(TOKEN.THEN, "Expect 'then' after if condition."); 
            HStatement thenBranch;
            if(match(TOKEN.COLON)){
                List<HStatement> body = block();
                thenBranch = new HStatement.Block(body);
            }else{
                thenBranch = statement();
            }
            HStatement elseBranch = null;
            if (match(TOKEN.ELSE)) {
                if(match(TOKEN.COLON)){
                    List<HStatement> body = block();
                    elseBranch = new HStatement.Block(body);
                }else{
                    elseBranch = statement();
                }
            }
            return new HStatement.If(condition, thenBranch, elseBranch);
        }

        private HStatement printStatement(){
            HExpression value = expression();
            if(previous().type == TOKEN.END){
                 return new HStatement.Print(value);
            }
            if(previous().type == TOKEN.END){
                return new HStatement.Print(value);
            }
            consume(TOKEN.SEMICOLON, "Expect ';' after value.");
            return new HStatement.Print(value);
        }
        private HStatement remove(){
            HExpression value = term();
            consume(TOKEN.FROM, "Expect 'from' after value.");
            consume(TOKEN.IDENTIFIER, "You can only add values to list or entities variables");
            HToken name = previous();
            consume(TOKEN.SEMICOLON, "Expect ';' after value.");
            return new HStatement.Remove(value,name);
        }
        private HStatement add(){
            HExpression value = term();
            consume(TOKEN.TO, "Expect 'to' after value.");
            consume(TOKEN.IDENTIFIER, "You can only add values to list or entities variables");
            HToken list = previous();
            if(match(TOKEN.AS)){
                HExpression key = expression();
                consume(TOKEN.SEMICOLON, "Expect ';' after value.");
                return new HStatement.AddEntity(value,list,key);
            }
            if(match(TOKEN.AT)){
                if(match(TOKEN.THE)){
                    if(match(TOKEN.TOP)){
                        HToken position = previous();
                        if(previous().type == TOKEN.END){
                            return new HStatement.Add(list, value,position : position);
                        }
                        consume(TOKEN.SEMICOLON, "Expect ';' after value.");
                        return new HStatement.Add(list, value,position : position);
                    }else if(match(TOKEN.END)){
                        HToken position = previous();
                        if(previous().type == TOKEN.END){
                            return new HStatement.Add(list, value,position :position);
                        }
                        consume(TOKEN.SEMICOLON, "Expect ';' after value.");
                        return new HStatement.Add(list, value,position :position);
                    }else{
                        error(previous(),"Expect 'top', ' end' after 'the'.");
                    }
                }else if(match(TOKEN.HASHTAG)){
                    HExpression index = term();
                    if(previous().type == TOKEN.END){
                        return new HStatement.Add(list, value,index : index);
                    }
                    consume(TOKEN.SEMICOLON, "Expect ';' after value.");
                    return new HStatement.Add(list, value,index : index);
                }else{
                    error(previous(),"Expect 'the top', 'the end' or a '#'index value.");
                }
            }
            if(previous().type == TOKEN.END){
                return new HStatement.Add(list, value);
            }
            consume(TOKEN.SEMICOLON, "Expect ';' after value.");
            return new HStatement.Add(list, value);
            
            
        }
        private HStatement cast(){
            consume(TOKEN.STRING,"The function of model must to be a string literal.");
            HToken methodName = previous();
            consume(TOKEN.FROM,"Expect 'from' keyword after name of function.");
            consume(TOKEN.IDENTIFIER,"Expect the name of the model after 'from' keyworld.");
            HToken modelName = previous();
            if(match(TOKEN.USING)){
                List<HExpression> parameters = new List<HExpression>();
                do{
                    parameters.Add(expression());
                }while(match(TOKEN.COMMA));
                consume(TOKEN.SEMICOLON, "Expect ';' after cast statement.");
                return new HStatement.Cast(methodName,modelName,parameters);
            }
            consume(TOKEN.SEMICOLON, "Expect ';' after cast statement.");
            return new HStatement.Cast(methodName,modelName);
        }
        private List<HStatement> block() {
            List<HStatement> statements = new List<HStatement>();
            while (!check(TOKEN.END) && !isAtEnd()) {
                statements.Add(declaration());
                if(check(TOKEN.ELSE)){
                    return statements;
                }
            }
            consume(TOKEN.END, "Expect 'end' after block.");
            return statements;
        }

        private HStatement expressionStatement(){
            HExpression expr = expression();
            if(previous().type == TOKEN.END){
                return new HStatement.Expression(expr);
            }
            consume(TOKEN.SEMICOLON, "Expect ';' after expression.");
            return new HStatement.Expression(expr);
            
            
        }

        // Parse expressions 
        private HExpression expression() {
            return assignment();
        }

        private HExpression assignment() {
            HExpression expr = Or();
            if (match(TOKEN.EQUAL,TOKEN.TO)) {
                HToken equals = previous();
                HExpression value = assignment();
                if (expr is HExpression.Variable) {
                    HToken name = ((HExpression.Variable)expr).name;
                    return new HExpression.Assign(name, value);
                }else if(expr is HExpression.EntityProperty){
                    return new HExpression.AssignEntity((HExpression.EntityProperty)expr,value);
                }
                error(equals, "Invalid assignment target."); 
            }
            return expr;
        }
        private HExpression Or() {
            HExpression expr = And();
            while (match(TOKEN.OR)) {
                HToken op = previous();
                HExpression right = And();
                expr = new HExpression.Logical(expr, op, right);
            }
            return expr;
        }
        private HExpression And() {
            HExpression expr = equality();
            while (match(TOKEN.AND)) {
                HToken op = previous();
                HExpression right = equality();
                expr = new HExpression.Logical(expr, op, right);
            }
            return expr;
        }
        
        
        private HExpression equality() {
            HExpression expr = comparison();

            while (match(TOKEN.BANG_EQUAL, TOKEN.EQUAL_EQUAL, TOKEN.IS)) {
                HToken op = previous();
                HExpression right = comparison();
                expr = new HExpression.Binary(expr, op, right);
            }

            return expr;
        }
        private HExpression comparison() {
            HExpression expr = term();

            while (match(TOKEN.GREATER, TOKEN.GREATER_EQUAL, TOKEN.LESS, TOKEN.LESS_EQUAL)) {
                HToken op = previous();
                HExpression right = term();
                expr = new HExpression.Binary(expr, op, right);
            }

            return expr;
        }
        private HExpression term() {
            HExpression expr = factor();

            while (match(TOKEN.MINUS, TOKEN.PLUS)) {
                HToken op = previous();
                HExpression right = factor();
                expr = new HExpression.Binary(expr, op, right);
            }

            return expr;
        }
        private HExpression factor() {
            HExpression expr = unary();

            while (match(TOKEN.SLASH, TOKEN.STAR)) {
                HToken op = previous();
                HExpression right = unary();
                expr = new HExpression.Binary(expr, op, right);
            }

            return expr;
        }
        private HExpression unary() {
            if (match(TOKEN.BANG, TOKEN.MINUS)) {
                HToken op = previous();
                HExpression right = unary();
                return new HExpression.Unary(op, right);
            }

            return size();
        }
        private HExpression size() {
            if (match(TOKEN.SIZE)) {
                consume(TOKEN.OF,"The size functions expect 'of' keyword");
                return new HExpression.Size(expression());
            }
            return primary();
        }
        private HExpression primary() {
            if (match(TOKEN.FALSE)) return new HExpression.Literal(false);
            if (match(TOKEN.TRUE)) return new HExpression.Literal(true);
            if (match(TOKEN.NIL)) return new HExpression.Literal(null);

            if (match(TOKEN.NUMBER, TOKEN.STRING)) {
                return new HExpression.Literal(previous().literal);
            }
            if (match(TOKEN.IDENTIFIER)) {
                HToken name = previous();
                if(match(TOKEN.OF)){
                    consume(TOKEN.IDENTIFIER,"Read property expect a entity name");
                    HToken value = previous();
                    return new HExpression.EntityProperty(name,value);  
                }
                return new HExpression.Variable(previous());  
            }
            if (match(TOKEN.HASHTAG)) {
                HExpression index = expression();
                consume(TOKEN.FROM, "Expect 'from' after expression.");
                HExpression name = expression();
                return new HExpression.ArrayValue(name,index);
            }
            if (match(TOKEN.LEFT_PAREN)) {
                HExpression expr = expression();
                consume(TOKEN.RIGHT_PAREN, "Expect ')' after expression.");
                return new HExpression.Grouping(expr);
            }
            if(match(TOKEN.ENTITY)){
                consume(TOKEN.COLON,"Expected ':' afeter 'entity' keyworld");
                List<HExpression.KeyValue> values = new List<HExpression.KeyValue>();
                do {
                    values.Add(keyValue());
                } while (match(TOKEN.COMMA) && !check(TOKEN.END));
                consume(TOKEN.END,"The entity definition needs end with 'end' keyword");
                return new HExpression.Entity(values.ToArray());
            }
            if(match(TOKEN.LEFT_BRACKET)){
                List<HExpression> values = new List<HExpression>();
                if(match(TOKEN.RIGHT_BRACKET)){
                    return new HExpression.Array(values);
                }
                do {
                    values.Add(expression());
                } while (match(TOKEN.COMMA));
                consume(TOKEN.RIGHT_BRACKET, "Expect ']' after array values.");
                return new HExpression.Array(values);
            }else{
                throw error(peek(), "Expect expression.");
            }
        }

        private HExpression.KeyValue keyValue(){
            if (match(TOKEN.IDENTIFIER)) {
                HToken name = previous();
                consume(TOKEN.IS,"Expected 'is' after identifier");
                HExpression expr = expression();
                return new HExpression.KeyValue(name,expr);
            }
            throw error(peek(),"Expected a property name");
        }
        
        
        //This are the implementations of tools to parse the statements and expressions
        //We have a collection of simple functions that jumo to next token and check the types.
        private void synchronize() {
            advance();
            while (!isAtEnd()) {
                if (previous().type == TOKEN.SEMICOLON || previous().type == TOKEN.END ) return;
                switch (peek().type) {
                    case TOKEN.DEFINE:
                    case TOKEN.FOREACH:
                    case TOKEN.IF:
                    case TOKEN.WHILE:
                    case TOKEN.SHOW:
                    case TOKEN.ADD:
                    case TOKEN.REMOVE:
                    return;
                }
                advance();
            }
        }

        //Match functions check if the token is part of a group of others tokens with similar priority
        //If is true for someone, the token will jump automatically to next token
        private bool match(params TOKEN[] types) {
            foreach (TOKEN type in types) {
                if (check(type)) {                     
                    advance();                           
                    return true;                         
                }                                      
            }
            return false;                            
        }

        //This function make the check for the match and 
         bool check(TOKEN type) {
            if (isAtEnd()) return false; 
            return peek().type == type;          
        }

        //Increase the the interator of tokens up to the final token and always returns the last token if the tokens over
        HToken advance() {   
            if (!isAtEnd()) current++;
            return previous();        
        }

        //Checks if the current token is the End Of File
        bool isAtEnd() {      
            return peek().type == TOKEN.EOF;     
        }

        //Only gets the current token
        HToken peek() {        
            return tokens[current];    
        }                                

        //Gets the previous token
        private HToken previous() {       
            return tokens[current - 1];
        } 
        private HToken consume(TOKEN type, string message) {
            if (check(type)) return advance();
            throw error(peek(), message);
        }
        private ParseError error(HToken token, string message) {
            return new ParseError("Parsing error at line " + token.line + " : " + message);
        }
        public class ParseError : System.Exception {
            public ParseError(string message) : base(message){
            }
        }
        
    }
}
