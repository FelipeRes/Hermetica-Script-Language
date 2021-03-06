# Hermetica-Script-Language
Language designed to interpret the effect cards of Hermetica Game. This language is similar to english to facilitate complexity of programming cards effect to non-programmers.
The programming logic is the same of other programming languages, but the sintax has a lot of prepositions to look like a human speaking.
```c++
show "hello magician";
```

## Introduction
Hermetica Script Language is a specific domain language to solve problems in Hermetica Card Game. _Can I use this language in other domains?_ Yes, you can! But you need to understand why this language exists. The main problem in Hermetica Card Game is how can I programming card effects without writing a specific method or function for each card in-game and how can I serialize these effect implementations to skip the problem that is building the code to each new card added? After study some open source code in community projects and check the all limitation tecnology, I concluded that the best solution is a specific script language to comunicate directly with the game core. This is the most difficult solution but is the most scalable solution for multiplatform games and the best way to maintain all of the control of the code, whithout any dependence of strange libraries and plugins. 

With a specific language, we can use it to solve other problems like scripting tests and AI programming and we can serialize all of these things. This language was developed to use as a terminal language like python in a Linux console and the game has a special console that I can run hermetica lang commands and it will change the game at runtime. The syntax is simple and near the English to be used by any person that knows the minimum about programming. This language haven't floats for example, because the game doesn't use float to work. The other problem with float is this tool can run in a backend server to support a online mode in game, but without a fixed point float, the user can get a incosistent data.

This language was development inside a Unity project, so the console to work with it is a game object. To setup your own console, use the _IConsole_ interface. The environment setup work with reflections to make the connection with the game core, so you can connect with you game too. 


## Sintax

### Data types

- **number** : only integer with 32 bits.
- **text**   : list of character and any text inside quotation marks
- **nil**    : the result of incompatible operations or not initialized variables.
- **list**   : a list of numbers, text, anything.
- **boolean**: a value that can be true or false.
- **entity** : group of key-values pair of text and anything.

### Assign values
Assign is an statement that defines a new variable or sets a value to an already existent variable. While you defines a new variable, you can set a value optionaly. The type of variables are iferred by the type of the value or expression used.  
To create a variable, use the sintax:

> _define_ \<identifier\>;
>
>  _define_ \<identifier\> _to_ \<expression\>;

Examples:

```c++
define variable;                     //recive nil as value
define number to 10;                 //number are only integer
define text to "Card Name";          //text is inside double quote
define key to true;                  //boolean values can be only true or false

define a to 10;                      //define with expressions
define b to 20;                      
define c to a + b;                   //assign by expression

define a to nil;                     //means that 'a' does not have any value
```
The variables can have theirs values changed to any value.

### Lists
Lists area group of nil or any values. 
Sintax:
> _[ \<value\>,\<value\> ..._]_

Defining lists:
```c++
define values to ["one","two","three"];  //you can define a list with comma separeted values
```
Lists can have any value:
```c++
values to [1,"text",true,["other_list"]]; 
```
Reading lists:
```c++
show #1 from values;                   //out the first value from list
show #2 from values;                   //out the seconde value from list
show #1 from values + #2 from values;  //out the sum of first and second value from list
```
Getting the size of the list:
```c++
defines list_size to size of the list;
```
Push values to list an define positions. The default position is the end of list.
```c++
add 1 to values;
add cat to cats;
add word to text at the end;
add word to text at the top;
add word to text at #3;
```
Removing values from list:
```c++
remove #1 from values;
remove first from values;
remove last from values;
```
You can use pop function to get the value and remove the list at the same time:
```c++
define first_value to pop #1 from values;
```
### Entities
Entities are an abstract representation of a concept. They have a name and a list of unique attributes. A entity can't have two attributes with the same name, so it work like a dictonary. 
Sintax:
> _entity:_ \<attribute\> _is_ \<expression\>, \<attribute\> _is_ \<value\> ... end
Example:
```c++
//defining entities
define apple to entity:
    color is "red",             
    taste is "sweet"
end
```
Reading entities values:
```c++
define color to color of apple;
show color;     //out "red"
```
Set other values in attributes of entities:
```c++
color of apple to "green";
```
Inserting a new attribute to entity
```c++
add "small" to apple as "size";
```
Removing a attribute of entity
```c++
remove "taste" of apple;
```

## Flow control
### If and else statement
The **if** statement is used to compare values and control the fluxe of program. In other hand, it can make decisions.
Sintax:
> _if_ \<logical expression\> _then_ \<statement\>
>
> _if_ \<logical expression\> _then_ \<statement\> _else_ \<statement\>

Use a block to if statement is optional, check the examples:
```javascript
if power of card < 10 then show card;
```
Using a else examples:
```javascript
if cardId is 10 then manifest cardId; else draw;

if (target == 2) then:
    remove this;
    power of targert to 10;
end
```
Take care with else beacause you can use it to finish blocks:
```javascript
if cardId is 1 then:
    if power of cardId is > 5 then:
        show cardId;
    else:
        remove cardsId;
    end
end
```
Observe that you can make **else if** by put a if immediately after the else because if is a statement.

### While statement
'While' is a loop statement tha repeat a statement while one conditional is true.
Sintax:
> _while_ \<logical expression\> _then_ \<statement\>
Example:
```javascript
while life <= 30 then life to + 1;
while color of last from hand is not white then draw;
```
**While** statements can operate blocks of statements: 
```javascript
while size of arena_cards < 3 then:
    manifest first from deck;
    manifest first from deck;
    remove first from arena;
end
```
### Foreach statement
**Foreach** is a loop statement for traversing items in a collection.
Sintax:
> _foreach_ \<identifiers\> _in_ \<list type\> then \<statement\>

Example:
```javascript
foreach card in hand then power of card to 0;
foreach card in arena then if life of card < 3 then remove card;
```
**Foreachs** can use with blocks too:
```javascript
foreach object in entities then:
    if color of object is blue then:
        remove object
        draw
    else endturn;
end
```
## Get Started
To use Hermetica Script Language in your game or application, you need only to setup a console. Check this unity example:
```c#
using UnityEngine;
using HermeticaInterpreter;

public class HermeticaConsole : MonoBehaviour, IConsole{

    //setup the environment
    HEnvironment environment;
    HInterpreter interpreter;
    
    //initialize the enviroment and the console
    public void Start(){
        environment = new HEnvironment();
        interpreter = new HInterpreter(environment,this);
        interpreter.environment = environment;
    }
    
    //a method to recive code inputs. You can create a visual console with a InputField that send the text here. 
    public void Read(string code){
    
        //initializing the pre processors.
        HScanner scanner = new HScanner(input.text);
        HParser parser = new HParser(scanner.scanTokens());
        
        //start to interpret
        try{
        
            List<HStatement> statments = parser.parse();
            
            //if the parse gets error, the statements will be null
            if(statments!=null){
                interpreter.interpret(statments);
            }
            
        }catch(System.Exception e){
        
            //show the exception
            Show(e.Message);
        }
    }
    
    //the out put in log window of Unity. You can create a black window console with unity UI.
    public void Show(string message){
        Debug.Log(message);
    }
}
```
You can attach this component to any GameObject and type code or send strings to test.

## Integration
This language was created to communicate with a game core API. To understand more about reading this [article](https://dev.to/feliperes/card-games-programming-1-game-core-4e8h) that I wrote. You can integrate your game by adding your game core object to the environment using the method _defineModel_.
```c#
HEnvironment environment;
HInterpreter interpreter;

//your game core reference
GameCoreSample coreSample;

public void Start(){

    //initializing your game core
    coreSample = new GameCoreSample();
    
    //initializing the enviroment and deine the game core as a model
    environment = new HEnvironment();
    environment.defineModel("game",coreSample);
    
    interpreter = new HInterpreter(environment,this);
}
```
In the script you can use the sintax:
> _cast_ "method" _from_ _\<model name\>_;
> _cast_ "method" _from_ _\<model name\>_ _using_ \<value\>, \<value\> ...; 

For example, if you game core has a function called "NextPlayerTurn" you can call it:
```javascript
cast "NextPlayerTurn" from game; 
```

If you game core has a function called "Attack" that recive two parameters, you can call it:
```javascript
cast "Attack" from game using card, target; 
```
If the function that you are calling not exist or has a specific number of arguments or the type of arguments aren't different, you will get a Exception from Hermertica's interpreter. You can add a lot of modules to integrate with the hermetica environment, but remember: you can only call functions but you will not receive any return. Hermetica's interpreter understands that the game core api has only input api and all output of the game must be serialized in logs. This is the basic architecture of a turn-based game, and can not work well with real-time game. But you can yet use it to create a console tool to modify your game at development.

### Integrate game object or poco class
You can also integrate a game object by add a new entity that represents that game. Check the example:
```c#
HEnvironment environment;
HInterpreter interpreter;

//the card object has a power property for example
Card card;

public void Start(){

    //initializing the card and creating a entity
    card = new Card();
    Entity entity = new Entity(card);
    
    //initializing the enviroment and deine the game core as a model
    environment = new HEnvironment();
    environment.define("card",card);
    
    interpreter = new HInterpreter(environment,this);
}
```
In the script, you can use the card like any entity:
```javascript
//change the power of card
power of card to 3;

//read the value of power of card
define damage to power of card * 2;
```
But you can't add new properties or remove the properties of the card
```javascript
//throw a exception: This entity is fixed by enviroment. It haven't the property 'stamina'.
add 3 to card as "stamina";

//throw a exception: This entity is fixed by enviroment. You can't remove property of it.
remove "power" from card;
```
You can add a lot of objects to the environment and modify their properties easily at runtime. But there is a limitation of type, you can only change the value of int, boolean and string types.

Thank you to Robert Nystrom.
[Crafting interpreters book](http://www.craftinginterpreters.com/)
[Crafting interpreters repository](https://craftinginterpreters.com/)









