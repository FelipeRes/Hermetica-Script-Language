using System.Collections.Generic;

namespace HermeticaInterpreter{
    public abstract class HExpression{
        public interface Visitor<T> {
            T visitBinaryExpr(Binary expression);
            T visitUnaryExpr(Unary expression);
            T visitLiteralExpr(Literal expression);
            T visitGroupingExpr(Grouping expression);
            T visitVariableExpr(Variable expression);
            T visitAssignExpr(Assign expression);
            T visitLogicalExpr(Logical expression);
            T visitArrayExpr(Array expression);
            T visitArrayValueExpr(ArrayValue expression);
            T visitSizeExpr(Size expression);
            T visitKeyValueExpr(KeyValue expression);
            T visitEntityExpr(Entity expression);
            T visitEntityPropertyExpr(EntityProperty expression);
            T visitAssignEntityExpr(AssignEntity expression);
        }
        abstract public  T accept<T>(Visitor<T> visitor);
        public class Binary : HExpression{
            public HExpression left;
            public HToken op;
            public HExpression right;
            public Binary(HExpression left, HToken op, HExpression right) {
                this.left = left;
                this.op = op;
                this.right = right;
            }
            override public string ToString(){
                return "( "+op.toString()+ " " +left.ToString() + " " +right.ToString()+" )";
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitBinaryExpr(this);
            }
            
        }
        public class Unary : HExpression{
            public HToken op;
            public HExpression right;
            public Unary( HToken op, HExpression right) {
                this.op = op;
                this.right = right;
            }
            override public string ToString(){
                return "( "+op.toString()+ " " +right.ToString()+" )";
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitUnaryExpr(this);
            }
        }

        public class Literal : HExpression {
            public object value;
            public Literal(object value) {
                this.value = value;
            }
            override public string ToString(){
                return value.ToString();
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitLiteralExpr(this);
            }

            
        }
        public class Grouping : HExpression {
            public HExpression expression;
            public Grouping(HExpression expression) {
                this.expression = expression;
            }
            override public string ToString(){
                return expression.ToString();
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitGroupingExpr(this);
            }
        }
        public class Variable : HExpression {
            public HToken name;
            public Variable(HToken name) {
                this.name = name;
            }
            override public string ToString(){
                return name.lexeme;
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitVariableExpr(this);
            }
        }
        public class Assign : HExpression {
            public HToken name;
            public HExpression value;
            public Assign(HToken name, HExpression value) {
                this.name = name;
                this.value = value;
            }
            override public string ToString(){
                return name.lexeme + " = " + value.ToString();
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitAssignExpr(this);
            }
        }
        public class Size : HExpression {
            public HExpression expression;
            public Size(HExpression expression) {
                this.expression = expression;
            }
            override public string ToString(){
                return expression.ToString();
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitSizeExpr(this);
            }
        }
        public class Array : HExpression {
            public List<HExpression> values;
            public Array(List<HExpression> values) {
                this.values = values;
            }
            override public string ToString(){
                string print = "[";
                foreach(HExpression expression in values){
                    print += values.ToString() + ","; 
                }
                print += "]";
                return values.ToString();
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitArrayExpr(this);
            }
        }
        public class ArrayValue : HExpression {
            public HExpression name;
            public HExpression index;
            public ArrayValue(HExpression name, HExpression index) {
                this.index = index;
                this.name = name;
            }
            override public string ToString(){
                return name.ToString() + " " +index.ToString();
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitArrayValueExpr(this);
            }
        }
        public class Logical : HExpression{
            public HExpression left;
            public HToken op;
            public HExpression right;
            public Logical(HExpression left, HToken op, HExpression right) {
                this.left = left;
                this.op = op;
                this.right = right;
            }
            override public string ToString(){
                return "( "+op.toString()+ " " +left.ToString() + " " +right.ToString()+" )";
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitLogicalExpr(this);
            }
            
        }
        public class KeyValue : HExpression{
            public HToken key;
            public HExpression value;
            public KeyValue(HToken key, HExpression value) {
                this.key = key;
                this.value = value;
            }
            override public string ToString(){
                return key.toString()+ " " + value.ToString();
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitKeyValueExpr(this);
            }
        }
        public class Entity : HExpression{
            public HExpression.KeyValue[] dict;
            public Entity(HExpression.KeyValue[] dict) {
                this.dict = dict;
            }
            override public string ToString(){
                return dict.ToString();
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitEntityExpr(this);
            }
        }
        public class EntityProperty : HExpression{
            public HToken property;
            public HExpression entity;
            public EntityProperty(HToken property,HExpression entity) {
                this.property = property;
                this.entity = entity;
            }
            override public string ToString(){
                return property.ToString();
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitEntityPropertyExpr(this);
            }
        }
        public class AssignEntity : HExpression {
            public EntityProperty property;
            public HExpression value;
            public AssignEntity(EntityProperty property,HExpression value) {
                this.value = value;
                this.property = property;
            }
            override public string ToString(){
                return property.ToString() + " to " + value.ToString();
            }
            override public  T accept<T>(Visitor<T> visitor) {
                return visitor.visitAssignEntityExpr(this);
            }
        }
        
        
    }
}
