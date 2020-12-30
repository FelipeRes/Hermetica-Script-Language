using System.Collections.Generic;
using UnityEngine;
namespace HermeticaInterpreter{

    public class Entity{
        private static object GetPropValue(object src, string propName){
            if(src.GetType().GetField(propName)!=null){
                return src.GetType().GetField(propName).GetValue(src);
            }
            throw new System.Exception("This entity is fixed by enviroment. It haven't the property '"+propName+"'.");
            
        }
        private static void SetPropValue(object src, string propName, object value){
            if(src.GetType().GetField(propName)!=null){
                src.GetType().GetField(propName).SetValue(src, value);
            }
            throw new System.Exception("This entity is fixed by enviroment. It haven't the property '"+propName+"'.");
        }
        public Dictionary<string,object> keyPair;
        private object main;
        public Entity(){
            keyPair = new Dictionary<string,object>();
        }
        public Entity(object main){
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
                    return GetPropValue(main,name);
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
    }
}
