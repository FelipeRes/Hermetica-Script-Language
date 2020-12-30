

namespace HermeticaInterpreter{
    public interface IEnvironment{
        void define(string name, object value);
        object get(HToken name);
        void assign(HToken name, object value);
        void defineModel(string name, object model);
        object getModel(string name);
    }
}
