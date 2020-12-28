# Hermetica-Script-Language
Language designed to interpret the effect cards of Hermetica Game. This language is similar to english to facilitate complexity of programming cards effect to non-programmers.
The programming logic is the same of other programming languages, but the sintax has a lot of prepositions to look like a human speaking.
```c++
show "hello magician";
```

## Introduction
Hermetica Script Language is a specific domain language to solve problems in Hermetica Card Game. _Can I use this language in other domains?_ Yes, you can! But you need to understand why this language exists. The main problem in Hermetica Card Game is how can I programming card effects without writing a specific method or function for each card in-game and how can I serialize these effect implementations to skip the problem that is building the code to each new card added? After study some open source code in community projects and check the all limitation tecnology, I concluded that the best solution is a specific script language to comunicate directly with the game core. This is the most difficult solution but is the most scalable solution for multiplatform games and the best way to maintain all of the control of the code, whithout any dependence of strange libraries and plugins. 

With a specific language, we can use it to solve other problems like scripting tests and AI programming and we can serialize all of these things. This language was developed to use as a terminal language like python in a Linux console and the game has a special console that I can run hermetica lang commands and it will change the game at runtime. The syntax is simple and near the English to be used by any person that knows the minimum about programming. This language haven't floats for example, because the game doesn't use float to work. 

The environment setup work with reflections to make the connection with the game core, so you can connect with you game too. 


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
    else then endturn;
end
```
