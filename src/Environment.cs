using System.Collections.Generic;

namespace HermeticaInterpreter{
    public class Environment : IEnvironment{
        // Start is called before the first frame update
        private Dictionary<string,object> models = new Dictionary<string, object>();
        private  Dictionary<string, object> values = new Dictionary<string, object>();
        public Environment enclosing;
        public Environment() {
            this.enclosing = null;
        }
        public Environment(Environment newEnclosing) {
            this.enclosing = newEnclosing;
        }
        virtual public void define(string name, object value) {
            if(!values.ContainsKey(name)){
                values.Add(name, value);
            }else{
                throw new System.Exception($"Variable '" + name + "' already defined");
            }
        }

        virtual public object get(HToken name) {
            if (values.ContainsKey(name.lexeme)) {
                return values[name.lexeme];
            }
            
            if (enclosing != null) {
                return enclosing.get(name);
            }else{
            }
            throw new System.Exception(
                "Undefined variable to get '" + name.lexeme + "'.");
        }
        virtual public void assign(HToken name, object value) {
            if (values.ContainsKey(name.lexeme)) {
                values[name.lexeme] =  value;
                return;
            }
            if (enclosing != null) {
                enclosing.assign(name, value);
                return;
            }
            throw new System.Exception("Undefined variable to assign '" + name.lexeme + "'. : " + name.lexeme);
        }
        virtual public void defineModel(string name, object model){
            if (!models.ContainsKey(name)) {
                models.Add(name,model);
            }
        }
        virtual public object getModel(string name){
            if (models.ContainsKey(name)) {
                return models[name];
            }
            throw new System.Exception("Undefined model: '" + name + "'");
        }
    }
}
