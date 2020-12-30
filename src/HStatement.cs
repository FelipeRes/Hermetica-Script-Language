using System.Collections.Generic;

namespace HermeticaInterpreter{
    public abstract class HStatement {
        public interface Visitor<T> {
            T visitEpxressionStmt(Expression expression);
            T visitPrintStmt(Print expression);
            T visitVariableStmt(Variable expression);
            T visitBlockStmt(Block expression);
            T visitIfStmt(If expression);
            T visitWhileStmt(While expression);
            T visitForeachStmt(Foreach expression);
            T visitAddStmt(Add expression);
            T visitAddEntityStmt(AddEntity expression);
            T visitRemoveStmt(Remove expression);
            T visitCastStmt(Cast expression);
        } 
        abstract public  T accept<T>(Visitor<T> visitor);
        public class Expression : HStatement{
            public HExpression expression;
            public Expression(HExpression expression) {
                this.expression = expression;
            }
            override public string ToString(){
                return expression.ToString();
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitEpxressionStmt(this);
            }
        }
        public class Print : HStatement{
            public HExpression expression;
            public Print(HExpression expression) {
                this.expression = expression;
            }
            override public string ToString(){
                return expression.ToString();
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitPrintStmt(this);
            }
            
        }
        public class Variable : HStatement{
            public HExpression initializer;
            public HToken name;
            public Variable(HToken name, HExpression initializer) {
                this.name = name;
                this.initializer = initializer;
            }
            override public string ToString(){
                return name.lexeme + " = " + initializer.ToString();
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitVariableStmt(this);
            }
            
        }
         public class Block : HStatement{
            public List<HStatement> statements;
            public Block(List<HStatement> statements) {
                this.statements = statements;
            }
            override public string ToString(){
                return statements.ToString();
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitBlockStmt(this);
            }
            
        }
        public class If : HStatement{
            public HExpression condition;
            public HStatement thenBranch;
            public HStatement elseBranch;

            public If(HExpression condition,HStatement thenBranch,HStatement elseBranch) {
                this.condition = condition;
                this.thenBranch = thenBranch;
                this.elseBranch = elseBranch;
            }
            override public string ToString(){
                return condition.ToString();
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitIfStmt(this);
            }
            
        }
        public class While : HStatement{
            public HExpression condition;
            public HStatement body;

            public While(HExpression condition,HStatement body) {
                this.condition = condition;
                this.body = body;
            }
            override public string ToString(){
                return condition.ToString();
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitWhileStmt(this);
            }
            
        }
        public class Foreach : HStatement{
            public HToken variable;
            public HStatement body;
            public HExpression list;

            public Foreach(HToken variable,HExpression list, HStatement body) {
                this.variable = variable;
                this.list = list;
                this.body = body;
            }
            override public string ToString(){
                return variable.ToString();
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitForeachStmt(this);
            }
            
        }
        public class Add : HStatement{
            public HExpression value;
            public HToken list;
            public HExpression index;
            public HToken position;
            public Add(HToken list, HExpression value, HExpression index = null,HToken position = null) {
                this.list = list;
                this.value = value;
                this.index = index;
                this.position = position;
            }
            override public string ToString(){
                return value.ToString();
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitAddStmt(this);
            }
            
        }
        public class AddEntity : HStatement{
            public HExpression value;
            public HToken entityName;
            public HExpression key;
            public AddEntity(HExpression value,HToken entityName,HExpression key) {
                this.entityName = entityName;
                this.value = value;
                this.key = key;
            }
            override public string ToString(){
                return value.ToString();
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitAddEntityStmt(this);
            }
            
        }
        public class Remove : HStatement{
            public HExpression value;
            public HToken name;
            public Remove(HExpression value,HToken name) {
                this.name = name;
                this.value = value;
            }
            override public string ToString(){
                return value.ToString();
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitRemoveStmt(this);
            }
            
        }
        public class Cast : HStatement{
            public HToken methodName;
            public HToken model;
            public List<HExpression> args;
            public Cast(HToken methodName,HToken model,List<HExpression> args = null) {
                this.methodName = methodName;
                this.model = model;
                this.args = args;
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitCastStmt(this);
            }
            
        }
        
    }
}
