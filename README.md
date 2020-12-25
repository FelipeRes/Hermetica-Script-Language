# Hermetica-Script-Language
Language designed to interpret the effect cards of Hermetica Game.

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

define object to entity:             //create a type of object
    first to "one",
    second to "two",
    third to "three"
end

define a to 10;
define b to 20;
define c to a + b;                   //assign by expression

first of object = c;                 //changes the type from text to number

define a to nil;                     //means that a does not have any value

```
The variables can have theirs values changed to any value.

### Lists
Lists area group of any values. 
```c++
//defining lists
define values to ["one","two","three"];  //you can define a list with comma separeted values

//lists can have any value
values to [1,"text",true,["other_list"]]; 

//reading lists
print #1 from values;                   //out the first value from list
print #2 from values;                   //out the seconde value from list
print #1 from values + #2 from values;  //out the sum of first and second value from list

//getting the size of the list
defines list_size to size of the list;

//push values to list end of list
add 1 to values;
add cat to cats;
add word to text at the end;
add word to text at the top;
add word to text at #3;

//pop get something in a list by a index and remove it from list
define something to pop #3 from values;

//remove vales from list
remove #1 from values;
remove first from values;
remove last from values;
```

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

### Objects
```c++
//defining tables
define my_table to table:               //create a type of object
    first is 1,
    second is 2,
    third is 3
end

//reading lists
first of my_table = 2;
```



### Spells
Spells are blocks of statements that can be called in other parts of the code. Spells can recive paremeters to modify the spell acting.
```c++
//define a simple spell
define Kill to spell:
    life to 0;
    print "you died";
end

//calling spell
spell Kill;          //prints "you died";
```
Spells can recive a list of optional arguments. If you create a list before spell block, this list can recive external values that will be cloned to a list environment. If the arguments type not match with the the spell application, you can get error. 
```c++
//defining a spell with arguments
define Improve to spell to list card, value:
    power of card to + value;
end

//calling spell with parameters
Improve [card,3];   //power of card increase to 3;
Improve [card];     //sintax error, functions gets error because value does not exist
```


## Environment

## Libraries

## Get Started
