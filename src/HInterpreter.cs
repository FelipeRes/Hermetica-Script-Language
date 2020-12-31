using System.Collections.Generic;
using System.Reflection;
using System;

namespace HermeticaInterpreter{
    public class HInterpreter : HExpression.Visitor<object>, HStatement.Visitor<object>{
        public Environment environment;
        private IConsole console;
        public HInterpreter(Environment environment, IConsole console){
            this.environment = environment;
            this.console = console;
        }
        //execution
        public void interpret(List<HStatement> statements) { 
            try {
                foreach (HStatement statement in statements) {
                    execute(statement);
                }
            }catch (HParser.ParseError error) {
                throw error;
            }catch (System.Exception error) {
                throw new System.Exception("Interpreting error: " + error.Message);
            }
        }
        public object visitBinaryExpr(HExpression.Binary expression){
            object left = evaluate(expression.left);
            object right = evaluate(expression.right); 

            switch (expression.op.type) {
            case TOKEN.BANG_EQUAL: return !isEqual(left, right);
            case TOKEN.EQUAL_EQUAL: return isEqual(left, right);
            case TOKEN.IS: return isEqual(left, right);
            case TOKEN.GREATER:
                checkNumberOperands(expression.op, left, right);
                return (int)left > (int)right;
            case TOKEN.GREATER_EQUAL:
                checkNumberOperands(expression.op, left, right);
                return (int)left >= (int)right;
            case TOKEN.LESS:
                checkNumberOperands(expression.op, left, right);
                return (int)left < (int)right;
            case TOKEN.LESS_EQUAL:
                checkNumberOperands(expression.op, left, right);
                return (int)left <= (int)right;
            case TOKEN.MINUS:
                checkNumberOperand(expression.op, right);
                return (int)left - (int)right;
            case TOKEN.SLASH:
                checkNumberOperands(expression.op, left, right);
                return (int)left / (int)right;
            case TOKEN.STAR:
                checkNumberOperands(expression.op, left, right);
                return (int)left * (int)right;
            case TOKEN.PLUS:
                if (left is int && right is int) {
                    return (int)left + (int)right;
                } 
                if (left is string && right is string) {
                    return (string)left + (string)right;
                }
            break;
            }
            return null;
        }

        public object visitUnaryExpr(HExpression.Unary expression){
            object right = evaluate(expression.right);
            switch (expression.op.type) {
            case TOKEN.MINUS:
                return -(int)right;
            case TOKEN.BANG:
                return !isTruthy(right);
            }

            // Unreachable.
            return null;
        }
        public object visitLiteralExpr(HExpression.Literal expression){
            return expression.value;
        }
        public object  visitGroupingExpr(HExpression.Grouping expression){
            return evaluate(expression.expression);
        }
        public object  visitVariableExpr(HExpression.Variable expression){
            return environment.get(expression.name);
        }
        public object  visitArrayExpr(HExpression.Array expression){
            List<object> values = new List<object>();
            foreach(HExpression exp in  expression.values){
                values.Add(evaluate(exp));
            }
            return values.ToArray();
        }
        public object  visitArrayValueExpr(HExpression.ArrayValue expression){
            object value = evaluate(expression.name);
            object index = evaluate(expression.index);
            if(value is object[]){
                return arrayValue((object[])value,index);
            }
            throw new Exception("Only list type are acepted to read by index.");
            
        }
        public object visitAssignExpr(HExpression.Assign expr) {
            object value = evaluate(expr.value);
            environment.assign(expr.name, value);
            return value;
        }
        public object visitAssignEntityExpr(HExpression.AssignEntity expr) {
            object entity = evaluate(expr.property.entity);
            object value = evaluate(expr.value);
            if(entity != null){
                if(entity is Entity){
                    ((Entity)entity).Add(expr.property.property.lexeme,value);
                    //environment.assign(expr.property.entity,entity);
                }else{
                    throw new System.Exception(expr.property + " is not a entity");
                }
            }else{
                throw new System.Exception("Expected entity is null");
            }
            return expr.value;
        }
        public object visitLogicalExpr(HExpression.Logical expr) {
            object left = evaluate(expr.left);
            if (expr.op.type == TOKEN.OR) {
                if (isTruthy(left)) return left;
            } else {
                if (!isTruthy(left)) return left;
            }

            return evaluate(expr.right);
        }
        public object visitSizeExpr(HExpression.Size expr) {
            object obj = evaluate(expr.expression);
            if(obj is string){
                return ((string)obj).Length;
            }
            if(obj is object[]){
                return ((object[])obj).Length;
            }
            return null;
        }
        public object visitKeyValueExpr(HExpression.KeyValue expr){
            return evaluate(expr.value);
        }
        public object visitEntityExpr(HExpression.Entity expr){
            Entity entity = new Entity();
            foreach(HExpression.KeyValue pair in expr.dict){
                object value = evaluate(pair.value);
                entity.Add(pair.key.lexeme,value);
            }
            return entity;
        }
        public object visitEntityPropertyExpr(HExpression.EntityProperty expr){
            object entity = evaluate(expr.entity);
            if(entity is Entity){
                return ((Entity)entity).Get(expr.property.lexeme);
            }
            throw new System.Exception("You try read a property of a non-entity type");
        }
        //implement statements
        public object visitPrintStmt(HStatement.Print statment){
            object value = evaluate(statment.expression);
            if(console!=null){
                console.Show(stringify(value));
            }
            return evaluate(statment.expression);
        }
        public object visitEpxressionStmt(HStatement.Expression statment){
            return evaluate(statment.expression);
        }
        public object visitVariableStmt(HStatement.Variable statment){
            object value = null;
            if (statment.initializer != null) {
                value = evaluate(statment.initializer);
            }
            environment.define(statment.name.lexeme, value);
            return value;
        }
        public object visitBlockStmt(HStatement.Block stmt) {
            Environment newEnvironment = new Environment(environment);
            executeBlock(stmt.statements, newEnvironment);
            return null;
        }

        public object visitIfStmt(HStatement.If stmt) {
            if (isTruthy(evaluate(stmt.condition))) {
                execute(stmt.thenBranch);
            } else if (stmt.elseBranch != null) {
                execute(stmt.elseBranch);
            }
            return null;
        }

        
        public object visitWhileStmt(HStatement.While stmt) {
            while (isTruthy(evaluate(stmt.condition))) {
                execute(stmt.body);
            }
            return null;
        }

        public object visitForeachStmt(HStatement.Foreach stmt) {
            Environment newEnvironment = new Environment(environment);
            object checkType = evaluate(stmt.list);
            if(IsList(checkType)){
                object[] list = (object[])checkType;
                if(list.Length > 0){
                    newEnvironment.define(stmt.variable.lexeme, list[0]);
                    for(int i = 0; i<list.Length; i++){
                        newEnvironment.assign(stmt.variable, list[i]);
                        exceuteInline(stmt.body,newEnvironment);
                    }
                }
            }
            return null;
        }
        public object visitAddStmt(HStatement.Add stmt){
            object variable = environment.get(stmt.list);
            object value = evaluate(stmt.value);
            object[] list = null;
            if(variable is object[]){
                if(stmt.index != null){
                    object index = evaluate(stmt.index);
                    if(index is int){
                        list = addToList((object[])variable,stmt.value,(int)index-1);
                    }else{
                        throw new System.Exception("The index must to be a number value");
                    }
                }else if(stmt.position != null){
                    if(stmt.position.type == TOKEN.TOP){
                        list = addToList((object[])variable,stmt.value,0);
                    }else if(stmt.position.type == TOKEN.END){
                        list = addToList((object[])variable,stmt.value,((object[])variable).Length);
                    }else{
                        throw new System.Exception("Positions can be only 'top', 'end' or a number.");
                    }
                }else{
                    list = addToList((object[])variable,stmt.value,((object[])variable).Length);
                }
            }else{
                throw new System.Exception("You can add value only to list type.");
            }
            environment.assign(stmt.list,list);
            return value;
        }
        public object visitAddEntityStmt(HStatement.AddEntity stmt){
            object value = evaluate(stmt.value);
            object key = evaluate(stmt.key);
            object entity = environment.get(stmt.entityName);
            if(entity is Entity){
                if(key is string){
                    Entity e = (Entity)entity;
                    e.Add((string)key,value);
                }else{
                    throw new System.Exception("Value property name can only be a string type");
                }
            }else{
                throw new System.Exception("You can add property only to entity types.");
            }
            return value;
            
        }
        public object visitRemoveStmt(HStatement.Remove stmt){
            object value = evaluate(stmt.value);
            object entity = environment.get(stmt.name);
            if(entity is Entity){
                if(value is string){
                    Entity e = (Entity)entity;
                    object oldValue = e.Get((string)value);
                    e.Remove((string)value);
                    return oldValue;
                }else{
                    throw new System.Exception("Value property name can only be a string type");
                }
            }else if(entity is object[]){
                List<object> list = new List<object>();
                list.AddRange((object[])entity);
                int index = Array.IndexOf((object[])entity, value);
                list.RemoveAt(index);
                return list.ToArray();
            }else{
                throw new System.Exception("You can add property only to entity types.");
            }
            
        }
        public object visitCastStmt(HStatement.Cast stmt){
            object model = environment.getModel(stmt.model.lexeme);
            List<object> arguments = new List<object>();
            List<Type> typeList = new List<Type>();
            if(stmt.args!=null){
                foreach(HExpression expression in stmt.args){
                    object result = evaluate(expression);
                    arguments.Add(result);
                    typeList.Add(result.GetType());
                }
            }
            if(model != null){
                Type type = model.GetType();
                if(stmt.methodName.literal is string){
                    string name = (string)stmt.methodName.literal;
                    MethodInfo magicMethod = type.GetMethod(name,typeList.ToArray());
                    if(magicMethod!=null){
                        magicMethod.Invoke(model,arguments.ToArray());
                        return null;
                    }
                    throw new System.Exception("The function name " + stmt.model.lexeme + " does not exist");
                }
                throw new System.Exception("The function name " + stmt.model.lexeme + " must be a string literal");
                
            }
            throw new System.Exception("The model " + stmt.model.lexeme + " not exist.");
        }

        //execute block
        public object exceuteInline(HStatement statement, Environment environment ){
            Environment previous = this.environment;
            try {
                this.environment = environment;
                execute(statement);
                
            } finally {
                this.environment = previous;
            }
            return null;
        }
        public object executeBlock(List<HStatement> statements, Environment environment) {
            Environment previous = this.environment;
            try {
                this.environment = environment;
                foreach (HStatement statement in statements) {
                    execute(statement);
                }
            } finally {
                this.environment = previous;
            }
            return null;
        }

        
        //Evaluating
        private object[] addToList(object[] list, object value, int position){
            List<object> newList = new List<object>();
            newList.AddRange(list);
            if(newList.Count == 0){
                newList.Add(value);
            }else if(position >= newList.Count || position <= newList.Count){
                newList.Insert(position,value);
            }else{
                throw new Exception("Index must be within the bounds of the list.");
            }
            
            return newList.ToArray();
        }
        private object arrayValue(object[] list, object index){
            if(index is int){
                if((int)index > list.Length || (int)index <1){
                    throw new System.Exception("The index value can't be greater than array size or less 1");
                }else{
                    return list[(int)index-1];
                }
            }else{
                throw new System.Exception("Only int type is acepted as index");
            }
        }

        private void execute(HStatement statement) {
            statement.accept(this);
        }

        private object evaluate(HExpression expr) {
            return expr.accept(this);
        }
        private bool isTruthy(object obj) {
            if (obj == null) return false;
            if (obj is bool) return (bool)obj;
            return true;
        }
        private bool isEqual(object a, object b) {
            if (a == null && b == null) return true;
            if (a is int && b is int) return (int)a == (int)b;
            if (a is string && b is string) return (string)a == (string)b;
            throw new System.Exception("Operands must be numbers or texts");
        }

        //Error
        private void checkNumberOperand(HToken op, object operand) {
            if (operand is int) return;
            throw new System.Exception("Operand must be a number: " + op.toString());
        }
        private void checkNumberOperands(HToken op, object left, object right) {
            if (left is int && right is int) return;
            throw new System.Exception("Operands must be numbers. " + op.toString());
        }
        private bool IsString(object obj){
            if(obj is string){
                return true;
            }else{
                throw new System.Exception("Exprected a string type");
            }
        }
        private bool isNumber(object obj){
            if(obj is int){
                return true;
            }else{
                throw new System.Exception("Exprected a number type");
            }
        }
        private bool IsList(object obj){
            if(obj is object[]){
                return true;
            }else{
                throw new System.Exception("Exprected a list type");
            }
        }
        private string stringify(object obj) {
            if (obj == null) return "nil";

            if (obj is int) {
                return obj.ToString();
            }
            if (obj is string) {
                return "\"" + obj.ToString() + "\"";
            }
            if (obj is object[]) {
                string print = "[";
                for(int i = 0; i<((object[])obj).Length; i++){
                    if(i == ((object[])obj).Length - 1){
                        print += stringify(((object[])obj)[i]);
                    }else{
                        print += stringify(((object[])obj)[i]) + ",";
                    }
                }
                print += "]";
                return print;
            }
            if (obj is Entity){
                string print = "Entity:\n";
                Dictionary<string,object> dict = ((Entity)obj).keyPair;
                foreach(KeyValuePair<string,object> key in dict){
                    print += key.Key + " is ";
                    print += stringify(key.Value);
                    print += "\n";
                }
                print += "end";
                return print;
            }
            return obj.ToString();
        }
    }
}
