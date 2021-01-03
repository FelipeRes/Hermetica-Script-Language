using System.Collections.Generic;
using System.Reflection;
namespace HermeticaInterpreter{

    public class Entity{
        private static object GetPropValue(object src, string propName){
            if(src.GetType().GetField(propName)!=null){
                return src.GetType().GetField(propName).GetValue(src);
            }
            if(src.GetType().GetProperty(propName)!=null){
                return src.GetType().GetProperty(propName).GetValue(src);
            }
            throw new System.Exception("This entity is fixed by enviroment. It haven't the property '"+propName+"'.");
            
        }
        private static void SetPropValue(object src, string propName, object value){
            if(src.GetType().GetField(propName)!=null){
                src.GetType().GetField(propName).SetValue(src, value);
                return;
            }
            if(src.GetType().GetProperty(propName)!=null){
                src.GetType().GetProperty(propName).SetValue(src, value);
                return;
            }
            throw new System.Exception("This entity is fixed by enviroment. It haven't the property '"+propName+"'.");
        }
        private Dictionary<string,object> _keyPair;
        public Dictionary<string,object> keyPair{
            get{
                if(main!=null){
                    return stringfy(main);
                }return _keyPair;
            }
            set{
                _keyPair = value;
            }
        }
        private object main;
        public Entity(){
            _keyPair = new Dictionary<string,object>(); 
            keyPair = new Dictionary<string,object>();
        }
        public Entity(object main){
            _keyPair = new Dictionary<string,object>(); 
            keyPair = new Dictionary<string,object>();
            this.main = main;
        }
        public void Add(string name, object value){
            if(main!=null){
                if(GetPropValue(main,name)!=null){
                    SetPropValue(main,name,value);
                }else{
                    throw new System.Exception("This entity is fixed by enviroment. You can't add property to it.");
                }
            }else{
                if(!keyPair.ContainsKey(name)){
                    keyPair.Add(name,value);
                }else{
                    keyPair[name] = value;
                }
            }   
            
            
        }
        public object Get(string name){
            if(main!=null) {
                if(GetPropValue(main,name)!= null){
                    object value = GetPropValue(main,name);
                    if(value is int ||  value is bool || value is null){
                        return value;
                    }else if(value is int[] || value is string[] || value is bool[]){
                        return ConvertToArray(value);
                    }else if(value is string[] || value is char[]){
                        return value.ToString();
                    }{
                        return new Entity(value);
                    }
                    
                }else{
                    return null;
                }
            }else if(keyPair.ContainsKey(name)){
                return keyPair[name];
            }else{
                return null;
            }
        }
        public void Remove(string name){
            if(main!=null) {
                throw new System.Exception("This entity is fixed by enviroment. You can't remove property of it.");
            }else if(keyPair.ContainsKey(name)){
                keyPair.Remove(name);
            }
            
        }
        public Dictionary<string,object> stringfy(object main){
            Dictionary<string,object> values = new Dictionary<string, object>();
            foreach(FieldInfo field in main.GetType().GetFields()){
                object value = field.GetValue(main);
                if(value is int[] ||value is string[] ||value is bool[] || value is char[]){
                    values.Add(field.Name,ConvertToArray(value));
                }else if(value is char || value is string){
                    values.Add(field.Name,value.ToString());
                }else if(value is int || value is bool || value is null){
                    values.Add(field.Name,value);
                }else{
                    values.Add(field.Name,new Entity(value));
                }
                
            }
            return values;
        }
        public object[] ConvertToArray(object value){
            List<object> array = new List<object>();
            if(value is int[]){
                foreach(int number in (int[])value){
                    array.Add(number);
                }
            }else if(value is string[]){
                foreach(string text in (string[])value){
                    array.Add(text);
                }
            }else if(value is bool[]){
                foreach(bool boolean in (bool[])value){
                    array.Add(boolean);
                }
            }else if(value is char[]){
                foreach(char character in (char[])value){
                    array.Add(character.ToString());
                }
            }
            return array.ToArray();
        }
    }
}
