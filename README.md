# Hermetica-Script-Language
Language designed to interpret the effect cards of Hermetica Game. This language is similar to english to facilitate complexity of programming cards effect to non-programmers.
The programming logic is the same of other programming languages, but the sintax has a lot of prepositions to look like a human speaking.

## Introduction

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

first of object = c;                 //changes the type from text to number

define a to nil;                     //means that a does not have any value
```
The variables can have theirs values changed to any value.

### Lists
Lists area group of nil or any values. 
Defining lists:
```c++
define values to ["one","two","three"];  //you can define a list with comma separeted values
```
Lists can have any value:
```c++
values to [1,"text",true,["other_list"]]; 
```
Reading lists
```c++
show #1 from values;                   //out the first value from list
show #2 from values;                   //out the seconde value from list
show #1 from values + #2 from values;  //out the sum of first and second value from list
```
Getting the size of the list
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
Remove values from list
```c++
remove #1 from values;
remove first from values;
remove last from values;
```
You can use pop function to get the value and remove the list at the same time.
```c++
define first_value to pop #1 from values;
```
### Entities
Entities are an abstract representation of a concept. They have a name and a list of unique attributes. A entity can't have two attributes with the same name, so it work like a dictonary. Sintax:
> _table:_ \<attribute\> _is_ \<expression\>, \<attribute\> _is_ \<value\> ... end

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
### If statement
The **if** statement works exactly like a **python language** and have the current sintax:
> _if_ \<logical expression\> _then_ \<statement\>
>
> if \<logical expression\> _then_ \<statement\> _else_ \<statement\>

Use a block to if statement is optional, check the examples:
```javascript
//some times if statements can be written almost literally
if power of card < 10 then remove [card];

// if the card has the id 10, put it in the arena or draw a card
if cardId is 10 then manifest [cardId] else draw;

if (target == 2) then :
    remove this;
    power of targert to 10;
end

```
Observe that you can make **else if** by put a if immediately after the else because if is a statement.

### While statement

### For statement

### Foreach statement

## Environment

## Libraries

## Get Started
