# Hermetica-Script-Language
Language designed to interpret the effect cards of Hermetica Game.

## Introduction

## Sintax

### Data types

- **number** : only integer with 32 bits.
- **text**   : list of character and any text inside quotation marks
- **nil**    : the result of incompatible operations or not initialized variables.
- **list**   : a list of numbers, text, anything.
- **object** : group of key-values pair of text and anything.

### Assign values
Assign is an statement that creates a new variable or sets a value to an already existent variable. 
To create a variable, use the sintax:
> _define_ \<identifier\>;
>
>  _define_ \<identifier\> _to_ \<value\>;
>
>  _define_ \<identifier\> = \<value\>;

Examples:

```c++
define my_variable;                     //recive nil as value
define my_number to 10;                 //number are only integer
define my_text to "Card Name";          //text is inside double quote
define my_list to [0,1,"word",myText];  //list can be have any object
define my_object to object:               //create a type of object
    first to 1,
    second to 2,
    third to 3
end

my_number to 10 + 20;                   //my_number value turn to 30
my_string to "hello" + " word";         //concat the values "hello word
my_list to my_list + ["another"];       //the '+' operator will join the lists
```

### If statement

### While statement

### For statement

### Foreach statement

### Lists
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

### Objects

```c++
//defining lists
define values to ["first","second","third"];

//reading lists
first to #1 from values;
second to #2 from values;
third to #1 from values + #2 from values;
splice = #1 to 10 from values
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
define Improve to spell [card, value]:
    power of card to + value;
end

//calling spell with parameters
Improve [card,3];   //power of card increase to 3;
Improve [card];     //sintax error, functions gets error because value does not exist
```
Spells can return values intead of end.
```c++
//defining a spell with arguments
define Pow to spell [value, expoent] : return value**expoent;

//calling spell with parameters
define result to Pow [2,2];
```
Spell can return other spells
```c++
//defining a spell with arguments
define CreatePrint to spell [text] : return spell : print[text]; 

//calling spell with parameters
define myPrint to CreatePrint["hellow word"];
myPrint;
```

## Environment

## Libraries

## Get Started
